namespace AuthentificationJWT.Api.Extensions.SpoonacularFood;

public static class SpoonacularFoodExtensions
{
    public static IServiceCollection AddSpoonacularFoodServices(this IServiceCollection services)
    {
        //services.AddSingleton<IFoodService, RetryPolicySpoonacularFoodService>();

        //services.AddSingleton<SpoonacularFoodService>();
        //services.AddSingleton<IFoodService>(x =>
        //    new ResilientSpoonacularFoodService(x.GetRequiredService<SpoonacularFoodService>()));

        services.AddResilient<
            SpoonacularFoodService,
            IFoodService,
            ResilientSpoonacularFoodService
        >();

        return services;
    }

    public static WebApplication MapSpoonacularFoodEndpointsApi(this WebApplication app)
    {
        app.MapGet(
                FoodEndpoints.GetProductsByQueryAndNumber,
                GetProductsByQueryAndNumberHandlerAsync
            )
            .WithTags(FoodEndpoints.Tag)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetProductsByQueryAndNumberHandlerAsync(
        IFoodService foodService,
        string query,
        int number
    )
    {
        var foods = await foodService.GetGroceryProductsByQueryAndNumberAsync(query, number);
        return foods != null ? Results.Ok(foods) : Results.NotFound();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace AuthenticationJWT.Api.BusinessLogic.Extensions.SpoonacularFood;

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

        services.AddScoped<IRecommendedWineService, RecommendedWineService>();

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

        app.MapGet(FoodEndpoints.GetWineRecommendations, GetWineRecommendationHandler)
            .WithTags(FoodEndpoints.Tag)
            .AllowAnonymous();

        app.MapPost(FoodEndpoints.AddRecommendedWine, AddRecommendedWineHandler)
            .WithTags(FoodEndpoints.Tag)
            .AllowAnonymous();

        app.MapPut(FoodEndpoints.UpdateRecommendedWine, UpdateRecommendedWineHandler)
            .WithTags(FoodEndpoints.Tag)
            .AllowAnonymous();

        app.MapDelete(FoodEndpoints.DeleteRecommendedWine, DeleteRecommendedWineHandler)
            .WithTags(FoodEndpoints.Tag)
            .AllowAnonymous();

        return app;
    }

    private static async Task<IResult> GetProductsByQueryAndNumberHandlerAsync(
        IFoodService foodService,
        [FromQuery] string query,
        [FromQuery] int number
    )
    {
        var foods = await foodService.GetGroceryProductsByQueryAndNumberAsync(query, number);
        return foods != null ? Results.Ok(foods) : Results.NotFound();
    }

    private static RecommendedWineResponse? GetWineRecommendationHandler(
       IRecommendedWineService recommendedWineService
    )
    {
        var foods = recommendedWineService.GetWineRecommendation();
        return foods;
    }

    private static IResult AddRecommendedWineHandler(
       IRecommendedWineService recommendedWineService,
       [FromBody] CreateRecommendedWineRequest recommendedWineRequest
    )
    {
        var wine = recommendedWineService.AddRecommendedWine(recommendedWineRequest);
        return wine != null ? Results.Created(FoodEndpoints.AddRecommendedWine, wine) : Results.NotFound();
    }

    private static IResult UpdateRecommendedWineHandler(
      IRecommendedWineService recommendedWineService,
      [FromBody] EditRecommendedWineRequest recommendedWineRequest
    )
    {
        int? wineId = recommendedWineService.UpdateRecommendedWine(recommendedWineRequest);
        return wineId != null ? Results.Ok() : Results.NotFound();
    }

    private static IResult DeleteRecommendedWineHandler(
      IRecommendedWineService recommendedWineService,
      [FromQuery] int wineId
    )
    {
        try
        {
            recommendedWineService.DeleteRecommendedWine(wineId);
        }
        catch 
        {
            Results.NotFound();
        }
        
        return Results.NoContent();
    }
}

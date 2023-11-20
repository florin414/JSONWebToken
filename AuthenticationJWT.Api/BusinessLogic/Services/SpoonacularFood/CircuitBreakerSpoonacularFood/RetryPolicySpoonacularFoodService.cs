namespace AuthenticationJWT.Api.BusinessLogic.Services.SpoonacularFood.CircuitBreakerSpoonacularFood;

public class RetryPolicySpoonacularFoodService : IFoodService
{
    private readonly IHttpClientFactory httpClientFactory;
    private SpoonacularFoodApiSettings SpoonacularFoodApiSettings { get; set; }
    private readonly IAsyncPolicy<HttpResponseMessage> retryPolicy = Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(
            x =>
                x.StatusCode >= HttpStatusCode.InternalServerError
                || x.StatusCode == HttpStatusCode.RequestTimeout
        )
        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

    public RetryPolicySpoonacularFoodService(
        IHttpClientFactory httpClientFactory,
        IOptions<SpoonacularFoodApiSettings> options
    )
    {
        this.httpClientFactory =
            httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        SpoonacularFoodApiSettings = options.Value;
    }

    public async Task<ProductsResponse?> GetGroceryProductsByQueryAndNumberAsync(
        string query,
        int number
    )
    {
        var client = httpClientFactory.CreateClient();
        var apiKey = SpoonacularFoodApiSettings.ApiKey;
        var apiUrl = SpoonacularFoodApiSettings.ApiUrl;

        var apiUrlWithValues = apiUrl.ReplaceApiUrlWithValues(query, number, apiKey);

        var foodResponse = await retryPolicy.ExecuteAsync(() => client.GetAsync(apiUrlWithValues));

        var food = await foodResponse.Content.ReadFromJsonAsync<ProductsResponse?>();

        return food;
    }

    public async Task<RecommendedWineResponse?> GetWineRecommendationByTypeWineAndNumberAsync(string wine, int number)
    {
        var client = httpClientFactory.CreateClient();
        var apiKey = SpoonacularFoodApiSettings.ApiKey;
        var apiUrl = SpoonacularFoodApiSettings.ApiUrl;

        var apiUrlWithValues = apiUrl.ReplaceApiUrlWithValues(wine, number, apiKey);

        var wineResponse = await retryPolicy.ExecuteAsync(() => client.GetAsync(apiUrlWithValues));

        var food = await wineResponse.Content.ReadFromJsonAsync<RecommendedWineResponse?>();

        return food;
    }
}

namespace AuthentificationJWT.Api.Services.SpoonacularFood;

public class SpoonacularFoodService : IFoodService
{
    private readonly IHttpClientFactory httpClientFactory;
    private SpoonacularFoodApiSettings SpoonacularFoodApiSettings { get; set; }

    public SpoonacularFoodService(
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

        var foodResponse = await client.GetAsync(apiUrlWithValues);

        if (foodResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        var food = await foodResponse.Content.ReadFromJsonAsync<ProductsResponse?>();

        return food;
    }
}

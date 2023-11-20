namespace AuthenticationJWT.Api.Domain.Helpers.EndpointsDefinition;

public static class FoodEndpoints
{
    private const string BaseUrl = "/food";
    public const string Tag = "Food";
    public const string GetProductsByQueryAndNumber = BaseUrl + "/productsByQueryAndNumber";
    public const string GetWineRecommendations = BaseUrl + "/recommendedWine";
    public const string UpdateRecommendedWine = BaseUrl + "/recommendedWine";
    public const string DeleteRecommendedWine = BaseUrl + "/recommendedWine";
    public const string AddRecommendedWine = BaseUrl + "/recommendedWine";
}

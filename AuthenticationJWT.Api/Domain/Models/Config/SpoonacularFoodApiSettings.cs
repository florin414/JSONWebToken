namespace AuthenticationJWT.Api.Domain.Models.Config;

public class SpoonacularFoodApiSettings
{
    public const string SpoonacularFoodApi = "SpoonacularFoodApi";
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
}

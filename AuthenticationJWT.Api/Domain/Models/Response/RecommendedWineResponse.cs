namespace AuthenticationJWT.Api.Domain.Models.Response;

public class RecommendedWineResponse
{
    public RecommendedWine[] RecommendedWines { get; set; }
    public int TotalFound { get; set; }
}

namespace AuthenticationJWT.Api.Domain.Models.Request;

public class CreateRecommendedWineRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? ImageUrl { get; set; }
    public double? AverageRating { get; set; }
    public double? RatingCount { get; set; }
    public double? Score { get; set; }
    public string? Link { get; set; }
}

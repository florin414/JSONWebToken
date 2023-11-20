namespace AuthenticationJWT.Api.Domain.Helpers;

public static class MapperRecommendedWine
{
    public static RecommendedWine AsRecommendedWine(this CreateRecommendedWineRequest recommendedWineRequest)
    {
        RecommendedWine recommendedWine = new()
        {
            Id = Random.Shared.Next(),
            Title = recommendedWineRequest.Title,
            Description = recommendedWineRequest.Description,
            AverageRating = recommendedWineRequest.AverageRating,
            ImageUrl = recommendedWineRequest.ImageUrl,
            Link = recommendedWineRequest.Link,
            Price = recommendedWineRequest.Price,
            RatingCount = recommendedWineRequest.RatingCount,
            Score = recommendedWineRequest.Score,
        };

        return recommendedWine;
    }

    public static EntityRecommendedWineResponse AsEntityRecommendedWineResponse(this CreateRecommendedWineRequest recommendedWineRequest)
    {
        EntityRecommendedWineResponse entityRecommendedWineResponse = new()
        {
            Id = Random.Shared.Next(),
            Title = recommendedWineRequest.Title,
            Description = recommendedWineRequest.Description,
            AverageRating = recommendedWineRequest.AverageRating,
            ImageUrl = recommendedWineRequest.ImageUrl,
            Link = recommendedWineRequest.Link,
            Price = recommendedWineRequest.Price,
            RatingCount = recommendedWineRequest.RatingCount,
            Score = recommendedWineRequest.Score,
        };

        return entityRecommendedWineResponse;
    }
    public static RecommendedWine AsRecommendedWine(this EditRecommendedWineRequest recommendedWineRequest)
    {
        RecommendedWine recommendedWine = new()
        {
            Id = recommendedWineRequest.Id,
            Title = recommendedWineRequest.Title,
            Description = recommendedWineRequest.Description,
            AverageRating = recommendedWineRequest.AverageRating,
            ImageUrl = recommendedWineRequest.ImageUrl,
            Link = recommendedWineRequest.Link,
            Price = recommendedWineRequest.Price,
            RatingCount = recommendedWineRequest.RatingCount,
            Score = recommendedWineRequest.Score,
        };

        return recommendedWine;
    }

    public static EntityRecommendedWineResponse AsEntityRecommendedWineResponse(this EditRecommendedWineRequest recommendedWineRequest)
    {
        EntityRecommendedWineResponse entityRecommendedWineResponse = new()
        {
            Id = recommendedWineRequest.Id,
            Title = recommendedWineRequest.Title,
            Description = recommendedWineRequest.Description,
            AverageRating = recommendedWineRequest.AverageRating,
            ImageUrl = recommendedWineRequest.ImageUrl,
            Link = recommendedWineRequest.Link,
            Price = recommendedWineRequest.Price,
            RatingCount = recommendedWineRequest.RatingCount,
            Score = recommendedWineRequest.Score,
        };

        return entityRecommendedWineResponse;
    }
    public static RecommendedWineResponse RecommendedWineResponse { get; } = new()
    {
        RecommendedWines = MerlotWineRepository.recommendedWineResponse.RecommendedWines,
        TotalFound = MerlotWineRepository.recommendedWineResponse.TotalFound,
    };
}

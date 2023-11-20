namespace AuthenticationJWT.Api.BusinessLogic.Services.SpoonacularFood;

public class RecommendedWineService : IRecommendedWineService
{
    public RecommendedWineService() {}

    public RecommendedWineResponse GetWineRecommendation()
    {
        RecommendedWineResponse response = MerlotWineRepository.recommendedWineResponse;
        return response;
    }

    public EntityRecommendedWineResponse AddRecommendedWine(CreateRecommendedWineRequest wine)
    {
        var recommendedWine = wine.AsRecommendedWine();

        MerlotWineRepository.AddRecommendedWine(recommendedWine);

        var entityRecommendedWineResponse = wine.AsEntityRecommendedWineResponse();

        return entityRecommendedWineResponse;
    }

    public int UpdateRecommendedWine(EditRecommendedWineRequest newWine)
    {
        var recommendedWine = newWine.AsRecommendedWine();

        MerlotWineRepository.UpdateRecommendedWine(recommendedWine);

        return newWine.Id;
    }

    public void DeleteRecommendedWine(int wine)
    {
        MerlotWineRepository.DeleteRecommendedWine(wine);
    }
}

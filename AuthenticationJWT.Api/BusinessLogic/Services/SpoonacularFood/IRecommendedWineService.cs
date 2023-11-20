namespace AuthenticationJWT.Api.BusinessLogic.Services.SpoonacularFood;

public interface IRecommendedWineService
{
    RecommendedWineResponse GetWineRecommendation();
    EntityRecommendedWineResponse AddRecommendedWine(CreateRecommendedWineRequest wine); 
    int UpdateRecommendedWine(EditRecommendedWineRequest newWine);
    void DeleteRecommendedWine(int wine);
}

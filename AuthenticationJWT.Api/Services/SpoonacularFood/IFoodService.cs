namespace AuthentificationJWT.Api.Services.SpoonacularFood;

public interface IFoodService
{
    Task<ProductsResponse?> GetGroceryProductsByQueryAndNumberAsync(string query, int number);
}

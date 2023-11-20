namespace AuthenticationJWT.Api.DataAccess;

public static class MerlotWineRepository
{
    public static RecommendedWineResponse recommendedWineResponse = new()
    {
        RecommendedWines = [
            new RecommendedWine()
            {
                Id = 428396,
                Title = "Rolling Stones 50th Anniversary Forty Licks Merlot Wine",
                Description = @"The 2012 Merlot captures the attitude of Mendocino County's style. 
                                Aromas of black cherry, leather, and tobacco are followed by rich flavors of cinnamon over vanilla. 
                                This dry red wine is a fine match for herb-roasted chicken, grilled beef or smoky chili.",
                Price = "$16.989999771118164",
                ImageUrl = "https://spoonacular.com/productImages/428396-312x231.jpg",
                AverageRating = 0.9599999785423279,
                RatingCount = 7,
                Score = 0.9145454330877825,
                Link = "https://www.amazon.com/Rolling-Stones-Anniversary-Merlot-Mendocino/dp/B01NCT6E9V?tag=spoonacular-20",
            },
            new RecommendedWine()
            {
                Id = 431508,
                Title = "Madsen Family Cellars State Merlot Wine",
                Description = @"This well-structured blend of Red Mountain and Wahluke Slope grapes has been aged 20 months in French and American oak barrels. 
                                Rich with cherries, cranberries and toffee with a smooth mouthfeel and a long, lingering finish. 
                                This big-bodied wine goes well with grilled lamb or beef. Seth Ryan vineyards, Red Mountain AVA and Stone Tree Vineyards, Wahluke Slope AVA. 
                                Wine Advocate 2010, issue 190 - 91 points",
                Price = "$22.0",
                ImageUrl = "https://spoonacular.com/productImages/431508-312x231.jpg",
                AverageRating = 1,
                RatingCount = 3,
                Score = 0.9,
                Link = "https://www.amazon.com/Columbia-Winery-Valley-Merlot-750mL/dp/B01ATV4HL2?tag=spoonacular-20",
            },
            new RecommendedWine()
            {
                Id = 431233,
                Title = "Rolling Stones 50th Anniversary Forty Licks Merlot Wine",
                Description = @"This rich, ripe merlot opens with aromas of wood spice and brambleberry jam notes, followed by flavors of raspberries, cherries and a touch of vanilla. 
                                Sweet cherries continue through the silky finish, rounded out with fine tannins.",
                Price = "$9.989999771118164",
                ImageUrl = "https://spoonacular.com/productImages/431233-312x231.jpg",
                AverageRating = 1,
                RatingCount = 2,
                Score = 0.8571428571428572,
                Link = "https://www.amazon.com/2014-Sagelands-Merlot-750-mL/dp/B01ETSP6WK?tag=spoonacular-20",
            },
            new RecommendedWine()
            {
                Id = 428601,
                Title = "Vampire Merlot",
                Description = @"smooth medium-bodied with a fresh, black cherry aroma, and hints of herbal spices. aged in oak, 
                                our merlot develops graceful fruit flavors in the cellar, complemented by subtle shadings of vanilla and toast from the oak",
                Price = "$17.950000762939453",
                ImageUrl = "https://spoonacular.com/productImages/428601-312x231.jpg",
                AverageRating = 1,
                RatingCount = 2,
                Score = 0.8571428571428572,
                Link = "https://www.amazon.com/2015-Vampire-California-Merlot-750/dp/B015OU4U9C?tag=spoonacular-20",
            }
        ],
        TotalFound = 4,
    };

    public static void AddRecommendedWine(RecommendedWine recommendedWine)
    {
        RecommendedWine[] newArray = new RecommendedWine[recommendedWineResponse.RecommendedWines.Length + 1];
        Array.Copy(recommendedWineResponse.RecommendedWines, newArray, recommendedWineResponse.RecommendedWines.Length);
        newArray[^1] = recommendedWine;
        recommendedWineResponse.RecommendedWines = newArray;
        recommendedWineResponse.TotalFound += 1;
    }

    public static void DeleteRecommendedWine(int id)
    {
        int indexToRemove = Array.FindIndex(recommendedWineResponse.RecommendedWines, wine => wine.Id == id);

        if (indexToRemove != -1)
        {
            RecommendedWine[] newArray = new RecommendedWine[recommendedWineResponse.RecommendedWines.Length - 1];
            Array.Copy(recommendedWineResponse.RecommendedWines, newArray, indexToRemove);
            Array.Copy(recommendedWineResponse.RecommendedWines, indexToRemove + 1, newArray, indexToRemove, recommendedWineResponse.RecommendedWines.Length - indexToRemove - 1);
            recommendedWineResponse.RecommendedWines = newArray;
            recommendedWineResponse.TotalFound -= 1;
        }
    }

    public static void UpdateRecommendedWine(RecommendedWine recommendedWine)
    {
        int indexToUpdate = Array.FindIndex(recommendedWineResponse.RecommendedWines, wine => wine.Id == recommendedWine.Id);

        if (indexToUpdate != -1)
        {
            recommendedWineResponse.RecommendedWines[indexToUpdate] = recommendedWine;
        }
    }
}

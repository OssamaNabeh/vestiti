using Microsoft.AspNetCore.Mvc.RazorPages;
using StyleSphere.Data;

namespace StyleSphere.Pages;

public class IndexModel : PageModel
{
    public IList<CatalogProduct> Trending { get; private set; } = new List<CatalogProduct>();
    public IList<CatalogProduct> BestSellers { get; private set; } = new List<CatalogProduct>();
    public IList<CatalogProduct> NewArrivals { get; private set; } = new List<CatalogProduct>();
    public CatalogProduct? HeroProduct { get; private set; }

    public void OnGet()
    {
        // Pick 2 top-rated items per category so every Trending filter tab has items
        Trending = Catalog.Categories
            .SelectMany(c => Catalog.All
                .Where(p => p.Category == c.Name && p.Stock > 0)
                .OrderByDescending(p => p.Tags.Contains("trending"))
                .ThenByDescending(p => p.Rating)
                .Take(2))
            .ToList();
        BestSellers = Catalog.All.OrderByDescending(p => p.Reviews).Take(8).ToList();
        NewArrivals = Catalog.All.OrderBy(p => p.DaysOld).Take(4).ToList();
        HeroProduct = Catalog.Find(3);
    }
}

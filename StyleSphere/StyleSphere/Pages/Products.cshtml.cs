using Microsoft.AspNetCore.Mvc.RazorPages;
using StyleSphere.Data;

namespace StyleSphere.Pages;

public class ProductsModel : PageModel
{
    public IList<CatalogProduct> Products { get; private set; } = new List<CatalogProduct>();
    public IList<string> Categories { get; private set; } = new List<string>();
    public IList<string> Brands { get; private set; } = new List<string>();
    public decimal MaxPrice { get; private set; }

    public void OnGet()
    {
        Products = Catalog.All.OrderByDescending(p => p.Reviews).ToList();
        Categories = Catalog.All.Select(p => p.Category).Distinct().ToList();
        Brands = Catalog.All.Select(p => p.Brand).Distinct().OrderBy(b => b).ToList();
        MaxPrice = Math.Ceiling(Catalog.All.Max(p => p.Price) / 50m) * 50m;
    }
}

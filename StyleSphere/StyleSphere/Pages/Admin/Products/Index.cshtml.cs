using Microsoft.AspNetCore.Mvc.RazorPages;
using StyleSphere.Models;

namespace StyleSphere.Pages.Admin.Products;

public class IndexModel : PageModel
{
    public IList<Product> Products { get; set; } = default!;

    public Task OnGetAsync()
    {
        var random = new Random();
        var categories = new[]
        {
            new Category { CategoryID = 1, Name = "Men" },
            new Category { CategoryID = 2, Name = "Women" },
            new Category { CategoryID = 3, Name = "Shoes" },
            new Category { CategoryID = 4, Name = "Accessories" }
        };

        var productNames = new[]
        {
            "Classic Denim Jacket", "Silk Evening Dress", "Leather Ankle Boots", "Vintage Sunglasses",
            "Cotton T-Shirt", "Wool Winter Coat", "Running Sneakers", "Leather Handbag",
            "Casual Chinos", "Floral Summer Dress", "Canvas Sneakers", "Silver Watch",
            "Hooded Sweatshirt", "Elegant Blouse", "High Heels", "Leather Belt",
            "Cargo Pants", "Maxi Skirt", "Sandals", "Crossbody Bag",
            "Polo Shirt", "Wrap Dress", "Oxford Shoes", "Baseball Cap",
            "Jeans", "Cardigan", "Loafers", "Scarf",
            "Blazer", "Jumpsuit", "Boots", "Necklace"
        };

        Products = Enumerable.Range(1, 25).Select(i => new Product
        {
            ProductID = i,
            Name = productNames[random.Next(productNames.Length)],
            Description = "Premium quality product with excellent craftsmanship and design.",
            Price = (decimal)(random.NextDouble() * 250 + 19.99),
            Stock = random.Next(0, 60),
            Image = $"https://picsum.photos/50/50?random={i + 200}",
            CategoryID = random.Next(1, 5),
            Category = categories[random.Next(categories.Length)]
        }).OrderBy(p => p.Name).ToList();

        return Task.CompletedTask;
    }
}


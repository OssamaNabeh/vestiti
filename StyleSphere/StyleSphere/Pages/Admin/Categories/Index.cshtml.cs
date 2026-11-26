using Microsoft.AspNetCore.Mvc.RazorPages;
using StyleSphere.Models;

namespace StyleSphere.Pages.Admin.Categories;

public class IndexModel : PageModel
{
    public IList<Category> Categories { get; set; } = default!;

    public Task OnGetAsync()
    {
        var random = new Random();
        
        Categories = new List<Category>
        {
            new Category 
            { 
                CategoryID = 1, 
                Name = "Men", 
                Description = "Clothing and accessories for men",
                Products = Enumerable.Range(1, random.Next(5, 12)).Select(i => new Product { ProductID = i }).ToList()
            },
            new Category 
            { 
                CategoryID = 2, 
                Name = "Women", 
                Description = "Clothing and accessories for women",
                Products = Enumerable.Range(1, random.Next(5, 12)).Select(i => new Product { ProductID = i + 10 }).ToList()
            },
            new Category 
            { 
                CategoryID = 3, 
                Name = "Shoes", 
                Description = "Footwear for all occasions",
                Products = Enumerable.Range(1, random.Next(5, 12)).Select(i => new Product { ProductID = i + 20 }).ToList()
            },
            new Category 
            { 
                CategoryID = 4, 
                Name = "Accessories", 
                Description = "Fashion accessories and add-ons",
                Products = Enumerable.Range(1, random.Next(5, 12)).Select(i => new Product { ProductID = i + 30 }).ToList()
            }
        }.OrderBy(c => c.Name).ToList();

        return Task.CompletedTask;
    }
}


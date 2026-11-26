using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StyleSphere.Data;

namespace StyleSphere.Pages;

public class detailsModel : PageModel
{
    public record Review(string Author, int Stars, string Title, string Body, string Date, string Size);

    public CatalogProduct? Product { get; private set; }
    public IList<CatalogProduct> Related { get; private set; } = new List<CatalogProduct>();
    public IList<Review> Reviews { get; private set; } = new List<Review>();

    static readonly Review[] Pool =
    {
        new("Giulia R.", 5, "Exactly what I hoped for", "Beautiful quality and the fit is spot on. The fabric feels far more expensive than the price suggests.", "2 weeks ago", "M"),
        new("Marco D.", 5, "Worth every cent", "Arrived in two days, packaged beautifully. I’ve already received compliments the first time I wore it.", "1 month ago", "L"),
        new("Sara K.", 4, "Great, runs slightly small", "Lovely piece. I’d size up if you’re between sizes — otherwise no complaints at all.", "1 month ago", "S"),
        new("Yuki T.", 5, "Third order, never disappointed", "Consistent sizing and quality across everything I’ve bought. Vestiti is my go-to now.", "2 months ago", "M"),
        new("Daniel P.", 4, "Solid and well made", "Stitching is clean and the materials feel premium. Colour is a touch deeper than in the photos.", "2 months ago", "L"),
        new("Amira H.", 5, "Looks amazing", "Fits perfectly and the colour is gorgeous. Fast delivery too.", "3 months ago", "XS"),
    };

    public IActionResult OnGet(int id)
    {
        Product = Catalog.Find(id);
        if (Product == null) return Page();
        Related = Catalog.Related(Product, 4).ToList();
        Reviews = Enumerable.Range(0, 4).Select(i => Pool[(id + i) % Pool.Length]).ToList();
        return Page();
    }
}

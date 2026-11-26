using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models;

namespace StyleSphere.Pages.Admin.Products;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CreateModel(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public SelectList Categories { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        Categories = new SelectList(categories, "CategoryID", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            Categories = new SelectList(categories, "CategoryID", "Name");
            return Page();
        }

        // Handle image upload
        if (ImageFile != null && ImageFile.Length > 0)
        {
            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("ImageFile", "Only JPG, PNG, and GIF images are allowed.");
                var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                Categories = new SelectList(categories, "CategoryID", "Name");
                return Page();
            }

            // Validate file size (5MB max)
            if (ImageFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image size must be less than 5MB.");
                var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                Categories = new SelectList(categories, "CategoryID", "Name");
                return Page();
            }

            // Generate unique filename
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            
            // Ensure directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            Product.Image = fileName;
        }

        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}


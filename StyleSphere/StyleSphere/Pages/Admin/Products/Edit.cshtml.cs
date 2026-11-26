using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models;

namespace StyleSphere.Pages.Admin.Products;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public EditModel(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public SelectList Categories { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
        if (product == null)
        {
            return NotFound();
        }
        Product = product;

        var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        Categories = new SelectList(categories, "CategoryID", "Name", Product.CategoryID);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            Categories = new SelectList(categories, "CategoryID", "Name", Product.CategoryID);
            return Page();
        }

        var existingProduct = await _context.Products.FindAsync(Product.ProductID);
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Handle image upload if a new file is provided
        if (ImageFile != null && ImageFile.Length > 0)
        {
            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("ImageFile", "Only JPG, PNG, and GIF images are allowed.");
                var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                Categories = new SelectList(categories, "CategoryID", "Name", Product.CategoryID);
                return Page();
            }

            // Validate file size (5MB max)
            if (ImageFile.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image size must be less than 5MB.");
                var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                Categories = new SelectList(categories, "CategoryID", "Name", Product.CategoryID);
                return Page();
            }

            // Delete old image if exists
            if (!string.IsNullOrEmpty(existingProduct.Image))
            {
                var oldImagePath = Path.Combine(_environment.WebRootPath, "images", "products", existingProduct.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
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

            existingProduct.Image = fileName;
        }

        // Update other properties
        existingProduct.Name = Product.Name;
        existingProduct.Description = Product.Description;
        existingProduct.Price = Product.Price;
        existingProduct.Stock = Product.Stock;
        existingProduct.CategoryID = Product.CategoryID;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(Product.ProductID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.ProductID == id);
    }
}


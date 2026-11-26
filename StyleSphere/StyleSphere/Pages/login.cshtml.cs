using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models;
using System.ComponentModel.DataAnnotations;

namespace StyleSphere.Pages;

public class loginModel : PageModel
{
    private readonly AppDbContext _context;

    public loginModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == Input.Email && u.Password == Input.Password);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return Page();
        }

        // Set session (simplified - in production, use proper authentication)
        HttpContext.Session.SetString("UserId", user.UserID.ToString());
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserRole", user.Role);

        TempData["SuccessMessage"] = $"Welcome back, {user.Name}!";
        return RedirectToPage("/Index");
    }
}

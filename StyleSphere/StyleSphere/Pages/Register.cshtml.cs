using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models;
using System.ComponentModel.DataAnnotations;

namespace StyleSphere.Pages;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;

    public RegisterModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public class InputModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
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

        // Check if email already exists
        if (await _context.Users.AnyAsync(u => u.Email == Input.Email))
        {
            ModelState.AddModelError("Input.Email", "Email already exists. Please use a different email.");
            return Page();
        }

        // Create new user
        var user = new User
        {
            Name = Input.Name,
            Email = Input.Email,
            Phone = Input.Phone,
            Password = Input.Password, // In production, hash this password
            Role = "Customer"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Set session
        HttpContext.Session.SetString("UserId", user.UserID.ToString());
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserRole", user.Role);

        TempData["SuccessMessage"] = $"Welcome to Vestiti, {user.Name}! Your account has been created successfully.";
        return RedirectToPage("/Index");
    }
}


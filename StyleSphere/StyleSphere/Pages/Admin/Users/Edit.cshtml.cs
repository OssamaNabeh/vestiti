using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models;

namespace StyleSphere.Pages.Admin.Users;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public User User { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.UserID == id);
        if (user == null)
        {
            return NotFound();
        }
        User = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingUser = await _context.Users.FindAsync(User.UserID);
        if (existingUser == null)
        {
            return NotFound();
        }

        // Check if email already exists (excluding current user)
        if (await _context.Users.AnyAsync(u => u.Email == User.Email && u.UserID != User.UserID))
        {
            ModelState.AddModelError("User.Email", "Email already exists.");
            return Page();
        }

        // Update properties
        existingUser.Name = User.Name;
        existingUser.Email = User.Email;
        existingUser.Phone = User.Phone;
        existingUser.Role = User.Role;

        // Only update password if provided
        if (!string.IsNullOrWhiteSpace(User.Password))
        {
            existingUser.Password = User.Password;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(User.UserID))
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

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.UserID == id);
    }
}


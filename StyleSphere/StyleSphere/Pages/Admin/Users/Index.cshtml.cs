using Microsoft.AspNetCore.Mvc.RazorPages;
using StyleSphere.Models;

namespace StyleSphere.Pages.Admin.Users;

public class IndexModel : PageModel
{
    public IList<User> Users { get; set; } = default!;

    public Task OnGetAsync()
    {
        var random = new Random();
        var firstNames = new[] { "John", "Sarah", "Michael", "Emily", "David", "Jessica", "James", "Amanda", "Robert", "Lisa", "William", "Jennifer", "Richard", "Michelle", "Joseph", "Ashley" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Wilson", "Anderson", "Thomas", "Taylor" };
        var roles = new[] { "Admin", "Customer", "Customer", "Customer", "Customer" };

        Users = Enumerable.Range(1, 15).Select(i =>
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            return new User
            {
                UserID = i,
                Name = $"{firstName} {lastName}",
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com",
                Phone = $"{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                Password = "password123",
                Role = roles[random.Next(roles.Length)]
            };
        }).OrderBy(u => u.Name).ToList();

        return Task.CompletedTask;
    }
}


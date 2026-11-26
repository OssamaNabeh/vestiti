using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StyleSphere.Pages;

public class checkoutModel : PageModel
{
    public string? UserName { get; private set; }

    public void OnGet()
    {
        UserName = HttpContext.Session.GetString("UserName");
    }
}

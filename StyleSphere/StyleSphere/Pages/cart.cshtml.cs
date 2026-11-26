using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StyleSphere.Pages;

public class cartModel : PageModel
{
    public List<CartItemViewModel> CartItems { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    public class CartItemViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
    }

    public Task OnGetAsync()
    {
        // Note: Cart is managed client-side via localStorage
        // This page will display cart items loaded via JavaScript
        // The actual cart data is handled in the view via JavaScript
        CartItems = new List<CartItemViewModel>();
        Subtotal = 0;
        Tax = 0;
        Total = 0;
        return Task.CompletedTask;
    }
}

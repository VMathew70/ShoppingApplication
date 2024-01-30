using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopRazor.Helpers;
using ShopRazor.Models;

namespace ShopRazor.Pages
{
    public class ViewCartModel : PageModel
    {

        public CartViewModel cartViewModel = new CartViewModel();
        public void OnGet()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            
            cartViewModel.CartItems = cartItems;
            cartViewModel.GrandTotal = cartItems.Sum(f => f.Quantity * f.Price);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ShopRazor.Helpers;
using ShopRazor.Models;

namespace ShopRazor.Components
{
   
    public class MiniCartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart");


            MiniCartViewModel miniCartViewModel;
            if (cartItems == null || cartItems.Count == 0)
            {
                miniCartViewModel = new MiniCartViewModel();
                miniCartViewModel.TotalAmount = 0;
                miniCartViewModel.NumberofProducts = 0;
            }
            else
            {
                miniCartViewModel = new MiniCartViewModel();
                miniCartViewModel.NumberofProducts = cartItems.Sum(x => x.Quantity);
                miniCartViewModel.TotalAmount = cartItems.Sum(x => x.Quantity * x.Price);
            }

            return await Task.FromResult((IViewComponentResult)View(miniCartViewModel));

        }
    }
}

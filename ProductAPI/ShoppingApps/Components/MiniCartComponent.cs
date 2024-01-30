using Microsoft.AspNetCore.Mvc;
using ShoppingApps.General;
using ShoppingApps.Models;
using System.Collections.Generic;

namespace ShoppingApps.Components
{
    public class MiniCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart");


            MiniCartViewModel miniCartViewModel;
            if (cartItems == null || cartItems.Count == 0)
            {
                miniCartViewModel = null;
            }
            else
            {
                miniCartViewModel = new MiniCartViewModel();
                miniCartViewModel.NumberofProducts = cartItems.Sum(x => x.Quantity); 
                miniCartViewModel.TotalAmount = cartItems.Sum(x => x.Quantity * x.Price);
            }
            
            return View(miniCartViewModel);

        }
    }
}

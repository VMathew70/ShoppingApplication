using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopRazor.Helpers;
using ShopRazor.Models;

namespace ShopRazor.Pages
{
    public class CartModel : PageModel
    {
        private readonly HttpClient _apiclient;
        //Uri urladdress = new Uri("https://localhost:7174/api");
        private Uri urladdress;

        public List<Product> productList = new List<Product>();
        private readonly ILogger<IndexModel> _logger;

        public CartModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiclient = new HttpClient();
            string url = configuration.GetValue<string>("ServiceAPI:APIUrl");
            urladdress = new Uri(url);
            _apiclient.BaseAddress = urladdress;
        }

        public IActionResult OnGetBuy(int id)
        {
            //Get product
            HttpResponseMessage httpResponseMessage = _apiclient.GetAsync(urladdress + "/Product/id?id=" + id).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Product product = JsonConvert.DeserializeObject<Product>(result);
                List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                CartItem cartItem = cart.Where(f => f.ProductId == id).FirstOrDefault();

                int? orderQuantity = cartItem == null ? 1 : cartItem.Quantity + 1;

                if (orderQuantity <= product.quantity)
                {

                    //Check quantity and add
                    if (cartItem == null)
                        cart.Add(new CartItem(product));
                    else
                        cartItem.Quantity += 1;

                    HttpContext.Session.SetJson("Cart", cart);

                   
                }
                else
                {
                    TempData["Success"] = "Stock not available";

                }
            }
            return Redirect(Request.Headers["Referer"].ToString());

        }

        public IActionResult OnGetRemove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.Where(f => f.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity >= 1)
                --cartItem.Quantity;
            else
                cart.RemoveAll(p => p.ProductId == id);

            if(cartItem.Quantity == 0)
            {
                cart.RemoveAll(p => p.ProductId == id);
            }

            if (cart.Count == 0 )
            {
                //HttpContext.Session.Remove("cart");
                HttpContext.Session.SetJson("Cart", cart);
                return RedirectToPage("Index");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "Product Removed";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult OnGetClear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToPage("Index");

        }



    }
}

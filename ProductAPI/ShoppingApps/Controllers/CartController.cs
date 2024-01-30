using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoppingApps.General;
using ShoppingApps.Models;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoppingApps.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _apiclient;
        private readonly IConfiguration _configuration;
        private Uri urladdress; 

        public CartController(IConfiguration configuration)
        {
            _apiclient = new HttpClient();
            _apiclient.BaseAddress = urladdress;
            _configuration = configuration;
            string url = configuration.GetValue<string>("ServiceAPI:APIUrl");
            urladdress = new Uri(url);
            //_apiclient.BaseAddress = new Uri("https://localhost:7174/api");
            _apiclient.BaseAddress = urladdress;

        }

        public IActionResult CheckOut()
        {
            return View();
        }

        public async Task<IActionResult> CreateOrder(Customer customer)
        {
            if (ModelState.IsValid)
            {
                List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart");

                //cartItems.ToList().ForEach(c => c.d = value);               

                var ordernumber = Guid.NewGuid().ToString();

                cartItems.ToList().ForEach(i =>
                {
                    i.ordernumber = ordernumber;
                    i.customername = customer.customername;
                    i.email = customer.email;
                });

                var newlist = JsonConvert.SerializeObject(cartItems);
                StringContent content = new StringContent(newlist, Encoding.UTF8, "application/json");

                var result = await _apiclient.PostAsync(urladdress + "/Order", content);

                if (result.IsSuccessStatusCode)
                {
                    HttpContext.Session.Remove("Cart");
                    //EmailCustomer(customer, ordernumber);
                    TempData["Success"] = "Your Order :"+ ordernumber;

                    MailRequest mailRequest = new MailRequest();
                    mailRequest.ToEmail = customer.email;
                    mailRequest.Subject = "Your Shopping Order";
                    mailRequest.Body = "Dear " + customer.customername+ Environment.NewLine +
                                       "Thanks for your order " + ordernumber+ Environment.NewLine+
                                        "Kind Regards" + Environment.NewLine +
                                        "Customer Services";

                    var mailrequest = JsonConvert.SerializeObject(mailRequest);
                    content = new StringContent(mailrequest, Encoding.UTF8, "application/json");

                    result = await _apiclient.PostAsync(urladdress + "/Email", content);



                    return View("FinalPage");
                }
                else
                {
                    return View("CheckOut");
                }

            }
            else
            {
                return View("CheckOut");
            }


        }


        public IActionResult Index()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartViewModel = new CartViewModel();
            cartViewModel.CartItems = cartItems;
            cartViewModel.GrandTotal = cartItems.Sum(f => f.Quantity * f.Price);

            return View(cartViewModel);
        }

        public IActionResult Add(int id)
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

                    TempData["Success"] = "Product added";
                }
                else
                {
                    TempData["Success"] = "Stock not available";

                }
            }


            return Redirect(Request.Headers["Referer"].ToString());
        }


        public IActionResult Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.Where(f => f.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
                --cartItem.Quantity;
            else
                cart.RemoveAll(p => p.ProductId == id);

            if (cartItem.Quantity == 0)
            {
                cart.RemoveAll(p => p.ProductId == id);
            }
            if (cart.Count == 0)
            {
                // HttpContext.Session.Remove("cart");
                HttpContext.Session.SetJson("Cart", cart);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["Success"] = "Product Removed";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Home");
        }

        public void EmailCustomer(Customer customer, string ordernumber)
        {

           


            //string businessEmail = _configuration.GetValue<string>("BusinessEmail");

            //using (MailMessage mm = new MailMessage(businessEmail, customer.email))
            //{
            //    mm.Subject = "Your Shopping Order";
            //    mm.IsBodyHtml = true;
            //    string htmlBody;
            //    htmlBody = "Dear " + customer.customername + ",<br><br>" +
            //               "Thanks for your order <br><br>" +
            //                "Kind Regards<br><br>" +
            //                "Customer Services";
            //    mm.Body = htmlBody;

            //    SmtpClient smtp = new SmtpClient();
            //    //smtp.Host = "smtp.gmail.com";
            //    //smtp.Host = "smtp.office365.com";
            //    smtp.Host = _configuration.GetValue<string>("EmailHost"); //


            //    smtp.EnableSsl = false;
            //    NetworkCredential NetworkCred = new NetworkCredential(businessEmail, "Palarivatt0m321!");

            //    smtp.UseDefaultCredentials = false;
            //    smtp.Credentials = NetworkCred;
            //    smtp.Port = 25;//587
            //    smtp.Send(mm);
            //    //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
            //}
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopRazor.Helpers;
using ShopRazor.Models;
using System.Text;

namespace ShopRazor.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly HttpClient _apiclient;
        //Uri urladdress = new Uri("https://localhost:7174/api");
        private Uri urladdress;

        public CheckOutModel(ILogger<IndexModel> logger, IConfiguration configuration)
        { 
            _logger = logger;
            _apiclient = new HttpClient(); string url = configuration.GetValue<string>("ServiceAPI:APIUrl");
            urladdress = new Uri(url);
            _apiclient.BaseAddress = urladdress;
        }

        [BindProperty]
        public Customer customer { get; set; }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
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
                    TempData["Success"] = "Your Order :" + ordernumber;

                    MailRequest mailRequest = new MailRequest();
                    mailRequest.ToEmail = customer.email;
                    mailRequest.Subject = "Your Shopping Order";
                    mailRequest.Body = "Dear " + customer.customername + Environment.NewLine +
                                       "Thanks for your order " + ordernumber + Environment.NewLine +
                                        "Kind Regards" + Environment.NewLine +
                                        "Customer Services";

                    var mailrequest = JsonConvert.SerializeObject(mailRequest);
                    content = new StringContent(mailrequest, Encoding.UTF8, "application/json");

                    result = await _apiclient.PostAsync(urladdress + "/Email", content);

                    return RedirectToPage("FinalStage");

                }
                else
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }

            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopRazor.Models;

namespace ShopRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _apiclient;
        //Uri urladdress = new Uri("https://localhost:7174/api");
        private Uri urladdress;

        public List<Product> productList = new List<Product>();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiclient = new HttpClient();
            string url = configuration.GetValue<string>("ServiceAPI:APIUrl");
            urladdress = new Uri(url);
            _apiclient.BaseAddress = urladdress;
        }

        public void OnGet()
        {
            HttpResponseMessage httpResponseMessage = _apiclient.GetAsync(urladdress + "/Product").Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<Product>>(result);
            }           
        }
    }
}

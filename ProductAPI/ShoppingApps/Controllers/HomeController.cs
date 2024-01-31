using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ShoppingApps.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ShoppingApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _apiclient;

        private Uri urladdress;
        
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiclient = new HttpClient();
            //_apiclient.BaseAddress = urladdress;
            string url = configuration.GetValue<string>("ServiceAPI:APIUrl");
             urladdress = new Uri(url);
            //_apiclient.BaseAddress = new Uri("https://localhost:7174/api");
            _apiclient.BaseAddress = urladdress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Product> productList = new List<Product>();
            HttpResponseMessage httpResponseMessage = _apiclient.GetAsync(urladdress + "/Product").Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                productList = JsonConvert.DeserializeObject<List<Product>>(result);
            }
            TempData["Success"] = "";
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

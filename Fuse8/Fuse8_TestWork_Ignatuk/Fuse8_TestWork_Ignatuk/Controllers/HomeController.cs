using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fuse8_TestWork_Ignatuk.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http;

namespace Fuse8_TestWork_Ignatuk.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

     
        public IActionResult Index()
        {
            return View();
        }

      
        public IActionResult Crypto()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                ViewData["Message"] = "Для просмотра содержимого, необходимо войти под учёткой";
                return RedirectToAction("Login", "Account");
            }
  
        }

        private readonly IHttpClientFactory _clientFactory;
        private static string API_KEY = "вставьтесвойапикей";
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Crypto(Models.Coin model)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.DefaultRequestHeaders.Add("Accepts", "application/json");

            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "1";
            queryString["convert"] = "USD";

            URL.Query = queryString.ToString();
            string result = await client.GetStringAsync(URL.ToString());

            var КотировкиВалют = new List<Coin>();
            var SteamDetails = JsonConvert.DeserializeObject<dynamic>(result);

            var x = new Coin();
            x.name = SteamDetails.data[0].name.ToString();
            x.symbol = SteamDetails.data[0].symbol.ToString();
            x.price = SteamDetails.data[0].quote.USD.price.ToString();
            x.percent_change_1h = SteamDetails.data[0].quote.USD.percent_change_1h.ToString();
            x.percent_change_24h = SteamDetails.data[0].quote.USD.percent_change_24h.ToString();
            x.market_cap = SteamDetails.data[0].quote.USD.market_cap.ToString();
            x.last_updated = SteamDetails.data[0].quote.USD.last_updated.ToString();
            x.logo = "~/images/btc_logo.png";
            КотировкиВалют.Add(x);
            return View(КотировкиВалют);
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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

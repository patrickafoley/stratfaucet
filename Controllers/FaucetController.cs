using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitcoinLib.Services.Coins.Cryptocoin;

namespace stratfaucet.Controllers
{
     [Route("api/[controller]")]
    public class FaucetController : Controller
    {

        private static readonly ICryptocoinService CoinService = new CryptocoinService("http://localhost:26174", "u", "p", "", 3);

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet("GetBalance")]
        public Balance GetBalance() {
            var bal = CoinService.GetBalance();
            return new Balance {
                balance = bal
            };
        }

        public class Balance {
           public decimal balance { get; set; } 
        }
    }
}

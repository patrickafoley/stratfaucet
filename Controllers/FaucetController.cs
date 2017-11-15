using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BitcoinLib.Services.Coins.Cryptocoin;
using stratfaucet.Lib;

namespace stratfaucet.Controllers
{
    [Route("api/[controller]")]
    public class FaucetController : Controller
    {
        private WalletUtils walletUtils;
        public FaucetController(IConfiguration config)
        {
            walletUtils = new WalletUtils(config);
        }
   
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
            return walletUtils.GetBalance();
        }

        [HttpPost("SendCoin")]
        public Transaction SendCoin([FromBody] Recipient address) {
            return walletUtils.SendCoin(address);
        }
    }
}

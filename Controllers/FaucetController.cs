using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using stratfaucet.Lib;
using Refit;

namespace stratfaucet.Controllers

{
    [Route("api/[controller]")]
    public class FaucetController : Controller
    {
        private IWalletUtils walletUtils;
        private IReCaptchaAPI recaptchaAPI;
        private IConfiguration config;
        private IHostingEnvironment env;
        public FaucetController(IConfiguration _config, IHostingEnvironment _env)
        {
            config = _config;
            env = _env;
            recaptchaAPI = RestService.For<IReCaptchaAPI>("https://www.google.com", new RefitSettings { });
            walletUtils = new WalletUtils(config);
        }


        [Route("/Error")]
        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet("GetBalance")]
        public async Task<Balance> GetBalance() {
            return await walletUtils.GetBalance();
        }

       [HttpPost("SendCoin")]
        public async Task<ActionResult> SendCoin([FromBody] Recipient recipient)
        {
          try{
            recipient.ip_address = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Console.WriteLine($"SendCoin received for {recipient.address} from {recipient.ip_address}");
            if(await VerifyCaptcha(recipient.captcha, recipient.ip_address) || env.IsDevelopment()){

              if (Throttling.Transactions.Count > 100)
              {
                  throw new FaucetException("Too many faucet users");
              }

              Throttling.Transactions.GetOrAdd(recipient.address, recipient);
              return new JsonResult(Throttling.WaitForTransaction(recipient.address));
            }else {
              throw new FaucetException("Captcha verification failed.");
            }

          } catch(FaucetException faucetException){
            this.HttpContext.Response.StatusCode = 500;
            return new JsonResult(new Error(faucetException.Message));
          }
        }

        private async Task<bool> VerifyCaptcha(string captchaResponse, string remoteIp) {
           var resp =  await recaptchaAPI.Verify(new Captcha{
                Secret =  config["Faucet:ReCaptchaSecret"],
                Response = captchaResponse,
                // RemoteIP = remoteIp
            });

            return resp.Success;
        }
    }
}

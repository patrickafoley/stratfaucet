using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Refit;
using stratfaucet.request;

namespace stratfaucet.Lib
{
    public class WalletUtils : IWalletUtils
    {
        private IConfiguration _config;

        private String apiUrl;

        private int port;

        private IStratisWalletAPI stratApi;

        private decimal coinDivisor = 100000000M;
        public WalletUtils(IConfiguration config)
        {
            _config = config;
            apiUrl = (_config["Faucet:FullNodeApiUrl"] != null ? _config["Faucet:FullNodeApiUrl"] : "http://127.0.0.1");
            port = Int32.Parse((_config["Faucet:FullNodeApiPort"] != null ? _config["Faucet:FullNodeApiPort"] : "37220"));

            stratApi = RestService.For<IStratisWalletAPI>(apiUrl + ":" + port,
            new RefitSettings
            {
            }
          );
        }
        public async Task<Balance> GetBalance()
        {
            try
            {
                var bal = await stratApi.GetBalance("testwallet");
                var address = await stratApi.GetUnusedAddress("testwallet", "account 0", 0);
                return new Balance
                {
                    balance = (bal.BalancesList.First().AmountConfirmed / coinDivisor),
                    returnAddress = address.Substring(address.IndexOf("\"") + 1, address.LastIndexOf("\"") -1)
                };
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return new Balance
                {
                    balance = 0
                };
            }
        }
        public async Task<Transaction> SendCoin(Recipient recipient)
        {
            var amount = (await GetBalance()).balance / 100;
        
            BuildTransaction buildTransaction = new BuildTransaction{
                WalletName = "testwallet",
                AccountName = "account 0",
                CoinType = 1, 
                Password = "123456",
                DestinationAddress = recipient.address,
                Amount = amount.ToString(), 
                FeeType = "low",
                AllowUnconfirmed = true
            };
            var transaction = await stratApi.BuildTransaction(buildTransaction);

            SendTransaction sendTransaction = new SendTransaction{
                Hex = transaction.Hex
            };

          var resp =  await stratApi.SendTransaction(sendTransaction);
          return new Transaction();
        }
        public bool newRecipient(Recipient recipient)
        {
            return true;
        }

    }
}
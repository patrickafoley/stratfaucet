using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BitcoinLib.Services.Coins.Cryptocoin;

namespace stratfaucet.Lib
{
    public class LegacyWalletUtils
    {
        private IConfiguration _config;
        private ICryptocoinService CoinService;
        private String walletURL;
        private String user;
        private String password;
        public LegacyWalletUtils(IConfiguration config)
        {
            _config = config;
            walletURL = _config["Faucet:WalletURL"];
            user = _config["Faucet:User"];
            password = _config["Faucet:Password"];

            CoinService = new CryptocoinService(walletURL, user, password, "", 3);
        }

        public Balance GetBalance()
        {
            try
            {
                var bal = CoinService.GetBalance();
                var returnAddress = CoinService.GetAccountAddress("");
                return new Balance
                {
                    balance = bal,
                    returnAddress = returnAddress
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

        public Transaction SendCoin(Recipient recipient) 
        {   
            var amount = (GetBalance().balance/100); 
            Console.WriteLine("Sending " + amount);

            if(newRecipient(recipient)){
                var conf = CoinService.SendFrom("", recipient.address, amount);
                return new Transaction {
                    confirmation = conf
                };
            } else {
                return null;
            }
        }

        private string getActiveSendAddress() 
        {
            foreach(var address in CoinService.GetAddressesByAccount("")){
                if(CoinService.GetAddressBalance(address) > 1) {
                    return address;
                }
            }

            return null;
        }

        // TODO put something in here to prevent abuse 
        private bool newRecipient(Recipient recipient) 
        {   
            return true;
        }

    }
}
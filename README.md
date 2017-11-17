# stratfaucet

This is a basic faucet for Stratis Platform test tokens (TSTRAT). It is written in dotnet core/typescript/Angular. It uses BitcoinLib against the stratisX `stratisd` RPC and is bundled with Docker containers.

## Runing the faucet with Docker

* Edit appsettings.json.docker:

``` 
  "Faucet": {
    "WalletURL": "http://wallet:26174",
    "User": "<username>", 
    "Password": "<password>"
  }
```

* Edit Docker_stratisX/stratis.conf.docker 
```
rpcuser=<username>
rpcpassword=<password>
---
```


* Build the dotnet core container 
``` 
docker build . 
```

* Build the `stratisd` Docker container 

```
cd Docker_stratisX/
docker build . 
```

* Create a docker network 

`docker network create mynet`

* Start the `stratisd` container on the created network
```
docker run --name wallet --network mynet -p 26174:26174 -it <container id>
```

* Start stratfaucet container
```
docker run --network mynet -p 5000:5000 -it <stratfaucet container id>
```

# TODO items

* Security audit. Use a wallet password in addition to the RPC passwords
* Fix default controller bug that causes site to not reload 
* Write backup scripts for the wallet.dat files 
* Figure out how to put docker containers on external storage 
* Get cheaper hosting


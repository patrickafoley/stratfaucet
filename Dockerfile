FROM microsoft/dotnet:2.0.0-runtime-jessie
WORKDIR /stratisX

RUN apt-get update \ 
	&& apt-get install -y git build-essential \
	 libssl-dev \
	 libdb++-dev \
	 libboost-all-dev \
	 libqrencode-dev  \
	 libminiupnpc-dev

RUN git clone https://github.com/stratisproject/stratisX.git \ 
	&& cd stratisX/src \
	&& make -f makefile.unix 

WORKDIR /stratfaucet

RUN git clone https://github.com/patrickafoley/stratfaucet \
	&& cd stratfaucet \
	&& dotnet restore \
	&& dotnet build 

CMD ["dotnet", "run"]
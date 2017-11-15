FROM microsoft/dotnet:2.0.0-sdk-jessie
WORKDIR /stratisX

# StratX dependencies:
RUN apt-get update \ 
	&& apt-get install -y git build-essential \
	 libssl-dev \
	 libdb++-dev \
	 libboost-all-dev \
	 libqrencode-dev  \
	 libminiupnpc-dev 

# dotnet dependencies:
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash -
RUN apt-get install -y nodejs

RUN git clone https://github.com/stratisproject/stratisX.git \ 
	&& cd stratisX/src \
	&& make -f makefile.unix 

WORKDIR /

RUN git clone https://github.com/patrickafoley/stratfaucet \
	&& cd stratfaucet \
	&& npm install \
	&& dotnet restore \
	&& dotnet publish 

EXPOSE 5000

CMD /stratisX/src/stratisd ; cd stratfaucet ; dotnet run 
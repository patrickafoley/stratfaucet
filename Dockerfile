FROM microsoft/dotnet:2.0.0-sdk-jessie
WORKDIR /

# StratX dependencies:
RUN apt-get update \ 
	&& apt-get install -y git build-essential \
	 libssl-dev \
	 libdb++-dev \
	 libboost-all-dev \
	 libqrencode-dev  \
	 libminiupnpc-dev \
	 lsof

COPY stratis.conf.docker /root/.stratis/stratis.conf
COPY stratis.conf.docker /root/.stratis/testnet/stratis.conf
COPY start.sh /

# dotnet dependencies:
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash -
RUN apt-get install -y nodejs

# install stratisX
RUN git clone https://github.com/stratisproject/stratisX.git \ 
	&& cd stratisX/src \
	&& make -f makefile.unix 

# install stratfaucet 
WORKDIR /

RUN git clone https://github.com/patrickafoley/stratfaucet 

COPY appsettings.json.docker /stratfaucet/appsettings.json 

RUN cd stratfaucet \
	&& npm install \
	&& dotnet restore \
	&& dotnet publish 

EXPOSE 5000

RUN chmod +x /start.sh
CMD /start.sh
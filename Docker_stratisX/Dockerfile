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

# install stratisX
RUN git clone https://github.com/stratisproject/stratisX.git \ 
	&& cd stratisX/src \
	&& make -f makefile.unix 

RUN cp /stratisX/src/stratisd /usr/local/bin

EXPOSE 26174

CMD echo "Running testnet faucet" ; /stratisX/src/stratisd -rpcallowip=* -testnet 
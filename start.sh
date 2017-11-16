#!/bin/bash

nohup /stratisX/src/stratisd -testnet & 
tail -f nohup.out &
cd /stratfaucet
dotnet run
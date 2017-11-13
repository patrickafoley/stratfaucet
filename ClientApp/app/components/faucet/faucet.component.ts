import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'faucet',
    templateUrl: './faucet.component.html'
})
export class FaucetComponent {

    public balance: Balance; 

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/Faucet/GetBalance').subscribe(result => {
           this.balance = result.json() as Balance;
        }, error => console.error(error));
    }
}

interface Balance {
    balance: number; 
}
import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

export declare var grecaptcha:any;

@Component({
    selector: 'faucet',
    templateUrl: './faucet.component.html'
})
export class FaucetComponent {
    public balance: Balance;
    public sendCoinForm: FormGroup;
    public transaction: Transaction;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private fb: FormBuilder) {
        http.get(baseUrl + 'api/Faucet/GetBalance').subscribe(result => {
            this.balance = result.json() as Balance;
        }, error => console.error(error));

        this.buildForm();
    }

    formErrors = {
        'address': ''
    };

    private buildForm(): void {
        this.sendCoinForm = this.fb.group({
            "address": ["", Validators.required]
        });
    }

    public onSendClick() {
        let captcha = grecaptcha.getResponse();
        if (this.sendCoinForm && captcha.length > 0) {
            const data = new SendCoin(this.sendCoinForm.get('address').value, captcha)
            this.http
            .post(this.baseUrl + 'api/Faucet/SendCoin', data).subscribe(result => {
                this.transaction = result.json() as Transaction;
                this.sendCoinForm.setValue({address: ""})
            }, error => {

              alert(error.json().message);
            })
        }
    }
}

class SendCoin {
    constructor(address: string, captcha: string) {
        this.address = address;
        this.captcha = captcha;
    }
    address: string;
    captcha: string;
}

class Balance {
    balance: number;
    returnAddress: string;
}

class Transaction {
    transactionId: string;
}

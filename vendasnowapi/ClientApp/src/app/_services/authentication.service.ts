import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GenericHttpService } from './genericHttpService';

@Injectable({ providedIn: 'root' })
export class AuthenticationService extends GenericHttpService<any>{
    constructor(private http: HttpClient) {
        super(http);
            }

            confirmUser(user) {
                return this.post('account/confirm', user);
            }

            confirmUserForgot(user) {
                return this.post('account/confirmForgot', user);
            }
}

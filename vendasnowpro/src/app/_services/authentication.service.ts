import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { GenericHttpService } from './genericHttpService';
import { ApplicationUser } from 'src/app/_model/application-user';
import { LoginUser } from '../_model/login-user-model'

@Injectable({ providedIn: 'root' })
export class AuthenticationService extends GenericHttpService<any>{
  protected baseUrl = `${environment.urlApi}`;

  constructor(private http: HttpClient) {
    super(http);
  }

  logout() {
    localStorage.removeItem('vendasnow_user');
  }

  addCurrentUser(user) {
    localStorage.setItem('vendasnow_user', JSON.stringify(user));
  }

  clearUser() {
    localStorage.removeItem('vendasnow_user');
  }

  getCurrentUser() {
    return new BehaviorSubject<any>(JSON.parse(localStorage.getItem('vendasnow_user'))).getValue();
  }

  login(user) {
    return this.postAll('account/loginVendasNow', user);
  }

  register(user) {
    return this.postAll('account/registerVendasNow', user);
  }

  recoverPassword(user) {
    return this.postAll('account/recoverPassword', user);
  }

  changePassword(user) {
    return this.postAll('account/changePassword', user);
  }

}

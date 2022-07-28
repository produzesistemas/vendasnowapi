import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { GenericHttpService } from './genericHttpService';
import { ApplicationUser } from 'src/app/_model/application-user';
import { LoginUser } from '../_model/login-user-model'
import { Storage } from '@capacitor/storage';

@Injectable({ providedIn: 'root' })
export class AuthenticationService extends GenericHttpService<any>{
  protected baseUrl = `${environment.urlApi}`;
  // currentUser: any;
  isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
  currentAccessToken = null;
  constructor(private http: HttpClient) {
    super(http);
  }

  logout() {
    this.clear();
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

  async setObject(value: any) {
    await Storage.set({ key: 'vendasnow_user', value: JSON.stringify(value) });
  }

  async getCurrentUser() {
    const ret = await Storage.get({ key: 'vendasnow_user' });
    return JSON.parse(ret.value);
  }
  
  async clear() {
    await Storage.clear();
  }

  

}

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
    protected baseSite = `${environment.urlApi}`;
    // private currentUserSubject: BehaviorSubject<any>;
    // public currentUser: BehaviorSubject<any>;

    constructor(private http: HttpClient) {
        super(http);
        // this.currentUser = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('petixco_user')));
        // this.currentUser = this.currentUserSubject.asObservable();
    }

    registerPartner(user: LoginUser) {
        return this.postAll('account/registerPartner', user);
    }

    registerMaster(user: LoginUser) {
        return this.postAll('account/registerMaster', user);
    }

    registerClient(user: ApplicationUser) {
        return this.postAll('account/registerClient', user);
    }

    logoutClient() {
        localStorage.removeItem('oxidu_client_user');
        // this.currentUser.next(null);
    }

    logoutMaster() {
      localStorage.removeItem('oxidu_master_user');
      // this.currentUser.next(null);
  }

  logoutPartner() {
    localStorage.removeItem('oxidu_partner_user');
    // this.currentUser.next(null);
  }

    addCurrentUserMaster(user) {
        localStorage.setItem('oxidu_master_user', JSON.stringify(user));
    }

    addCurrentUserPartner(user) {
      localStorage.setItem('oxidu_partner_user', JSON.stringify(user));
  }

  addCurrentUserClient(user) {
    localStorage.setItem('oxidu_client_user', JSON.stringify(user));
}

  clearUserPartner() {
    localStorage.removeItem('oxidu_partner_user');
}

clearUserMaster() {
  localStorage.removeItem('oxidu_master_user');
}

clearUserClient() {
  localStorage.removeItem('oxidu_client_user');
}

getCurrentUserPartner() {
  return new BehaviorSubject<any>(JSON.parse(localStorage.getItem('oxidu_partner_user'))).getValue();
}

getCurrentUserClient() {
  return new BehaviorSubject<any>(JSON.parse(localStorage.getItem('oxidu_client_user'))).getValue();
}

getCurrentUserMaster() {
  return new BehaviorSubject<any>(JSON.parse(localStorage.getItem('oxidu_master_user'))).getValue();
}
getClientsStore() {
  return this.http.get<any>(`${this.getUrlApi()}account/getClients`);
}

    save(store: FormData) {
        return this.post('account/save', store);
    }

    login(user) {
        return this.postAll('account/loginVendasNow', user);
    }

    loginPartner(user) {
        return this.postAll('account/loginPartner', user);
    }

    getByFilter(filter: any) {
        return this.postAll('account/filter', filter);
      }

      register(user) {
        return this.postAll('account/register', user);
    }

    recoverPassword(user) {
        return this.postAll('account/recoverPassword', user);
    }

    deleteById(id) {
        return this.delete(`account/${id}`);
  }

  disableUser(user) {
    return this.postAll('account/disable', user);
}

enableUser(user) {
    return this.postAll('account/enable', user);
}

getClients() {
    return this.http.get<any>(`${this.getUrlApi()}account/getClients`);
}

changePassword(user) {
    return this.postAll('account/changePassword', user);
}

confirmUser(user) {
  return this.postAll('account/confirm', user);
}

}

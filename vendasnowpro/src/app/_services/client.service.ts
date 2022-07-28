import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GenericHttpService } from './genericHttpService';
import { Client } from '../_model/client-model';

@Injectable({ providedIn: 'root' })

export class ClientService extends GenericHttpService<Client> {
    constructor(private http: HttpClient) {
        super(http);
    }

    getByFilter(filter: any) {
        return this.postAll('client/filter', filter);
      }

    deleteById(entity) {
        return this.post('client/delete', entity);
  }

  active(entity) {
    return this.post('client/active', entity);
 }

 save(entity) {
    return this.post('client/save', entity);
 }

}

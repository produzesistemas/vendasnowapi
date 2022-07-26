import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GenericHttpService } from './genericHttpService';
import { Client } from '../_model/client-model';

@Injectable({ providedIn: 'root' })

export class ClientService extends GenericHttpService<Client> {
    constructor(private http: HttpClient) {
        super(http);
    }

    getAll() {
        return this.http.get<Client[]>(`${this.getUrlApi()}Client/getAll`);
    }

    deleteById(entity) {
        return this.post('Client/delete', entity);
  }

  active(entity) {
    return this.post('Client/active', entity);
 }

 save(entity) {
    return this.post('Client/save', entity);
 }

}

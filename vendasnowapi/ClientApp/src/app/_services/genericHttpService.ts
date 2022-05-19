import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class GenericHttpService<T> {
  protected jsonDataToResourceFn: (jsonData: any) => any;

  constructor(
    private httpService: HttpClient
  ) { }

  protected getUrlApi() {
    return environment.urlApi;
  }

  public getAll(url: string): Observable<T[]> {
    return this.httpService.get<T[]>(`${environment.urlApi}` + url);
  }

  public get(url: string): Observable<T> {
    return this.httpService.get<T>(`${environment.urlApi}` + url);
  }

  public post(url: string, body: any): Observable<T> {
    return this.httpService.post<T>(`${environment.urlApi}` + url, body);
  }

  public postAll(url: string, body: any): Observable<T[]> {
    return this.httpService.post<T[]>(`${environment.urlApi}` + url, body);
  }

  public delete(url: string): Observable<T> {
    return this.httpService.delete<any>(`${environment.urlApi}` + url);
  }

  protected getHeaders(json?: boolean) {
    return { headers: new HttpHeaders().set('Access-Control-Allow-Origin', '*')
    .set('Content-Type', 'application/x-www-form-urlencoded')
    .set('Authorization', 'Bearer') };
  }

  protected handleError(error: any): Observable<any> {
    console.log('Erro na requisição =>', error);
    return throwError(error);
  }

}

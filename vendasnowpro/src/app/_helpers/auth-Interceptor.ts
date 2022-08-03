import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject, of, from } from 'rxjs';
import { catchError, filter, take, switchMap, finalize } from 'rxjs/operators';
import { AuthenticationService } from '../_services/authentication.service';
import { environment } from 'src/environments/environment';
import { Storage } from '@capacitor/storage';
import { Router } from '@angular/router';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private AUTH_HEADER = 'Authorization';
  private token;
  private refreshTokenInProgress = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  private MerchantId = `${environment.merchantId}`;
  private MerchantKey = `${environment.merchantKey}`;
  public currentUser;

  constructor(private authenticationService: AuthenticationService,  private router: Router,) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return from(this.authenticationService.getCurrentUser())
    .pipe(
      switchMap(token => {
        if (token !== null) {
          req = req.clone({
            headers: req.headers.set(this.AUTH_HEADER, 'bearer ' + token.token)
          });
        }
        return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error && error.status === 401) {
          this.authenticationService.logout();
          this.router.navigateByUrl('/login', { replaceUrl: true });
        } 
        else {
          console.log(throwError(error));
          return throwError(error);
        }
      })
    );

      })
  );

  }

 
  private addHeaderCielo(request: HttpRequest<any>): HttpRequest<any> {
    return request.clone({
      headers: request.headers.set('Content-Type', 'application/json')
        .set('MerchantId', this.MerchantId)
        .set('MerchantKey', this.MerchantKey),
      params: request.params.delete('Cielo', 'true')
    });
  }

  
}

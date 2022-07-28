import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject, of, from } from 'rxjs';
import { catchError, filter, take, switchMap, finalize } from 'rxjs/operators';
import { AuthenticationService } from '../_services/authentication.service';
import { environment } from 'src/environments/environment';
import { Storage } from '@capacitor/storage';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private AUTH_HEADER = 'Authorization';
  private token;
  private refreshTokenInProgress = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  private MerchantId = `${environment.merchantId}`;
  private MerchantKey = `${environment.merchantKey}`;
  public currentUser;

  constructor(private authenticationService: AuthenticationService) { }

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

        } else {
          return throwError(error);
        }
      })
    );

      })
  );

    // Check if we need additional token logic or not
    // if (this.isInBlockedList(req.url)) {
    //   return next.handle(req);
    // } else {
    //   return next.handle(req).pipe(
    //     catchError((error: HttpErrorResponse) => {
    //       if (error && error.status === 401) {
    //         // 401 errors are most likely going to be because we have an expired token that we need to refresh.
    //         if (this.refreshTokenInProgress) {
    //           // If refreshTokenInProgress is true, we will wait until refreshTokenSubject has a non-null value
    //           // which means the new token is ready and we can retry the request again
    //           return this.refreshTokenSubject.pipe(
    //             filter(result => result !== null),
    //             take(1),
    //             switchMap(() => next.handle(this.addAuthenticationTokenMaster(req)))
    //           );
    //         } else {
    //           this.refreshTokenInProgress = true;

    //           // Set the refreshTokenSubject to null so that subsequent API calls will wait until the new token has been retrieved
    //           this.refreshTokenSubject.next(null);

    //           return this.refreshAccessToken().pipe(
    //             switchMap((success: boolean) => {
    //               this.refreshTokenSubject.next(success);
    //               return next.handle(this.addAuthenticationTokenMaster(req));
    //             }),
    //             // When the call to refreshToken completes we reset the refreshTokenInProgress to false
    //             // for the next time the token needs to be refreshed
    //             finalize(() => this.refreshTokenInProgress = false)
    //           );
    //         }
    //       } else {
    //         return throwError(error);
    //       }
    //     })
    //   );
    // }


    // this.authenticationService.getObject().then((data: any) => {
    //   if (data !== null) {
    //     // this.currentUser = data;
    //     // this.token = this.currentUser.token;
    //   }


    // });

    // this.currentUser = this.authenticationService.currentUser;
    // if (req.params.get('Cielo') === 'true') {
    //   req = this.addHeaderCielo(req);
    // } else {
      // req = this.addAuthenticationTokenMaster(req);
    // }

    // if (req.params.get('Master') === 'true') {
    //   req.params['map'].delete('Master');
    //   req = this.addAuthenticationTokenMaster(req);
    // }

    // return next.handle(req).pipe(
    //   catchError((error: HttpErrorResponse) => {
    //     if (error && error.status === 401) {
    //       // 401 errors are most likely going to be because we have an expired token that we need to refresh.
    //       if (this.refreshTokenInProgress) {
    //         // If refreshTokenInProgress is true, we will wait until refreshTokenSubject has a non-null value
    //         // which means the new token is ready and we can retry the request again
    //         return this.refreshTokenSubject.pipe(
    //           filter(result => result !== null),
    //           take(1),
    //           switchMap(() => next.handle(this.addAuthenticationTokenMaster(req)))
    //         );
    //       } else {
    //         this.refreshTokenInProgress = true;

    //         // Set the refreshTokenSubject to null so that subsequent API calls will wait until the new token has been retrieved
    //         this.refreshTokenSubject.next(null);

    //         return this.refreshAccessToken().pipe(
    //           switchMap((success: boolean) => {
    //             this.refreshTokenSubject.next(success);
    //             return next.handle(this.addAuthenticationTokenMaster(req));
    //           }),
    //           // When the call to refreshToken completes we reset the refreshTokenInProgress to false
    //           // for the next time the token needs to be refreshed
    //           finalize(() => this.refreshTokenInProgress = false)
    //         );
    //       }
    //     } else {
    //       return throwError(error);
    //     }
    //   })
    // );
  }

  private refreshAccessToken(): Observable<any> {
    return of("secret token");
  }

  private addAuthenticationTokenMaster(request: HttpRequest<any>): HttpRequest<any> {

    // const currentUser = this.authenticationService.getObject();
    // if (currentUser) {
    //     this.token = currentUser.then(c => {
    //       c.value.token;
    //     })
    // } 
    // else {
    //   if (this.authenticationService.getCurrentUser()){
    //     this.token = this.authenticationService.getCurrentUser().token;

    //   }
    // }

    // this.loadUser().then((data: any) => {
    //     if (data !== null) {
    //       this.currentUser = data;
    //       this.token = this.currentUser.token;
    //     }
    //   });
    // if (this.currentUser !== null) {
    //   this.token = this.currentUser.token;
    // }
    // this.authenticationService.getObject().then((data: any) => {
    //   if (data !== null) {
    //     this.currentUser = data;
    //     this.token = this.currentUser.token;
    //   }
    // });
    // const token = this.authenticationService.getObject();
    // this.token = this.currentUser.token;
    this.currentUser = this.authenticationService.currentAccessToken;
    if (this.currentUser === undefined) {
      return request;
    }
    return request.clone({
      headers: request.headers.set(this.AUTH_HEADER, 'bearer ' + this.currentUser.token)
    });
  }

  private addHeaderCielo(request: HttpRequest<any>): HttpRequest<any> {
    return request.clone({
      headers: request.headers.set('Content-Type', 'application/json')
        .set('MerchantId', this.MerchantId)
        .set('MerchantKey', this.MerchantKey),
      params: request.params.delete('Cielo', 'true')
    });
  }

  async getObject() {
    let strObject = await Storage.get({ key: 'vendasnow_user' });
    if (strObject) {
      return JSON.parse(strObject.value)
    }
    return null
  }

  // Filter out URLs where you don't want to add the token!
  private isInBlockedList(url: string): Boolean {
    // Example: Filter out our login and logout API call
    if (url == `${environment.urlApi}/auth` ||
      url == `${environment.urlApi}/auth/logout`) {
      return true;
    } else {
      return false;
    }
  }

  // Add our current access token from the service if present
  private addToken(req: HttpRequest<any>) {
    if (this.authenticationService.currentAccessToken) {
      return req.clone({
        headers: req.headers.set(this.AUTH_HEADER, 'bearer ' + this.authenticationService.currentAccessToken)
      });
    } else {
      return req;
    }
  }

}

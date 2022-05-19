import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
// import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
    private countSpinner: number;

    constructor(
        // private authenticationService: AuthenticationService,
        private toastr: ToastrService,
        private spinner: NgxSpinnerService) {
        this.countSpinner = 0;
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.countSpinner++;
        this.spinner.show();
        return next.handle(request).pipe(
            finalize(() => {
                this.countSpinner--;
                if (this.countSpinner === 0) {
                    this.spinner.hide();
                }
            }),
            catchError(err => {
                const msgDefault = 'Falha na conexão, tente novamente.';
                if (err.status === 401) { 
                    this.toastr.error('Acesso negado.', 'Atenção!');
                    // this.authenticationService.logout();
                    localStorage.clear();
                    location.reload();
                } else if (err.status === 400) { 
                    if (typeof(err.error) == "string") {
                        this.toastr.error(err.error.toString(), 'Atenção!');
                    }

                    if (typeof(err.error) == "object") {
                        var enc = new TextDecoder("utf-8");
                        const enco = enc.decode(err.error);
                        this.toastr.error(enco.toString(), 'Atenção!');
                    }

                } else if (err.status === 403) {
                    this.toastr.error('Acesso negado.', 'Atenção!');
                } else {
                    this.toastr.error(msgDefault, 'Atenção!');
                }

                return throwError(err);
            }
            ));
    }
}

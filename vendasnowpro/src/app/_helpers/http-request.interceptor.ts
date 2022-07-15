import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from '../_services/authentication.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
    private countSpinner: number;

    constructor(
        private authenticationService: AuthenticationService,
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
                }

                if (err.status === 400) {
                    console.log(err);
                    if (Array.isArray(err.error)) {
                        if (err.url === environment.urlSandboxRequisicaoCielo) {
                            switch(err.error[0].Code) {
                                case 126:
                                    this.toastr.error('Data de expiração do cartão de crédito é inválida', 'Atenção!');
                                break;
                                case 999:
                                    this.toastr.error('Problemas na identificação do cartão', 'Atenção!');
                                break;
                                case 104:
                                    this.toastr.error('Cliente é obrigatório', 'Atenção!');
                                break;
                                case 105:
                                    this.toastr.error('Cliente é obrigatório', 'Atenção!');
                                break;
                                case 146:
                                    this.toastr.error('Código de segurança é inválido', 'Atenção!');
                                break;
                                case 129:
                                    this.toastr.error('Meio de pagamento não vinculado a loja ou Provider inválido', 'Atenção!');
                                break;
                                default:
                                    this.toastr.error('Código do erro ' + err.error[0].Code , 'Atenção!');
                                break;
                            }
                        }
                    } else {
                        this.toastr.error(err.error, 'Atenção!');
                    }

                }

                if (err.status === 500) {
                    this.toastr.error(err.error, 'Atenção!');
                }

                if (err.status === 403) {
                    this.toastr.error('Acesso negado.', 'Atenção!');
                }

                return throwError(err);
            }
            ));
    }
}

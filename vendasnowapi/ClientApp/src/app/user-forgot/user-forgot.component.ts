import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';
import { LoginUser } from '../_models/login-user-model';

@Component({
    selector: 'app-user-forgot',
    templateUrl: 'user-forgot.component.html'
})
export class UserForgotComponent implements OnInit {
    public hasResult: boolean = false;
    public msg: string;
    public classAlert: string;

    constructor(
        private route: ActivatedRoute,
        public authService: AuthenticationService
    ) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params.code == undefined || params.userid == undefined) {
                this.hasResult = false;
                this.msg = "Acesso inválido.";
                this.classAlert = "alert-danger";
            } else {
                let loginUser = new LoginUser();
                loginUser.code = params.code;
                loginUser.applicationUserId = params.userid;

                this.authService.confirmUserForgot(loginUser).subscribe(result => {
                    this.hasResult = true;
                    this.msg = "Usuário confirmado com sucesso. Faça login no app!";
                    this.classAlert = "alert-success";
                }, err => {
                    this.hasResult = true;
                    this.msg = (err && err.error && err.error.msg) ? err.error.msg : 'Acesso inválido.', 'Atenção!';
                    this.classAlert = "alert-danger";
                });
            }
        });
    }
}

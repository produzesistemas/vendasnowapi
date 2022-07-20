import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginUser } from '../_model/login-user-model';
import { AuthenticationService } from '../_services/authentication.service';
import { AlertController, ToastController } from '@ionic/angular';
import { ToastService } from '../_services/toast.service';
import { IonLoadingService } from '../_services/ion-loading.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit {
  public loginUser: LoginUser = new LoginUser();
  form: FormGroup;
  formRegister: FormGroup;
  formForgot: FormGroup;
  public submitted = false;
  public submittedRegister = false;
  public submittedForgot = false;
  public isRegister = false;
  public isLogin = true;
  public isForgot = false;
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    public toastController: ToastController,
    private ionLoaderService: IonLoadingService,
    private formBuilder: FormBuilder
    ) {}

    get f() { return this.form.controls; }
    get fr() { return this.formRegister.controls; }
    get ff() { return this.formForgot.controls; }

    ngOnInit() {
      // if (this.authenticationService.getCurrentUser()) {
      //     this.router.navigate(['partner-area']);
      // }
      this.form = this.formBuilder.group({
          email: ['', Validators.required],
          secret: ['', Validators.required]
      });

      this.formRegister = this.formBuilder.group({
        email: ['', Validators.required],
        secret: ['', Validators.required]
    });

    this.formForgot = this.formBuilder.group({
      email: ['', Validators.required],
      secret: ['', Validators.required]
  });


  }

  async presentToast(error: string){
    const toast = await this.toastController.create({
      message: error,
      duration: 2000,
      position: 'middle'
    });

    toast.present();
  }

  onLogin() {
    this.submitted = true;
    if (this.form.invalid) {
        return;
    }
    this.loginUser.email = this.form.controls.email.value;
    this.loginUser.secret = this.form.controls.secret.value;
    this.ionLoaderService.simpleLoader().then(()=>{
    this.authenticationService.login(this.loginUser)
    .subscribe(result => {
        this.authenticationService.clearUser();
        this.authenticationService.addCurrentUser(result);
        this.ionLoaderService.dismissLoader();
        return this.router.navigate(['/main']);
    });
  });
}



onRegister() {
  this.submittedRegister = true;
  if (this.formRegister.invalid) {
      return;
  }
  this.loginUser.email = this.formRegister.controls.email.value;
  this.loginUser.secret = this.formRegister.controls.secret.value;
  this.ionLoaderService.simpleLoader().then(()=>{
  this.authenticationService.register(this.loginUser)
  .subscribe(result => {
    this.ionLoaderService.dismissLoader();
    this.presentToast('Verifique sua caixa de email e clique no link para validar sua conta!');
  });
});
}

register() {
  this.isLogin = false;
  this.isForgot = false;
  this.isRegister = true;
}

backLogin() {
  this.isLogin = true;
  this.isForgot = false;
  this.isRegister = false;
}

}

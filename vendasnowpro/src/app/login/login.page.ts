import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { LoginUser } from '../_model/login-user-model';
import { AuthenticationService } from '../_services/authentication.service';
import { IonLoadingService } from '../_services/ion-loading.service';

@Component({
  selector: 'app-login',
  templateUrl: 'login.page.html',
  styleUrls: ['login.page.scss'],
})
export class LoginPage {

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
  currentUser;
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
      this.authenticationService.getCurrentUser().then((data: any) => {
        if (data !== null) {
          this.router.navigateByUrl('/main', { replaceUrl: true });
        }
      });
    
      this.form = this.formBuilder.group({
          email: ['', Validators.required],
          secret: ['', Validators.required]
      });

      this.formRegister = this.formBuilder.group({
        email: ['', Validators.required],
        userName: ['', Validators.required],
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
        this.authenticationService.clear();
        this.authenticationService.setObject(result);
        this.ionLoaderService.dismissLoader();
        this.router.navigateByUrl('/main', { replaceUrl: true });
        // return this.router.navigate(['/main']);
    });
  });
}



onRegister() {
  this.submittedRegister = true;
  if (this.formRegister.invalid) {
      return;
  }
  this.loginUser.email = this.formRegister.controls.email.value;
  this.loginUser.userName = this.formRegister.controls.userName.value;
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
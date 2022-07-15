import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginUser } from '../_model/login-user-model';
import { AuthenticationService } from '../_services/authentication.service';


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit {
  public loginUser: LoginUser = new LoginUser();
  form: FormGroup;
  public submitted = false;
  constructor(    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder) {}

    get f() { return this.form.controls; }

    ngOnInit() {
      // if (this.authenticationService.getCurrentUser()) {
      //     this.router.navigate(['partner-area']);
      // }
      this.form = this.formBuilder.group({
          email: ['', Validators.required],
          secret: ['', Validators.required]
      });
  }

  onLogin() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
  }

}

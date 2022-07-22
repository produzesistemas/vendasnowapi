import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../_services/authentication.service';
import { ToastController } from '@ionic/angular';
import { IonLoadingService } from '../_services/ion-loading.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-first-page',
  templateUrl: 'first-page.page.html',
  styleUrls: ['first-page.page.scss'],
})
export class FirstPage implements OnInit {

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    public toastController: ToastController,
    private ionLoaderService: IonLoadingService,
    private formBuilder: FormBuilder
  ) { }


  ngOnInit() {

  }

  
}

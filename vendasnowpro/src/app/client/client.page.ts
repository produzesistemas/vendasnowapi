import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { AuthenticationService } from '../_services/authentication.service';
import { IonLoadingService } from '../_services/ion-loading.service';
import { ClientService } from '../_services/client.service';
import { FilterDefaultModel } from '../_model/filter-default-model';

@Component({
  selector: 'app-client',
  templateUrl: 'client.page.html',
  styleUrls: ['client.page.scss'],
})
export class ClientPage {

  currentUser;
  public clients = [];
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    public toastController: ToastController,
    public clientService: ClientService,
    private ionLoaderService: IonLoadingService,
    private formBuilder: FormBuilder
    ) {}

    ngOnInit() {
      // this.authenticationService.getObject().then((data: any) => {
      //   if (data !== null) {
      //     this.currentUser = data;
      //   } else {
      //       this.router.navigateByUrl('/login', { replaceUrl: true });
      //   }
      // });

      const filter: FilterDefaultModel = new FilterDefaultModel();
      this.clientService.getByFilter(filter).subscribe(clients => {
        this.clients = clients;
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

}

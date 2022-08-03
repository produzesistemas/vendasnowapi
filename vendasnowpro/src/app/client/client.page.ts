import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IonModal, ModalController, ToastController } from '@ionic/angular';
import { AuthenticationService } from '../_services/authentication.service';
import { IonLoadingService } from '../_services/ion-loading.service';
import { ClientService } from '../_services/client.service';
import { FilterDefaultModel } from '../_model/filter-default-model';
import { startWith } from 'rxjs/operators';

@Component({
  selector: 'app-client',
  templateUrl: 'client.page.html',
  styleUrls: ['client.page.scss'],
})
export class ClientPage {
  @ViewChild(IonModal) modal: IonModal;
  currentUser;
  public clients = [];
  public allClients = [];
  form: FormGroup;
  public searchField: FormControl;
  public query: any;
  public submitted = false;
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    public toastController: ToastController,
    public clientService: ClientService,
    private ionLoaderService: IonLoadingService,
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder
    ) {}

    get f() { return this.form.controls; }

    ngOnInit() {
      // this.authenticationService.getObject().then((data: any) => {
      //   if (data !== null) {
      //     this.currentUser = data;
      //   } else {
      //       this.router.navigateByUrl('/login', { replaceUrl: true });
      //   }
      // });

      this.form = this.formBuilder.group({
        name: ['', Validators.required],
        telephone: ['']
    });

      const filter: FilterDefaultModel = new FilterDefaultModel();
      this.ionLoaderService.simpleLoader().then(()=>{
      this.clientService.getByFilter(filter).subscribe(clients => {
        this.clients = clients;
        this.allClients = clients;        
        this.ionLoaderService.dismissLoader();
      });
    });

    
  }

  onInput(event) {
    const query = event.target.value.toLowerCase();
    this.clients = this.allClients.filter((val) =>
      val.name.toLowerCase().includes(query)
    );
  }

  onCancel(event) {
    this.clients = [...this.allClients];
  }


  async presentToast(error: string){
    const toast = await this.toastController.create({
      message: error,
      duration: 2000,
      position: 'middle'
    });

    toast.present();
  }

  closeModal() {
    this.modal.dismiss(null, 'cancel');
  }

  async openModal() {
    this.modal.present();
  }

  confirm() {
    this.submitted = true;
    if (this.form.invalid) {
        return;
    }
    this.modal.dismiss(null, 'confirm');
  }


}

import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IonModal, ModalController, ToastController } from '@ionic/angular';
import { AuthenticationService } from '../_services/authentication.service';
import { IonLoadingService } from '../_services/ion-loading.service';
import { ClientService } from '../_services/client.service';
import { FilterDefaultModel } from '../_model/filter-default-model';
import { Client } from '../_model/client-model';
import { Network } from '@capacitor/network';
import { PluginListenerHandle } from '@capacitor/core';
@Component({
  selector: 'app-client',
  templateUrl: 'client.page.html',
  styleUrls: ['client.page.scss'],
})
export class ClientPage {
  @ViewChild(IonModal) modal: IonModal;
  currentUser: any;
  public clients = [];
  public client: Client = new Client();
  public allClients = [];
  form: FormGroup;
  public searchField: FormControl;
  public query: any;
  public submitted = false;
  networkStatus: any;
  networkListener: PluginListenerHandle;
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    public toastController: ToastController,
    public clientService: ClientService,
    private ionLoaderService: IonLoadingService,
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder
  ) { }

  get f() { return this.form.controls; }

  async ngOnInit() {
    this.currentUser = await this.authenticationService.getCurrentUser();
    if (this.currentUser === null) {
      this.router.navigateByUrl('/login', { replaceUrl: true });
    }
    this.networkStatus = await Network.getStatus();
    if (this.networkStatus.connected === false) {
      return this.presentToast("Dispositivo sem internet. Verifique a conexão e tente novamente.");
    }

    this.form = this.formBuilder.group({
      id: [''],
      name: ['', Validators.required]
    });

    this.load();
  }

  load() {
    const filter: FilterDefaultModel = new FilterDefaultModel();
    this.ionLoaderService.simpleLoader().then(() => {
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


  async presentToast(error: string) {
    const toast = await this.toastController.create({
      message: error,
      duration: 2000,
      position: 'top'
    });

    toast.present();
  }

  closeModal() {
    this.modal.dismiss(null, 'cancel');
  }

  async openModal() {
    this.modal.present();
  }

  async detail(client) {
    this.form.controls.id.setValue(client.id);
    this.form.controls.name.setValue(client.name);
    this.modal.present();
  }

  confirm() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    this.client.id = Number(this.form.controls.id.value);
    this.client.name = this.form.controls.name.value;
    this.ionLoaderService.simpleLoader().then(() => {
      this.clientService.save(this.client).subscribe(clients => {
        this.ionLoaderService.dismissLoader();
        this.modal.dismiss(null, 'confirm');
        this.load();
      });
    });
  }


}

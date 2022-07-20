import { Injectable } from '@angular/core';
import { LoadingController } from '@ionic/angular';
@Injectable({
  providedIn: 'root'
})
export class IonLoadingService {
    isShowingLoader = false;
  loader: any;
  constructor(public loadingController: LoadingController) { }

  // Simple loader
  async simpleLoader() {
    if (!this.isShowingLoader) {
        this.isShowingLoader = true
        this.loader = await this.loadingController.create({
          message: 'Aguarde',
          duration: 4000,
          spinner: "bubbles"
        });
        return await this.loader.present();
      }
  }
  // Dismiss loader
  async dismissLoader() {
    if (this.loader) {
        this.loader.dismiss()
        this.loader = null
        this.isShowingLoader = false
      }
  }
  // Auto hide show loader
  autoLoader() {
    this.loadingController.create({
      message: 'Loader hides after 4 seconds',
      duration: 4000
    }).then((response) => {
      response.present();
      response.onDidDismiss().then((response) => {
        console.log('Loader dismissed', response);
      });
    });
  }   
  // Custom style + hide on tap loader
  customLoader() {
    this.loadingController.create({
      message: 'Loader with custom style',
      duration: 4000,
      cssClass:'loader-css-class',
      backdropDismiss:true
    }).then((res) => {
      res.present();
    });
  }   
}
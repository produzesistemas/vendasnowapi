import { Platform, ToastController } from '@ionic/angular';

export class ToastService {

    constructor(public toastController: ToastController) { }

    async presentToast(error){
        const toast = await this.toastController.create({
          message: error,
          duration: 2000,
          position: 'middle'
        });
    
        toast.present();
      }

}
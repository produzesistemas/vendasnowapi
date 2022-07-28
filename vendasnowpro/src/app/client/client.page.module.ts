import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ClientPage } from './client.page';

import { ClientPageRoutingModule } from './client.page-routing.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    ClientPageRoutingModule
  ],
  declarations: [ClientPage]
})
export class ClientPageModule {}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FirstPage } from './first-page.page';
import { FirstPageRoutingModule } from './first-page-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IonicModule,
    FirstPageRoutingModule
  ],
  declarations: [FirstPage],
  exports: [ 
    FormsModule,
    ReactiveFormsModule ]
})
export class FirstPageModule {}

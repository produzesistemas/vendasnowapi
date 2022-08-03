import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { MainPageRoutingModule } from './main-routing.module';

import { MainPage } from './main.page';
// import { AuthInterceptor } from '../../app/_helpers/auth-Interceptor';
// import { HttpRequestInterceptor } from '../../app/_helpers/http-request.interceptor';
// import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
// import { RouteReuseStrategy } from '@angular/router';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    MainPageRoutingModule,

    // HttpClientModule
  ],
  // providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
  //   { provide: HTTP_INTERCEPTORS, useClass: HttpRequestInterceptor, multi: true },
  //   { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },],
  declarations: [MainPage]
})
export class MainPageModule {}

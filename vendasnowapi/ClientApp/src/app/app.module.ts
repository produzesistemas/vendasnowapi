import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthInterceptor } from '../app/_interceptor/auth-Interceptor';
import { HttpRequestInterceptor } from '../app/_interceptor/http-request.interceptor';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppLayoutComponent } from './_layouts/app-layout/app-layout.component';
import { AppHeaderComponent } from './_layouts/app-header/app-header.component';
import { DefaultLayoutComponent } from './_layouts/default-layout/default-layout.component';
import { DefaultHeaderComponent } from './_layouts/default-header/default-header.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { PermissionDirective } from './_directives/permission.directive';
registerLocaleData(ptBr);
export const options: Partial<IConfig> | (() => Partial<IConfig>) = null;


@NgModule({
  declarations: [
    AppComponent,
    AppLayoutComponent,
    AppHeaderComponent,
    DefaultLayoutComponent,
    DefaultHeaderComponent,
    PermissionDirective    
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgxSpinnerModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule.forRoot(),
    NgxMaskModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpRequestInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  exports: [
    PermissionDirective
  ]
})
export class AppModule { }

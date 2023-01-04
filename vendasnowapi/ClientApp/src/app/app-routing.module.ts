import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DefaultLayoutComponent } from 'src/app/_layouts/default-layout/default-layout.component';
import { DefaultModule } from 'src/app/default/default.module';
import { QuemSomosModule } from 'src/app/quem-somos/quem-somos.module';
import { AccessDeniedComponent } from 'src/app/access-denied/access-denied.component';
import { PoliticaPrivacidadeModule } from './politica-privacidade/politica-privacidade.module';
import { UserModule } from './user/user.module';
import { UserForgotModule } from './user-forgot/user-forgot.module';

const routes: Routes = [
  {
    path: '',
    component: DefaultLayoutComponent,
    children: [
      { path: '', redirectTo: 'index', pathMatch: 'full'},
      { path: 'index', loadChildren: () => DefaultModule },
      { path: 'quem-somos', loadChildren: () => QuemSomosModule },
      { path: 'privacidade/vendasnow', loadChildren: () => PoliticaPrivacidadeModule },
      { path: 'user/confirm', loadChildren: () => UserModule },
      { path: 'user-forgot/confirm', loadChildren: () => UserForgotModule },
    ]
  },
  {
    path: 'access-denied',
    component: AccessDeniedComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

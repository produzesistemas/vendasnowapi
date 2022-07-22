import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FirstPage } from './first-page.page';

const routes: Routes = [
  {
    path: '',
    component: FirstPage,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FirstPageRoutingModule {}

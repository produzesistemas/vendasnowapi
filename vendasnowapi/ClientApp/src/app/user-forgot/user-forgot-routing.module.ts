import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserForgotComponent } from './user-forgot.component';

const routes: Routes = [
    {
        path: '',
        component: UserForgotComponent
    },
    {
        path: ':userid/:code',
        component: UserForgotComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserForgotRoutingModule { }
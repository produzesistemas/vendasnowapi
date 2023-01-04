import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared.module';
import { UserForgotRoutingModule } from './user-forgot-routing.module';
import { UserForgotComponent } from './user-forgot.component';

@NgModule({
    imports: [
        CommonModule,
        UserForgotRoutingModule,
        SharedModule
    ],
    declarations: [UserForgotComponent],
    entryComponents: []
})
export class UserForgotModule {}
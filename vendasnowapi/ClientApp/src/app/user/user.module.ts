import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared.module';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';

@NgModule({
    imports: [
        CommonModule,
        UserRoutingModule,
        SharedModule
    ],
    declarations: [UserComponent],
    entryComponents: []
})
export class UserModule {}
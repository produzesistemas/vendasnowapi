import { NgModule } from '@angular/core';
import { DefaultComponent } from './default.component';
import { DefaultRoutingModule} from '../default/default-routing.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared.module';
@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        DefaultRoutingModule
      ],
    declarations: [
        DefaultComponent
    ],
    exports: [ DefaultComponent ]
})
export class DefaultModule { }

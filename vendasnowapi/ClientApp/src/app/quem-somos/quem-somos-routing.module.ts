import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuemSomosComponent } from './quem-somos.component';

const routes: Routes = [
    {
        path: '',
        component: QuemSomosComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuemSomosRoutingModule { }

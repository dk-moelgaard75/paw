

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PawEmployeeComponent } from './paw-employeeform/paw-employeeform.component';
import { PawCustomerformComponent } from './paw-customerform/paw-customerform.component';
import { PawHomeComponent } from './paw-home/paw-home.component';
import { PawLogonComponent } from './paw-logon/paw-logon.component';

const routes: Routes = [
  {path : 'logon', component: PawLogonComponent},
  {path : 'home', component: PawHomeComponent},
  {path : 'employee', component: PawEmployeeComponent},
  {path : 'customer', component: PawCustomerformComponent},
  {path: '', redirectTo: 'home', pathMatch: 'full' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

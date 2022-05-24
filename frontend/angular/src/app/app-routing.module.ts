import { PawTimeregistrationComponent } from './paw-timeregistration/paw-timeregistration.component';


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PawEmployeeComponent } from './paw-employeeform/paw-employeeform.component';
import { PawCustomerformComponent } from './paw-customerform/paw-customerform.component';
import { PawHomeComponent } from './paw-home/paw-home.component';
import { PawLogonComponent } from './paw-logon/paw-logon.component';
import { PawNoAccessComponent } from './paw-no-access/paw-no-access.component';
import { PawTaskformComponent } from './paw-taskform/paw-taskform.component';
import { AuthGuard } from './shared/auth.guard';
import { PawTasklistComponent } from './paw-tasklist/paw-tasklist.component';

const routes: Routes = [
  {path : 'logon', component: PawLogonComponent},
  {path : 'home', component: PawHomeComponent,  canActivate:[AuthGuard]},
  {path : 'employee', component: PawEmployeeComponent,  canActivate:[AuthGuard]},
  {path : 'customer', component: PawCustomerformComponent,  canActivate:[AuthGuard]},
  {path : 'task', component: PawTaskformComponent,  canActivate:[AuthGuard]},
  {path : 'no-access', component: PawNoAccessComponent,  canActivate:[AuthGuard]},
  {path : 'tasklist', component: PawTasklistComponent,  canActivate:[AuthGuard]},
  {path : 'timeregistration/:id', component: PawTimeregistrationComponent,  canActivate:[AuthGuard]},
  {path: '', redirectTo: 'logon', pathMatch: 'full' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

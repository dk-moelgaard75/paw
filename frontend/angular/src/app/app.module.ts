import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';  // <<<< import it here

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PawNavbarComponent } from './paw-navbar/paw-navbar.component';
import { PawCustomerformComponent } from './paw-customerform/paw-customerform.component';
import { PawEmployeeComponent } from './paw-employeeform/paw-employeeform.component';
import { PawLogonComponent } from './paw-logon/paw-logon.component';
import { PawHomeComponent } from './paw-home/paw-home.component';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PawTaskformComponent } from './paw-taskform/paw-taskform.component';
import { PawNoAccessComponent } from './paw-no-access/paw-no-access.component';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { PawTasklistComponent } from './paw-tasklist/paw-tasklist.component';

@NgModule({
  declarations: [
    AppComponent,
    PawNavbarComponent,
    PawCustomerformComponent,
    PawEmployeeComponent,
    PawLogonComponent,
    PawHomeComponent,
    PawTaskformComponent,
    PawNoAccessComponent,
    PawTasklistComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
     JwtHelperService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }

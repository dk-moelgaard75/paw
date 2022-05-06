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
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { PawTaskformComponent } from './paw-taskform/paw-taskform.component';

@NgModule({
  declarations: [
    AppComponent,
    PawNavbarComponent,
    PawCustomerformComponent,
    PawEmployeeComponent,
    PawLogonComponent,
    PawHomeComponent,
    PawTaskformComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

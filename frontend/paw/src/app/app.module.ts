import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PawNavbarComponent } from './paw-navbar/paw-navbar.component';
import { PawCustomerformComponent } from './paw-customerform/paw-customerform.component';
import { PawEmployeeformComponent } from './paw-employeeform/paw-employeeform.component';

@NgModule({
  declarations: [
    AppComponent,
    PawNavbarComponent,
    PawCustomerformComponent,
    PawEmployeeformComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

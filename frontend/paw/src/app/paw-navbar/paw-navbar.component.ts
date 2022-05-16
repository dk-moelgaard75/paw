import { ITaskObj } from './../shared/ITaskObj';
import { AuthenticationService } from './../services/authentication.service';
import { SharedService } from './../services/shared.service';
import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { PawCustomerformComponent } from '../paw-customerform/paw-customerform.component';
import { JwtHelperService  } from '@auth0/angular-jwt';
import { Router } from '@angular/router';


@Component({
  selector: 'app-paw-navbar',
  templateUrl: './paw-navbar.component.html',
  styleUrls: ['./paw-navbar.component.css']
})
export class PawNavbarComponent implements OnInit {
  public userName: string = '';
  private userRole: string = '';
  private _isAdmin: boolean = false;
  private apiToken: string | null;

  constructor(private jwtHelper: JwtHelperService, 
            private authenticationService: AuthenticationService,
            private router: Router ) { 
    this.apiToken = localStorage.getItem('token');
    if (this.apiToken != null)
    {
      const tokenData = this.jwtHelper.decodeToken(this.apiToken);
      console.log("unique_name: " + tokenData.unique_name);
      this.userName = tokenData.given_name
      this.userRole = tokenData.role
    }
  }

  ngOnInit(): void {
  }
  currentRole() {
    return this.userRole;
  }
  isAdmin() {
    return this.userRole === 'admin';
  }
  isEmployee() {
    return this.userRole === 'employee';
  }
    
  
  logOut()  {
    this.authenticationService.logout();
    this.router.navigate(['/logon']);
  }
}

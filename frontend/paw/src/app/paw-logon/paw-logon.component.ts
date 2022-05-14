import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './../services/authentication.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-paw-logon',
  templateUrl: './paw-logon.component.html',
  styleUrls: ['./paw-logon.component.css']
})
export class PawLogonComponent implements OnInit {
  invalidLogin: boolean = true;
  
  constructor(   
    private router: Router, 
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
  }
  signIn(credentials) {
    this.authenticationService.login(credentials)
      .subscribe(result => { 
        if (result)
          this.router.navigate(['/']);
        else  
          this.invalidLogin = true; 
      });
  }
}

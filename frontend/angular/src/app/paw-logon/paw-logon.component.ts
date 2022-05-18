import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './../services/authentication.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { first, Subscription } from 'rxjs';
import { SharedService } from '../services/shared.service';


@Component({
  selector: 'app-paw-logon',
  templateUrl: './paw-logon.component.html',
  styleUrls: ['./paw-logon.component.css']
})
export class PawLogonComponent implements OnInit {
  loginForm = new FormGroup({
    username: new FormControl('',Validators.required),
    password: new FormControl('', Validators.required),
  });
  public loading: boolean = false;
  public error: string = '';
  public submitted: boolean = false;
  private pawCredential: string = '';
  private returnUrl: string = '/home';
  logoutEventSubscription: Subscription;

  constructor(
    private router: Router, 
    private authService: AuthenticationService,
    private sharedService: SharedService) { 
      this.logoutEventSubscription = this.sharedService.getLogOutEvent().subscribe(() => {
        console.log("paw-logon called")  
        this.logOut();
      });
 
    }
  
    ngOnInit(): void {
  }
  get username() {
    return this.loginForm.get('username')!;
  }

  get password() {
    return this.loginForm.get('password')!;
  }


  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
        return;
    }
  
    
    //this.loginForm.controls
    this.loading = true;
    this.pawCredential = "{\"email\" : \"" + this.username.value + "\", \"password\" : \"" + this.password.value +"\"}";
    console.log(this.pawCredential);
    this.authService.login(this.pawCredential)
        .pipe(first())
        .subscribe({
          complete: ( ) => { 
            this.router.navigate([this.returnUrl]);
          },
          error: (err: any) => { 
              
              this.error = 'Logon mislykkeds';
              this.loading = false;
              }
            });
  }
  
  logOut() {
    this.authService.logout
    this.router.navigate(['/logon']);
  }


}
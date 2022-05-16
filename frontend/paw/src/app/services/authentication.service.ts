
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { JwtHelperService  } from '@auth0/angular-jwt';
import { IToken } from '../shared/IToken';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private  apiURL = 'http://paw.dk/api';
    //private  apiURL = 'https://localhost:44383/api';
    currentUser: any; 
    token!: IToken; 
    
    private tokenPayload: string = '';

    constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {
      let token = localStorage.getItem('token');
      if (token) {
        this.tokenPayload = JSON.stringify(this.jwtHelper.decodeToken(token));
        console.log(this.tokenPayload);
      }
    }

    
    login(customer: string): Observable<HttpResponse<IToken>> {
      console.log("login called");
      console.log("customer:" + customer);
      return this.http
        .post<IToken>(
          this.apiURL + '/authentication/',
          customer,
          {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
        ).pipe(map(data => {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            this.token = (data.body as IToken);
            console.log('token - body:' + this.token.token);
            //this.tokenPayload = JSON.stringify(this.jwtHelper.decodeToken(user));

            localStorage.setItem('token', this.token.token);

            console.log('Decode token: ' + JSON.stringify(this.jwtHelper.decodeToken(this.token.token)));
            console.log("expired: " + this.jwtHelper.isTokenExpired(this.token.token));
            const tokenData = this.jwtHelper.decodeToken(this.token.token);
            console.log("unique_name: " + tokenData.unique_name);
            console.log("given_name: " + tokenData.given_name);
            console.log("family_name: " + tokenData.family_name);
            console.log("email: " + tokenData.email);
            console.log("role: " + tokenData.role);
            this.currentUser = tokenData.given_name
            return data; 
            
      }));
    }
    
    handleError(error: any) {
      let errorMessage = '';
      if (error.error instanceof ErrorEvent) {
        // Get client-side error
        errorMessage = error.error.message;
      } else {
        // Get server-side error
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
      window.alert(errorMessage);
      return throwError(() => {
        return errorMessage;
      });
    }
     
    logout() { 
       localStorage.removeItem('token');
       this.currentUser = null;
       
     }
   
    isLoggedIn() { 
       return this.jwtHelper.isTokenExpired('token');
     }
}
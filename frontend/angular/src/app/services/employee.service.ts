
//apiURL = 'https://localhost:44383/api'
//Based upon https://www.positronx.io/angular-httpclient-http-service/

//Get header from response: https://stackoverflow.com/questions/69738104/get-headers-from-post-response

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse,HttpEvent} from '@angular/common/http';
import { IEmployee } from '../shared/IEmployee';
import { Observable, throwError } from 'rxjs';
import { retry, catchError,map, pluck } from 'rxjs/operators';
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  
  apiURL = 'http://paw.dk/api'; 
  private apiToken: string | null;
  
  
  constructor(private http: HttpClient) {
    this.apiToken = localStorage.getItem('token');
  }
  /*========================================
    CRUD Methods for consuming RESTful API
  =========================================*/
  // Http Options
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  getEmployees(): Observable<HttpResponse<IEmployee[]>> {
    return this.http.get<IEmployee[]>(
        this.apiURL + '/employee',
        {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken}),observe: 'response'}
        )
      .pipe(retry(1), catchError(this.handleError)); 
  }
  // HttpClient API get() method => Fetch employee
  getEmployee(id: any): Observable<IEmployee> {
    return this.http
      .get<IEmployee>(this.apiURL + '/employee/' + id,
      {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken})})
      .pipe(retry(1), catchError(this.handleError));
  }
  getEmployeeByEmail(email: string): Observable<HttpResponse<IEmployee>> {
    console.log('getEmployeeByEmail:' + email);
    return this.http
      .get<IEmployee>(this.apiURL + '/verify/' + email,
      {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken}),observe: 'response'})
      .pipe(retry(1)); 
  }
  // HttpClient API post() method => Create employee
  createEmployee(employee: IEmployee): Observable<IEmployee> {
    return this.http
      .post<IEmployee>(
        this.apiURL + '/employee',
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken})}
      )
      .pipe(retry(1), catchError(this.handleError));
  }
  getEmployeesWithoutHeader() : Observable<IEmployee[]>{
    return this.http.get<IEmployee[]>(
      this.apiURL +'/employee/',
      {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken})}).
      pipe(retry(1),catchError(this.handleError));
  }



  createEmployeeWithHeaders(employee: IEmployee): Observable<HttpResponse<IEmployee>> {
    return this.http
      .post<IEmployee>(
        this.apiURL + '/employee',
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken}),observe: 'response'}
      ).pipe(catchError(this.handleError));
  }
  
  // HttpClient API put() method => Update employee
  updateEmployee(id: any, employee: IEmployee): Observable<IEmployee> {
    return this.http
      .put<IEmployee>(
        this.apiURL + '/employee/' + id,
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken})}
      )
      .pipe(retry(1), catchError(this.handleError));
  }
  updateEmployeeWithHeader(id: any, employee: IEmployee):  Observable<HttpResponse<IEmployee>> {
    return this.http
      .put<IEmployee>(
        this.apiURL + '/employee/',
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken}),observe: 'response'}
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  // HttpClient API delete() method => Delete employee
  deleteEmployee(id: any) {
    return this.http
      .delete(this.apiURL + '/employee/' + id, 
      {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken}),observe: 'response'})
      .pipe(retry(1), catchError(this.handleError));
  }
  uniqueEmailValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
        
      return this.http
        .get<IEmployee>(this.apiURL + '/verify/' + control.value,
          { headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.apiToken }), observe: 'response' }).
        pipe(
          map((exists) => (exists ? { emailExists: true } : null)),
          catchError(async (err) => null)
        );
    };
  }
  // Error handling
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
}
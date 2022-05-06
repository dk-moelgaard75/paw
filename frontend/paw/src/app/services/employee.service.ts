
//Based upon https://www.positronx.io/angular-httpclient-http-service/

//Get header from response: https://stackoverflow.com/questions/69738104/get-headers-from-post-response

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse,HttpEvent} from '@angular/common/http';
import { IEmployee } from '../shared/IEmployee';
import { Observable, throwError } from 'rxjs';
import { retry, catchError,map, pluck } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  apiURL = 'http://acme.com/api';
  
  constructor(private http: HttpClient) {}
  /*========================================
    CRUD Methods for consuming RESTful API
  =========================================*/
  // Http Options
  
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };


  // HttpClient API get() method => Fetch employeeslist
  getEmployees(): Observable<IEmployee[]> {
    return this.http
      .get<IEmployee[]>(this.apiURL + '/employee')
      .pipe(retry(1), catchError(this.handleError)); 
  }
  getEmployeesWithHeader() : Observable<HttpResponse<IEmployee[]>>{
    return this.http.get<IEmployee[]>(
      this.apiURL +'/employee/',
      {observe: 'response'}).
      pipe(retry(1),catchError(this.handleError));
  }

  // HttpClient API get() method => Fetch employee
  getEmployee(id: any): Observable<IEmployee> {
    return this.http
      .get<IEmployee>(this.apiURL + '/employee/' + id)
      .pipe(retry(1), catchError(this.handleError));
  }

  // HttpClient API post() method => Create employee
  createEmployee(employee: IEmployee): Observable<IEmployee> {
    return this.http
      .post<IEmployee>(
        this.apiURL + '/employee',
        JSON.stringify(employee),
        this.httpOptions
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  createEmployeeWithHeaders(employee: IEmployee): Observable<HttpResponse<IEmployee>> {
    return this.http
      .post<IEmployee>(
        this.apiURL + '/employee',
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
      ).pipe(catchError(this.handleError));
  }
  
  // HttpClient API put() method => Update employee
  updateEmployee(id: any, employee: IEmployee): Observable<IEmployee> {
    return this.http
      .put<IEmployee>(
        this.apiURL + '/employee/' + id,
        JSON.stringify(employee),
        this.httpOptions
      )
      .pipe(retry(1), catchError(this.handleError));
  }
  updateEmployeeWithHeader(id: any, employee: IEmployee):  Observable<HttpResponse<IEmployee>> {
    return this.http
      .put<IEmployee>(
        this.apiURL + '/employee/',
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  // HttpClient API delete() method => Delete employee
  deleteEmployee(id: any) {
    return this.http
      .delete(this.apiURL + '/employee/' + id, {observe: 'response'})
      .pipe(retry(1), catchError(this.handleError));
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
import { ITimeReg } from './../shared/ITimeReg';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, retry, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimeregService {
  private apiToken: string | null;
  apiURL = 'http://paw.dk/api'; 
  //apiURL = 'https://localhost:44383/api'

  constructor(private http:HttpClient) {
    this.apiToken = localStorage.getItem('token');
   }
  createTimereg(timereg: ITimeReg): Observable<any> {
    return this.http
      .post<ITimeReg>( 
        this.apiURL + '/timereg',
        JSON.stringify(timereg),
        {headers: new HttpHeaders({'Content-Type':  'application/json'})}
      )
      .pipe(retry(1), catchError(this.handleError));
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
  /*
  createEmployee(employee: IEmployee): Observable<IEmployee> {
    return this.http
      .post<IEmployee>(
        this.apiURL + '/employee',
        JSON.stringify(employee),
        {headers: new HttpHeaders({'Content-Type':  'application/json', 'Authorization' : 'Bearer ' + this.apiToken})}
      )
      .pipe(retry(1), catchError(this.handleError));
  }
  */
}

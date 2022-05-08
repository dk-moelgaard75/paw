
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse,HttpEvent} from '@angular/common/http';
import { ICustomer } from '../shared/ICustomer';
import { Observable, throwError } from 'rxjs';
import { retry, catchError,map, pluck } from 'rxjs/operators';
import { Icu } from '@angular/compiler/src/i18n/i18n_ast';


@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  apiURL = 'http://acme.com/api';
  //apiURL = 'https://localhost:44316/api';
  constructor(private http: HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
    // HttpClient API get() method => Fetch employees list
  getCustomers(): Observable<ICustomer[]> {
    return this.http.get<ICustomer[]>(
      this.apiURL + '/customer')
      .pipe(retry(1), catchError(this.handleError)); 
  }
  getCustomer(id: any): Observable<ICustomer> {
    return this.http
      .get<ICustomer>(this.apiURL + '/customer/' + id)
      .pipe(retry(1), catchError(this.handleError));
  }
  createCustomer(customer: ICustomer): Observable<HttpResponse<ICustomer>> {
    return this.http
      .post<ICustomer>(
        this.apiURL + '/customer/',
        JSON.stringify(customer),
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
      ).pipe(catchError(this.handleError));
  }
  updateCustomer(id: any, customer: ICustomer):  Observable<HttpResponse<ICustomer>> {
    return this.http
      .put<ICustomer>(
        this.apiURL + '/customer/',
        JSON.stringify(customer),
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
      )
      .pipe(retry(1), catchError(this.handleError));
  }
  deleteCustomer(id: any) {
    return this.http
      .delete(this.apiURL + '/customer/' + id, {observe: 'response'})
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

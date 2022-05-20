import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ITaskObj } from '../shared/ITaskObj';
import { retry, catchError,map, pluck } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskobjService {
  //apiURL = 'http://paw.dk/api';
  apiURL = 'https://localhost:44324/api';

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


  // HttpClient API get() method => Fetch takslist
  getTasks(): Observable<HttpResponse<ITaskObj[]>> {
    return this.http.get<ITaskObj[]>(
      this.apiURL + '/task/',
      {observe: 'response'})
      .pipe(retry(1), catchError(this.handleError)); 
  }
  getTask(id: any): Observable<ITaskObj> {
    return this.http
      .get<ITaskObj>(this.apiURL + '/task/' + id)
      .pipe(retry(1), catchError(this.handleError));
  }

  createTask(taskObj: ITaskObj): Observable<HttpResponse<ITaskObj>> {
    return this.http
      .post<ITaskObj>(
        this.apiURL + '/task/',
        JSON.stringify(taskObj),
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  updateTask(id: any, taskObj: ITaskObj):  Observable<HttpResponse<ITaskObj>> {
    return this.http
      .put<ITaskObj>(
        this.apiURL + '/task/',
        JSON.stringify(taskObj),
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),observe: 'response'}
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  deleteTask(id: any) {
    return this.http
      .delete(this.apiURL + '/task/' + id, {observe: 'response'})
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

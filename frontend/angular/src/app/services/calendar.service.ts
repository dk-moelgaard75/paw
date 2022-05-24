import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse,HttpEvent} from '@angular/common/http';
import { Observable, Subscriber, throwError } from 'rxjs';
import { retry, catchError,map, pluck, retryWhen, take, delay } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class CalendarService {

  apiURL = 'http://paw.dk/api'; 
  //apiURL = 'https://localhost:44360/api' 

  constructor(private http: HttpClient) { }
  
  getCalendarId(startdate: string): Observable<any> {
    console.log("getCalendarId kaldt:" +startdate);
    return this.http.post(
        this.apiURL + '/calendar/'+startdate, 
        "",
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),responseType: 'text',observe: 'response'}
      )
      .pipe(retry(0), catchError(this.handleError));
  }
  createCalendar(id : string, startdate: string):Observable<any> {
    console.log("calender URL:" + this.apiURL + '/calendar/'+id+'/'+startdate);
    return this.http.get(
        this.apiURL + '/calendar/'+id+'/'+startdate,
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),responseType: 'text',observe: 'response'}
      )
      .pipe(retry(3), catchError(this.handleError)); 
  }
  createCalendar2(id : string, startdate: string):Observable<any> {
    console.log("calender URL:" + this.apiURL + '/calendar/'+id+'/'+startdate);
    return this.http.get(
        this.apiURL + '/calendar/'+id+'/'+startdate,
        {headers: new HttpHeaders({'Content-Type':  'application/json'}),responseType: 'text',observe: 'response'}
      )
      .pipe(map(response => {
        if (response.body?.length == 0) {
          throw new Error(); // Will be caught by `map` and reemitted as an error notification.
        }
        return response;
      }),
      retryWhen(errors => errors.pipe(take(3), delay(2000))),)
  }
  /*
  map(response => {
    if (something) {
      throw new Error(); // Will be caught by `map` and reemitted as an error notification.
    }
    return response;
  }),
  retryWhen(errors => errors.pipe(take(2), delay(1000))),
).subscribe(...);
*/

  
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

import { Injectable } from '@angular/core';
import { Observable, Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private subject = new Subject<any>();

  constructor() { }
  doLogout() {
    console.log("Shared service - doLogout called");
    this.subject.next("");

  }
  getLogOutEvent() : Observable<any> {
    console.log("Shared service  - getLogOutEvent called");
    return this.subject.asObservable();
  }
}

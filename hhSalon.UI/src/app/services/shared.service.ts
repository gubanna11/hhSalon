import { Observable } from "rxjs/internal/Observable";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs/internal/Subject";

@Injectable({
  providedIn: 'root'
})

export class SharedService {
    private subject = new Subject<any>();
 
    sendData(message: any) {
        this.subject.next(message);
    }
 
    getData(): Observable<any> {
        return this.subject.asObservable();
    }
  }
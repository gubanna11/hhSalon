import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { Service } from '../models/service';


@Injectable({
  providedIn: 'root'
})
export class ServicesService {
  private url = 'Services';

  constructor(private http: HttpClient) { }

  public getServices(groupId: number): Observable<Service[]>{
    return this.http.get<Service[]>(`${environment.apiUrl}/${this.url}/${groupId}`);
  }

  public createService(service: Service): Observable<Service[]>{
   return this.http.post<Service[]>(`${environment.apiUrl}/${this.url}`, service);
  }

  public deleteService(service:Service): Observable<Service[]>{
    return this.http.delete<Service[]>(`${environment.apiUrl}/${this.url}/${service.id}`);
  }

  public updateService(service:Service): Observable<Service[]>{
    return this.http.put<Service[]>(`${environment.apiUrl}/${this.url}`, service);
  }
}

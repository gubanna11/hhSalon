import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Attendance } from '../models/attendance';

@Injectable({
  providedIn: 'root'
})
export class AttendancesService {

  private url = 'Attendances';

  constructor(private http: HttpClient) { }

  // public getServices(groupId: number): Observable<Attendace[]>{
  //   return this.http.get<Service[]>(`${environment.apiUrl}/${this.url}/${groupId}`);
  // }

  // public createAttendance(attendance: Attendance): Observable<Attendance[]>{
  //  return this.http.post<Attendance[]>(`${environment.apiUrl}/${this.url}`, attendance);
  // }

  public createAttendance(attendance: Attendance){
    return this.http.post<Attendance[]>(`${environment.apiUrl}/${this.url}`, attendance);
   }


   // MY
  public MyNotRenderedNotPaidAttendances(userId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/my-not-rendered-not-paid-attendances/${userId}`);
  }


  public MyNotRenderedIsPaidAttendances(userId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/my-not-rendered-is-paid-attendances/${userId}`);
  }

  public MyHistoryAttendances(userId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/my-history/${userId}`);
  }


  //WORKER'S

  public WorkerHistoryAttendances(workerId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/worker-history/${workerId}`);
  }

  public WorkerNotRenderedNotPaidAttendances(workerId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/worker-not-rendered-not-paid-attendances/${workerId}`);
  }
  
  public WorkerNotRenderedIsPaidAttendances(workerId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/worker-not-rendered-is-paid-attendances/${workerId}`);
  }
  

  public getFreeTimeSlots(workerId: any, date: any){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/time-slots/${workerId}/${date}`);
  }
  

  // public deleteService(service:Service): Observable<Service[]>{
  //   return this.http.delete<Service[]>(`${environment.apiUrl}/${this.url}/${service.id}`);
  // }

  // public updateService(service:Service): Observable<Service[]>{
  //   return this.http.put<Service[]>(`${environment.apiUrl}/${this.url}`, service);
  // }
}

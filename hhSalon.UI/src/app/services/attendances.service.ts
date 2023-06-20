import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Attendance } from '../models/attendance';

@Injectable({
  providedIn: 'root'
})
export class AttendancesService {

  private url = 'Attendances';

  constructor(private http: HttpClient) { }

  public createAttendance(attendance: Attendance){
    return this.http.post<Attendance>(`${environment.apiUrl}/${this.url}`, attendance);
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
  
  public WorkerNotRenderedAttendances(workerId: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/worker-not-rendered-attendances/${workerId}`);
  }

 
  
  public getFreeTimeSlots(workerId: any, date: any){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/time-slots/${workerId}/${date}`);
  }

 
  public updateAttendances(attendances: any[]){
    return this.http.put(`${environment.apiUrl}/${this.url}/update-attendances`, attendances);
  }

  public updateAttendance(attendance: any){
    return this.http.put(`${environment.apiUrl}/${this.url}`, attendance);
  }


  public deleteAttendance(id: number){
    return this.http.delete(`${environment.apiUrl}/${this.url}/${id}`)
  }
  



  public getAllAttendances(){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}/all-attendances`);
  }

  public filterAttendances(content: string){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.url}?content=${content}`);
  }
}

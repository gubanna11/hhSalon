import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Schedule } from '../models/schedule';

@Injectable({
  providedIn: 'root'
})
export class WorkersService {

  private url = 'Workers';
  constructor(private http: HttpClient) { 
  }

  public getWorkers(){
    return this.http.get<any>(`${environment.apiUrl}/${this.url}`);
  }

  public getWorkerById(workerId: string){
    return this.http.get<any>(`${environment.apiUrl}/${this.url}/info?workerId=${workerId}`);
  }


  public getWorkersByGroupId(groupId:number){
    return this.http.get<any>(`${environment.apiUrl}/${this.url}/${groupId}`);
  }

  public createWorkerSchedule(schedules:Schedule[]){    
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/schedule/create`, schedules);
  }

  public updateWorker(worker:any){    
    return this.http.put<any>(`${environment.apiUrl}/${this.url}`, worker);
  }

  public deleteWorker(workerId:string){    
    return this.http.delete<any>(`${environment.apiUrl}/${this.url}?workerId=${workerId}`,);
  }

}

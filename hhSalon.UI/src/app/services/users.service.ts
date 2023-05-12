import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Schedule } from '../models/schedule';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private url = 'Users';
  constructor(private http: HttpClient) { 
  }

  public getUsers(){
    return this.http.get<any>(`${environment.apiUrl}/${this.url}`);
  }

  public getUserById(id: string){
    return this.http.get<any>(`${environment.apiUrl}/${this.url}/${id}`);
  }


  public updateUser(user: any){
    return this.http.put<any>(`${environment.apiUrl}/${this.url}`, user);
  }
}

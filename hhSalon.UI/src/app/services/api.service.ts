import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private url = 'Auth';
  constructor(private http: HttpClient) { 
  }

  getUsers(){
    return this.http.get<any>(`${environment.apiUrl}/${this.url}`);
  }

  
}

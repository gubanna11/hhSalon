import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url = 'Auth';
  private userPayload: any;

  constructor(private http: HttpClient,
    private router: Router) {
      
      this.userPayload = this.decodedToken();
      
     }

  

  public signUp(userObj: any){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/register`, userObj);
  } 

  public signUpWithRole(userObj: any, role: string){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/register?role=${role}`, userObj);
  } 

  public signUpWorker(worker: any){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/worker-register`, worker);
  }

  
  public login(loginObj: any){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/authenticate`, loginObj);
  }

  public signOut(){
    localStorage.removeItem('token');
  }


  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue);
  }

  getToken(){
    return localStorage.getItem('token');
  }

  isLoggedIn():boolean{
    //if there is a token - true
    return !!localStorage.getItem('token');
  }
  

  decodedToken(){
    const jwtHelperService = new JwtHelperService();
    const token = this.getToken()!;

    return jwtHelperService.decodeToken(token);
  }


  getFullNameFromToken(){
    if(this.userPayload)
      return this.userPayload.unique_name;
  }

  getRoleFromToken(){
    if(this.userPayload)
      return this.userPayload.role;
  }

  getIdFromToken(){
    if(this.userPayload)
      return this.userPayload.nameid;
  }

  
}

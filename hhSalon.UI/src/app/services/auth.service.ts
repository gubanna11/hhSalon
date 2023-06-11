import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenApiModel } from '../models/token-api';
import { UserStoreService } from './user-store.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit {
  private url = 'Auth';
  private userPayload: any;


  ///////
  role: string = '';
  id: string = '';

  constructor(private http: HttpClient,
    private router: Router,
    private userStore: UserStoreService) {      
      this.userPayload = this.decodedToken();   
      
      this.userStore.getRoleFromStore().subscribe(
        roleValue => {
          const roleFromToken = this.getRoleFromToken();
          this.role = roleValue || roleFromToken;
          console.log(this.role);
          
       })  

       this.userStore.getIdFromStore().subscribe(
        idValue => {
          const idFromToken = this.getIdFromToken();
          this.id = idValue || idFromToken;          
       })  
     }


  ngOnInit(): void {

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

  storeRefreshToken(tokenValue: string){
    localStorage.setItem('refreshToken', tokenValue);
  }


  getToken(){
    return localStorage.getItem('token');
  }

  getRefreshToken(){
    return localStorage.getItem('refreshToken');
  }
  

  isLoggedIn():boolean{
    //if there is a token - true
    // if(this.userPayload)
    //   if(new Date(this.userPayload.exp * 1000) <= new Date())
    //     localStorage.removeItem('token');
    
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


  renewToken(tokenApi: TokenApiModel){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/refresh`, tokenApi);
  }
  
}

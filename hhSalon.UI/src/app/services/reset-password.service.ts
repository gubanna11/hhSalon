import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResetPassword } from '../models/reset-password';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
  private url:string = 'Auth'
  constructor(private http: HttpClient) { }

  sendResetPasswordLink(email:string){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/send-reset-email/${email}`, {});
  }

  resetPassword(resetPasswordObj: ResetPassword){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}/reset-password`, resetPasswordObj);
  }
}

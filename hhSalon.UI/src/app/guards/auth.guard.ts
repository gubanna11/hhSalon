import { Injectable } from '@angular/core';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from '../services/auth.service';
import { CanActivate, Router } from '@angular/router';
import * as toastr from 'toastr';
import { LogLevel } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService,
    private router: Router,
    public toast:NgToastService
    ){
      //toastr.options.timeOut = 30000;      
  }
  canActivate():boolean{    
    if(this.auth.isLoggedIn()){
      return true;
    }      

    
    toastr.error('Log in', 'Error', {timeOut: 5000});
    this.router.navigate(['login']);
    return false;
  }
  
}

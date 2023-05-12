import { Injectable } from '@angular/core';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from '../services/auth.service';
import { CanActivate, Router } from '@angular/router';
import * as toastr from 'toastr';

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
    if(this.auth.isLoggedIn())
      return true;

    //this.toast.error({detail: "ERROR", summary: "Log in!", duration: 5000})
    toastr.error('Log in', 'Error', {timeOut: 5000});
    this.router.navigate(['/login']);
    return false;
  }
  
}

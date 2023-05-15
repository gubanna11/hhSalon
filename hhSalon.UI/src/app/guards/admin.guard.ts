import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { UserStoreService } from '../services/user-store.service';
import * as toastr from 'toastr';;

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private auth: AuthService,
    private userStore: UserStoreService,
    private router: Router,
    ){
      //toastr.options.timeOut = 30000;      
  }
  canActivate():boolean{
    
    let role = '';
    this.userStore.getRoleFromStore().subscribe(roleVal => {
      
      const roleFromToken = this.auth.getRoleFromToken();
       role = roleVal || roleFromToken;
    })
    
    if(role !== 'Admin'){        
      toastr.error("You aren't allowed", 'Error', {timeOut: 5000});
      this.router.navigate(['login']);
      return false;
     }
     else{
      return true;
     }
  }
  
}

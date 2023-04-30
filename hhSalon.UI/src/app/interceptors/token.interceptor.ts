import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { NgToastService } from 'ng-angular-popup';
import { Router } from '@angular/router';
import { SharedService } from '../services/shared.service';
import { UserStoreService } from '../services/user-store.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(
    private auth: AuthService,
    private toast: NgToastService,
    private router: Router,
    private sharedService: SharedService,
    private userStore: UserStoreService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();
    
    if(myToken){
      request = request.clone({        
        setHeaders: {Authorization: `Bearer ${myToken}`}
      })
    }

    return next.handle(request).pipe(
      catchError((err:any) => {
        if(err instanceof HttpErrorResponse){
          if(err.status === 401){                   
            this.userStore.setRoleForStore("Client");
            
            this.toast.warning({detail: "Warning", summary: "Please Log in!"})
            this.auth.signOut();
            this.sharedService.sendData(false);
            this.router.navigate(['login']);
          }

          // if(err.status === 403){
          //   this.toast.warning({detail: "Warning", summary: "Ooops...)"})
          //   this.router.navigate(['login']);
          // }
        }

        return throwError(() => err);
      })
    );
  }
}

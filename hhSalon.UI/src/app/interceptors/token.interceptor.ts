import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { SharedService } from '../services/shared.service';
import { UserStoreService } from '../services/user-store.service';
import * as toastr from 'toastr';
import { TokenApiModel } from '../models/token-api';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(
    private auth: AuthService,
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
            //this.userStore.setRoleForStore("Client");
            // toastr.warning('Please Log in!', 'Warning');
            // this.auth.signOut();
            // this.sharedService.sendData(false);
            // this.router.navigate(['login']);

            //handle
            return this.handleUnAuthorizedError(request, next);
          }

          if(err.status === 403){
            toastr.error('You are not allowed', 'Error!');
            this.router.navigate(['login']);
          }
        }

        return throwError(() => err);
      })
    );
  }

  handleUnAuthorizedError(req: HttpRequest<any>, next: HttpHandler){
    let tokenApiModel = new TokenApiModel();

    tokenApiModel.accessToken = this.auth.getToken()!;
    tokenApiModel.refreshToken = this.auth.getRefreshToken()!;
    return this.auth.renewToken(tokenApiModel)
      .pipe(
        switchMap((data: TokenApiModel) => {
          this.auth.storeRefreshToken(data.refreshToken);
          this.auth.storeToken(data.accessToken);

          req = req.clone({        
            setHeaders: {Authorization: `Bearer ${data.accessToken}`}
          })

          return next.handle(req);
         }),
        catchError((err) => {
          
          return throwError(()=> err );

          // return throwError(()=>{
          //   //this.userStore.setRoleForStore("Client");
          //   toastr.warning('Token is expired! Please Log in again', 'Warning');
          //   // this.auth.signOut();
          //   // this.sharedService.sendData(false);
          //   this.router.navigate(['login']);
          // })

        })
      )
  }
}

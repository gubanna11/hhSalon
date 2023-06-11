import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { ResetPasswordService } from 'src/app/services/reset-password.service';
import { SharedService } from 'src/app/services/shared.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import * as toastr from 'toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";

  loginForm!: FormGroup;


  public resetPasswordEmail!: string;
  public isValidEmail!: boolean;

  constructor(private formBuilder: FormBuilder,
     private router: Router,
     private auth: AuthService,
     private sharedService: SharedService,
     private userStore: UserStoreService,
     private chatService: ChatService,
     private resetService: ResetPasswordService,
     ){

  }

  ngOnInit(): void {
    // if(this.auth.isLoggedIn()){
    //   this.toast.info({detail: "You are already logged in"});
    //   this.router.navigate(['/']);
    // }
      
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });


  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }


  onLogin(){
    if(this.loginForm.valid){
       
      //send the obj to db
      this.auth.login(this.loginForm.value)
        .subscribe({
          next:(res) => {
            
            //alert(res.message)
            this.loginForm.reset();
            this.auth.storeToken(res.accessToken);
            this.auth.storeRefreshToken(res.refreshToken);

            const tokenPayload = this.auth.decodedToken();
            
            this.userStore.setFullNameForStore(tokenPayload.unique_name);
            this.userStore.setRoleForStore(tokenPayload.role);
            this.userStore.setIdForStore(tokenPayload.nameid);
            
            toastr.success(tokenPayload.unique_name, 'Success', {timeOut: 1000});

            this.chatService.addUser(tokenPayload.nameid).subscribe(
              () => {                                
                this.chatService.userId = tokenPayload.nameid;
                this.chatService.createChatConnection(tokenPayload.nameid);
              }              
            );
            
            this.sharedService.sendData(true);
            this.router.navigate(["/"]);
          },
          error:(err) => {
            //alert(err?.error.message);       
            console.log(err.error.message);
            toastr.error(err.error.message, 'ERROR', {timeOut: 5000});
          }
        });
      
    }
    else{
      //throw the error using toaster and with required fields
      ValidateForm.validateAllFormFields(this.loginForm);
      
    }
  }

  checkValidEmail(event: string){
    const value = event;
    const pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,3}$/;
    this.isValidEmail = pattern.test(value);
    return this.isValidEmail;
  }



  confirmToSend(){
    if(this.checkValidEmail(this.resetPasswordEmail)){
      console.log(this.resetPasswordEmail);

      //API
      this.resetService.sendResetPasswordLink(this.resetPasswordEmail).subscribe({
        next:(res) => {
            toastr.success(res.message,'Success', {timeOut: 3000});

            this.resetPasswordEmail = '';

            const buttonRef = document.getElementById('closeBtn');
            buttonRef?.click();
        },
        error:(err)=>{
            toastr.error(err.error.message,'Error', {timeOut: 3000});
        }
      })
    }

  }

}


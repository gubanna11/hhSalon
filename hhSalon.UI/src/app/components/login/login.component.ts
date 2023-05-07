import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { SharedService } from 'src/app/services/shared.service';
import { UserStoreService } from 'src/app/services/user-store.service';

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

  constructor(private formBuilder: FormBuilder,
     private router: Router,
     private auth: AuthService,
     private toast: NgToastService,
     private sharedService: SharedService,
     private userStore: UserStoreService,
     private chatService: ChatService,
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
            this.auth.storeToken(res.token);

            const tokenPayload = this.auth.decodedToken();
            
            this.userStore.setFullNameForStore(tokenPayload.unique_name);
            this.userStore.setRoleForStore(tokenPayload.role);
            this.userStore.setIdForStore(tokenPayload.nameid);
            
            
            
            this.chatService.addUser(tokenPayload.nameid).subscribe(
              () => {                
                this.chatService.userId = tokenPayload.nameid;
                this.chatService.createChatConnection(tokenPayload.nameid);
              }              
            );

            // this.chatService.getChatList(tokenPayload.nameid).subscribe(
            //   list => {
            //      this.chatService.chatList = list;
            //      this.chatService.unreadCount  = list.filter(l => !l.isRead).length;                
            //   }
            // )
            


            this.toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
            this.sharedService.sendData(true);
            this.router.navigate(["/"]);
          },
          error:(err) => {
            //alert(err?.error.message);       
            console.log(err.error.message);
                 
            this.toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
          }
        });
      
    }
    else{
      //throw the error using toaster and with required fields
      ValidateForm.validateAllFormFields(this.loginForm);
      
      //  alert("invalid");
    }
  }

}


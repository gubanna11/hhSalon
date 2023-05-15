import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import * as toastr from 'toastr';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit{
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";

  signUpForm! : FormGroup;

  passwordMismatch: boolean = false;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private auth: AuthService,
    ){

  }

  ngOnInit(): void {
    this.signUpForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });

    
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  // get passwordMismatch() {
  //   const password = this.signUpForm.get('password');
  //   const confirmPassword = this.signUpForm.get('confirmPassword');
  //     if(password && confirmPassword)
  //       return password.value !== confirmPassword.value;   
  //     return true;
  // }

  confirmPassword(confirmPassword:any){
    const password = this.signUpForm.get('password');
      if(password && confirmPassword)
        this.passwordMismatch = password.value !== confirmPassword.value;         
  }

  onSingUp(){

    if(this.signUpForm.valid && !this.passwordMismatch){

      this.auth.signUp(this.signUpForm.value)
        .subscribe({
          next: (res) => {
            toastr.success(res.message, 'SUCCESS', {timeOut: 5000});
            //this.signUpForm.reset();
            this.router.navigate(['/login']);
          },
          error: (err) => {
            toastr.error(err.error.message, 'ERROR', {timeOut: 5000});
          }
        })
    }else{
      ValidateForm.validateAllFormFields(this.signUpForm);
    }

  }
}

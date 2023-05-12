import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';

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

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private toast: NgToastService
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

  get passwordMismatch() {
    const password = this.signUpForm.get('password');
    const confirmPassword = this.signUpForm.get('confirmPassword');
      if(password && confirmPassword)
        return password.value !== confirmPassword.value;   
      return true;
  }

  confirmPassword(confirmPassword:any){
    if(confirmPassword){
      confirmPassword.value
    }
  }

  onSingUp(){

    if(this.signUpForm.valid && !this.passwordMismatch){

      this.auth.signUp(this.signUpForm.value)
        .subscribe({
          next: (res) => {
            this.toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
            //this.signUpForm.reset();
            this.router.navigate(['/login']);
          },
          error: (err) => {
            this.toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
          }
        })
    }else{
      ValidateForm.validateAllFormFields(this.signUpForm);
    }

  }
}

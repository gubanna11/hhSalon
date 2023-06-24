import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { confirmPasswordValidator } from 'src/app/helpers/confirm-password';
import ValidateForm from 'src/app/helpers/validateForm';
import { ResetPassword } from 'src/app/models/reset-password';
import { ResetPasswordService } from 'src/app/services/reset-password.service';

import * as toastr from 'toastr';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss']
})
export class ResetComponent implements OnInit {
    resetPasswordForm!:FormGroup;
    emailToReset!: string;
    emailToken!: string;
    resetPasswordObj = new ResetPassword();

    constructor(private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private resetService: ResetPasswordService,
      private router: Router,
      ){}
    
    
    ngOnInit(): void {
        this.resetPasswordForm = this.formBuilder.group({
            password: [null, Validators.required],
            confirmPassword: [null, Validators.required]
        },{
          validator: confirmPasswordValidator("password", "confirmPassword")
        });

        this.route.queryParams.subscribe(val => {
          this.emailToReset = val['email'];
          let emailToken = val['code'];

          this.emailToken = emailToken.replace(/ /g, '+');
          
        })
    }

    reset(){
      if(this.resetPasswordForm.valid){
          this.resetPasswordObj.email = this.emailToReset;
          this.resetPasswordObj.newPassword = this.resetPasswordForm.value.password;
          this.resetPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmPassword;
          this.resetPasswordObj.emailToken = this.emailToken;

          this.resetService.resetPassword(this.resetPasswordObj).subscribe({
            next:(res)=>{
              toastr.success(res.message, 'Success', {timeOut: 3000});
              this.router.navigate(['login']);
            },
            error:(err)=>{
               
                toastr.error(err.error.message.replace('\n', '<br/>'), 'ERROR', {timeOut: 3000});
            }
          })
      }else{
        ValidateForm.validateAllFormFields(this.resetPasswordForm);
      }
    }


}

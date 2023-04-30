import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-admin-create',
  templateUrl: './admin-create.component.html',
  styleUrls: ['./admin-create.component.scss']
})
export class AdminCreateComponent {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";

  signUpForm! : FormGroup;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private toast: NgToastService,
    private userStore: UserStoreService,
    ){

  }

  ngOnInit(): void {
    this.userStore.getRoleFromStore().subscribe(roleVal => {
      
      const roleFromToken = this.auth.getRoleFromToken();
      let role = roleVal || roleFromToken;
      
      if(role != 'Admin'){
        this.toast.warning({detail: "Oooops..."});
        this.router.navigate(['login']);
      }
    })

    //конструктор auth достает токен 1 раз(когда торкається конструктора)
    //соответсвенно, если до єтого (из-за того шо юзер сюда зашел) перезапустился сайт, то у токена роль клиент
    //и когда заходишь через админа то через auth.getrolefromtoken получаем client

    //поэтому шоб при перезагрузке работало нужен auth.getrolefromtoken
    //а чтоби при переавторизации работало, то token from store

      // let role = this.auth.getRoleFromToken();
      // console.log("from token:  "+role);
      // if(role != 'Admin'){
      //   this.toast.warning({detail: "Oooops..."});
      //   this.router.navigate(['login']);
      // }
    

    this.signUpForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";
  }

  onSingUp(){
    if(this.signUpForm.valid){

      this.auth.signUpWithRole(this.signUpForm.value, "Admin")
        .subscribe({
          next: (res) => {
            this.toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
            //this.signUpForm.reset();
            this.router.navigate(['users']);
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

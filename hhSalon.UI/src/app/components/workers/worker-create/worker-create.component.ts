import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateForm';
import { Days } from 'src/app/models/enums/Days';
import { Group } from 'src/app/models/group';
import { AuthService } from 'src/app/services/auth.service';
import { GroupsService } from 'src/app/services/groups.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import * as toastr from 'toastr';

@Component({
  selector: 'app-worker-create',
  templateUrl: './worker-create.component.html',
  styleUrls: ['./worker-create.component.scss']
})
export class WorkerCreateComponent {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  groupsList: Group[] = [];
  signUpForm! : FormGroup;
  

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private userStore: UserStoreService,
    private groupsService: GroupsService,
    ){


  }

  ngOnInit(): void {

    this.userStore.getRoleFromStore().subscribe(roleVal => {
      
      const roleFromToken = this.auth.getRoleFromToken();
      let role = roleVal || roleFromToken;

      // if(role != 'Admin'){
      //   //this.toast.warning({detail: "Oooops..."});
      //   //toastr.success(res.message,'SUCCESS', {timeOut: 5000})
      //   this.router.navigate(['login']);
      // }
    })

    this.groupsService.getGroups().subscribe(
      (result: Group[]) => {
        this.groupsList = result;        
      }
    );


    this.signUpForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', Validators.required],
      groupsIds: ['', Validators.required],
      address: ['', Validators.required],
      gender: ['Female', Validators.required],
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
     
      this.auth.signUpWorker(this.signUpForm.value)
        .subscribe({
          next: (res) => {
            toastr.success(res.message,'SUCCESS', {timeOut: 5000})
            
            this.router.navigate([`worker-schedule-create/${res.workerId}`]);
          },
          error: (err) => {
            toastr.error(err.error.message.replace('\n', '<br/>'), 'ERROR', {timeOut: 5000});
          }
        })
    }else{
      ValidateForm.validateAllFormFields(this.signUpForm);
    }

  }
}

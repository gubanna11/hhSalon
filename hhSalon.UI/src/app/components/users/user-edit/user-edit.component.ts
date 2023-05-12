import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from 'src/app/services/users.service';
import * as toastr from 'toastr'; 

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit{
  userId?: string | null;
  user: any;

  constructor(private usersService: UsersService,  
    private route: ActivatedRoute,
    private router: Router,
    ) {
      toastr.options.timeOut = 5000;
    }
    
  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
    this.userId = routeParams.get('userId');
   
    if(this.userId)
      this.usersService.getUserById(this.userId).subscribe(
        (user) => {
            this.user = user;            
        }
      )
  }


  Save(){
    this.usersService.updateUser(this.user).subscribe({
      next: (res) => {
        toastr.success(res.message, 'SUCCESS')
      }
    })
  }


}

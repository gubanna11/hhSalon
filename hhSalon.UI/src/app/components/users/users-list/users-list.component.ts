import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss']
})
export class UsersListComponent implements OnInit{
  public users: any = [];
  
  constructor( private api: ApiService,
    private router: Router,
    private userStore: UserStoreService,
    private usersService: UsersService
    ){

  }

  ngOnInit(): void {
    // this.userStore.getRoleFromStore().subscribe(role => {
    //   if(role != 'Admin')
    //   this.router.navigate(['']);
    // });
      
    // this.api.getUsers()
    // .subscribe(res => {
    //   this.users = res;
    // })

    this.usersService.getUsers()
    .subscribe(res => {
      this.users = res;
    })
  }
}

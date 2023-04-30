import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit{
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

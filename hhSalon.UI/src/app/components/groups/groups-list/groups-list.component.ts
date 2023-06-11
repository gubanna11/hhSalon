import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Group } from 'src/app/models/group';
import { AuthService } from 'src/app/services/auth.service';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupsListComponent implements OnInit{
  groups:Group[] = [];
  // role: string = '';

  constructor(private groupsService: GroupsService,
              private sharedService: SharedService,
              private router: Router,
              private userStore: UserStoreService,
              public auth: AuthService,
      ){
      }

 
  ngOnInit(): void {
    this.groupsService.getGroups().subscribe(
      (result: Group[]) => {this.groups = result; }
    );
    
    // this.userStore.getRoleFromStore().subscribe(
    //   roleValue => {
    //     const roleFromToken = this.auth.getRoleFromToken();
    //     this.role = roleValue || roleFromToken;
    //  })  

  }


  deleteGroup(group: Group){

    this.groupsService
      .deleteGroup(group)
        .subscribe((groups) => {
          this.groups = groups;
          this.sharedService.sendData(groups);
          //this.router.navigate(['groups'])
        });
  }

 
}

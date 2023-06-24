import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Group } from 'src/app/models/group';
import { AuthService } from 'src/app/services/auth.service';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupsListComponent implements OnInit{
  groups:Group[] = [];

  constructor(private groupsService: GroupsService,
              private sharedService: SharedService,
              public auth: AuthService,
      ){
      }

 
  ngOnInit(): void {
    this.groupsService.getGroups().subscribe(
      (result: Group[]) => {this.groups = result; }
    );
  }


  deleteGroup(group: Group){

    this.groupsService
      .deleteGroup(group)
        .subscribe((groups) => {
          this.groups = groups;
          this.sharedService.sendData(groups);
        });
  }
 
}

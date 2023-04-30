import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Group } from 'src/app/models/group';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupsListComponent {
  groups:Group[] = [];

  constructor(private groupsService: GroupsService,
              private sharedService: SharedService,
              private router: Router,
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
        //this.router.navigate(['groups'])
      });
  }

 
}

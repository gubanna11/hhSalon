import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Group } from 'src/app/models/group';
import { EventsService } from 'src/app/services/events.service';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupsListComponent implements OnInit, AfterViewInit{
  groups:Group[] = [];

  constructor(private groupsService: GroupsService,
              private sharedService: SharedService,
              private router: Router,
              public eventsService: EventsService,
      ){
      }
  ngAfterViewInit(): void {
    this.eventsService.confirmDelete();
  }

 
  ngOnInit(): void {
    this.groupsService.getGroups().subscribe(
      (result: Group[]) => {this.groups = result; }
    );
    
    
  }

  confirmDelete(){
    //console.log(event.target);
    
    //console.log(document.getElementsByClassName('delete')[0]);
    //console.log(document);
    
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

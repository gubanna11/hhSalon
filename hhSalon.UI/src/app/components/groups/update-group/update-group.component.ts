import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from 'src/app/models/group';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-update-group',
  templateUrl: './update-group.component.html',
  styleUrls: ['./update-group.component.scss']
})
export class UpdateGroupComponent implements OnInit {
  group?:Group;
  mistake: string = '';

  constructor(private groupsService:GroupsService,
    private sharedService: SharedService,
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit(): void {
    
    const routeParams = this.route.snapshot.paramMap;
    const groupId = Number(routeParams.get('groupId'));

    this.groupsService.getGroupById(groupId).subscribe((group: Group) => (
      this.group = group));

  }

  updateGroup(group:Group){
    // if(this.group != undefined && this.group?.name.length < 3){
    //   this.mistake = "Name < 3";
    // }
     
    this.groupsService.updateGroup(group)
      .subscribe((groups) => {
        //this.groupsUpdated.emit(groups);
        this.sharedService.sendData(groups);
        this.router.navigate(['groups'])
    });
  }
}

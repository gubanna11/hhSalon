import { HttpEvent } from '@angular/common/http';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Group } from 'src/app/models/group';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';
import * as toastr from 'toastr';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.scss']
})
export class CreateGroupComponent implements OnInit{
  group? :Group;
  //file: File | null = null;
  
  ngOnInit(): void {
  }

  constructor(private groupsService:GroupsService,
              private sharedService: SharedService,
              private router: Router
              ){
    this.group = new Group();
  }


  createGroup(group:Group){
 
    this.groupsService.createGroup(group)
      .subscribe({
       next: (groups) => {
        //this.groupsUpdated.emit(groups);
        this.sharedService.sendData(groups);
        toastr.success('Group was created!', 'SUCCESS');
        this.router.navigate(['groups'])
        },
        error: (err) =>{
          //this.mistake = error.error.errors.Name, 
          toastr.error(err.error.message, 'Error');
      
          
        }         
      });
      
  }

  
}

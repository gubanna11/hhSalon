import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Group } from 'src/app/models/group';
import { Service } from 'src/app/models/service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';
import * as toastr from 'toastr';

@Component({
  selector: 'app-create-service',
  templateUrl: './create-service.component.html',
  styleUrls: ['./create-service.component.scss']
})
export class CreateServiceComponent implements OnInit{
  service?: Service;
  groups: Group[] = [];
  selectedGroupName?: string | null;
 
  
  constructor(private groupsService: GroupsService,
    private servicesService: ServicesService,
    private router: Router,
    ){
      this.service = new Service();
  }

  ngOnInit(): void {
    this.groupsService.getGroups().subscribe( (groups) => this.groups = groups );

    this.groupsService.getGroupById(1).subscribe( (group) =>  this.selectedGroupName = group.name);
  }


  selectChanged(group?:any){      
    this.selectedGroupName = group.options[group.selectedIndex].textContent;     
  }

  createService(service:Service){
    this.groupsService.getGroupById(service.groupId).subscribe( (group) =>  this.selectedGroupName = group.name);

    this.servicesService.createService(service).subscribe({
      next:(services) => {
        toastr.success('Service was created!', 'SUCCESS', {timeOut: 5000});
        this.router.navigate([`services/${service.groupId}/${this.selectedGroupName}`])
      },  
      error:(err) => {         
        toastr.error(err.error.message,'ERROR', {timeOut: 5000});
      }
    })
  }
}

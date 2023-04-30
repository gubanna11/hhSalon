import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Group } from 'src/app/models/group';
import { Service } from 'src/app/models/service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';

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
    private toast: NgToastService,
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
        this.router.navigate([`services/${service.groupId}/${this.selectedGroupName}`])
      },  
      error:(err) => {               
        this.toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
      }
    })
  }
}

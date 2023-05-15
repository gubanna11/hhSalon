import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Group } from 'src/app/models/group';
import { Service } from 'src/app/models/service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';
import * as toastr from 'toastr';

@Component({
  selector: 'app-update-service',
  templateUrl: './update-service.component.html',
  styleUrls: ['./update-service.component.scss']
})
export class UpdateServiceComponent implements OnInit{
  @Input() service?: Service | null;
  @Output() servicesUpdated = new EventEmitter<Service[]>();
  selectedGroupName?: string | null;
  groups: Group[] = [];


  constructor(private groupsService: GroupsService,
    private servicesService: ServicesService,
    private router: Router,
    ){
      
  }
  ngOnInit(): void {
    this.groupsService.getGroups().subscribe( (groups) => this.groups = groups );
  }


  updateService(service: Service){
    let select  = document.getElementById('selectGroup') as HTMLSelectElement;

       if(select){
        this.selectedGroupName = select[select.selectedIndex].textContent;        
    }

    this.servicesService.updateService(service).subscribe({
      next:(services) => {
        
        this.servicesUpdated.emit(services);
        toastr.success('The service was updated!', 'SUCCESS', {timeOut: 5000});
        this.router.navigate([`services/${service.groupId}/${this.selectedGroupName}`])

      },
      error:(err) => {     
        toastr.error(err.error.message,'ERROR', {timeOut: 5000});
      }
  })
   }

}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Group } from 'src/app/models/group';
import { Service } from 'src/app/models/service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';

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
    private toast: NgToastService,
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

        this.router.navigate([`services/${service.groupId}/${this.selectedGroupName}`])

      },
      error:(err) => {               
        this.toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
      }
  })
   }

}

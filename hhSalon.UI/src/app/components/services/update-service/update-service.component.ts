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
      this.groupsService.getGroups().subscribe( (groups) => this.groups = groups );

    //this.groupsService.getGroupById(1).subscribe( (group) =>  this.selectedGroupName = group.name);
      
  }
  ngOnInit(): void {
    let select: any  = document.getElementById('selectGroup');
    
    if(select != null){
      for(let opt of select.options)
        if(opt.value == this.service?.groupId)
          opt.selected = true;
    }

  }

  selectChanged(group?:any){  
    this.selectedGroupName = group.options[group.selectedIndex].textContent; 
  }

  updateService(service: Service){
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

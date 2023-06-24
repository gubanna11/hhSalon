import { Component,  OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Service } from 'src/app/models/service';
import { AuthService } from 'src/app/services/auth.service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-services-list',
  templateUrl: './services-list.component.html',
  styleUrls: ['./services-list.component.scss']
})
export class ServicesListComponent implements OnInit {
  groupName?: string | null;
  services: Service[] = [];

  serviceToEdit?:Service | null;
  role: string = '';
  
  constructor(private servicesService: ServicesService,
    private route: ActivatedRoute,
    private router: Router,
    private userStore: UserStoreService,
    private auth: AuthService){

  }


  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
    //const groupId = Number(routeParams.get('groupId'));
    this.groupName = routeParams.get('groupName');

    let groupId:number = 1;

    this.route.params.subscribe(params => {
       groupId = +params['groupId']; // + = Number

       this.groupName = params['groupName'];

       this.servicesService.getServices(groupId).subscribe(        {
          next:  (services: Service[]) => {
            this.services = services
          },
          error: (err)=>{
            this.services = [];
          }
       }
       
      );
    })

    this.userStore.getRoleFromStore().subscribe(
      roleValue => {
        const roleFromToken = this.auth.getRoleFromToken();
        this.role = roleValue || roleFromToken;
     })  
  }


  deleteService(service:Service){
    this.servicesService.deleteService(service).subscribe({
          next: (services:Service[]) => {
            this.services = services;
            //this.router.navigate([`services/${service.groupId}/${this.groupName}`])
            },
            error: (err) => {
              console.log(err.error.message);
              
            }

        }
      
      );
  }

  updateService(service:Service){
    this.serviceToEdit = service;
  }

  updatedServicesList(services: Service[]){
    console.log(services);
    
    if(services != undefined)
      this.services = services;
    this.serviceToEdit = null;

    

  //   this.route.params.subscribe(params => {
  //     this.groupName = params['groupName'];      
  //  })

    //this.groupService.getGroupById(services[0].groupId).subscribe((group) => this.groupName = group.name);

  }
}

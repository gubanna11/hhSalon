import { Time, getLocaleDateFormat } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Attendance } from 'src/app/models/attendance';
import { Group } from 'src/app/models/group';
import { Service } from 'src/app/models/service';
import { AttendancesService } from 'src/app/services/attendances.service';
import { AuthService } from 'src/app/services/auth.service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';
import { WorkersService } from 'src/app/services/workers.service';

@Component({
  selector: 'app-create-attendance',
  templateUrl: './create-attendance.component.html',
  styleUrls: ['./create-attendance.component.scss']
})
export class CreateAttendanceComponent implements OnInit {
  attendance: Attendance = new Attendance();

  groups: Group[] = [];
  services?: Service[] = [];
  workers?: any = [];
  slots:any[] = [];

  time?:Time;

  activeSlot?: string;

  slotsNull = '';

  constructor(
    private attendancesService: AttendancesService,
    private router: Router,
    private userStore: UserStoreService,
    private auth: AuthService,
    private groupsService: GroupsService,
    private servicesService: ServicesService,
    private workersService:  WorkersService,
  ) {
    this.attendance = new Attendance();
    this.attendance.groupId = 1;
    this.attendance.serviceId = 1;
    
  }

  ngOnInit(): void {
   this.groupsService.getGroups().subscribe( (groups) => this.groups = groups );

   this.servicesService.getServices(1).subscribe (services => {
    this.services = services 
    //if(this.attendance != undefined)
        this.attendance.price = this.services[0].price
    });    

   this.workersService.getWorkersByGroupId(1).subscribe(workers => {
    this.workers = workers; 
    
        this.attendance.workerId = this.workers[0].id;
        // this.attendance.date = new Date();

        // this.attendancesService.getFreeTimeSlots(this.attendance.workerId, this.attendance.date.toDateString()).subscribe(
        //   result => {
        //       this.slots = result
        //   }
        // );

        console.log(this.attendance);
        
    
    });      

    //this.attendance.date = new Date(); 
    
  }

  timeSelected(slot:any){
    this.activeSlot = slot;
    
    if(this.attendance)
      this.attendance.time = slot;    
  }


  groupChanged(attendance:any){
    const groupId = attendance.options[attendance.selectedIndex].value;
    
    this.servicesService.getServices(groupId).subscribe (services => {
      this.services = services; 
      if(this.attendance != undefined) //if null then we can't reach the property of null object
      {
        this.attendance.serviceId = this.services[0].id;
        this.attendance.price = this.services[0].price
      }
    });
    

    this.workersService.getWorkersByGroupId(groupId).subscribe(workers => {
      this.workers = workers;
      
      if(this.workers.length == 0){
        this.attendance.workerId = '';
        this.slots = [];
      }
      else
      {
        this.attendance.workerId = this.workers[0].id;      
        this.updateTime();
      }
      
    });
    
  }

  serviceChanged(service:any){
    let id = service.options[service.selectedIndex].value;
    
    if(this.services && this.attendance)      
        for(let service of this.services)
          if(service.id == id)
            this.attendance.price = service.price    

            //console.log(this.attendance);
  }

  updateTime(){
    console.log(this.attendance);
    
    if(this.attendance.date != undefined)
    {
      this.attendancesService.getFreeTimeSlots(this.attendance.workerId, this.attendance.date).subscribe(
        result => {
            this.slots = result
            if(this.slots.length == 0)
              this.slotsNull = 'There is no free time!';
            else
              this.slotsNull = '';
        }
      );
    }
          
}


  createAttendance(attendance: Attendance){
    this.userStore.getIdFromStore().subscribe(
      idValue => {
        const idFromToken = this.auth.getIdFromToken();
        attendance.clientId = idValue || idFromToken;
      })

      //const hoursA = document.forms[0]['time'].value.split(':')[0];
      //const minutesA = document.forms[0]['time'].value.split(':')[1];
      //attendance.time = ({hours: hoursA, minutes: minutesA});
      
      

      this.attendancesService.createAttendance(attendance).subscribe(
        {
          next: () => this.router.navigate(['my-not-rendered-attendances']),
          error: (error) => console.log(error.error)          
        }        
        
      );
  }
}

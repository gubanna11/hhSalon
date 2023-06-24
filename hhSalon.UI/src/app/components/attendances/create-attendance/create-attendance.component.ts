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
import * as toastr from 'toastr';

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
    
    }); 
  }

  timeSelected(slot:any){
    if (this.activeSlot == slot){
      this.activeSlot = undefined;
      this.attendance.time = undefined;
      return;
    }

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
            this.attendance.price = service.price;
  }

  updateTime(){
   
    if(this.attendance.date != undefined)
    {
      
      if(!this.dateIsCorrect()){
        this.slots = [];
        this.slotsNull = '';
        return;
      }

      this.slots = [];
      this.attendancesService.getFreeTimeSlots(this.attendance.workerId, this.attendance.date).subscribe(
        result => {
          if(result.length > 0){
            result.map(
              r => {
                let time  = r.split(':');
                
                this.slots.push(time[0] + ':' + time[1]);
              }
            )
          }

            if(this.slots.length == 0)
              this.slotsNull = 'There is no free time!';
            else
              this.slotsNull = '';
        }
      );
    }
          
}


  createAttendance(attendance: Attendance){
    if(attendance.time == undefined){
      toastr.error('Select the time', 'Error', {timeOut: 2000});
      return;
    }

    if(!this.dateIsCorrect()){
      return;
    }

    this.userStore.getIdFromStore().subscribe(
      idValue => {
        const idFromToken = this.auth.getIdFromToken();
        attendance.clientId = idValue || idFromToken;
      })
    

      this.attendancesService.createAttendance(attendance).subscribe(
        {
          next: () => {
            toastr.success("You've created a new appointment!", 'Success', {timeOut: 3000})
            this.router.navigate(['my-not-rendered-attendances'])
          },
          error: (error) =>{
            console.log(error.error)          
            toastr.error(error.error.message, 'ERROR', {timeOut: 5000});
          } 
        }        
        
      );
  }


  dateIsCorrect(){
    if(this.attendance.date != undefined)
    {
      if(new Date(this.attendance.date).getTime() < (new Date(Date.now()).getTime())){
      
          toastr.error("You can't choose this date", 'Error', {timeOut: 2000});
          return false;
      }
      return true;
    }

    return false;
  }
}

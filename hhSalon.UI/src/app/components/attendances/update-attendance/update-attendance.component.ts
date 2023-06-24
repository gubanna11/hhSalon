import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Group } from 'src/app/models/group';
import { Service } from 'src/app/models/service';
import { AttendancesService } from 'src/app/services/attendances.service';
import { GroupsService } from 'src/app/services/groups.service';
import { ServicesService } from 'src/app/services/services.service';

@Component({
  selector: 'app-update-attendance',
  templateUrl: './update-attendance.component.html',
  styleUrls: ['./update-attendance.component.scss']
})
export class UpdateAttendanceComponent implements OnInit {
  @Input() attendance:any;
  @Output() attendanceOutput = new EventEmitter();

  groups:Group[] = [];
  services:Service[]=[];

  isPaid: string = 'No';
  
  constructor(
    private attendancesService:AttendancesService,
    private servicesService:ServicesService,
    private groupsService:GroupsService,
  ) {  }

  ngOnInit(): void {
    this.attendance.date = this.attendance.date.split('T')[0];

    if(this.attendance.isPaid == 'Yes'){
      this.isPaid = 'Yes';      
    }

    this.groupsService.getGroupsByWorkerId(this.attendance.workerId).subscribe(
      list => this.groups = list
    )

    this.servicesService.getServices(this.attendance.groupId).subscribe(
      list => this.services= list
    )
  }

  update(attendance:any){
    this.attendancesService.updateAttendance(attendance).subscribe( () => {
      
      if(this.isPaid == 'Yes'){
          this.attendancesService.WorkerNotRenderedIsPaidAttendances(attendance.workerId).subscribe(
            list => {
              this.attendanceOutput.emit(list)
            }
          )
      }
      else if (this.isPaid == 'No'){
        this.attendancesService.WorkerNotRenderedNotPaidAttendances(attendance.workerId).subscribe(
          list => {
            this.attendanceOutput.emit(list)
          }
        )
      }
    }      
    );
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
  }

  serviceChanged(service:any){
    let id = service.options[service.selectedIndex].value;
    
    if(this.services && this.attendance)      
        for(let service of this.services)
          if(service.id == id)
            this.attendance.price = service.price;
  }



  //////////////////////
  closeModal(event:any){
    const modal = document.getElementById("myModal");
    const closeBtn = document.getElementsByClassName('close')[0];
    
    if(modal && (event.target == modal || event.target == closeBtn))
    {
      this.attendanceOutput.emit();
    }
    
  }
  
}

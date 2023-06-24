import { Component, OnInit, Output } from '@angular/core';
import { AttendancesService } from 'src/app/services/attendances.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-worker-not-rendered-attendances',
  templateUrl: './worker-not-rendered-attendances.component.html',
  styleUrls: ['./worker-not-rendered-attendances.component.scss']
})
export class WorkerNotRenderedAttendancesComponent implements OnInit{

  attendances: any[] = [];

  workerId: string = "";
  workerName: string = "";

  isPaid = false;
  all = false;

  @Output() attendanceToEdit: any;

  constructor(
    private attendanceService: AttendancesService,
     private userStore: UserStoreService,
     private auth: AuthService,
  ){

  }

  ngOnInit(): void {
    let userId: string = "";

    this.userStore.getIdFromStore().subscribe(
      idValue => {
        const idFromToken = this.auth.getIdFromToken();
        this.workerId = idValue || idFromToken;
      })

      
    this.userStore.getFullNameFromStore().subscribe(
      fullName => {
        const fullNameFromToken = this.auth.getFullNameFromToken();
        this.workerName = fullName || fullNameFromToken;
      })

    this.attendanceService.WorkerNotRenderedNotPaidAttendances(this.workerId).subscribe(
      result => {
        this.attendances = result;  
                
      }
    )    
  }

  filterNotPaid(){
    this.attendanceService.WorkerNotRenderedNotPaidAttendances(this.workerId).subscribe(
      result => {this.attendances = result}
    )   
    this.isPaid = false;
  }

  filterIsPaid(){
    this.attendanceService.WorkerNotRenderedIsPaidAttendances(this.workerId).subscribe(
      result => this.attendances = result
    )   
    this.isPaid = true;
    
  }

  filterAll(){
    this.attendanceService.WorkerNotRenderedAttendances(this.workerId).subscribe(
      result => this.attendances = result
    )   
    this.all = true;
    this.isPaid = false;
  }

  editAttendance(attendance:any){    
    this.attendanceToEdit = attendance;
  }


  newAttendance(attendances:any){
    this.attendanceToEdit = undefined;

    if(attendances != undefined)
      this.attendances = attendances;
    
  }


  attendanceChanged(attendance:any){
    this.attendanceService.updateAttendance(attendance).subscribe(
      ()=>{
        if(!this.isPaid && !this.all){
          this.attendanceService.WorkerNotRenderedNotPaidAttendances(this.workerId).subscribe(
            list => this.attendances = list
          )
        }else if (this.isPaid && !this.all){
          this.attendanceService.WorkerNotRenderedIsPaidAttendances(this.workerId).subscribe(
            list => this.attendances = list
          )
        }
        else{
          this.attendanceService.WorkerNotRenderedAttendances(this.workerId).subscribe(
            list => this.attendances = list
          )
        }
      }
    )    
  }


  deleteAttendance(id: number){
    this.attendanceService.deleteAttendance(id).subscribe(
      () => {
        if(!this.isPaid && !this.all){
          this.attendanceService.WorkerNotRenderedNotPaidAttendances(this.workerId).subscribe(
            list => this.attendances = list
          )
        }else if (this.isPaid && !this.all){
          this.attendanceService.WorkerNotRenderedIsPaidAttendances(this.workerId).subscribe(
            list => this.attendances = list
          )
        }
        else{
          this.attendanceService.WorkerNotRenderedAttendances(this.workerId).subscribe(
            list => this.attendances = list
          )
        }
      }
    )
  }
}

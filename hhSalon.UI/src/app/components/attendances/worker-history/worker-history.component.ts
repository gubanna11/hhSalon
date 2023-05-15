import { Component, OnInit, Output } from '@angular/core';
import { AttendancesService } from 'src/app/services/attendances.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-worker-history',
  templateUrl: './worker-history.component.html',
  styleUrls: ['./worker-history.component.scss']
})
export class WorkerHistoryComponent implements OnInit{
  attendances: any[] = [];

  workerId: string = "";
  workerName: string = "";

  @Output() attendanceToEdit: any;
  
  constructor(
    private attendanceService: AttendancesService,
     private userStore: UserStoreService,
     private auth: AuthService
  ){

  }

  ngOnInit(): void {
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

    this.attendanceService.WorkerHistoryAttendances(this.workerId).subscribe(
      result => {
        this.attendances = result;
      }
    )    
  }


  deleteAttendance(id: number){
    this.attendanceService.deleteAttendance(id).subscribe(
        () => {
          this.attendanceService.WorkerHistoryAttendances(this.workerId).subscribe(
            result => {
              this.attendances = result;
            }
          )   
        }
      )
  }


  editAttendance(attendance:any){
    this.attendanceToEdit = attendance;
  }

  newAttendance(attendances:any){
    this.attendanceToEdit = undefined;

    this.attendanceService.WorkerHistoryAttendances(this.workerId).subscribe(
      result => {
        this.attendances = result;
      }
    )   
    
  }
}

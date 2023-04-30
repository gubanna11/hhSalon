import { Component, OnInit } from '@angular/core';
import { AttendancesService } from 'src/app/services/attendances.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-my-not-rendered-attendances',
  templateUrl: './my-not-rendered-attendances.component.html',
  styleUrls: ['./my-not-rendered-attendances.component.scss']
})
export class MyNotRenderedAttendancesComponent implements OnInit{

  attendances: any[] = [];

  userId: string = "";
  name: string = "";

  isShown:boolean = false;

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
        this.userId = idValue || idFromToken;
      })

            
    this.userStore.getFullNameFromStore().subscribe(
      fullName => {
        const fullNameFromToken = this.auth.getFullNameFromToken();
        this.name = fullName || fullNameFromToken;
      })

    this.attendanceService.MyNotRenderedNotPaidAttendances(this.userId).subscribe(
      result => {
        this.attendances = result;
        console.log(this.attendances);
        
        if(this.attendances.length > 0)
          this.isShown = true;
      }
    )    
  }

  filterNotPaid(){
    this.attendanceService.MyNotRenderedNotPaidAttendances(this.userId).subscribe(
      result => {this.attendances = result
      console.log(this.attendances);}
    )   
  }

  filterIsPaid(){
    this.attendanceService.MyNotRenderedIsPaidAttendances(this.userId).subscribe(
      result => this.attendances = result
    )   
    this.isShown = false;
  }

}

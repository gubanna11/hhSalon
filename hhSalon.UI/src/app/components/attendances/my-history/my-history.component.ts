import { Component, OnInit } from '@angular/core';
import { AttendancesService } from 'src/app/services/attendances.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-my-history',
  templateUrl: './my-history.component.html',
  styleUrls: ['./my-history.component.scss']
})
export class MyHistoryComponent implements OnInit{
  attendances: any[] = [];

  userId: string = "";
  name: string = "";

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

    this.attendanceService.MyHistoryAttendances(this.userId).subscribe(
      result => {
        this.attendances = result;
      }
    )    
  }

  
}

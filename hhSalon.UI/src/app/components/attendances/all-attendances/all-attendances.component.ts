import { Component, OnInit } from '@angular/core';
import { AttendancesService } from 'src/app/services/attendances.service';

@Component({
  selector: 'app-all-attendances',
  templateUrl: './all-attendances.component.html',
  styleUrls: ['./all-attendances.component.scss']
})
export class AllAttendancesComponent implements OnInit {
  attendances: any[] = [];

  constructor(
    private attendancesService: AttendancesService,
  ){    
  }

  ngOnInit(): void {
    this.attendancesService.getAllAttendances().subscribe(
      (list) =>  this.attendances = list
    )
  }

  search(event: any){
    let content = event.target.value;

    this.attendancesService.filterAttendances(content).subscribe(
      (attendances) => {
          this.attendances = attendances;
      }
    )
    
  }

}

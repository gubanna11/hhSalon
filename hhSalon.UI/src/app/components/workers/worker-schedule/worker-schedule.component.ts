import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Days } from 'src/app/models/enums/Days';
import { Schedule } from 'src/app/models/schedule';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';
import { WorkersService } from 'src/app/services/workers.service';

@Component({
  selector: 'app-worker-schedule',
  templateUrl: './worker-schedule.component.html',
  styleUrls: ['./worker-schedule.component.scss']
})
export class WorkerScheduleComponent implements OnInit {
  schedules:Schedule[] = [];
  Days = Days
  workerId?: string | null;
  index:number = -1;

  constructor(
    private route:ActivatedRoute,
    private workersService:WorkersService,
    private toast: NgToastService,
    private router: Router,
    private userStore: UserStoreService,
    private auth: AuthService,
  ){  }

  ngOnInit(): void {
    this.userStore.getRoleFromStore().subscribe(roleVal => {
      
      const roleFromToken = this.auth.getRoleFromToken();
      let role = roleVal || roleFromToken;
      
      if(role != 'Admin'){
        this.toast.warning({detail: "Oooops..."});
        this.router.navigate(['login']);
      }
    })

    const routeParams = this.route.snapshot.paramMap;
    this.workerId = routeParams.get('workerId');    

    this.schedules.length = Object.keys(Days).length/2;
    
    if(this.workerId)
      for(let i = 0; i < this.schedules.length; i ++)
        this.schedules[i] = {day: this.Days[i], workerId: this.workerId};       
  }
  
  createSchedule(schedules:Schedule[]){
  
    // schedules = schedules.filter(element => {
    //   return (element.start != null && element.end != null);
    // });
    

    this.workersService.createWorkerSchedule(schedules) 
    .subscribe({
      next: (res) => {
        this.toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
        
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
      }
    })
 
  }

}

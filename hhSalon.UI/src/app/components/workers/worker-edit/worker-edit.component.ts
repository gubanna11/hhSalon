import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastComponent, NgToastService } from 'ng-angular-popup';
import { Days } from 'src/app/models/enums/Days';
import { Group } from 'src/app/models/group';
import { GroupsService } from 'src/app/services/groups.service';
import { WorkersService } from 'src/app/services/workers.service';

@Component({
  selector: 'app-worker-edit',
  templateUrl: './worker-edit.component.html',
  styleUrls: ['./worker-edit.component.scss'],
})

export class WorkerEditComponent implements OnInit{
  workerId?: string | null;
  worker: any;
  Days = Days;
  groups: Group[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workersService: WorkersService,
    private toast: NgToastService,
    private groupsService: GroupsService,
    ){

  }


  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
     this.workerId = routeParams.get('workerId');
    
    
     if(this.workerId)
      this.workersService.getWorkerById(this.workerId).subscribe(
        result => {
          this.worker = result;
          // for(let i = 0; i < Object.keys(Days).length/2; i++){
          //     if( this.worker.schedules[i] == null)
          //       this.worker.schedules[i] = {day: this.Days[i], workerId: this.workerId};             
          // }

       

        //   console.log(this.worker);
          
        })

        this.groupsService.getGroups().subscribe(
          (result: Group[]) => this.groups = result        
        );
    
  }



  Save(){
    console.log(this.worker);
    
  //   if(!(this.worker.address && this.worker.firstName && this.worker.lastName &&
  //       this.worker.gender && this.worker.groupsIds && this.worker.userName && this.worker.email))
  //     {
  //       this.toast.error({detail: 'Fill all properties', duration: 5000});
  //       console.log(1);
        
  //     }
      
  // else
    this.workersService.updateWorker(this.worker).subscribe(
      {
        next: res =>{
          this.toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
        
          this.router.navigate(['/workers']);
        },
        error: (err) => {
          console.log(err.error.message);
          
          this.toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
        }
      }
    )
   }
}

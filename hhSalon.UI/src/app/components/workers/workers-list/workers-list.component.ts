import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { WorkersService } from 'src/app/services/workers.service';

@Component({
  selector: 'app-workers-list',
  templateUrl: './workers-list.component.html',
  styleUrls: ['./workers-list.component.scss']
})
export class WorkersListComponent implements OnInit{
  workers: any[] = [];
  role:string = '';

  constructor(
    private workersService: WorkersService,
    private userStore: UserStoreService,
    private auth: AuthService,
  ){  }

  ngOnInit(): void {
    this.workersService.getWorkers().subscribe(
      result => {this.workers = result;
      }
    )


    
    this.userStore.getRoleFromStore().subscribe(
      roleValue => {
        const roleFromToken = this.auth.getRoleFromToken();
        this.role = roleValue || roleFromToken;
     })  
  }


  deleteWorker(workerId:string){
    this.workersService.deleteWorker(workerId).subscribe(
      result => {
        this.workers = result;
      }
    )
  }

}

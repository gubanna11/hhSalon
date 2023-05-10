import { Component, OnInit } from '@angular/core';
import { WorkersService } from 'src/app/services/workers.service';

@Component({
  selector: 'app-workers-list',
  templateUrl: './workers-list.component.html',
  styleUrls: ['./workers-list.component.scss']
})
export class WorkersListComponent implements OnInit{
  //filter all, certain group, name
  workers: any[] = [];

  constructor(
    private workersService: WorkersService,
  ){  }

  ngOnInit(): void {
    this.workersService.getWorkers().subscribe(
      result => {this.workers = result;
      }
    )
  }


  deleteWorker(workerId:string){
    this.workersService.deleteWorker(workerId).subscribe(
      result => {this.workers = result; console.log(result);
      }
    )
  }

}

import { AfterViewInit, Component, HostListener, OnDestroy, OnInit, ViewEncapsulation} from '@angular/core';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit, AfterViewInit{

  constructor(
  ){}
  ngAfterViewInit(): void {
    throw new Error('Method not implemented.');
  }
 
  
  ngOnInit(): void {    
     
  }
 }

import { AfterViewInit, Component, ElementRef, HostListener, OnDestroy, OnInit, QueryList, Renderer2, ViewChild, ViewChildren, ViewEncapsulation} from '@angular/core';
import { LogLevel } from '@microsoft/signalr';
import { NgToastService } from 'ng-angular-popup';
import { HeaderComponent } from './components/header/header.component';
import { GroupsMenuComponent } from './components/groups-menu/groups-menu.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit, AfterViewInit{
 // @ViewChild('footer') footerElement!: ElementRef;
  //@ViewChildren('footer') footerElements!: QueryList<ElementRef>;
 
 @ViewChild(HeaderComponent) header!: HeaderComponent;
  constructor(
    private renderer: Renderer2
  ){    }
  
  ngOnInit(): void {    
   
  }

  ngAfterViewInit() {
  
  //  this.footerElements.forEach(
  //   f => {
  //     f.nativeElement.addEventListener('click', function(){
  //       console.log('CLICK');
        
  //     })
  //   }
  //  )
    const menuArrows: QueryList<ElementRef<HTMLDivElement>> = this.header.menuArrows;
    const links :QueryList<ElementRef<HTMLDivElement>> = this.header.menu_links;



      links.forEach(
        l => {
          l.nativeElement.addEventListener('click', function(){
            
            
              for(let link of links){
                if (link != l){
                  if(link.nativeElement.firstElementChild)
                   link.nativeElement.firstElementChild.classList.remove('active');
                }
              }
              if(l.nativeElement.firstElementChild)
               l.nativeElement.firstElementChild.classList.toggle('active');
          })
        }
      )

      document.onclick = function(e){
        for(let link of links)
            if (!e.composedPath().includes(link.nativeElement)) {
              if(link.nativeElement.firstElementChild)
                   link.nativeElement.firstElementChild.classList.remove('active');
            }
    }
    
    document.onkeydown = function (e) {
        if (e.keyCode === 27)
            for (let link of links)
                if (!e.composedPath().includes(link.nativeElement)) 
                 if(link.nativeElement.firstElementChild)
                   link.nativeElement.firstElementChild.classList.remove('active');
    }


    
  }

}
 

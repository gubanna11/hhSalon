import { AfterViewInit, Component, ElementRef, HostListener, OnDestroy, OnInit, QueryList, Renderer2, ViewChild, ViewChildren, ViewEncapsulation} from '@angular/core';
import { HeaderComponent } from './components/header/header.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements AfterViewInit{
  @ViewChild(HeaderComponent) header!: HeaderComponent;

  constructor(){}  

  ngAfterViewInit() {
 
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
 

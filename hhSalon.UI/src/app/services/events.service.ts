import { AfterViewInit, Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventsService implements AfterViewInit {

  constructor() { 
  }
  ngAfterViewInit(): void {
    this.confirmDelete();
  }


  confirmDelete(){
    const deletes = document.querySelectorAll('.delete');

    const deletes_a = document.querySelectorAll('.delete-confirm');

    
        deletes_a.forEach(a => {
          
            a.addEventListener('click', function () {
              
                let parentA = a.parentElement;
              
                
                deletes.forEach(del => {
                    if (del !== parentA)
                        del.classList.remove('active');
                      })
                   
                parentA?.classList.toggle('active');
            });
        })
      
    
    
      document.onclick = function (e) {
        deletes.forEach(del => {
          if (!e.composedPath().includes(del)) {
            del.classList.remove('active');
          }
        }) 
          
      }
      
      document.onkeydown = function (e) {
          if (e.keyCode === 27)
          deletes.forEach(del => {
            if (!e.composedPath().includes(del))
              del.classList.remove('active'); 
          })        
      }
  
  }

 
}

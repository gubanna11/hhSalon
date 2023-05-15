import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));


  const deletes = document.querySelectorAll('.delete');

  const deletes_a = document.querySelectorAll('.delete-confirm');
  
  console.log(deletes);
  if (deletes_a.length > 0) {
    
      deletes_a.forEach(a => {
          a.addEventListener('click', function () {
  
              let parentA = a.parentElement;
  
              deletes.forEach(del => {
                  if (del != parentA)
                      del.classList.remove('active');})
              parentA?.classList.toggle('active');
          });
      })
    }
  
  
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
 
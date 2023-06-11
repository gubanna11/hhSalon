import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { ElementRef, QueryList, ViewChildren } from '@angular/core';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));

//   @ViewChildren('delete') el : QueryList<ElementRef>;
// console.log(el);

 
 
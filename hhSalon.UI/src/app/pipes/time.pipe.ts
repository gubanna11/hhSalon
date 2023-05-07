import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time'
})
export class TimePipe implements PipeTransform {

  transform(date: Date): string {
    let arr = new Date(date).toLocaleTimeString().split(':');
    
    
    let a = (arr[arr.length - 1].split(' '))[1];

    return arr[0] + ':' + arr[1] + ' ' + a;
  }
  }



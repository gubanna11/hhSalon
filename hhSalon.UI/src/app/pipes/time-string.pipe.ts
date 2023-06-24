import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeString'
})
export class TimeStringPipe implements PipeTransform {

  transform(time: string): string{
    let arr = time.split(':');
    time = arr[0] + ':' + arr[1];
    return time;
  }

}

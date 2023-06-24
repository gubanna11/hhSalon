import { AfterViewInit, Component, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements AfterViewInit, OnDestroy{
  footer!: HTMLElement;

  ngAfterViewInit(): void {
    this.footer = document.getElementsByClassName('footer')[0] as HTMLElement;
    this.footer.style.display = 'none';
    document.body.style.overflow = 'hidden';
  }


  ngOnDestroy(): void {
    this.footer.style.display = 'block';
    document.body.style.overflow = 'inherit';
  }

}

import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { LogLevel } from '@microsoft/signalr';
import { NgToastService } from 'ng-angular-popup';
import { AttendancesService } from 'src/app/services/attendances.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

//import {render} from 'creditcardpayments/creditCardPayments';



@Component({
  selector: 'app-my-not-rendered-attendances',
  templateUrl: './my-not-rendered-attendances.component.html',
  styleUrls: ['./my-not-rendered-attendances.component.scss']
})
export class MyNotRenderedAttendancesComponent implements OnInit{

  attendances: any[] = [];

  userId: string = "";
  name: string = "";

  totalPrice: number = 0;
  @ViewChild('paymentRef') paymentRef!: ElementRef;
isShown: boolean = false;
  selectedAttendances: any[] = [];

  constructor(
    private attendanceService: AttendancesService,
     private userStore: UserStoreService,
     private auth: AuthService,
  ){      
  }

  ngOnInit(): void {
    let userId: string = "";

    this.userStore.getIdFromStore().subscribe(
      idValue => {
        const idFromToken = this.auth.getIdFromToken();
        this.userId = idValue || idFromToken;
      })

            
    this.userStore.getFullNameFromStore().subscribe(
      fullName => {
        const fullNameFromToken = this.auth.getFullNameFromToken();
        this.name = fullName || fullNameFromToken;
      })

      
    this.attendanceService.MyNotRenderedNotPaidAttendances(this.userId).subscribe(
      result => {
        this.attendances = result;
console.log(result);

        if(result.length > 0)
          this.isShown = true;

          this.calculateTotalPrice(result);
     
                
        window.paypal.Buttons({
          style: {
            //  layout:'horizontal',
            // color: 'blue',
            // shape: 'rect',
            // label: 'paypal'

            size: 'small',
            color: 'gold',
            shape: 'pill'
          },
          createOrder: (data: any, actions: any) => {
            return actions.order.create({
              purchase_units: [
                {
                  amount: {
                    value: this.totalPrice.toString(),
                    currency_code : 'USD'
                  }
                }
              ]
            })
          },
          onApprove: (data: any, actions: any) => {
            return actions.order.capture().then((details:any) => {
             
             
              if(this.selectedAttendances.length > 0){
                
                for(let att of this.selectedAttendances){
                  att.isPaid = 'Yes';
               }

                this.attendanceService.updateAttendances(this.selectedAttendances).subscribe({
                  next: () => {
                    this.selectedAttendances = [];
                    
                    this.attendanceService.MyNotRenderedNotPaidAttendances(this.userId).subscribe(
                      result => {
                        this.attendances = result;
                        this.calculateTotalPrice(result);
                        toastr.success('', 'SUCCESS', {timeOut: 5000});
                      }
                    );
                  },                  
                  error: (error) => {
                    console.log(error.error)}
                })
              }
              else{
                
               for(let att of this.attendances){
                  att.isPaid = 'Yes';
               }
                
                this.attendanceService.updateAttendances(this.attendances).subscribe({
                  next: () => {
                    this.attendances = [];
                    this.totalPrice = 0;
                    toastr.success('', 'SUCCESS', {timeOut: 5000});
                  },                  
                  error: (error) => console.log(error.error)
                })
              }
              
            })
          },
          onError: (error: any) => {
            console.log(error);
          }
        }).render(this.paymentRef.nativeElement);          
        
      }
    )    

  }


  filterNotPaid(){
    this.attendanceService.MyNotRenderedNotPaidAttendances(this.userId).subscribe(
      result => {
        this.attendances = result;
        this.calculateTotalPrice(result);
      }
    )   

  }

  filterIsPaid(){
   
    this.totalPrice = 0;
    this.attendanceService.MyNotRenderedIsPaidAttendances(this.userId).subscribe(
      result => this.attendances = result
    )  
    
  }


  Pay(attendance: any){    
    if(!this.selectedAttendances.includes(attendance)){
      this.selectedAttendances.push(attendance);

      this.calculateTotalPrice(this.selectedAttendances);
    }
      
    else{
      const index = this.selectedAttendances.indexOf(attendance);

      if (index > -1) { 
        this.selectedAttendances.splice(index, 1);
        this.totalPrice -= attendance.price;
      }
      
      
      if(this.selectedAttendances.length === 0){      
        this.attendances.map(
          a => {
            this.totalPrice += a.price;
          }
        )        
      }

      return;
    }
    
    

    // render(
    //   {
    //     id: "#myPayPalButtons",
    //     currency: "USD",
    //     value: price.toString(),
    //     onApprove: (details) =>{
    //         alert('Transaction Successfull');
    //     }
    //   }
    // )
  }


  calculateTotalPrice(attendances:any[]){
    this.totalPrice = 0;
    attendances.map(a => {
      this.totalPrice += a.price;
    });
  }
}

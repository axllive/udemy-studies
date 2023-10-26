import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyReqiestCount = 0;

  constructor( private spinnerService: NgxSpinnerService ) { }

  busy(){
    this.busyReqiestCount++;
    this.spinnerService.show(undefined, {
      type: 'ball-grid-beat',
      bdColor: 'rgba(255, 255, 255, 1)',
      color: "rgba(255, 255, 255, 1)",
      fullScreen: true,
    })
  }

  idle(){
    this.busyReqiestCount--;
    if(this.busyReqiestCount <=0){
      this.busyReqiestCount = 0;
      this.spinnerService.hide();
    }
  }
}

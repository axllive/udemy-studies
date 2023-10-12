import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  /* para comunicação pai-filho entre nível componentes */
/*   @Input() usersFromHomeComponent : any; */
  /* relação filho-pai entre nível componentes */
  @Output() cancelRegister = new EventEmitter();
  
  model:any = {}
  usrname: string = "";
  
  constructor( private accountService: AccountService, private router: Router, private toast: ToastrService ) {}

  ngOnInit(): void {}

  register()
  {  
    this.accountService.register(this.model).subscribe({
      next: () =>{
        this.cancel();
        this.login();
      },
      error: err =>{
        let errorJ = JSON.parse(JSON.stringify(err));
        
        if (errorJ.error.errors.password != undefined) this.toast.error(errorJ.error.errors.password);  
        if (errorJ.error.errors.username != undefined) this.toast.error(errorJ.error.errors.username);  
        
      }
    })
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: () => {   
        this.usrname = this.accountService.getCurrentUser();
        this.router.navigateByUrl('/members');
       },
       error: error =>  this.toast.error(error.error)
    })
  }

  cancel()
  {
    this.cancelRegister.emit(false);
  }
}

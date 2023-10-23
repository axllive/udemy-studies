import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  model: any = {}
  usr: any = {}
  statusOnline = false;
  dropdownIsOpen = false;
  usrname : string = this.accountService.getCurrentUser();
  //whe angular is in strict mode, is mandatory initalize the proprieties
  currentUser$: Observable< User | null> = of(null);

  constructor(public accountService : AccountService, private router: Router, private toast: ToastrService) {}

  ngOnInit(): void{
    this.currentUser$ = this.accountService.currentUser$;
    if (this.accountService.getCurrentUser() != "") {
      let jsonUsr = JSON.parse( this.accountService.getCurrentUser() );
      console.log(jsonUsr);
      this.usrname = jsonUsr.username;
      this.usr = jsonUsr;
      if(jsonUsr) this.statusOnline = true;
    }
  }
  
  login(){
    this.accountService.login(this.model).subscribe({
      next: () => {   
        let jsonUsr = JSON.parse( this.accountService.getCurrentUser() );
        this.usrname = jsonUsr.username;
        this.usr = jsonUsr;
        this.statusOnline = true;
        this.router.navigateByUrl('/members');
        this.toast.success('Login successful')
       }
    })
  }

  logout() 
  {
    this.accountService.logout();
    this.usrname = "";
    this.router.navigateByUrl("/");
  }

  statusChangeOffline(e: Event){ this.statusOnline = false;  e.stopPropagation(); }
  
  statusChangeOnline(e: Event){ this.statusOnline = true;  e.stopPropagation(); }
  
}

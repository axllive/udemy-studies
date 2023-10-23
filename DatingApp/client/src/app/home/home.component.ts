import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  currentUser$: Observable< User | null> = of(null);
  registerMode = false;
  users: any;

  constructor( public accountService : AccountService, private toast: ToastrService ) {  }
  
  ngOnInit(): void 
  {
    this.currentUser$ = this.accountService.currentUser$;
    if (this.accountService.getCurrentUser() != "" ){
      this.accountService.getUsers().subscribe( data => this.users = data );
    }
  }

  registerToggle()
  {
    this.registerMode =  !this.registerMode;
  }

  cancelRegisterMode( event : boolean )
  {
    this.registerMode = event;
  }

}

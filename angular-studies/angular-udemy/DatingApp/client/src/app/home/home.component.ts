import { HttpClient } from '@angular/common/http';
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

  constructor( private http: HttpClient, public accountService : AccountService, private toast: ToastrService ) {  }
  
  ngOnInit(): void 
  {
    this.currentUser$ = this.accountService.currentUser$;
    this.getUsers();
  }

  registerToggle()
  {
    this.registerMode =  !this.registerMode;
  }

  getUsers()
  {
    this.http.get('https://localhost:5001/api/user').subscribe({
      next: response => this.users = response,
      error: error => { console.log(error) },
      complete: () => { }
    })
  }

  cancelRegisterMode( event : boolean )
  {
    this.registerMode = event;
  }

}

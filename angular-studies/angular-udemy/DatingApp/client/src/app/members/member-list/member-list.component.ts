import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: any;

  ngOnInit(): void { this.getUsers(this.accountService.currentUser$); }
  constructor( private http: HttpClient, private accountService : AccountService ) {  }

  getUsers(model: any)
  {
    this.http.get('https://localhost:5001/api/user', model).subscribe({
      next: response => this.users = response,
      error: error => { console.log(error) },
      complete: () => { console.log('Request has completed') }
    })
  }
}

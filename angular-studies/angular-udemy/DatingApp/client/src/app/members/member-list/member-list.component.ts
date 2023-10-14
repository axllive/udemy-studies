import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: any;

  constructor( private accountService : AccountService ) {  }

  
  ngOnInit(): void { this.accountService.getUsers().subscribe( data => { this.users = data } )  }

}

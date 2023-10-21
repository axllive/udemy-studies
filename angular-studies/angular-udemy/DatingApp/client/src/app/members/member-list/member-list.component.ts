import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  constructor( public accountService : AccountService, public membersService: MembersService ) {  }

  
  ngOnInit(): void { this.membersService.getMembers().subscribe();  }

}

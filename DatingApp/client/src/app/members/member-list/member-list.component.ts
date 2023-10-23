import { Component, OnInit } from '@angular/core';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  constructor( public membersService: MembersService ) { this.membersService.getMembers().subscribe();  }

  
  ngOnInit(): void {  }

}

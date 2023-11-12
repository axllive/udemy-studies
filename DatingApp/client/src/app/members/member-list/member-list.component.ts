import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
members: Member[] = [];
pagination: Pagination | undefined;
usrParams: UserParams | undefined;
usr: User | undefined;
genderList = [{value: 'male', display: 'Woman'}, {value: 'female', display: 'Man'}, {value: 'Other', display: 'All'}];

  constructor( public membersService: MembersService, private accountService: AccountService ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user =>{
          if (user){
            this.usrParams = new UserParams(user);
            this.usr = user;
          }
        }
      });
      if(this.usrParams)
        this.loadMembers();  
    }

  
  ngOnInit(): void {  }

  loadMembers(){
    if(this.usrParams)
    this.membersService.getMembers(this.usrParams).subscribe({
      next: response => {
        if(response.result && response.pagination)
        {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      }
    })
  }

  resetFilter(){
    if(this.usr){
      this.usrParams = new UserParams(this.usr);
      this.loadMembers()
    }
  }

  pageChanged(event: any){
    if(this.usrParams)
      if(this.usrParams.pageNumber !== event.page){
        this.usrParams.pageNumber = event.page;
        this.loadMembers();
      }
  }
}

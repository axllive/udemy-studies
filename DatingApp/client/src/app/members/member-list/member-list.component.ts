import { Component, OnInit } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
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

  constructor( public membersService: MembersService, private accountService: AccountService, router: Router ) {
      router.events.subscribe({
        next: (event) => {
          if(event instanceof NavigationStart){
            // Verifica se a navegação é devido a um popstate (pressionar o botão voltar)
            const isPopState = event.navigationTrigger === 'popstate';

            // Verifica se há um estado anterior na pilha de histórico
            const hasPreviousState = router.getCurrentNavigation()?.previousNavigation != null;
            
            //meu estado de filtro só é recuperado se há um evento VOLTAR
            if (isPopState && hasPreviousState)
              this.usrParams = this.membersService.getUserParams();
          }
            
        }
      });
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user =>{
          if (user){
            let prms = this.membersService.getUserParams();
            if(prms){
              this.usrParams = this.membersService.getUserParams();
            }
            else{ this.usrParams = new UserParams(user); }
            this.usr = user;
          }
        }
      });
      if(this.usrParams)
        this.loadMembers();  
    }

  
  ngOnInit(): void {  }

  loadMembers(){
    if(this.usrParams){
      this.membersService.setUserParams(this.usrParams);
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

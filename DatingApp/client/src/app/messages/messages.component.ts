import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/pagination';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';
import { Member } from '../_models/member';
import { chatedWith } from '../_models/chatedWith';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages?: Message[];
  chatedUsers?: chatedWith[];
  selectedChatUsr?: chatedWith;
  usersChated?: Member[];
  pagination?: Pagination;
  paginationChated?: Pagination;
  container = 'Outbox';
  pageNumber = 1;
  pageSize = 5;
  chatPageNumber = 1;
  chatPageSize = 8;
  searchQuery?: string;

  /**
   *
   */
  constructor(private messageService: MessageService, private accountService: AccountService) {  }

  ngOnInit(): void {
    this.loadChatedWith();
  }

  loadMessages(){
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next: response =>{
        this.messages = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  loadChatedWith(){
    this.messageService.getChatedUsers(this.chatPageNumber, this.chatPageSize).subscribe({
      next: response =>{
        this.chatedUsers = response.result;
        this.paginationChated = response.pagination;
      }
    })
  }
  pageChanged(event: any){
    if(this.chatPageNumber != event.page){
      this.chatPageNumber = event.page;
      this.loadChatedWith();
    }
  }
  pageChanges(event: any){
    if(this.pageNumber != event.page){
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

  getMessageThread(usr: chatedWith){
    this.selectedChatUsr = usr;
    this.messageService.getMessageThread(this.chatPageNumber, this.chatPageSize, usr.username).subscribe({
      next: response =>{
        this.messages = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  filter(){
      if(this.searchQuery){
        var query: string = this.searchQuery;
        this.chatedUsers = this.chatedUsers?.filter( x=> x.username.includes(query))}
  }
}

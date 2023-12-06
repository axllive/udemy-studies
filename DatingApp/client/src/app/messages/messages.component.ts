import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/pagination';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';
import { Member } from '../_models/member';
import { chatedWith } from '../_models/chatedWith';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages?: Message[];
  chatedUsers?: chatedWith[];
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
  constructor(private messageService: MessageService) {  }

  ngOnInit(): void {
    this.loadMessages();
    this.loadChatedWith();
  }

  loadMessages(){
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next: response =>{
        this.messages = response.result;
        console.log(this.messages);
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

  filter(){
      if(this.searchQuery){
        var query: string = this.searchQuery;
        this.chatedUsers = this.chatedUsers?.filter( x=> x.username.includes(query))}
  }
}

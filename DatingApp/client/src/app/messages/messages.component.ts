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
  container = 'Outbox';
  pageNumber = 1;
  pageSize = 5;

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
    this.messageService.getChatedUsers(this.pageNumber, this.pageSize).subscribe({
      next: response =>{
        this.chatedUsers = response.result;
        console.log(this.chatedUsers);
        this.pagination = response.pagination;
      }
    })
  }

  pageChanges(event: any){
    if(this.pageNumber != event.page){
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }
}

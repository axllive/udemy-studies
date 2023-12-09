import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit{
  @Input() messages: Message[] | undefined;
  currentUsr?: string;

  constructor(private accountService: AccountService) {  }

  ngOnInit(): void { 
    if (this.accountService.getCurrentUser() != "") {
      let jsonUsr = JSON.parse( this.accountService.getCurrentUser() );
      this.currentUsr = jsonUsr.username;
    }
  }

}

import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/message';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
  imports: [CommonModule, TimeagoModule]
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

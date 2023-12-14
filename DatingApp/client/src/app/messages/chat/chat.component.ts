import { CommonModule } from '@angular/common';
import { Component, Input,  ElementRef, ViewChild} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/message';
import { AccountService } from 'src/app/_services/account.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
  imports: [CommonModule, TimeagoModule, FormsModule]
})
export class ChatComponent{
  @Input() messages: Message[] | undefined;
  @Input() username?: string;
  @ViewChild('scroll', { read: ElementRef }) public scroll?: ElementRef;
  @ViewChild('messageForm') messageForm?: NgForm;
  currentUsr?: string;
  messageContent = '';

  constructor(private accountService: AccountService, private messageService: MessageService) {  }

  ngAfterViewChecked(): void { 
    if (this.accountService.getCurrentUser() != "") {
      let jsonUsr = JSON.parse( this.accountService.getCurrentUser() );
      this.currentUsr = jsonUsr.username;
    }
    if(this.scroll){
      this.scroll.nativeElement.scrollTop = this.scroll.nativeElement.scrollHeight;
    }      
  }

  sendMessage(){
    if(this.username)
    this.messageService.sendMessage(this.username, this.messageContent).subscribe({
      next: message => {
        this.messages?.push(message);
        this.messageForm?.reset();
      }
    })
  }
}

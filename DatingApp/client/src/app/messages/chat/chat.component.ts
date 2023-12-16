import { CommonModule } from '@angular/common';
import { Component, Input,  ElementRef, ViewChild, OnInit, AfterViewInit} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { ToastrService } from 'ngx-toastr';
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
export class ChatComponent {
  @Input() messages: Message[] | undefined;
  @Input() username?: string;
  @ViewChild('scroll', { read: ElementRef }) public scroll?: ElementRef;
  @ViewChild('messageForm') messageForm?: NgForm;
  currentUsr?: string;
  messageContent = '';
  @Input('hover-class') hoverClass:any;
  tempHour = '';
  parentDom = {} as Document;

  constructor(private accountService: AccountService, private messageService: MessageService, private toast: ToastrService) {  }

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

  cancelSend(msg: Message){
    if(msg.dateread == null)
      this.messageService.unsendMessage(msg.id).subscribe({
      next: () => {
        
        this.messages?.splice( this.messages?.findIndex( x => x.id == msg.id) , 1);

        this.toast.success('Message unsent');
      }
      })
  }

  delIcon(event: any){
    this.tempHour = event.target.parentNode.children[0].textContent;
    this.parentDom =event.target.parentNode.children[0];
    event.target.parentNode.children[0].textContent = 'Unsend ';
    event.target.classList.remove('fa-circle-thin');
    event.target.classList.add('fa-times-circle-o');
    event.target.classList.add('text-danger');
  }
  unreadIcon(event: any){
    this.parentDom.textContent = this.tempHour;
    event.target.classList.remove('fa-times-circle-o');
    event.target.classList.remove('text-danger');
    event.target.classList.add('fa-circle-thin');
  }

}

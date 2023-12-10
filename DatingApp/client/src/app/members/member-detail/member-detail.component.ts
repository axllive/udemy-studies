import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { AccountService } from 'src/app/_services/account.service';
import { Pagination } from 'src/app/_models/pagination';
import { ChatComponent } from 'src/app/messages/chat/chat.component';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports:[CommonModule, TabsModule, GalleryModule, TimeagoModule, ChatComponent]
})
export class MemberDetailComponent implements OnInit{
  messages?: Message[];
  member: Member | undefined;
  images: GalleryItem[] = [];
  mainPhoto: string | undefined = "";
  chatPageNumber = 1;
  chatPageSize = 8;
  pagination?: Pagination;

  constructor(private messageService: MessageService, private accountService: AccountService , private memberService: MembersService, private route: ActivatedRoute, private location: Location, private toast: ToastrService) {  }

  ngOnInit(): void { this.loadMember(); }

  loadMember(){
    const username = this.route.snapshot.paramMap.get('name');
    if (!username) return;
    this.memberService.getMemberByName(username).subscribe({
      next: member => {
        this.member = member;
        this.mainPhoto = member?.photos.find(x => x.ismain)?.url;
        this.getImages();
        if(this.member) 
        this.getMessageThread(this.member); 
      }
    })
  }

  getImages(){
    if(!this.member) return;
    for (const photo of this.member?.photos){
      this.images.push(new ImageItem({src: photo.url, thumb:photo.url}))
    }
  }

  voltar() {
    this.location.back();
  }


  addLike(member: Member){
    this.memberService.addLike(member.username).subscribe({
      next: () => {
        this.toast.success('You have liked ' + member.kwonas);
      }
    })
  }

  getMessageThread(usr: Member){
    this.messageService.getMessageThread(this.chatPageNumber, this.chatPageSize, usr.username).subscribe({
      next: response =>{
        this.messages = response.result;
        console.log(this.messages);
        this.pagination = response.pagination;
      }
    })
  }
}

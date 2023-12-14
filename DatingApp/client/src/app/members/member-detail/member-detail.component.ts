import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
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
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  messages?: Message[];
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  mainPhoto: string | undefined = "";
  chatPageNumber = 1;
  chatPageSize = 8;
  pagination?: Pagination;
  activeTab?: TabDirective;

  constructor(private messageService: MessageService, private accountService: AccountService , private memberService: MembersService, private route: ActivatedRoute, private location: Location, private toast: ToastrService) {  }

  ngOnInit(): void { 
    this.route.data.subscribe({
      next: data => this.member = data['member']
    });
    this.route.queryParams.subscribe({
      next: params =>{
        params['tab'] && this.selectTab(params['tab']);
      }
    });
    
    this.getImages();
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading == 'Messages'){
      if(this.member)
        this.getMessageThread(this.member);
    }
  }

  selectTab(heading: string){
    if(this.memberTabs){
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  loadMember(){
    const username = this.route.snapshot.paramMap.get('name');
    if (!username) return;
    this.memberService.getMemberByName(username).subscribe({
      next: member => {
        this.member = member;
        
      }
    })
  }

  getImages(){
    if(!this.member) return;
    this.mainPhoto = this.member?.photos.find(x => x.ismain)?.url;
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
        this.pagination = response.pagination;
      }
    })
  }
}

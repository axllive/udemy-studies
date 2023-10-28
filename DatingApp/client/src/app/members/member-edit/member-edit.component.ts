import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { GalleryItem, ImageItem } from 'ng-gallery';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined
  images: GalleryItem[] = [];

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any){
    if(this.editForm?.dirty) {return $event.returnValue = true;}
    else return $event.returnValue = false;
  }
  
  member: Member | undefined;
  user: User | null = null;
  
  constructor( private toast: ToastrService , private memberService: MembersService, private accountService: AccountService ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: usr => this.user = usr
    })
  }

  ngOnInit(): void {
    this.loadMember();
  }
  
  loadMember(){
    if(!this.user) return;
    this.memberService.getMemberByName(this.user.username).subscribe({
      next: membr => {
        this.member = membr
        this.getImages();
      }
    })
  }

  updateMember(){
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toast.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      }
    })
    
  }

  getImages(){
    if(!this.member) return;
    for (const photo of this.member?.photos){
      this.images.push(new ImageItem({src: photo.url, thumb:photo.url}))
    }
  }
}

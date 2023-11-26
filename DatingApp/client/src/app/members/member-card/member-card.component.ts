import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit{
  @Input() member: Member | undefined;
  mainPhotoUrl: string | undefined= "";
  
  constructor(private memberService: MembersService, private toast: ToastrService) {}
  ngOnInit(): void {
    this.mainPhotoUrl = this.member?.photos.find( (item) => item.ismain )?.url;
  }


  addLike(member: Member){
    this.memberService.addLike(member.username).subscribe({
      next: () => {
        this.toast.success('You have liked ' + member.kwonas);
      }
    })
  }
}

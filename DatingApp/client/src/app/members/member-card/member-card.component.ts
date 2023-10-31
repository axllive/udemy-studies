import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit{
  @Input() member: Member | undefined;
  mainPhotoUrl: string | undefined= "";
  
  constructor() {}
  ngOnInit(): void {
    this.mainPhotoUrl = this.member?.photos.find( (item) => item.ismain )?.url;
  }

}

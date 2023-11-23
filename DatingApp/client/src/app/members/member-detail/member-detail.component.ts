import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports:[CommonModule, TabsModule, GalleryModule, TimeagoModule]
})
export class MemberDetailComponent implements OnInit{
  member: Member | undefined;
  images: GalleryItem[] = [];
  mainPhoto: string | undefined = "";

  constructor(private memberService: MembersService, private route: ActivatedRoute, private location: Location) {  }
  
  ngOnInit(): void { this.loadMember() }

  loadMember(){
    const username = this.route.snapshot.paramMap.get('name');
    if (!username) return;
    this.memberService.getMemberByName(username).subscribe({
      next: member => {
        this.member = member,
        this.mainPhoto = member?.photos.find(x => x.ismain)?.url;
        this.getImages()
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
}

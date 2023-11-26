import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit{
members: Member[] | undefined;
predicate = 'liked';
pageNumber = 1;
pageSize = 12;
pagination: Pagination | undefined;

constructor( private memeberService: MembersService ) {}

ngOnInit(): void { this.loadLikes() }

loadLikes(){
  this.memeberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
    next: response =>{
      this.members = response.result;
      console.log(this.members);
      this.pagination = response.pagination;
    }
  })
}

pageChanged(event: any){
  if(this.pagination)
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadLikes();
    }
}
}

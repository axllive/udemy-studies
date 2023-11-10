import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  usrTokenObject = localStorage.getItem("user");
  private currentUsersSource = new BehaviorSubject< any[] | null> (null);
  private currentUserSource = new BehaviorSubject< any | null> (null);
  currentUsers$ = this.currentUsersSource.asObservable();
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>;

  constructor(private http: HttpClient) { }

  getMembers(page?:number, itemsPerPage?:number){
    let params = new HttpParams;

    if(page && itemsPerPage){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<Member[]>
    (this.baseUrl + 'users',{observe: 'response', params}).pipe(
      map(response =>{
        if(response.body){
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if(pagination){
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
      })
    )
    
  }

  getMemberById(id: number){
    return this.http.get<Member>(this.baseUrl + 'users/user/'+ id ).pipe(
      //para usar como array no component que recebe o objeto
      //é necessário tipá-lo dentro do Observable
      map(( (member : Member) =>{
        const usr = member;
        if (usr) {
          this.currentUserSource.next(usr);
        }
      })
    )
    )
  }

  getMemberByName(name: string){
    return this.http.get<Member>(this.baseUrl + 'users/user/byname/'+ name ).pipe(
      //para usar como array no component que recebe o objeto
      //é necessário tipá-lo dentro do Observable
      map(( (member : Member) =>{
        const usr = member;
        if (usr) {
          this.currentUserSource.next(usr);
          return usr;
        }
        else return;
      })
    )
    )
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member);
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/'+ photoId,  photoId);
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

}

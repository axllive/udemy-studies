import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { getPaginatedResults, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  usrTokenObject = localStorage.getItem("user");
  private currentUsersSource = new BehaviorSubject< any[] | null> (null);
  private currentUserSource = new BehaviorSubject< any | null> (null);
  currentUsers$ = this.currentUsersSource.asObservable();
  usrParams: UserParams | undefined;
  

  constructor(private http: HttpClient) { }

  getMembers(userParams: UserParams){

    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    this.usrParams = userParams;
    return getPaginatedResults<Member[]>(this.baseUrl + 'users', params, this.http);
    
  }

  getUserParams(){
    return this.usrParams;
  }

  setUserParams(params: UserParams){
    this.usrParams = params;
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
        this.currentUserSource.next(usr);
        return usr;
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

  addLike(username: string){
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number){
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);

    return getPaginatedResults<Member[]>(this.baseUrl + 'likes', params, this.http);
  }

}

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  usrTokenObject = localStorage.getItem("user");
  private currentUsersSource = new BehaviorSubject< any[] | null> (null);
  private currentUserSource = new BehaviorSubject< any | null> (null);
  currentUsers$ = this.currentUsersSource.asObservable();

  constructor(private http: HttpClient) { }

  getMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'users?jsonUsr='+ this.usrTokenObject?.toString()).pipe(
      //para usar como array no component que recebe o objeto
      //é necessário tipá-lo dentro do Observable
      map(( (member : Member[]) =>{
        const usr = member;
        if (usr) {
          this.currentUsersSource.next(usr);
        }
      })
    )
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


}

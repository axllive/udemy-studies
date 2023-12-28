import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  //objeto< tipo1 | tipo2 > ->>>o objeto pode assumir os dois tipos
  private currentUserSource = new BehaviorSubject<User | null>(null);
  private currentUserNameSource = new BehaviorSubject<string | null>(null);
  private currentUsersSource = new BehaviorSubject< any[] | null> (null);
  //convenção de Angular/TypeScript o $ ao final da variável, significa que ela
  //é um Observable
  currentUser$ = this.currentUserSource.asObservable();
  currentUserName$ = this.currentUserNameSource.asObservable();
  currentUsers$ = this.currentUsersSource.asObservable();

  constructor( private http: HttpClient, private toast: ToastrService ) { }

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map( ( (member : User) =>{
        const usr = member;
        if (usr) {
          localStorage.setItem('user', JSON.stringify(usr));
          this.currentUserSource.next(usr);
        }
      })
      )
    )
  }

  register(model: any)
  {
    return this.http.post<User>( this.baseUrl + 'account/register', model ).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  getCurrentUser(){
    const usrJson = localStorage.getItem("user");
    if (usrJson != null) {
      const usrObj = JSON.parse(usrJson.toString());
      this.currentUserNameSource.next(usrObj.username);
      return usrJson.toString();
    }
    else {
      return "";
    }

  }

  getUsers()
  {
    return this.http.get<User[]>('https://localhost:5001/api/users?jsonUsr=' +   this.getCurrentUser().toString(),  ).pipe(
      //para usar como array no component que recebe o objeto
      //é necessário tipá-lo dentro do Observable
      map(( (member : User[]) =>{
        const usr = member;
        if (usr) {
          this.currentUsersSource.next(usr);
        }
      })
    )
    )
  }

  setCurrentUser (usr : User){
    usr.roles = [];
    const roles = this.getDecodedToken(usr.token).role;
    Array.isArray(roles) ? usr.roles = roles: usr.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(usr));
    this.currentUserSource.next(usr);
  }

  logout () 
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]));
  }
}

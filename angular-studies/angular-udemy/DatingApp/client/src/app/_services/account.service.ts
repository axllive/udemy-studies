import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  //objeto< tipo1 | tipo2 > ->>>o objeto pode assumir os dois tipos
  private currentUserSource = new BehaviorSubject<User | null>(null);
  //convenção de Angular/TypeScript o $ ao final da variável, significa que ela
  //é um Observable
  currentUser$ = this.currentUserSource.asObservable();

  constructor( private http: HttpClient ) { }

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

  setCurrentUser (usr : User){
    this.currentUserSource.next(usr);
  }

  logout () 
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}

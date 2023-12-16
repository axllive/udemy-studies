import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginatedResults, getPaginationHeaders } from './paginationHelper';
import { Message } from '../_models/message';
import { chatedWith } from '../_models/chatedWith';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMessages(pageNumber: number, pageSize: number, container: string){

      let params = getPaginationHeaders(pageNumber, pageSize);

      params = params.append('Container', container);
      
      return getPaginatedResults<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getChatedUsers(pageNumber: number, pageSize: number){
    
    let params = getPaginationHeaders(pageNumber, pageSize);

    return getPaginatedResults<chatedWith[]>(this.baseUrl + 'messages/chatUsers', params, this.http );
  }

  getMessageThread(pageNumber: number, pageSize: number, username: string){
    
    let params = getPaginationHeaders(pageNumber, pageSize);

    return getPaginatedResults<Message[]>(this.baseUrl + 'messages/thread/?username=' + username, params, this.http );
  }

  sendMessage(username: string, content: string){
    return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername: username, content});
  }

  unsendMessage(messageid: number){
    return this.http.delete( this.baseUrl + 'messages/unsend/?messageId=' + messageid );
  }
}

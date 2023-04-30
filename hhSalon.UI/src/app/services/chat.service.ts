import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ChatItem } from '../models/chatItem';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Message } from '../models/message';
import { NgToastService } from 'ng-angular-popup';
import { UsersService } from './users.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class ChatService {
  private url = 'Chat';
  private chatUrl = environment.apiUrl.split('/api')[0];

  private chatConnection?: HubConnection;
  
  userId: string = '';
  user:any;


  messages: Message[] = [];
  
  privateMessageInitiated = false;

  constructor(private http: HttpClient,
    private toast: NgToastService,
    private usersService:UsersService,
    private router: Router
    ) {   }

  public addUser(userId: string){      
    //let user = {id: userId};
    return this.http.post(`${environment.apiUrl}/${this.url}/add-user/${userId}`, userId);
   }


   public getChatList(userId: string):Observable<ChatItem[]>{      
    return this.http.get<ChatItem[]>(`${environment.apiUrl}/${this.url}/chat/${userId}`);
   }

   public getMessageOfUser(userId: string, fromUserId: string): Observable<Message[]>{
    return this.http.get<Message[]>(`${environment.apiUrl}/${this.url}/chat/messages?user=${userId}&from=${fromUserId}`);
   }

   public saveMessage(message: Message){
    return this.http.post<Message[]>(`${environment.apiUrl}/${this.url}/save-message`, message);
   }


   createChatConnection(userId: string){
    console.log("CREATE CHAT CONN");
    
    this.userId = userId;
    this.usersService.getUserById(userId).subscribe(
      user => this.user = user
    )
    
    this.chatConnection = new HubConnectionBuilder()
      .withUrl(`${this.chatUrl}/hubs/chat`).withAutomaticReconnect().build();

    this.chatConnection.start().catch(error => {      
      console.log(error);  
    })

    this.chatConnection.on('UserConnected', () => {
      this.addUserConnectionId();
    })




      // this.chatConnection.on('OpenPrivateChat', (newMessage: Message) => {
      //   this.messages = [...this.messages, newMessage]; // у отримувача
        
      //   console.log(this.router.url);
      //   if(this.router.url != '/chat/' + newMessage.fromId){
      //     console.log('/chat/' + newMessage.fromId);
      //     console.log('url: ' + this.router.url);
          
      //     this.toast.info({detail: "New message!", summary: newMessage.fromUser.userName + ": " + newMessage.content});  
      //   }        
        
      // })

      this.chatConnection.on('NewPrivateMessage', (newMessage: Message) => {

        if(this.router.url != '/chat/' + newMessage.fromId){
          console.log('/chat/' + newMessage.fromId);
          console.log('url: ' + this.router.url);
          console.log('TOAST');
          console.log(newMessage.fromUser.userName + ": " + newMessage.content);
          
          let mess =  newMessage.fromUser.userName + ": " + newMessage.content;
          this.toast.info({detail: "New message!", summary: mess.toString(), duration: 10000});  
        }    
        else{
          this.messages = [...this.messages, newMessage];           
        }
        
      })


      this.chatConnection.on('ClosePrivateChat', () => {
        //this.messages = [];
        //this.modalService.dismissAll();
      })
   }

   async addUserConnectionId(){
    return this.chatConnection?.invoke('AddUserConnectionId', this.userId)
      .catch(error => console.log(error)
      )
  }


  stopChatConnection(){
    this.chatConnection?.stop().catch(error => console.log(error));
  }


  // createPrivateChat(to: any, from: any){  
  //   const message: Message = {
  //     fromId: this.userId,
  //     fromUser: this.user,
  //     toId: to.id,
  //     toUser: to,
  //     content: '',
  //     date: new Date()
  //   };

  //   this.usersService.getUserById(from.id).subscribe(
  //     user => message.fromUser = user
  //   )
                
  //       // this.saveMessage(message).subscribe();

  //       return this.chatConnection?.invoke('CreatePrivateChat', message).then(() => {
  //         //this.messages = [...this.messages, message];// у нас
  //         console.log("create  a chat");
          
  //         //console.log(this.messages);
  //       })
  //         .catch(error =>{
  //           console.log(error.error)          
  //           //this.messages = [...this.messages, message];
  //         } 
  //       )

  // }


  async sendMessage(from: any, to: any, content: string){
  
        const message: Message = {
          fromId: from.id,
          fromUser: from,
          toId: to.id,
          toUser: to,
          content: content,
          date: new Date()
        };
        


        //this.messages = [...this.messages, message];

        this.saveMessage(message).subscribe();

        //this.createPrivateChat(message.toUser, message.fromUser);
       
        
        // if(isFirstMessage){
        //   //this.privateMessageInitiated = true;
        //   return this.chatConnection?.invoke('CreatePrivateChat', message).then(() => {
        
        //     this.messages = [...this.messages, message];// у нас
            
            
        //   })
        //     .catch(error =>{
        //       console.log(error.error)          
        //       this.messages = [...this.messages, message];
        //     } 
        //     )

        // } else{
          //this.toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
          return this.chatConnection?.invoke('ReceivePrivateMessage', message).then( () => {
            this.messages = [...this.messages, message];// у нас
            
          })
            .catch(error => {
              console.log(error.error)
              this.messages = [...this.messages, message];// у нас
            }
          )
        //}

      }


      async closePrivateChatMessage(){
        return this.chatConnection?.invoke('RemovePrivateChat', this.userId)
        .catch(error => console.log(error)
        )
      }

  }

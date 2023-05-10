import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, map } from 'rxjs';
import { ChatItem } from '../models/chatItem';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Message } from '../models/message';
import { NgToastService } from 'ng-angular-popup';
import { UsersService } from './users.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

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


  chatList: ChatItem[] = [];
  unreadCount: number = 0;


  constructor(private http: HttpClient,
     private toast: NgToastService,
    //private toast: ToastrService,
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

   public getMessageOfUser(userId: string, otherUserId: string): Observable<Message[]>{
    return this.http.get<Message[]>(`${environment.apiUrl}/${this.url}/chat/messages?user=${userId}&other=${otherUserId}`);
   }

   public saveMessage(message: Message){
    return this.http.post<Message[]>(`${environment.apiUrl}/${this.url}/save-message`, message);
   }

   public updateMessage(message: Message){
    return this.http.put<Message[]>(`${environment.apiUrl}/${this.url}`, message);
   }


   createChatConnection(userId: string){        
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


        this.getChatList(this.userId).subscribe(
          list => {
             this.chatList = list;
             this.unreadCount  = list.filter(l => !l.isRead && l.toUserId === this.userId).length;                
          }
        )
      })


      this.chatConnection.on('NewPrivateMessage', (newMessage: Message) => {
        
        
          if(this.router.url != '/chat/' + newMessage.fromId){
              newMessage.isRead = false;

              let mess =  newMessage.fromUser.userName + ": " + newMessage.content;

              this.toast.info({detail: "New message!", summary: mess.toString()});          

              this.saveMessage(newMessage).subscribe(
                () => { 
                  //this.toast.info({detail: "New message!", summary: mess.toString(), duration: 15000});          
                  
                });

          }    
          else{
                    newMessage.isRead = true;

                    this.messages = [...this.messages, newMessage];             

                    this.messages.map(
                      message => {
                        if(message.toId === this.userId && message.isRead === false){
                          message.isRead = true;
                        }
                      }
                    )


                    this.saveMessage(newMessage).subscribe(
                      {
                        next: () =>{
                          
                        },
                        error: (error)=> console.log(error.error)            
                      });


                      this.chatConnection?.invoke('ReadPrivateMessage', newMessage).then( () => {  
                        
                        })
                          .catch(error => {
                            console.log(error.error)
                          }
                        )
                  }
          
          this.updateChatList(newMessage);
          
          this.unreadCount = this.chatList.filter(l => l.messageUnreadCount > 0 && l.toUserId === this.userId).length;
          
          return;
      })


      this.chatConnection.on('MyMessageIsRead', (message: Message) => {
        this.messages.filter(m => !m.isRead).map(m => m.isRead = true);        

        let chat = this.chatList.filter(c => c.userId === this.userId && c.toUserId === message.toId)[0];
        chat.isRead = true;
        chat.messageUnreadCount = 0;
      })


      this.chatConnection.on('ClosePrivateChat', () => {
        
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



  async sendMessage(from: any, to: any, content: string){
  
        const message: Message = {
          fromId: from.id,
          fromUser: from,
          toId: to.id,
          toUser: to,
          content: content,
          date: new Date(),
          isRead: false
        };
        this.updateChatList(message);

        this.chatConnection?.invoke('ReceivePrivateMessage', message).then( () => {
            this.messages = [...this.messages, message];// у нас
            
          })
            .catch(error => {
              //if not online
              console.log('NOT ONLINE');
              
              this.saveMessage(message).subscribe(
                () => { 
                  
                });
              console.log(error.error)
              this.messages = [...this.messages, message];// у нас
            }
          )

      }


      async closePrivateChatMessage(){
        return this.chatConnection?.invoke('RemovePrivateChat', this.userId)
        .catch(error => console.log(error)
        )
      }


      updateChatList(newMessage:Message){

        this.chatList.map(
          c => {
            if((c.userId === newMessage.fromId && c.toUserId == newMessage.toId) || (c.userId === newMessage.toId && c.toUserId == newMessage.fromId)){
              c.lastMessage = newMessage.content;
              c.userId = newMessage.fromId;
              c.user = newMessage.fromUser;

              c.toUserId = newMessage.toId;
              c.toUser = newMessage.toUser;

              c.isRead = newMessage.isRead;
              c.date = newMessage.date;

              if(!c.isRead)
                c.messageUnreadCount++;
            }            
          }
        )

        

        if(this.chatList.filter(c => c.toUserId === newMessage.toId || c.userId === newMessage.toId).length === 0){
          let item: ChatItem = {
            lastMessage : newMessage.content,
            userId : newMessage.fromId,
            user : newMessage.fromUser,

            messageUnreadCount: 1,

            toUserId : newMessage.toId,
            toUser : newMessage.toUser,
            isRead : newMessage.isRead,
            date: newMessage.date
          }

          if(newMessage.isRead){
            item.messageUnreadCount = 0;
          }else{
            item.messageUnreadCount = 1;
          }

          this.chatList = [...this.chatList, item]
        }

        this.chatList.sort((a, b) => (new Date(b.date).getTime() - new Date(a.date).getTime()));
        
      }


       updateMessages(){
          let mess: Message | undefined;
          
          
          this.messages.map(
            message => {
              if(message.toId === this.userId && !message.isRead){
                console.log(message);
                
                mess = message;
                message.isRead = true;
                this.updateMessage(message).subscribe();                
              }
            }
          )
          
          if(mess != undefined){
              this.unreadCount--;
              this.chatList.filter(l => l.userId === mess?.fromId && l.toUserId === this.userId)[0].messageUnreadCount = 0;

              
                this.chatConnection?.invoke('ReadPrivateMessage', mess).then( () => {
                  
                })
                  .catch(error => {
                    //if not online
                    console.log(error.error)
                  }
                )
          }
        
      }
  }

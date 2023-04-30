import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ChatItem } from 'src/app/models/chatItem';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy{
  toUserId: string = '';
  toUser: any;

  userId: string = '';

  chatList: ChatItem[] = [];

  tracker :any[] = []; 
  
  constructor(private route: ActivatedRoute,
    private auth: AuthService,
    private userStore: UserStoreService,
    public chatService: ChatService,
    private usersService: UsersService,
    private router: Router
    ){}


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.toUserId = params['toUser']; 
      
      if(this.toUserId != ''){
        this.usersService.getUserById(this.toUserId).subscribe(
          user => this.toUser = user
         )         
      }
      else{
        this.toUser = undefined;
      }
   })   


   this.userStore.getIdFromStore().subscribe(
    idValue => {
      const idFromToken = this.auth.getIdFromToken();
      this.userId = idValue || idFromToken;

      this.chatService.getChatList(this.userId).subscribe(
        list => {this.chatList = list;
          
           if(this.toUserId != ''){
            //this.chatService.createChatConnection(this.userId);
          //   if(
          //     this.chatList.filter(c => {
          //     c.userId == this.toUserId}).length == 0
          //   ){
          //     let item = new ChatItem();
          //     item.userId = this.toUserId;
          //     item.user = this.toUser;
          //     item.lastMessage = '';
          //     this.chatList.push(item);
          //     console.log(this.chatList);
              
          //   }
           }
        }
       )
   })    


   this.chatService.getMessageOfUser(this.userId, this.toUserId).subscribe(
    (messages) =>{
      this.chatService.messages = messages;     
    }
   )

   
   
  } 

  toUserChanged(chat: any){
    if(chat.userId === this.userId){
      this.toUserId = chat.toUserId;
    }
    else{
      this.toUserId = chat.userId;
    }

    // this.usersService.getUserById(this.toUserId).subscribe(
    //   user => this.toUser = user
    //  )    
     
    

     this.chatService.getMessageOfUser(this.userId, this.toUserId).subscribe(
      (messages) =>{
        this.chatService.messages = messages;     
      }
     )

    // this.chatService.createPrivateChat(this.toUser);
  
     //this.chatService.createChatConnection(this.userId);
     
     //this.chatService.closePrivateChatMessage();
    this.router.navigate(['chat', this.toUserId]);
  }


  sendMessage(content: any){
    this.tracker.push({
      to: this.toUserId,
    });
    //this.chatService.createPrivateChat(this.toUser, content);

    let count = this.tracker.filter(
      t => t.to == this.toUserId
    ).length;


    // this.usersService.getUserById(this.userId).subscribe(
    //   user => {
    //     if(count === 1)
    //     this.chatService.sendMessage(user, this.toUser, content, true);
    //   else
    //     this.chatService.sendMessage(user, this.toUser, content, false);
    //   }
    //)

    this.usersService.getUserById(this.userId).subscribe(
      user => {
        this.chatService.sendMessage(user, this.toUser, content);
      }
    )
   
  }

  ngOnDestroy(): void {
    // this.chatService.closePrivateChatMessage();
    //remove from group
    //when someone wants to write us, then the group will be created again and we'll receive the message (maybe :) )
  }
}

import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
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

  //chatList: ChatItem[] = [];

  chatSelected: ChatItem = new ChatItem();
  public now:any = new Date();
  
  constructor(private route: ActivatedRoute,
    private auth: AuthService,
    private userStore: UserStoreService,
    public chatService: ChatService,
    private usersService: UsersService,
    private router: Router,
    ){
    }


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
        list => {
          this.chatService.chatList = list;          
          
          
           if(this.toUserId != ''){
            this.chatService.getMessageOfUser(this.userId, this.toUserId).subscribe(
              (messages) =>{
                this.chatService.messages = messages;     
              }
             )

           }
        }
       )
   })    

      this.chatService.getChatList(this.userId).subscribe(
        list => {
          this.chatService.chatList = list;

          if(this.toUserId != ''){
            this.chatSelected = this.chatService.chatList.filter(l => 
              l.toUserId === this.toUserId || l.userId === this.toUserId)[0];
          }
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


     this.chatService.getMessageOfUser(this.userId, this.toUserId).subscribe(
      (messages) =>{
        this.chatService.messages = messages;   
        this.chatService.updateMessages();    
      }
     )

     this.chatSelected = chat;

    this.router.navigate(['chat', this.toUserId]);
  }


  sendMessage(content: any){
 
    let userThis:any;
    this.usersService.getUserById(this.userId).subscribe(
      userValue => {
        userThis = userValue;
        this.chatService.sendMessage(userThis, this.toUser, content);
      }
    )

   
  }

  ngOnDestroy(): void {
  }
}

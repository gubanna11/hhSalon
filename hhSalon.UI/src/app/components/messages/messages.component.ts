import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {  
  @Input() messages: Message[] = [];

  userId: string = '';

  constructor(private chatService: ChatService,
    private userStore: UserStoreService,
    private auth: AuthService,
    ){}

  ngOnInit(): void {
    this.chatService.updateMessages();

   this.userStore.getIdFromStore().subscribe(
    idValue => {
      const idFromToken = this.auth.getIdFromToken();
      this.userId = idValue || idFromToken;
   })  

  }
}

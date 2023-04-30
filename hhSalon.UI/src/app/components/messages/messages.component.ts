import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {  
  @Input() messages: Message[] = [];
//this userId/ If it equals to fromId than 'you'
  constructor(private usersService: UsersService){}

  ngOnInit(): void {
   
  }
}

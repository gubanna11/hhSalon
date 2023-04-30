import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  templateUrl: './chat-input.component.html',
  styleUrls: ['./chat-input.component.scss']
})
export class ChatInputComponent {
    content: string = ''  

    @Output() contentEmitter = new EventEmitter();
    @ViewChild('messageForm') messageForm: NgForm | undefined;

    sendMessage(){
      if(this.content.trim() !== ""){
        this.contentEmitter.emit(this.content);
      }
  
      this.content = '';
    }
}

import { Component, OnInit } from '@angular/core';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  message: any;
  constructor(private messagesService: MessageService) { }

  ngOnInit() {
    this.messagesService.getMessage().subscribe(message => { this.message = message; });
  }

}

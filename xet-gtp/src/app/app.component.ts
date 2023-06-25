import { Component } from '@angular/core';
import { Dialogue } from './models/dialogue';
import { XetService } from './services/xet.service';
import { ConversationType } from './models/conversation-type';
import { ContextType } from './models/context-type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  comentario: string = '';

  conversations: Dialogue[] = [];

  contextType: ContextType = ContextType.json;

  conversationType: ConversationType = ConversationType.chatComContexto;

  public get ConversationType(): typeof ConversationType {
    return ConversationType;
  }

  public get ContextType(): typeof ContextType {
    return ContextType;
  }

  constructor(private service: XetService) {}

  onSendQuestion(): void {
    this.service
      .post(this.comentario, this.conversationType, this.contextType)
      .subscribe((c: Dialogue) => {
        this.conversations.push({
          answer: c.answer,
          question: c.question,
        });
      });
  }
}

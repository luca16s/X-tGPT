import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Dialogue } from '../models/dialogue';
import { ConversationType } from '../models/conversation-type';
import { ContextType } from '../models/context-type';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})
export class XetService {
  http = inject(HttpClient);
  private url: string = 'https://localhost:7070/';

  post(
    question: string,
    method: ConversationType,
    contextType: ContextType
  ): Observable<Dialogue> {
    const data = { question: question, context: contextType };
    let url = '';

    switch (method) {
      case ConversationType.chatComContexto: {
        url = `${this.url}chat-com-contexto`;
        break;
      }
      case ConversationType.chatSemContexto: {
        url = `${this.url}chat-sem-contexto`;
        break;
      }
      case ConversationType.completionsComContexto: {
        url = `${this.url}completions-com-contexto`;
        break;
      }
      case ConversationType.completionsSemContexto: {
        url = `${this.url}completions-com-contexto`;
        break;
      }
    }

    return this.http.post<Dialogue>(url, data, httpOptions);
  }
}

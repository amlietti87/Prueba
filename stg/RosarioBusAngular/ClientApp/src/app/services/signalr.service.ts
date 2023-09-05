import { Injectable, ErrorHandler } from "@angular/core";
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService } from '../shared/common/message.service';
import * as signalR from '@aspnet/signalr';
import { environment } from "../../environments/environment";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { MessageSignalR } from "../shared/model/base.model";

@Injectable()
export class SignalRService {

    private _hubConnection: signalR.HubConnection | undefined
    private _messages: Subject<MessageSignalR> = new Subject();

    public readonly OnMessageRecibed: Observable<MessageSignalR> = this._messages.asObservable();



    constructor() {
        this.initHub();
    }



    private initHub(): Promise<void> {
        console.log('initHub');


        this._hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(environment.signalrUrl, signalR.HttpTransportType.LongPolling)
            .configureLogging(signalR.LogLevel.Debug)
            .build();



        this._hubConnection.on('Send', (message: MessageSignalR) => {
            console.log(message);
            this._messages.next(message);
        });

        this._hubConnection.on('JoinGroup', (data: string) => {

        });

        this._hubConnection.on('LeaveGroup', (data: string) => {

        });

        return this._hubConnection.start().catch(err => {
            console.error(err.toString());
        }

        );
    }

    reconectHub(_hubConnection: signalR.HubConnection): Promise<void> {
        if (this._hubConnection) {
            try {
                this._hubConnection.stop();
            } catch {

            }
        }

        return this.initHub();
    }

    joinGroup(group: string): void {
        if (this._hubConnection == null || this._hubConnection.state == signalR.HubConnectionState.Disconnected) {
            this.reconectHub(this._hubConnection).then(() => this._hubConnection.invoke('JoinGroup', group));
        }
        else {
            this._hubConnection.invoke('JoinGroup', group);
        }

    }


    leaveGroup(group: string): void {

        if (this._hubConnection == null || this._hubConnection.state == signalR.HubConnectionState.Disconnected) {
            this.reconectHub(this._hubConnection).then(() => this._hubConnection.invoke('LeaveGroup', group));
        }
        else {
            this._hubConnection.invoke('LeaveGroup', group);
        }
    }
}



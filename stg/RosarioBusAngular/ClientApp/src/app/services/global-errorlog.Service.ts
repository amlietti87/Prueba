import { Injectable, ErrorHandler, Injector } from "@angular/core";
import { HttpErrorResponse } from '@angular/common/http';
//import { MessageService } from '../shared/common/message.service';
import { RestError } from '../shared/model/error.model';

import { NotificationService } from '../shared/notification/notification.service';
import { environment } from "../../environments/environment";
import { MessageService } from "../shared/common/message.service";

@Injectable()
export class GlobalErrorLogService {
    private name: String = 'GlobalErrorLogService';
    private message = 'Error.';
    private _notifiService: NotificationService;
    private _messageService: MessageService;
    constructor(private injector: Injector,
    ) { }



    logError(error: any) {



        if (!this._notifiService) {
            this._notifiService = this.injector.get(NotificationService);
        }


        if (!this._messageService) {
            this._messageService = this.injector.get(MessageService);
        }


        const date = new Date().toString();

        var _restError = (error) as RestError;

        if (error instanceof HttpErrorResponse) {
            try {
                var errorBody = error.error as RestError;
                if (errorBody.Status == "ValidationError") {
                    this._notifiService.warn(errorBody.Messages.toString());
                }
                else if (errorBody.Status == "Warning") {
                    this._messageService.error(errorBody.Messages.toString(), ' ');
                }
                else if (errorBody.Status == "ConcurrencyValidator") {
                    console.error(date, 'ConcurrencyValidator.', errorBody.Messages.toString());
                    //this._messageService.error(errorBody.Messages.toString(), ' ');
                }
                else {
                    this._notifiService.error(errorBody.Messages.toString());
                }

            } catch (ex) {
                console.error(date, 'There was a Type error.', error);
                if (!environment.production) {
                    this._notifiService.error(error.message, error.error);
                }
                else {
                    console.error(date, 'There was a Type error.', error.message);
                }

            }

        }
        else if (_restError) {
            try {
                // var errorBody = JSON.parse(error) as RestError;
                if (!environment.production) {
                    this._notifiService.error(_restError.message);
                }
                else {
                    console.error(date, 'There was a Type error.', _restError);
                }
            } catch (error) {
                if (!environment.production) {
                    this._notifiService.error(error.message);
                }
                else {
                    console.error(date, 'There was a Type error.', error.message);
                }
            }

        } else if (error instanceof TypeError) {
            console.error(date, 'There was a Type error.', error.message);
        } else if (error instanceof Error) {
            console.error(date, 'There was a general error.', error.message);
        } else {
            console.error(date, 'Nobody threw an error but something happened!', error);
        }

        console.error(date, 'Nobody threw an error but something happened!', error);
    }



}

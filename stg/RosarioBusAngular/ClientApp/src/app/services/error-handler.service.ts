import { Injectable, ErrorHandler } from "@angular/core";
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService } from '../shared/common/message.service';
import { GlobalErrorLogService } from './global-errorlog.Service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

    constructor(private errorLogService: GlobalErrorLogService) {
    }

    handleError(error: any): void {
        this.errorLogService.logError(error);
    }
}



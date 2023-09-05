import { Injectable } from '@angular/core';
import * as toastr from "toastr";
@Injectable()
export class NotifyService {

    info(message: string, title?: string, options?: any): void {
        toastr.info(message, title, options)
    }

    success(message: string, title?: string, options?: any): void {
        toastr.success(message, title, options);
    }

    warn(message: string, title?: string, options?: any): void {
        toastr.warning(message, title, options);
    }

    error(message: string, title?: string, options?: any): void {
        toastr.error(message, title, options);
    }

}
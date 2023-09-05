import { Injectable, Injector } from '@angular/core';


import * as moment from 'moment';
import * as Push from 'push.js'; // if using ES6
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { NotificationServiceProxy, UserNotification } from './notification-service';
import { EventEmitter, Output } from '@angular/core';


export interface IFormattedUserNotification {
    userNotificationId: string;
    text: string;
    time: string;
    creationTime: Date;
    icon: string;
    state: String;
    data: any;
    url: string;
    isUnread: boolean;
    severity: number;
}

@Injectable()
export class UserNotificationHelper extends AppComponentBase {


    @Output() refresh: EventEmitter<any> = new EventEmitter();
    @Output() recived: EventEmitter<string> = new EventEmitter();
    @Output() read: EventEmitter<string> = new EventEmitter();

    constructor(
        injector: Injector,
        private _notificationService: NotificationServiceProxy
    ) {
        super(injector);
    }

    getUrl(type: number): string {
        //switch (type) {


        //No url for this notification
        return '';
    }

    /* PUBLIC functions ******************************************/

    getUiIconBySeverity(severity: number): string {
        switch (severity) {
            case 1:
                return 'fa fa-check';
            case 2:
                return 'fa fa-warning';
            case 3:
                return 'fa fa-bolt';
            case 4:
                return 'fa fa-bomb';
            case 5:
            default:
                return 'fa fa-info';
        }
    }

    format(userNotification: UserNotification): IFormattedUserNotification {
        let formatted: IFormattedUserNotification = {
            userNotificationId: userNotification.id.toString(),
            text: userNotification.Text,
            time: moment().format('YYYY-MM-DD HH:mm:ss'),
            creationTime: moment().toDate(),
            icon: this.getUiIconBySeverity(userNotification.id),
            state: userNotification.state.toString(),
            data: userNotification.id,
            url: this.getUrl(userNotification.state),
            severity: userNotification.id,
            isUnread: userNotification.state == 0
        };

        return formatted;
    }

    show(userNotification: IFormattedUserNotification): void {

        //Application notification
        //Tecso.notifications.showUiNotifyForUserNotification(userNotification, {
        //    'onclick': () => {
        //        //Take action when user clicks to live toastr notification
        //        let url = this.getUrl(userNotification);
        //        if (url) {
        //            location.href = url;
        //        }
        //    }
        //});

        //Desktop notification
        //Push.create('AbpZeroTemplate', {
        //    body: userNotification.text,
        //    icon: 'assets/app/media/img/logos/logo-1.png',
        //    timeout: 6000,
        //    onClick: function() {
        //        window.focus();
        //        this.close();
        //    }
        //});
    }

    setAllAsRead(callback?: () => void): void {
        this._notificationService.setNotificationAsRead('').subscribe(() => {
            this.refresh.emit(null);
            if (callback) {
                callback();
            }
        });
    }

    setAsRead(userNotificationId: string, callback?: (userNotificationId: string) => void): void {

        this._notificationService.setNotificationAsRead(userNotificationId).subscribe(() => {
            this.read.emit(userNotificationId);
            if (callback) {
                callback(userNotificationId);
            }
        });
    }

    openSettingsModal(): void {
        //this.settingsModal.show();
    }
}





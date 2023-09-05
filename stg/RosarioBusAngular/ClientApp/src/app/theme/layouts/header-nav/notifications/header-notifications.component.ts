import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';

import { UserNotificationHelper, IFormattedUserNotification } from './UserNotificationHelper';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { NotificationServiceProxy, UserNotification } from './notification-service';

@Component({
    templateUrl: './header-notifications.component.html',
    selector: '[headerNotifications]',
    encapsulation: ViewEncapsulation.None
})
export class HeaderNotificationsComponent extends AppComponentBase implements OnInit {

    notifications: IFormattedUserNotification[] = [];
    unreadNotificationCount = 0;



    constructor(
        injector: Injector,
        private _notificationService: NotificationServiceProxy,
        private _userNotificationHelper: UserNotificationHelper
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadNotifications();
        this.registerToEvents();
    }

    loadNotifications(): void {
        this._notificationService.getUserNotifications(undefined, 3, 0).subscribe(result => {
            this.unreadNotificationCount = result.DataObject.unreadCount;
            this.notifications = [];

            $.each(result.DataObject.items, (index, item: UserNotification) => {

                this.notifications.push(this._userNotificationHelper.format(<any>item));
            });
        });
    }

    registerToEvents() {

        //this._userNotificationHelper.recived.subscribe(userNotification => {
        //    this._userNotificationHelper.show(userNotification);
        //    this.loadNotifications();
        //});


        this._userNotificationHelper.refresh.subscribe((a) => {
            this.loadNotifications();
        });


        this._userNotificationHelper.read.subscribe((userNotificationId) => {

            for (let i = 0; i < this.notifications.length; i++) {
                if (this.notifications[i].userNotificationId === userNotificationId) {
                    this.notifications[i].state = "0";
                    this.notifications[i].isUnread = false;
                }
            }

            this.unreadNotificationCount -= 1;
        });



    }

    setAllNotificationsAsRead(): void {
        this._userNotificationHelper.setAllAsRead();
    }



    setNotificationAsRead(userNotification: IFormattedUserNotification): void {

        this._userNotificationHelper.setAsRead(userNotification.userNotificationId);
    }

    show(userNotification: IFormattedUserNotification): void {

        this._userNotificationHelper.show(userNotification);
    }




    gotoUrl(url): void {
        if (url) {
            location.href = url;
        }
    }
}

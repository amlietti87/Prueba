import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';

import * as moment from 'moment';
import { Dto, ADto, ResponseModel } from '../../../../shared/model/base.model';
import { environment } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';


@Injectable()
export class NotificationServiceProxy {
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient) {

        this.identityUrl = environment.identityUrl + '/Notification';
        this.baseUrl = this.identityUrl;

    }

    getUserNotifications(state: number, maxResultCount: number, skipCount: number): Observable<ResponseModel<GetNotificationsOutput>> {
        let url_ = this.baseUrl + "/GetUserNotifications?";
        return this.http.get<ResponseModel<GetNotificationsOutput>>(url_);
    }


    setNotificationAsRead(input: string): Observable<void> {
        let url_ = this.baseUrl + "/SetNotificationAsRead?id=" + input;

        return this.http.post<void>(url_, input);

    }


}



export class UserNotification {

    userId: number;
    state: number;
    id: number;
    Text: string;

}

export class GetNotificationsOutput {
    unreadCount: number;
    totalCount: number;
    items: UserNotification[];
}
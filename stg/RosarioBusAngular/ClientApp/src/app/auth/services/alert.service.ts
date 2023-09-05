import { Injectable } from "@angular/core";
import { NavigationStart, Router } from "@angular/router";
import { Observable } from "rxjs";
import { Subject } from "rxjs/Subject";
import { Alert, AlertType } from "../models/alerts";

@Injectable()
export class AlertService {
    private subject = new Subject<any>();
    private keepAfterNavigationChange = false;

    constructor(private _router: Router) {
        // clear alert message on route change
        _router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                if (this.keepAfterNavigationChange) {
                    // only keep for a single location change
                    this.keepAfterNavigationChange = false;
                } else {
                    // clear alert
                    this.clear();
                }
            }
        });
    }



    getMessage(): Observable<any> {
        return this.getAlert();
    }

    getAlert(): Observable<any> {
        return this.subject.asObservable();
    }


    success(message: string, keepAfterNavigationChange = false) {
        this.alert(AlertType.Success, message, keepAfterNavigationChange);
    }

    error(message: string, keepAfterNavigationChange = false) {
        //console.log('calling alert(), type: '+AlertType.Error + ' message: '+message);
        this.alert(AlertType.Error, message, keepAfterNavigationChange);

    }

    info(message: string, keepAfterNavigationChange = false) {
        this.alert(AlertType.Info, message, keepAfterNavigationChange);
    }

    warn(message: string, keepAfterNavigationChange = false) {
        this.alert(AlertType.Warning, message, keepAfterNavigationChange);
    }

    alert(type: AlertType, message: string, keepAfterNavigationChange = false) {
        this.keepAfterNavigationChange = keepAfterNavigationChange;
        this.subject.next(<Alert>{ type: type, message: message });
        //console.log('alert pushed');
    }



    clear() {
        // clear alerts
        this.subject.next();
    }

}


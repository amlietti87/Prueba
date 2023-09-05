import { Injectable } from '@angular/core';
import { PermissionCheckerService } from '../../../shared/common/permission-checker.service';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs';
import { NavigationStart, Router } from '@angular/router';
import { ResponseModel } from '../../../shared/model/base.model';
import { StorageService } from '../../../shared/common/services/storage.service';
import { AppBreadcrumbs, BreadcrumbsItem } from './breadcrumbs.model';

@Injectable()
export class BreadcrumbsService {

    public CurretBreadcrumbs: AppBreadcrumbs;

    constructor(private _router: Router) {
        this.defaultBreadcrumbs();
    }

    private subject = new Subject<AppBreadcrumbs>();

    getBreadcrumbs(): Observable<AppBreadcrumbs> {
        return this.subject.asObservable();
    }



    AddItem(name: string, icon?: string, route?: string, key?: string, funtion?: any) {
        this.CurretBreadcrumbs.items.push(new BreadcrumbsItem(name, icon, route, key, funtion));
        this.subject.next(this.CurretBreadcrumbs);
    }





    RemoveItem(key?: string) {

        if (key) {

            var index = this.CurretBreadcrumbs.items.findIndex(e => e.key == key);

            if (index > -1) {
                this.CurretBreadcrumbs.items.splice(index, this.CurretBreadcrumbs.items.length);
            }

        }
        else {
            this.CurretBreadcrumbs.items.pop();
        }


        this.subject.next(this.CurretBreadcrumbs);
    }


    defaultBreadcrumbs(title?: string, items?: BreadcrumbsItem[]) {
        this.CurretBreadcrumbs = new AppBreadcrumbs(title || 'Rosario Bus', 'la la-home', items);
        this.subject.next(this.CurretBreadcrumbs);
    }


}

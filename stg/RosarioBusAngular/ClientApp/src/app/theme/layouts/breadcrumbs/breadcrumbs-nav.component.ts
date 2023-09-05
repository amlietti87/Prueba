import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, OnDestroy } from '@angular/core';
import { Helpers } from '../../../helpers';

import { AppComponentBase } from '../../../shared/common/app-component-base';

import { ActivatedRoute, Router } from '@angular/router';
import { BreadcrumbsService } from './breadcrumbs.service';
import { AppBreadcrumbs, BreadcrumbsItem } from './breadcrumbs.model';
import { Subscription } from 'rxjs';

declare let mLayout: any;
@Component({
    selector: "breadcrumbs-nav",
    templateUrl: "./breadcrumbs.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class BreadcrumbsNavComponent extends AppComponentBase implements OnInit, OnDestroy, AfterViewInit {

    sub: Subscription;


    title: string;
    icon: string;
    items: BreadcrumbsItem[] = [];
    //permiso: false;

    constructor(
        injector: Injector,
        private _BreadcrumbsService: BreadcrumbsService,
        private _router: Router
    ) {
        super(injector);
        this.items = [];
    }
    ngOnInit() {
        this.sub = this._BreadcrumbsService.getBreadcrumbs().subscribe(m => {

            this.title = m.title;
            this.icon = m.icon;
            this.items = [];
            m.items.forEach(obj => {
                obj.isLast = false;
            });
            if (m.items[m.items.length - 1]) {
                m.items[m.items.length - 1].isLast = true;
            }

            this.items = m.items;
        })
    }


    ngAfterViewInit(): void {

    }

    ngOnDestroy(): void {
        if (this.sub) {
            this.sub.unsubscribe();
        }
    }

    onClickItemBreadcrumbs(item: BreadcrumbsItem): any {

        if (item.funtion) {
            item.funtion();
            return;
        }

        if (item.route) {
            this._router.navigate([item.route]);
        }

    }








}
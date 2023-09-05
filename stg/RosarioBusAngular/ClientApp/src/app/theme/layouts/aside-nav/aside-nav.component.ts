import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector } from '@angular/core';
import { Helpers } from '../../../helpers';
import { AppMenu } from './app-menu';
import { AppNavigationService } from './app-navigation.service';
//import { PermissionCheckerService } from '../../../shared/common/permission-checker.service';
import { AppComponentBase } from '../../../shared/common/app-component-base';
import { AppMenuItem } from './app-menu-item';
import { ActivatedRoute, Router } from '@angular/router';

declare let mLayout: any;
@Component({
    selector: "app-aside-nav",
    templateUrl: "./aside-nav.component.html",
    // encapsulation: ViewEncapsulation.None,
})
export class AsideNavComponent extends AppComponentBase implements OnInit, AfterViewInit {

    menu: AppMenu = null;
    //permiso: false;

    constructor(
        injector: Injector,
        //public permission: PermissionCheckerService,
        private _appNavigationService: AppNavigationService,
        private _router: Router
    ) {
        super(injector);
        this.menu = new AppMenu("", "", []);
    }
    ngOnInit() {

        this._appNavigationService.getMenu().subscribe(m => {

            if (m) {
                this.clearMenuitem(m.items);
                this.menu = m;
            }
            else {
                this.menu = new AppMenu("", "", []);
            }

        })
        this._appNavigationService.getLoadMenuAsyc();
    }


    private clearMenuitem(items: AppMenuItem[]) {

        items.forEach((item, index) => {


            if (item.permissionName) {
                item.granted = this.showMenuItem(item.permissionName);
            }
            else {
                item.granted = true;
            }

            if (item.items && item.items.length > 0) {
                this.clearMenuitem(item.items);
            }

            if (!item.permissionName && !item.route) {
                if (!item.items.some(x => x.granted)) {
                    item.granted = false;
                }
            }

        });
    }


    GoToRute(item: AppMenuItem): void {

        this._router.navigateByUrl(item.route);
    }



    ngAfterViewInit() {
        mLayout.initAside();

        setTimeout(() => {

            // var menu = mLayout.getAsideMenu();
            // let menu = (<any>$('#m_aside_left')).mMenu(); let item = $(menu).find('a[href="' + window.location.pathname + '"]').parent('.m-menu__item'); (<any>$(menu).data('menu')).setActiveItem(item);
            //var item = $(menu).find(".m-menu__item--active").parent('.m-menu__item');

            //var s = $(menu);
            //$('.m-menu__item--active').addClass('m-menu__item--open');

            $('.m-menu__item--active').each(function(index, element) {
                if ($(element).find('a.m-menu__toggle').length > 0) {
                    $(element).addClass("m-menu__item--open");
                }

            })


            //s.setActiveItem(item);

        }, 0);


    }


    showMenuItem(permissionName): boolean {
        //if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement'
        ////    && this._appSessionService.tenant && !this._appSessionService.tenant.edition
        //) {
        //    return false;
        //}

        if (permissionName) {
            return this.permission.isGranted(permissionName);
        }

        //if (menuItem.items && menuItem.items.length) {
        //    return this._appNavigationService.checkChildMenuItemPermission(menuItem);
        //}

        return true;
    }



}
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationCancel } from '@angular/router';
import { Helpers } from "./helpers";
import { PermissionCheckerService } from './shared/common/permission-checker.service';

import { AppNavigationService } from './theme/layouts/aside-nav/app-navigation.service';
import { ConfigurationService } from './shared/common/services/configuration.service';
import { async } from '@angular/core/testing';
import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AppComponentBase, ComponentCanDeactivate } from './shared/common/app-component-base';
import { MessageService } from './shared/common/message.service';


@Component({
    selector: 'body',
    templateUrl: './app.component.html',
    encapsulation: ViewEncapsulation.None,
    providers: [MessageService]
})
export class AppComponent implements OnInit {
    title = 'app';
    globalBodyClass = 'm-page--loading-non-block m-page--fluid m--skin- m-content--skin-light2 m-header--fixed m-header--fixed-mobile m-aside-left--enabled m-aside-left--skin-dark m-aside-left--offcanvas m-footer--push m-aside--offcanvas-default m-aside-left--fixed';

    constructor(private _router: Router,
        private permissionsService: PermissionCheckerService,
        private _p: AppNavigationService,
        private configurationService: ConfigurationService) {
    }

    ngOnInit() {




        this.permissionsService.loadPermissions();
        this.configurationService.loadConfigurations();
        this._p.getLoadMenuAsyc();



        this._router.events.subscribe((route) => {


            if (route instanceof NavigationStart) {
                Helpers.setLoading(true);
                Helpers.bodyClass(this.globalBodyClass);
            }
            if (route instanceof NavigationEnd) {
                Helpers.setLoading(false);
            }
            if (route instanceof NavigationCancel) {
                Helpers.setLoading(false);
            }
        });
    }

}



@Injectable()
export class CanDeactivateGuard implements CanDeactivate<ComponentCanDeactivate> {
    canDeactivate(component: ComponentCanDeactivate): boolean {

        if (!component.canDeactivate()) {
            if (confirm(component.confirmMessage())) {
                return true;
            } else {
                //Helpers.setLoading(false);

                return false;

            }
        }
        return true;
    }
}

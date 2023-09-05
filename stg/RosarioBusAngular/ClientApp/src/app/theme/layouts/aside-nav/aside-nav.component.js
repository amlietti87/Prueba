"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var app_menu_1 = require("./app-menu");
var app_navigation_service_1 = require("./app-navigation.service");
//import { PermissionCheckerService } from '../../../shared/common/permission-checker.service';
var app_component_base_1 = require("../../../shared/common/app-component-base");
var router_1 = require("@angular/router");
var AsideNavComponent = /** @class */ (function (_super) {
    __extends(AsideNavComponent, _super);
    //permiso: false;
    function AsideNavComponent(injector, 
    //public permission: PermissionCheckerService,
    _appNavigationService, _router) {
        var _this = _super.call(this, injector) || this;
        _this._appNavigationService = _appNavigationService;
        _this._router = _router;
        _this.menu = null;
        _this.menu = new app_menu_1.AppMenu("", "", []);
        return _this;
    }
    AsideNavComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._appNavigationService.getMenu().subscribe(function (m) {
            if (m) {
                _this.clearMenuitem(m.items);
                _this.menu = m;
            }
            else {
                _this.menu = new app_menu_1.AppMenu("", "", []);
            }
        });
        this._appNavigationService.getLoadMenuAsyc();
    };
    AsideNavComponent.prototype.clearMenuitem = function (items) {
        var _this = this;
        items.forEach(function (item, index) {
            if (item.permissionName) {
                item.granted = _this.showMenuItem(item.permissionName);
            }
            else {
                item.granted = true;
            }
            if (item.items && item.items.length > 0) {
                _this.clearMenuitem(item.items);
            }
            if (!item.permissionName && !item.route) {
                if (!item.items.some(function (x) { return x.granted; })) {
                    item.granted = false;
                }
            }
        });
    };
    AsideNavComponent.prototype.GoToRute = function (item) {
        this._router.navigateByUrl(item.route);
    };
    AsideNavComponent.prototype.ngAfterViewInit = function () {
        mLayout.initAside();
        setTimeout(function () {
            // var menu = mLayout.getAsideMenu();
            // let menu = (<any>$('#m_aside_left')).mMenu(); let item = $(menu).find('a[href="' + window.location.pathname + '"]').parent('.m-menu__item'); (<any>$(menu).data('menu')).setActiveItem(item);
            //var item = $(menu).find(".m-menu__item--active").parent('.m-menu__item');
            //var s = $(menu);
            //$('.m-menu__item--active').addClass('m-menu__item--open');
            $('.m-menu__item--active').each(function (index, element) {
                if ($(element).find('a.m-menu__toggle').length > 0) {
                    $(element).addClass("m-menu__item--open");
                }
            });
            //s.setActiveItem(item);
        }, 0);
    };
    AsideNavComponent.prototype.showMenuItem = function (permissionName) {
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
    };
    AsideNavComponent = __decorate([
        core_1.Component({
            selector: "app-aside-nav",
            templateUrl: "./aside-nav.component.html",
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            app_navigation_service_1.AppNavigationService,
            router_1.Router])
    ], AsideNavComponent);
    return AsideNavComponent;
}(app_component_base_1.AppComponentBase));
exports.AsideNavComponent = AsideNavComponent;
//# sourceMappingURL=aside-nav.component.js.map
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
var app_component_base_1 = require("../../../shared/common/app-component-base");
var router_1 = require("@angular/router");
var breadcrumbs_service_1 = require("./breadcrumbs.service");
var BreadcrumbsNavComponent = /** @class */ (function (_super) {
    __extends(BreadcrumbsNavComponent, _super);
    //permiso: false;
    function BreadcrumbsNavComponent(injector, _BreadcrumbsService, _router) {
        var _this = _super.call(this, injector) || this;
        _this._BreadcrumbsService = _BreadcrumbsService;
        _this._router = _router;
        _this.items = [];
        _this.items = [];
        return _this;
    }
    BreadcrumbsNavComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.sub = this._BreadcrumbsService.getBreadcrumbs().subscribe(function (m) {
            _this.title = m.title;
            _this.icon = m.icon;
            _this.items = [];
            m.items.forEach(function (obj) {
                obj.isLast = false;
            });
            if (m.items[m.items.length - 1]) {
                m.items[m.items.length - 1].isLast = true;
            }
            _this.items = m.items;
        });
    };
    BreadcrumbsNavComponent.prototype.ngAfterViewInit = function () {
    };
    BreadcrumbsNavComponent.prototype.ngOnDestroy = function () {
        if (this.sub) {
            this.sub.unsubscribe();
        }
    };
    BreadcrumbsNavComponent.prototype.onClickItemBreadcrumbs = function (item) {
        if (item.funtion) {
            item.funtion();
            return;
        }
        if (item.route) {
            this._router.navigate([item.route]);
        }
    };
    BreadcrumbsNavComponent = __decorate([
        core_1.Component({
            selector: "breadcrumbs-nav",
            templateUrl: "./breadcrumbs.component.html",
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            breadcrumbs_service_1.BreadcrumbsService,
            router_1.Router])
    ], BreadcrumbsNavComponent);
    return BreadcrumbsNavComponent;
}(app_component_base_1.AppComponentBase));
exports.BreadcrumbsNavComponent = BreadcrumbsNavComponent;
//# sourceMappingURL=breadcrumbs-nav.component.js.map
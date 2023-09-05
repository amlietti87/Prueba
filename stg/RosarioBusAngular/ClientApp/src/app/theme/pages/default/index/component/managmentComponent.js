"use strict";
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
var dashboardDirective_1 = require("./dashboardDirective");
var dashboardItem_1 = require("../model/dashboardItem");
var ManagmentComponent = /** @class */ (function () {
    function ManagmentComponent(componentFactoryResolver) {
        this.componentFactoryResolver = componentFactoryResolver;
    }
    ManagmentComponent.prototype.ngOnInit = function () {
        this.loadComponent();
    };
    ManagmentComponent.prototype.ngOnDestroy = function () {
    };
    ManagmentComponent.prototype.loadComponent = function () {
        var adItem = this.dashboardItem;
        //let adItem = new DashboardItem(Dashboard1Component, { headline: "seba", body: "body seba" });
        var componentFactory = this.componentFactoryResolver.resolveComponentFactory(adItem.component);
        var viewContainerRef = this.dashboardHost.viewContainerRef;
        viewContainerRef.clear();
        var componentRef = viewContainerRef.createComponent(componentFactory);
        componentRef.instance.data = adItem.data;
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", dashboardItem_1.DashboardItem)
    ], ManagmentComponent.prototype, "dashboardItem", void 0);
    __decorate([
        core_1.ViewChild(dashboardDirective_1.DashboardDirective),
        __metadata("design:type", dashboardDirective_1.DashboardDirective)
    ], ManagmentComponent.prototype, "dashboardHost", void 0);
    ManagmentComponent = __decorate([
        core_1.Component({
            selector: 'app-ad-ditem',
            template: "\n              <div class=\"app-porlet-item\">\n                <ng-template dashboard-host></ng-template>\n              </div>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ComponentFactoryResolver])
    ], ManagmentComponent);
    return ManagmentComponent;
}());
exports.ManagmentComponent = ManagmentComponent;
//# sourceMappingURL=managmentComponent.js.map
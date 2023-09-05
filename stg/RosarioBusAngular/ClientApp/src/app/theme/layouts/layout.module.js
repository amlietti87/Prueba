"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var layout_component_1 = require("./layout/layout.component");
var aside_left_minimize_default_enabled_component_1 = require("../pages/aside-left-minimize-default-enabled/aside-left-minimize-default-enabled.component");
var header_nav_component_1 = require("./header-nav/header-nav.component");
var default_component_1 = require("../pages/default/default.component");
var aside_nav_component_1 = require("./aside-nav/aside-nav.component");
var footer_component_1 = require("./footer/footer.component");
var quick_sidebar_component_1 = require("./quick-sidebar/quick-sidebar.component");
var scroll_top_component_1 = require("./scroll-top/scroll-top.component");
var tooltips_component_1 = require("./tooltips/tooltips.component");
var common_1 = require("@angular/common");
var router_1 = require("@angular/router");
var header_notifications_component_1 = require("./header-nav/notifications/header-notifications.component");
var href_prevent_default_directive_1 = require("../../directives/href-prevent-default.directive");
var unwrap_tag_directive_1 = require("../../directives/unwrap-tag.directive");
var breadcrumbs_nav_component_1 = require("./breadcrumbs/breadcrumbs-nav.component");
var LayoutModule = /** @class */ (function () {
    function LayoutModule() {
    }
    LayoutModule = __decorate([
        core_1.NgModule({
            declarations: [
                layout_component_1.LayoutComponent,
                aside_left_minimize_default_enabled_component_1.AsideLeftMinimizeDefaultEnabledComponent,
                header_nav_component_1.HeaderNavComponent,
                default_component_1.DefaultComponent,
                aside_nav_component_1.AsideNavComponent,
                footer_component_1.FooterComponent,
                quick_sidebar_component_1.QuickSidebarComponent,
                scroll_top_component_1.ScrollTopComponent,
                tooltips_component_1.TooltipsComponent,
                href_prevent_default_directive_1.HrefPreventDefaultDirective,
                unwrap_tag_directive_1.UnwrapTagDirective,
                header_notifications_component_1.HeaderNotificationsComponent,
                breadcrumbs_nav_component_1.BreadcrumbsNavComponent,
            ],
            exports: [
                layout_component_1.LayoutComponent,
                aside_left_minimize_default_enabled_component_1.AsideLeftMinimizeDefaultEnabledComponent,
                header_nav_component_1.HeaderNavComponent,
                default_component_1.DefaultComponent,
                aside_nav_component_1.AsideNavComponent,
                footer_component_1.FooterComponent,
                quick_sidebar_component_1.QuickSidebarComponent,
                scroll_top_component_1.ScrollTopComponent,
                tooltips_component_1.TooltipsComponent,
                href_prevent_default_directive_1.HrefPreventDefaultDirective,
                header_notifications_component_1.HeaderNotificationsComponent,
                breadcrumbs_nav_component_1.BreadcrumbsNavComponent
            ],
            imports: [
                common_1.CommonModule,
                router_1.RouterModule,
            ]
        })
    ], LayoutModule);
    return LayoutModule;
}());
exports.LayoutModule = LayoutModule;
//# sourceMappingURL=layout.module.js.map
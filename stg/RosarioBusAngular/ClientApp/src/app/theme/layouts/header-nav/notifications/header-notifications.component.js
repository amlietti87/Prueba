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
var UserNotificationHelper_1 = require("./UserNotificationHelper");
var app_component_base_1 = require("../../../../shared/common/app-component-base");
var notification_service_1 = require("./notification-service");
var HeaderNotificationsComponent = /** @class */ (function (_super) {
    __extends(HeaderNotificationsComponent, _super);
    function HeaderNotificationsComponent(injector, _notificationService, _userNotificationHelper) {
        var _this = _super.call(this, injector) || this;
        _this._notificationService = _notificationService;
        _this._userNotificationHelper = _userNotificationHelper;
        _this.notifications = [];
        _this.unreadNotificationCount = 0;
        return _this;
    }
    HeaderNotificationsComponent.prototype.ngOnInit = function () {
        this.loadNotifications();
        this.registerToEvents();
    };
    HeaderNotificationsComponent.prototype.loadNotifications = function () {
        var _this = this;
        this._notificationService.getUserNotifications(undefined, 3, 0).subscribe(function (result) {
            _this.unreadNotificationCount = result.DataObject.unreadCount;
            _this.notifications = [];
            $.each(result.DataObject.items, function (index, item) {
                _this.notifications.push(_this._userNotificationHelper.format(item));
            });
        });
    };
    HeaderNotificationsComponent.prototype.registerToEvents = function () {
        //this._userNotificationHelper.recived.subscribe(userNotification => {
        //    this._userNotificationHelper.show(userNotification);
        //    this.loadNotifications();
        //});
        var _this = this;
        this._userNotificationHelper.refresh.subscribe(function (a) {
            _this.loadNotifications();
        });
        this._userNotificationHelper.read.subscribe(function (userNotificationId) {
            for (var i = 0; i < _this.notifications.length; i++) {
                if (_this.notifications[i].userNotificationId === userNotificationId) {
                    _this.notifications[i].state = "0";
                    _this.notifications[i].isUnread = false;
                }
            }
            _this.unreadNotificationCount -= 1;
        });
    };
    HeaderNotificationsComponent.prototype.setAllNotificationsAsRead = function () {
        this._userNotificationHelper.setAllAsRead();
    };
    HeaderNotificationsComponent.prototype.setNotificationAsRead = function (userNotification) {
        this._userNotificationHelper.setAsRead(userNotification.userNotificationId);
    };
    HeaderNotificationsComponent.prototype.show = function (userNotification) {
        this._userNotificationHelper.show(userNotification);
    };
    HeaderNotificationsComponent.prototype.gotoUrl = function (url) {
        if (url) {
            location.href = url;
        }
    };
    HeaderNotificationsComponent = __decorate([
        core_1.Component({
            templateUrl: './header-notifications.component.html',
            selector: '[headerNotifications]',
            encapsulation: core_1.ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            notification_service_1.NotificationServiceProxy,
            UserNotificationHelper_1.UserNotificationHelper])
    ], HeaderNotificationsComponent);
    return HeaderNotificationsComponent;
}(app_component_base_1.AppComponentBase));
exports.HeaderNotificationsComponent = HeaderNotificationsComponent;
//# sourceMappingURL=header-notifications.component.js.map
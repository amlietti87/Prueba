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
var moment = require("moment");
var app_component_base_1 = require("../../../../shared/common/app-component-base");
var notification_service_1 = require("./notification-service");
var core_2 = require("@angular/core");
var UserNotificationHelper = /** @class */ (function (_super) {
    __extends(UserNotificationHelper, _super);
    function UserNotificationHelper(injector, _notificationService) {
        var _this = _super.call(this, injector) || this;
        _this._notificationService = _notificationService;
        _this.refresh = new core_2.EventEmitter();
        _this.recived = new core_2.EventEmitter();
        _this.read = new core_2.EventEmitter();
        return _this;
    }
    UserNotificationHelper.prototype.getUrl = function (type) {
        //switch (type) {
        //No url for this notification
        return '';
    };
    /* PUBLIC functions ******************************************/
    UserNotificationHelper.prototype.getUiIconBySeverity = function (severity) {
        switch (severity) {
            case 1:
                return 'fa fa-check';
            case 2:
                return 'fa fa-warning';
            case 3:
                return 'fa fa-bolt';
            case 4:
                return 'fa fa-bomb';
            case 5:
            default:
                return 'fa fa-info';
        }
    };
    UserNotificationHelper.prototype.format = function (userNotification) {
        var formatted = {
            userNotificationId: userNotification.id.toString(),
            text: userNotification.Text,
            time: moment().format('YYYY-MM-DD HH:mm:ss'),
            creationTime: moment().toDate(),
            icon: this.getUiIconBySeverity(userNotification.id),
            state: userNotification.state.toString(),
            data: userNotification.id,
            url: this.getUrl(userNotification.state),
            severity: userNotification.id,
            isUnread: userNotification.state == 0
        };
        return formatted;
    };
    UserNotificationHelper.prototype.show = function (userNotification) {
        //Application notification
        //Tecso.notifications.showUiNotifyForUserNotification(userNotification, {
        //    'onclick': () => {
        //        //Take action when user clicks to live toastr notification
        //        let url = this.getUrl(userNotification);
        //        if (url) {
        //            location.href = url;
        //        }
        //    }
        //});
        //Desktop notification
        //Push.create('AbpZeroTemplate', {
        //    body: userNotification.text,
        //    icon: 'assets/app/media/img/logos/logo-1.png',
        //    timeout: 6000,
        //    onClick: function() {
        //        window.focus();
        //        this.close();
        //    }
        //});
    };
    UserNotificationHelper.prototype.setAllAsRead = function (callback) {
        var _this = this;
        this._notificationService.setNotificationAsRead('').subscribe(function () {
            _this.refresh.emit(null);
            if (callback) {
                callback();
            }
        });
    };
    UserNotificationHelper.prototype.setAsRead = function (userNotificationId, callback) {
        var _this = this;
        this._notificationService.setNotificationAsRead(userNotificationId).subscribe(function () {
            _this.read.emit(userNotificationId);
            if (callback) {
                callback(userNotificationId);
            }
        });
    };
    UserNotificationHelper.prototype.openSettingsModal = function () {
        //this.settingsModal.show();
    };
    __decorate([
        core_2.Output(),
        __metadata("design:type", core_2.EventEmitter)
    ], UserNotificationHelper.prototype, "refresh", void 0);
    __decorate([
        core_2.Output(),
        __metadata("design:type", core_2.EventEmitter)
    ], UserNotificationHelper.prototype, "recived", void 0);
    __decorate([
        core_2.Output(),
        __metadata("design:type", core_2.EventEmitter)
    ], UserNotificationHelper.prototype, "read", void 0);
    UserNotificationHelper = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [core_1.Injector,
            notification_service_1.NotificationServiceProxy])
    ], UserNotificationHelper);
    return UserNotificationHelper;
}(app_component_base_1.AppComponentBase));
exports.UserNotificationHelper = UserNotificationHelper;
//# sourceMappingURL=UserNotificationHelper.js.map
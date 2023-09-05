"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var localization_service_1 = require("./localization.service");
var permission_checker_service_1 = require("./permission-checker.service");
var notify_service_1 = require("./notify.service");
var notification_service_1 = require("../notification/notification.service");
var setting_service_1 = require("./setting.service");
var message_service_1 = require("./message.service");
var PrimengDatatableHelper_1 = require("../helpers/PrimengDatatableHelper");
var rxjs_1 = require("rxjs");
var AppComponentBase = /** @class */ (function () {
    function AppComponentBase(injector) {
        this.unsubscriber = new rxjs_1.Subject();
        this.localization = injector.get(localization_service_1.LocalizationService);
        this.permission = injector.get(permission_checker_service_1.PermissionCheckerService);
        this.notify = injector.get(notify_service_1.NotifyService);
        this.setting = injector.get(setting_service_1.SettingService);
        this.message = injector.get(message_service_1.MessageService);
        this.notificationService = injector.get(notification_service_1.NotificationService);
        this.primengDatatableHelper = new PrimengDatatableHelper_1.PrimengDatatableHelper();
    }
    AppComponentBase.prototype.l = function (key) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        return this.ls("", key, args);
    };
    AppComponentBase.prototype.ls = function (sourcename, key) {
        var args = [];
        for (var _i = 2; _i < arguments.length; _i++) {
            args[_i - 2] = arguments[_i];
        }
        var localizedText = this.localization.localize(key, sourcename);
        if (!localizedText) {
            localizedText = key;
        }
        if (!args || !args.length) {
            return localizedText;
        }
        args[0].unshift(localizedText);
        return key;
        //abp.utils.formatString.apply(this, args[0]);
    };
    AppComponentBase.prototype.isGranted = function (permissionName) {
        return this.permission.isGranted(permissionName);
    };
    AppComponentBase.prototype.isGrantedAny = function () {
        var permissions = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            permissions[_i] = arguments[_i];
        }
        if (!permissions) {
            return false;
        }
        for (var _a = 0, permissions_1 = permissions; _a < permissions_1.length; _a++) {
            var permission = permissions_1[_a];
            if (this.isGranted(permission)) {
                return true;
            }
        }
        return false;
    };
    AppComponentBase.prototype.s = function (key) {
        return "";
        //abp.setting.get(key);
    };
    return AppComponentBase;
}());
exports.AppComponentBase = AppComponentBase;
//# sourceMappingURL=app-component-base.js.map
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
var crud_service_1 = require("../shared/common/services/crud.service");
var base_model_1 = require("../shared/model/base.model");
var http_1 = require("@angular/common/http");
require("rxjs/Rx");
require("rxjs/add/observable/throw");
require("rxjs/add/operator/map");
var auth_service_1 = require("../auth/auth.service");
var environment_1 = require("../../environments/environment");
var UserService = /** @class */ (function (_super) {
    __extends(UserService, _super);
    function UserService(http, authService) {
        var _this = _super.call(this, http) || this;
        _this.http = http;
        _this.authService = authService;
        _this.identityUrl = '';
        _this.identityUrl = environment_1.environment.identityUrl + '/User';
        _this.endpoint = _this.identityUrl;
        return _this;
    }
    UserService.prototype.getUsers = function (pageIndex, pageSize, sort) {
        var url = this.identityUrl + '/GetPagedList';
        var data = new base_model_1.UserFilter();
        data.Page = pageIndex;
        data.PageSize = pageSize;
        data.Sort = sort;
        return this.http.post(url, data);
    };
    UserService.prototype.getUserForEdit = function (id) {
        var url = this.identityUrl + '/GetByIdAsync?id=' + id;
        return this.http.get(url).map(function (response) {
            return response.json();
        });
    };
    UserService.prototype.PermiteLoginManualCurrentUser = function () {
        var url = this.identityUrl + '/PermiteLoginManualCurrentUser';
        return this.http.get(url);
    };
    UserService.prototype.createOrUpdateUser = function (data) {
        var url = this.identityUrl + '/UpdateEntity';
        if (data.Id <= 0) {
            url = this.identityUrl + '/SaveNewEntity';
        }
        return this.http.post(url, data);
    };
    UserService.prototype.getUserPermissionsForEdit = function (id) {
        var url = this.identityUrl + '/GetUserPermissionsForEdit?id=' + id;
        return this.http.get(url);
    };
    UserService.prototype.resetPassword = function (id) {
        var url = this.identityUrl + '/ResetPassword?id=' + id;
        return this.http.post(url, { id: id });
    };
    UserService.prototype.updateUserPermissions = function (input) {
        var url = this.identityUrl + '/UpdateUserPermissions';
        return this.http.post(url, input);
    };
    UserService.prototype.GetUserLineasForEdit = function (id) {
        var url = this.identityUrl + '/GetUserLineasForEdit?id=' + id;
        return this.http.get(url);
    };
    UserService.prototype.SetUserLineasForEdit = function (input) {
        var url = this.identityUrl + '/SetUserLineasForEdit';
        return this.http.post(url, input);
    };
    UserService.prototype.findUser = function (userName) {
        var url = this.identityUrl + '/GetByUserLdapAsync?userName=' + userName;
        return this.http.get(url);
    };
    UserService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient,
            auth_service_1.AuthService])
    ], UserService);
    return UserService;
}(crud_service_1.CrudService));
exports.UserService = UserService;
//# sourceMappingURL=user.service.js.map
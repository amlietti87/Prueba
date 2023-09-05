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
var storage_service_1 = require("./storage.service");
require("rxjs/Rx");
require("rxjs/add/observable/throw");
require("rxjs/add/operator/map");
var http_1 = require("@angular/common/http");
var environment_1 = require("../../../../environments/environment");
var ConfigurationService = /** @class */ (function () {
    function ConfigurationService(http, _localStorageService) {
        this.http = http;
        this._localStorageService = _localStorageService;
        this.identityUrl = '';
        this.identityUrl = environment_1.environment.identityUrl + '/api/Configurations';
    }
    ConfigurationService.prototype.requestAllByFilter = function (reqParams) {
        return this.http.get(this.identityUrl);
    };
    ConfigurationService.prototype.loadConfigurations = function () {
        var _this = this;
        var list = this._localStorageService.retrieve('Configurations');
        if (list) {
            return list;
        }
        else {
            this.requestAllByFilter().toPromise().then(function (r) {
                _this.SetCache(r);
                return r;
            });
        }
    };
    ConfigurationService.prototype.getConfiguration = function () {
        return this._localStorageService.retrieve('Configurations');
    };
    ConfigurationService.prototype.SetCache = function (res) {
        return this._localStorageService.store('Configurations', res);
    };
    ConfigurationService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient,
            storage_service_1.LocalStorageService])
    ], ConfigurationService);
    return ConfigurationService;
}());
exports.ConfigurationService = ConfigurationService;
//# sourceMappingURL=configuration.service.js.map
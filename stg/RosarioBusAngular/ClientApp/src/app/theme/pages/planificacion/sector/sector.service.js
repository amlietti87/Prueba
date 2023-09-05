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
var crud_service_1 = require("../../../../shared/common/services/crud.service");
var http_1 = require("@angular/common/http");
require("rxjs/Rx");
require("rxjs/add/observable/throw");
require("rxjs/add/operator/map");
var environment_1 = require("../../../../../environments/environment");
var auth_service_1 = require("../../../../auth/auth.service");
var SectorService = /** @class */ (function (_super) {
    __extends(SectorService, _super);
    function SectorService(http, authService) {
        var _this = _super.call(this, http) || this;
        _this.http = http;
        _this.authService = authService;
        _this.identityUrl = '';
        _this.identityUrl = environment_1.environment.identityUrl + '/Sector';
        _this.endpoint = _this.identityUrl;
        return _this;
    }
    SectorService.prototype.getSectorView = function (filter) {
        var url = this.endpoint + '/GetSectorView';
        var data = filter;
        return this.http.post(url, data);
    };
    SectorService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient,
            auth_service_1.AuthService])
    ], SectorService);
    return SectorService;
}(crud_service_1.CrudService));
exports.SectorService = SectorService;
//# sourceMappingURL=sector.service.js.map
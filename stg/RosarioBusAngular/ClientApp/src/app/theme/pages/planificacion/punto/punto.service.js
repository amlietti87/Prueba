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
var file_service_1 = require("../../../../shared/common/file.service");
var PuntoService = /** @class */ (function (_super) {
    __extends(PuntoService, _super);
    function PuntoService(http, authService, fileService) {
        var _this = _super.call(this, http) || this;
        _this.http = http;
        _this.authService = authService;
        _this.fileService = fileService;
        _this.identityUrl = '';
        _this.identityUrl = environment_1.environment.identityUrl + '/Puntos';
        _this.endpoint = _this.identityUrl;
        return _this;
    }
    PuntoService.prototype.RecuperarDatosIniciales = function (CodRec) {
        var params = new http_1.HttpParams();
        params = params.set('CodRec', CodRec.toString());
        return this.http.get(this.endpoint + '/RecuperarDatosIniciales', { params: params });
    };
    PuntoService.prototype.RecuperarGeoJson = function (CodRec) {
        var params = new http_1.HttpParams();
        params = params.set('CodRec', CodRec.toString());
        return this.http.get(this.endpoint + '/GetGeoJson', { params: params });
    };
    PuntoService.prototype.GetPuntosReporte = function (filter) {
        var _url = this.identityUrl + "/GetReporte";
        this.fileService.dowloadAuthenticatedByPost(_url, filter);
    };
    PuntoService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient,
            auth_service_1.AuthService,
            file_service_1.FileService])
    ], PuntoService);
    return PuntoService;
}(crud_service_1.CrudService));
exports.PuntoService = PuntoService;
//# sourceMappingURL=punto.service.js.map
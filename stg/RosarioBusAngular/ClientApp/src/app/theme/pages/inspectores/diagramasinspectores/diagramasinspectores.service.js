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
var environment_1 = require("../../../../../environments/environment");
var file_service_1 = require("../../../../shared/common/file.service");
var DiagramasInspectoresService = /** @class */ (function (_super) {
    __extends(DiagramasInspectoresService, _super);
    function DiagramasInspectoresService(http, fileService) {
        var _this = _super.call(this, http) || this;
        _this.http = http;
        _this.fileService = fileService;
        _this.planificacionUrl = '';
        _this.listModelOriginal = [];
        _this.planificacionUrl = environment_1.environment.planificacionUrl + '/InspDiagramasInspectores';
        _this.endpoint = _this.planificacionUrl;
        return _this;
    }
    DiagramasInspectoresService.prototype.getDiagramaMesAnio = function (id, turnoId, blockentity) {
        var url = this.endpoint + '/DiagramaMesAnioGrupo';
        return this.http.post(url, { Id: id, TurnoId: turnoId, Blockentity: blockentity });
    };
    DiagramasInspectoresService.prototype.getTurnosDeLaDiagramacion = function (id) {
        var url = this.endpoint + '/TurnosDeLaDiagramacion?Id=' + id;
        return this.http.get(url);
    };
    DiagramasInspectoresService.prototype.saveDiagramacion = function (Inspectores, Id, blockDate) {
        var url = this.endpoint + '/SaveDiagramacion';
        return this.http.post(url, { Inspectores: Inspectores, Id: Id, BlockDate: blockDate });
    };
    DiagramasInspectoresService.prototype.eliminarCelda = function (model) {
        var url = this.endpoint + '/EliminarCelda';
        return this.http.post(url, model);
    };
    DiagramasInspectoresService.prototype.publicarDiagramacion = function (Diagrmacion) {
        var url = this.endpoint + '/PublicarDiagramacion';
        return this.http.post(url, Diagrmacion);
    };
    DiagramasInspectoresService.prototype.setDiagramaOriginal = function (listModelOriginal) {
        this.listModelOriginal = listModelOriginal;
    };
    DiagramasInspectoresService.prototype.getRowFromDiagramaOriginal = function (row) {
        return this.listModelOriginal.find(function (e) { return e.NumeroDia == row.NumeroDia; });
    };
    DiagramasInspectoresService.prototype.getImprimirDiagrama = function (id, turnoId) {
        var url = this.endpoint + '/ImprimirDiagrama';
        this.fileService.dowloadAuthenticatedByPost(url, { Id: id, TurnoId: turnoId });
    };
    DiagramasInspectoresService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient,
            file_service_1.FileService])
    ], DiagramasInspectoresService);
    return DiagramasInspectoresService;
}(crud_service_1.CrudService));
exports.DiagramasInspectoresService = DiagramasInspectoresService;
//# sourceMappingURL=diagramasinspectores.service.js.map
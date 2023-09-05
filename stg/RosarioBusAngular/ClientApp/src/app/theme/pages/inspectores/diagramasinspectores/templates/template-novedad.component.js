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
// Angular
var core_1 = require("@angular/core");
var diagramasinspectores_model_1 = require("../../model/diagramasinspectores.model");
var app_component_base_1 = require("../../../../../shared/common/app-component-base");
var material_1 = require("@angular/material");
var _ = require("lodash");
var agregar_franco_component_1 = require("./agregar-franco.component");
var rangoshorarios_service_1 = require("../../rangoshorarios/rangoshorarios.service");
var TemplateNovedadComponent = /** @class */ (function (_super) {
    __extends(TemplateNovedadComponent, _super);
    // Public properties
    function TemplateNovedadComponent(injector, rangosHorarios) {
        var _this = _super.call(this, injector) || this;
        _this.rangosHorarios = rangosHorarios;
        _this.columns = [];
        _this.allowmodificarDiagramacion = false;
        _this.dialog = injector.get(material_1.MatDialog);
        _this.injector = injector;
        return _this;
    }
    TemplateNovedadComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.active = false;
        this.item = this.diaMes.Inspectores.find(function (e) { return e.Legajo == _this.legajo; });
        this.item.EsModificada = false;
        this.allowmodificarDiagramacion = this.permission.isGranted("Inspectores.Diagramacion.Modificar");
    };
    TemplateNovedadComponent.prototype.ngAfterViewInit = function () {
    };
    TemplateNovedadComponent.prototype.ngOnDestroy = function () {
        //this.subs.forEach(e => e.unsubscribe());
    };
    TemplateNovedadComponent.prototype.getColor = function () {
        return this.item.Color;
    };
    TemplateNovedadComponent.prototype.agregarFranco = function (row) {
        var _this = this;
        this.message.confirm('¿Insertar Franco?', '', function (a) {
            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                if (_this.item.PasadaSueldos != "S") {
                    var rangoFranco = _this.rangoshorariosItems.filter(function (r) { return r.EsFranco == true && r.EsFrancoTrabajado == false; });
                    if (rangoFranco.length == 1) {
                        _this.item.EsModificada = true;
                        _this.item.EsFranco = true;
                        _this.item.RangoHorarioId = rangoFranco[0].Id;
                        _this.item.NombreRangoHorario = rangoFranco[0].Description;
                        _this.item.Color = rangoFranco[0].Color;
                        _this.item.diaMesFecha = _this.diaMes.Fecha;
                        _this.columns.find(function (e) { return e.CodEmpleado == _this.item.CodEmpleado; }).CantFrancos++;
                        _this.active = true;
                    }
                    else if (rangoFranco.length > 1) {
                        var dialog;
                        dialog = _this.injector.get(material_1.MatDialog);
                        var dialogConfig = new material_1.MatDialogConfig();
                        dialogConfig.disableClose = false;
                        dialogConfig.autoFocus = true;
                        var cloneData = _.cloneDeep(_this.item);
                        dialogConfig.data = cloneData;
                        dialogConfig.data.rangoshorariosItems = _this.rangoshorariosItems;
                        dialogConfig.data.diaMes = _this.diaMes;
                        dialogConfig.data.listModel = _this.listModel;
                        //dialogConfig.width = '60%';
                        var dialogRef = _this.dialog.open(agregar_franco_component_1.AgregarFrancoComponent, dialogConfig);
                        dialogRef.afterClosed().subscribe(function (data) {
                            if (data) {
                                _this.item.RangoHorarioId = data.RangoHorarioId;
                                _this.rangosHorarios.getById(data.RangoHorarioId)
                                    .subscribe(function (e) {
                                    _this.item.EsModificada = true;
                                    _this.item.EsFranco = true;
                                    _this.item.NombreRangoHorario = e.DataObject.Description;
                                    _this.item.RangoHorarioId = e.DataObject.Id;
                                    _this.item.Color = e.DataObject.Color;
                                    _this.item.diaMesFecha = _this.diaMes.Fecha;
                                    _this.columns.find(function (e) { return e.CodEmpleado == _this.item.CodEmpleado; }).CantFrancos++;
                                    _this.active = true;
                                });
                            }
                        });
                    }
                    else if (rangoFranco.length == 0) {
                        _this.message.error('No existe rango horario para el franco', 'Franco Novedad');
                    }
                }
                else {
                    _this.message.error('Esta Jornada ya fue pasada a sueldo, no es posible editarla', 'Jornada Trabajada en sueldo');
                }
            }
        });
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], TemplateNovedadComponent.prototype, "legajo", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", diagramasinspectores_model_1.DiasMesDto)
    ], TemplateNovedadComponent.prototype, "diaMes", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateNovedadComponent.prototype, "zonasItems", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateNovedadComponent.prototype, "rangoshorariosItems", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateNovedadComponent.prototype, "listModel", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateNovedadComponent.prototype, "columns", void 0);
    TemplateNovedadComponent = __decorate([
        core_1.Component({
            selector: 'template-novedad',
            templateUrl: './template-novedad.component.html',
            exportAs: 'templateNovedad',
            styleUrls: ["./template-novedad.component.css"]
        }),
        __metadata("design:paramtypes", [core_1.Injector, rangoshorarios_service_1.RangosHorariosService])
    ], TemplateNovedadComponent);
    return TemplateNovedadComponent;
}(app_component_base_1.AppComponentBase));
exports.TemplateNovedadComponent = TemplateNovedadComponent;
//# sourceMappingURL=template-novedad.component.js.map
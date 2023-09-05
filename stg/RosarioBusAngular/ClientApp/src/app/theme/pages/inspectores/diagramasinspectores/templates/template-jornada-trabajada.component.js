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
var diagramasinspectores_service_1 = require("./../diagramasinspectores.service");
// Angular
var core_1 = require("@angular/core");
var diagramasinspectores_model_1 = require("../../model/diagramasinspectores.model");
var material_1 = require("@angular/material");
var edit_jornada_trabajada_component_1 = require("./edit-jornada-trabajada.component");
var app_component_base_1 = require("../../../../../shared/common/app-component-base");
var _ = require("lodash");
var diagramas_inspectores_validator_service_1 = require("../diagramas-inspectores-validator.service");
var TemplateJornadaTrabajadaComponent = /** @class */ (function (_super) {
    __extends(TemplateJornadaTrabajadaComponent, _super);
    function TemplateJornadaTrabajadaComponent(injector, diagramaService, _validator) {
        var _this = _super.call(this, injector) || this;
        _this.diagramaService = diagramaService;
        _this._validator = _validator;
        _this.loading = true;
        _this.columns = [];
        _this.setloading = new core_1.EventEmitter();
        _this.allowmodificarDiagramacion = false;
        _this.pagacambia = false;
        _this.dialog = injector.get(material_1.MatDialog);
        _this.injector = injector;
        return _this;
    }
    TemplateJornadaTrabajadaComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.item = this.diaMes.Inspectores.find(function (e) { return e.Legajo == _this.legajo; });
        //this.item.HoraDesde = new Date(this.item.HoraDesde.toString());
        //this.item.HoraHasta = new Date(this.item.HoraHasta.toString());
        this.item.HoraDesdeModificada = new Date(this.item.HoraDesdeModificada.toString());
        this.item.HoraHastaModificada = new Date(this.item.HoraHastaModificada.toString());
        this.allowmodificarDiagramacion = this.permission.isGranted("Inspectores.Diagramacion.Modificar");
    };
    TemplateJornadaTrabajadaComponent.prototype.ngAfterViewInit = function () {
    };
    TemplateJornadaTrabajadaComponent.prototype.ngOnDestroy = function () {
        //this.subs.forEach(e => e.unsubscribe());
    };
    TemplateJornadaTrabajadaComponent.prototype.getColor = function () {
        return this.item.Color;
    };
    TemplateJornadaTrabajadaComponent.prototype.editarJorTrabajo = function () {
        var _this = this;
        if (this.item.PasadaSueldos != "S") {
            var fpft = void 0;
            fpft = this._validator.ValidateFeriadoPermiteFrancoTrabajadoCelda(this.item, this.diaMes);
            if (!fpft.isValid && this.item.Pago == 0) {
                this.pagacambia = true;
            }
            var dialog;
            dialog = this.injector.get(material_1.MatDialog);
            var dialogConfig = new material_1.MatDialogConfig();
            dialogConfig.disableClose = false;
            dialogConfig.autoFocus = true;
            var cloneData = _.cloneDeep(this.item);
            dialogConfig.data = cloneData;
            dialogConfig.data.HoraDesdeModificada = this.item.HoraDesdeModificada;
            dialogConfig.data.HoraHastaModificada = this.item.HoraHastaModificada;
            dialogConfig.data.diaMes = this.diaMes;
            dialogConfig.data.zonasItems = this.zonasItems;
            dialogConfig.data.listModel = this.listModel;
            dialogConfig.data.diasMesAP = this.diasMesAP;
            dialogConfig.data.allowPagaCambia = this.pagacambia;
            var dialogRef = this.dialog.open(edit_jornada_trabajada_component_1.EditJornadaTrabajadaComponent, dialogConfig);
            dialogRef.afterClosed().subscribe(function (data) {
                if (data) {
                    _this.item.EsModificada = true;
                    _this.item.HoraDesdeModificada = data.HoraDesdeModificada;
                    _this.item.HoraHastaModificada = data.HoraHastaModificada;
                    var newZona = _this.zonasItems.find(function (z) { return z.Id == data.ZonaId; });
                    _this.item.ZonaId = data.ZonaId;
                    _this.item.NombreZona = newZona.Description;
                    _this.item.diaMesFecha = data.diaMes.Fecha;
                    _this.item.validations = [];
                    _this.item.Pago = data.Pago;
                    _this.item.Pago = data.Pago;
                    //this.notify.info('Guardado exitosamente');
                }
            });
        }
        else {
            this.message.error('Esta Jornada ya fue pasada a sueldo, no es posible editarla', 'Jornada Trabajada en sueldo');
        }
    };
    TemplateJornadaTrabajadaComponent.prototype.getErrorMessage = function (item) {
        return item.validations.map(function (e) { return e.Messages; }).join("-");
    };
    TemplateJornadaTrabajadaComponent.prototype.eliminarJornada = function (item) {
        var _this = this;
        var diaMes = this.diagramaService.getRowFromDiagramaOriginal(this.diaMes);
        var itemOriginal = diaMes.Inspectores.find(function (x) { return x.Legajo == _this.legajo; });
        this.message.confirm('¿Está seguro que quiere eliminar el registro?', '', function (a) {
            if (a.value) {
                if (item.EsModificada) {
                    if (itemOriginal) {
                        if (itemOriginal.EsFranco && !itemOriginal.EsFrancoTrabajado) {
                            //item modificado = item original
                            _this.item.Nombre = itemOriginal.Nombre;
                            _this.item.Apellido = itemOriginal.Apellido;
                            _this.item.CodEmpleado = itemOriginal.CodEmpleado;
                            _this.item.DescripcionInspector = itemOriginal.DescripcionInspector;
                            _this.item.InspColor = itemOriginal.InspColor;
                            _this.item.InspTurno = itemOriginal.InspTurno;
                            _this.item.Legajo = itemOriginal.Legajo;
                            _this.item.Id = itemOriginal.Id;
                            _this.item.EsJornada = itemOriginal.EsJornada;
                            _this.item.CodJornada = itemOriginal.CodJornada;
                            _this.item.EsFrancoTrabajado = itemOriginal.EsFrancoTrabajado;
                            _this.item.EsFranco = itemOriginal.EsFranco;
                            _this.item.EsNovedad = itemOriginal.EsNovedad;
                            _this.item.FrancoNovedad = itemOriginal.FrancoNovedad;
                            _this.item.RangoHorarioId = itemOriginal.RangoHorarioId;
                            _this.item.ZonaId = itemOriginal.ZonaId;
                            _this.item.HoraDesde = itemOriginal.HoraDesde;
                            _this.item.HoraHasta = itemOriginal.HoraHasta;
                            _this.item.HoraDesdeModificada = itemOriginal.HoraDesdeModificada;
                            _this.item.HoraHastaModificada = itemOriginal.HoraHastaModificada;
                            _this.item.Color = itemOriginal.Color;
                            _this.item.NombreZona = itemOriginal.NombreZona;
                            _this.item.NombreRangoHorario = itemOriginal.NombreRangoHorario;
                            _this.item.DescNovedad = itemOriginal.DescNovedad;
                            _this.item.PasadaSueldos = itemOriginal.PasadaSueldos;
                            // this.item.zonasItems = itemOriginal.zonasItems;
                            // this.item.rangosItems = itemOriginal.rangosItems;
                            // this.item.diaMes = itemOriginal.diaMes;
                            //this.item.diaMesFecha = itemOriginal.diaMesFecha;
                            _this.columns.find(function (e) { return e.CodEmpleado == _this.item.CodEmpleado; }).CantFrancos++;
                            _this.item.GrupoInspectoresId = itemOriginal.GrupoInspectoresId;
                            // this.item.validations = itemOriginal.validations;
                            _this.item.validations = [];
                            _this.item.EsModificada = false;
                        }
                        else {
                            if (itemOriginal.CodJornada != 0) {
                                _this.eliminar(item, _this.diaMes);
                            }
                            else {
                                _this.item.EsFranco = false;
                                _this.item.EsFrancoTrabajado = false;
                                _this.item.EsJornada = false;
                                _this.item.EsModificada = false;
                                _this.item.EsNovedad = false;
                                _this.item.FrancoNovedad = false;
                                _this.item.ZonaId = null;
                                _this.item.RangoHorarioId = null;
                                _this.item.EsModificada = false;
                            }
                        }
                    }
                    else {
                        _this.item.EsFranco = false;
                        _this.item.EsFrancoTrabajado = false;
                        _this.item.EsJornada = false;
                        _this.item.EsModificada = false;
                        _this.item.EsNovedad = false;
                        _this.item.FrancoNovedad = false;
                        _this.item.ZonaId = null;
                        _this.item.RangoHorarioId = null;
                    }
                }
                else {
                    _this.eliminar(item, _this.diaMes);
                }
            }
        });
    };
    TemplateJornadaTrabajadaComponent.prototype.eliminar = function (item, row) {
        var _this = this;
        var cloneData = _.cloneDeep(row);
        cloneData.BlockDate = this.BlockDate;
        cloneData.Inspectores = [item];
        console.log("Eliminar Jornada Trabajada:::::", cloneData);
        this.setloading.emit(true);
        this.diagramaService.eliminarCelda(cloneData)
            .finally(function () {
            _this.loading = false;
            _this.setloading.emit(false);
            _this.diagramacionBusyText = "Cargando...";
        })
            .subscribe(function (e) {
            _this.item.Nombre = e.DataObject.Nombre;
            _this.item.Apellido = e.DataObject.Apellido;
            _this.item.CodEmpleado = e.DataObject.CodEmpleado;
            _this.item.DescripcionInspector = e.DataObject.DescripcionInspector;
            _this.item.InspColor = e.DataObject.InspColor;
            _this.item.InspTurno = e.DataObject.InspTurno;
            _this.item.Legajo = e.DataObject.Legajo;
            _this.item.Id = e.DataObject.Id;
            _this.item.Nombre = e.DataObject.Nombre;
            _this.item.EsJornada = e.DataObject.EsJornada;
            _this.item.CodJornada = e.DataObject.CodJornada;
            _this.item.EsFrancoTrabajado = e.DataObject.EsFrancoTrabajado;
            _this.item.EsFranco = e.DataObject.EsFranco;
            _this.item.EsNovedad = e.DataObject.EsNovedad;
            _this.item.FrancoNovedad = e.DataObject.FrancoNovedad;
            _this.item.RangoHorarioId = e.DataObject.RangoHorarioId;
            _this.item.ZonaId = e.DataObject.ZonaId;
            _this.item.HoraDesde = e.DataObject.HoraDesde;
            _this.item.HoraHasta = e.DataObject.HoraHasta;
            _this.item.HoraDesdeModificada = e.DataObject.HoraDesdeModificada;
            _this.item.HoraHastaModificada = e.DataObject.HoraHastaModificada;
            _this.item.Color = e.DataObject.Color;
            _this.item.NombreZona = e.DataObject.NombreZona;
            _this.item.NombreRangoHorario = e.DataObject.NombreRangoHorario;
            _this.item.DescNovedad = e.DataObject.DescNovedad;
            _this.item.PasadaSueldos = e.DataObject.PasadaSueldos;
            // this.item.zonasItems = e.DataObject.zonasItems;
            // this.item.rangosItems = e.DataObject.rangosItems;
            // this.item.diaMes = e.DataObject.diaMes;
            _this.item.GrupoInspectoresId = e.DataObject.GrupoInspectoresId;
            _this.item.validations = [];
            _this.item.EsModificada = false;
            _this.notify.info('Guardado exitosamente');
        }, function (error) {
            _this.handleErros(error);
        });
    };
    TemplateJornadaTrabajadaComponent.prototype.handleErros = function (err) {
        console.log('sever error:', err); // debug
        if (err.error.Status == "ConcurrencyValidator") {
            this.notify.error("No es posible realizar los cambios ya que la entidad fue alterada por otro usuario.");
        }
        else if (err.error && err.error.Status == "ValidationError") {
            this.notify.error(err.error.Messages.toString());
        }
        else {
            this.notify.error("A ocurrido un erro por favor contactese con el administrador");
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Date)
    ], TemplateJornadaTrabajadaComponent.prototype, "BlockDate", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], TemplateJornadaTrabajadaComponent.prototype, "legajo", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", diagramasinspectores_model_1.DiasMesDto)
    ], TemplateJornadaTrabajadaComponent.prototype, "diaMes", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateJornadaTrabajadaComponent.prototype, "zonasItems", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateJornadaTrabajadaComponent.prototype, "listModel", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateJornadaTrabajadaComponent.prototype, "diasMesAP", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], TemplateJornadaTrabajadaComponent.prototype, "columns", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TemplateJornadaTrabajadaComponent.prototype, "setloading", void 0);
    TemplateJornadaTrabajadaComponent = __decorate([
        core_1.Component({
            selector: 'template-jornada-trabajada',
            templateUrl: './template-jornada-trabajada.component.html',
            exportAs: 'templateJornadaTrabajada',
            styleUrls: ["./template-jornada-trabajada.component.css"]
        }),
        __metadata("design:paramtypes", [core_1.Injector, diagramasinspectores_service_1.DiagramasInspectoresService, diagramas_inspectores_validator_service_1.DiagramasInspectoresValidatorService])
    ], TemplateJornadaTrabajadaComponent);
    return TemplateJornadaTrabajadaComponent;
}(app_component_base_1.AppComponentBase));
exports.TemplateJornadaTrabajadaComponent = TemplateJornadaTrabajadaComponent;
//# sourceMappingURL=template-jornada-trabajada.component.js.map
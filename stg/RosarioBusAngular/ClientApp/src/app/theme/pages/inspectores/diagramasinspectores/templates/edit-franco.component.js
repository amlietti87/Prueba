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
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
// Angular
var core_1 = require("@angular/core");
var diagramasinspectores_model_1 = require("../../model/diagramasinspectores.model");
var material_1 = require("@angular/material");
var app_component_base_1 = require("../../../../../shared/common/app-component-base");
var forms_1 = require("@angular/forms");
var moment = require("moment");
var diagramas_inspectores_validator_service_1 = require("../diagramas-inspectores-validator.service");
var EditFrancoComponent = /** @class */ (function (_super) {
    __extends(EditFrancoComponent, _super);
    function EditFrancoComponent(dialogRef, _validator, data, injector) {
        var _this = _super.call(this, injector) || this;
        _this.dialogRef = dialogRef;
        _this._validator = _validator;
        _this.data = data;
        _this.allowZona = false;
        _this.allowRango = false;
        _this.rangoUnico = false;
        _this.allowPago = false;
        _this.detail = data;
        return _this;
    }
    EditFrancoComponent.prototype.ngOnInit = function () {
        var rangosConFrancosTrabajados = this.detail.rangosItems.filter(function (rango) { return rango.EsFrancoTrabajado == true; });
        if (rangosConFrancosTrabajados.length == 1) {
            this.rangoUnico = true;
        }
        this.detail.HoraDesdeModificadaValue = {
            hour: null,
            minute: null
        };
        this.detail.HoraHastaModificadaValue = {
            hour: null,
            minute: null
        };
    };
    EditFrancoComponent.prototype.ngAfterViewInit = function () {
    };
    EditFrancoComponent.prototype.ngOnDestroy = function () {
        //this.subs.forEach(e => e.unsubscribe());
    };
    EditFrancoComponent.prototype.close = function () {
        this.dialogRef.close();
    };
    EditFrancoComponent.prototype.saveChanges = function (form) {
        var _this = this;
        this.allowZona = (this.detail.ZonaId == null || this.detail.ZonaId.toString() == "" || this.detail.ZonaId == undefined || this.detail.ZonaId.toString() == "null");
        this.allowRango = (this.detail.RangoHorarioId == null || this.detail.RangoHorarioId.toString() == "" || this.detail.RangoHorarioId == undefined || this.detail.RangoHorarioId.toString() == "null");
        this.allowPago = (this.detail.Pago == null || this.detail.Pago.toString() == "" || this.detail.Pago == undefined || this.detail.Pago.toString() == "null");
        if (this.allowZona || this.allowRango || this.allowPago)
            return;
        this.detail.RangoHorarioId = this.editFrancoForm.controls['RangoHorarioId'].value;
        this.detail.ZonaId = this.editFrancoForm.controls['ZonaId'].value;
        var fechaInicialMoment = moment(this.detail.diaMes.Fecha);
        var horaHastaModificadaValue = this.editFrancoForm.controls['HoraHastaModificadaValue'].value;
        var horaDesdeModificadaValue = this.editFrancoForm.controls['HoraDesdeModificadaValue'].value;
        //Hora Desde Formateada
        var horaDesdeModificada = new Date(fechaInicialMoment.year(), fechaInicialMoment.month(), fechaInicialMoment.date(), horaDesdeModificadaValue.hour, horaDesdeModificadaValue.minute);
        this.detail.HoraDesdeModificada = horaDesdeModificada;
        //Hora Hasta Formateada 
        var horaHastaModificada = new Date(fechaInicialMoment.year(), fechaInicialMoment.month(), fechaInicialMoment.date(), horaHastaModificadaValue.hour, horaHastaModificadaValue.minute);
        this.detail.HoraHastaModificada = horaHastaModificada;
        this.detail.diaMesFecha = this.detail.diaMes.Fecha;
        var agregarUnDia = false;
        // Validacion cambio de dia
        if (horaDesdeModificadaValue.hour > horaHastaModificadaValue.hour) {
            agregarUnDia = true;
            this.detail.HoraHastaModificada.setDate(this.detail.HoraHastaModificada.getDate() + 1);
        }
        else if (horaDesdeModificadaValue.hour == horaHastaModificadaValue.hour) {
            if (horaDesdeModificadaValue.minute > horaHastaModificadaValue.minute) {
                agregarUnDia = true;
                this.detail.HoraHastaModificada.setDate(this.detail.HoraHastaModificada.getDate() + 1);
            }
        }
        var fpe = new diagramasinspectores_model_1.ValidationResult();
        fpe.isValid = true;
        var hfi = new diagramasinspectores_model_1.ValidationResult();
        hfi.isValid = true;
        var hfg = new diagramasinspectores_model_1.ValidationResult();
        hfg.isValid = true;
        fpe = this._validator.ValidateFeriadoPermiteHsExtras(this.detail, this.detail.diaMes, this.detail.listModel, this.detail.diasMesAP);
        hfi = this._validator.ValidateHorasFeriadoParaInspector(this.detail, this.detail.listModel, this.detail.diasMesAP);
        hfg = this._validator.ValidateHorasFeriadoPorGrupo(this.detail, this.detail.listModel, this.detail.diasMesAP);
        var hfti = new diagramasinspectores_model_1.ValidationResult();
        hfti.isValid = true;
        var hftg = new diagramasinspectores_model_1.ValidationResult();
        hftg.isValid = true;
        hfti = this._validator.ValidateHorasFrancoTrabajadoParaInspector(this.detail, this.detail.listModel, this.detail.diasMesAP);
        hftg = this._validator.ValidateHorasFrancoTrabajadoPorGrupo(this.detail, this.detail.listModel, this.detail.diasMesAP);
        var hei = this._validator.ValidateHorasExtrasParaInspector(this.detail, this.detail.listModel, this.detail.diasMesAP);
        var heg = this._validator.ValidateHorasExtrasPorGrupo(this.detail, this.detail.listModel, this.detail.diasMesAP);
        if (!hfti.isValid || !hftg.isValid || !fpe.isValid || !hfi.isValid || !hfg.isValid || !hei.isValid || !heg.isValid) {
            if (!hfti.isValid) {
                this.notify.warn(hfti.Messages[0]);
                this.detail.validations.push(hfti);
            }
            if (!hftg.isValid) {
                this.notify.warn(hftg.Messages[0]);
                this.detail.validations.push(hftg);
            }
            if (!fpe.isValid) {
                this.notify.warn(fpe.Messages[0]);
                this.detail.validations.push(fpe);
            }
            if (!hfi.isValid) {
                this.notify.warn(hfi.Messages[0]);
                this.detail.validations.push(hfi);
            }
            if (!hfg.isValid) {
                this.notify.warn(hfg.Messages[0]);
                this.detail.validations.push(hfg);
            }
            if (!hei.isValid) {
                this.notify.warn(hei.Messages[0]);
                this.detail.validations.push(hei);
            }
            if (!heg.isValid) {
                this.notify.warn(heg.Messages[0]);
                this.detail.validations.push(heg);
            }
            return;
        }
        if (agregarUnDia) {
            //El turno termina al d�a siguiente
            this.message.confirm("", "El turno termina al dia siguiente", function (r) {
                if (r.value) {
                    _this.dialogRef.close(_this.detail);
                }
            });
        }
        else {
            this.dialogRef.close(this.detail);
        }
    };
    __decorate([
        core_1.ViewChild('editFrancoForm'),
        __metadata("design:type", forms_1.NgForm)
    ], EditFrancoComponent.prototype, "editFrancoForm", void 0);
    EditFrancoComponent = __decorate([
        core_1.Component({
            selector: 'exportarminutosporsector',
            templateUrl: './edit-franco.component.html',
            styleUrls: ["./edit-franco.component.css"]
        }),
        __param(2, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef, diagramas_inspectores_validator_service_1.DiagramasInspectoresValidatorService,
            diagramasinspectores_model_1.InspectorDiaDto,
            core_1.Injector])
    ], EditFrancoComponent);
    return EditFrancoComponent;
}(app_component_base_1.AppComponentBase));
exports.EditFrancoComponent = EditFrancoComponent;
//# sourceMappingURL=edit-franco.component.js.map
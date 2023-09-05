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
var diagramas_inspectores_validator_service_1 = require("../diagramas-inspectores-validator.service");
var diagramasinspectores_service_1 = require("../diagramasinspectores.service");
var AgregarFrancoComponent = /** @class */ (function (_super) {
    __extends(AgregarFrancoComponent, _super);
    // Public properties
    function AgregarFrancoComponent(dialogRef, _validator, _diagramacion, data, injector) {
        var _this = _super.call(this, injector) || this;
        _this.dialogRef = dialogRef;
        _this._validator = _validator;
        _this._diagramacion = _diagramacion;
        _this.data = data;
        _this.filter = data;
        return _this;
    }
    AgregarFrancoComponent.prototype.ngOnInit = function () {
        console.log("filter franco", this.filter);
    };
    AgregarFrancoComponent.prototype.ngAfterViewInit = function () {
    };
    AgregarFrancoComponent.prototype.ngOnDestroy = function () {
        //this.subs.forEach(e => e.unsubscribe());
    };
    AgregarFrancoComponent.prototype.close = function () {
        this.dialogRef.close(false);
    };
    AgregarFrancoComponent.prototype.agregarFranco = function (RangoHorarioId) {
        if (RangoHorarioId != null && RangoHorarioId != 'null') {
            this.filter.NombreRangoHorario = this.agregarFrancoForm.controls['ComboRangoHorario'].value;
            this.filter.diaMesFecha = this.filter.diaMes.Fecha;
            this.dialogRef.close(this.filter);
            console.log("agregar-franco::::", this.filter);
        }
    };
    __decorate([
        core_1.ViewChild('agregarFrancoForm'),
        __metadata("design:type", forms_1.NgForm)
    ], AgregarFrancoComponent.prototype, "agregarFrancoForm", void 0);
    AgregarFrancoComponent = __decorate([
        core_1.Component({
            selector: 'exportarminutosporsector',
            templateUrl: './agregar-franco.component.html',
        }),
        __param(3, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef, diagramas_inspectores_validator_service_1.DiagramasInspectoresValidatorService, diagramasinspectores_service_1.DiagramasInspectoresService,
            diagramasinspectores_model_1.InspectorDiaDto,
            core_1.Injector])
    ], AgregarFrancoComponent);
    return AgregarFrancoComponent;
}(app_component_base_1.AppComponentBase));
exports.AgregarFrancoComponent = AgregarFrancoComponent;
//# sourceMappingURL=agregar-franco.component.js.map
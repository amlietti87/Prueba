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
var core_1 = require("@angular/core");
var detail_component_1 = require("../../../../shared/manager/detail.component");
var material_1 = require("@angular/material");
var diagramasinspectores_model_1 = require("../model/diagramasinspectores.model");
var diagramasinspectores_service_1 = require("./diagramasinspectores.service");
var estadosdiagramainspectores_service_1 = require("../estadosdiagramainspectores/estadosdiagramainspectores.service");
var CreateOrEditDiagramasInspectoresModalComponent = /** @class */ (function (_super) {
    __extends(CreateOrEditDiagramasInspectoresModalComponent, _super);
    function CreateOrEditDiagramasInspectoresModalComponent(dialogRef, data, injector, service, estadosDiagrama, cdr) {
        var _this = _super.call(this, dialogRef, service, injector, data) || this;
        _this.data = data;
        _this.estadosDiagrama = estadosDiagrama;
        _this.cdr = cdr;
        _this.allowmodificargropoInsp = false;
        _this.allowAddZona = false;
        _this.grupoInspRequerido = false;
        _this.mesRequerido = false;
        _this.anioRequerido = false;
        _this.anioIncorrecto = false;
        _this.estadoRequerido = true;
        _this.cfr = injector.get(core_1.ComponentFactoryResolver);
        _this.IsInMaterialPopupMode = true;
        return _this;
    }
    CreateOrEditDiagramasInspectoresModalComponent.prototype.completedataBeforeShow = function (item) {
        if (this.detail.Id == null) {
            var today = new Date();
            this.detail.Anio = today.getFullYear();
            this.detail.Mes = today.getMonth() + 1;
            this.detail.EstadoDiagramaId = diagramasinspectores_model_1.EstadosDiagrama.Borrador;
            if (this.detail.EstadoDiagramaId == null || this.detail.EstadoDiagramaId.toString() == '' || this.detail.EstadoDiagramaId == 0) {
                this.estadoRequerido = true;
            }
            else {
                this.estadoRequerido = false;
            }
            this.cdr.detectChanges();
        }
    };
    CreateOrEditDiagramasInspectoresModalComponent.prototype.save = function (form) {
        this.grupoInspRequerido = (this.detail.GrupoInspectoresId == null || this.detail.GrupoInspectoresId.toString() == 'null' || this.detail.GrupoInspectoresId.toString() == '');
        this.mesRequerido = (this.detail.Mes == null || this.detail.Mes.toString() == 'null' || this.detail.Mes.toString() == '');
        this.anioRequerido = (this.detail.Anio == null || this.detail.Anio.toString() == 'null' || this.detail.Anio.toString() == '');
        this.anioIncorrecto = (this.anioRequerido || this.detail.Anio.toString().length != 4);
        if (this.grupoInspRequerido || this.mesRequerido || this.anioRequerido || this.anioIncorrecto)
            return;
        this.saving = true;
        this.completedataBeforeSave(this.detail);
        if (!this.validateSave()) {
            this.saving = false;
            return;
        }
        this.SaveDetail();
    };
    CreateOrEditDiagramasInspectoresModalComponent.prototype.OnMesInspectoresComboChanged = function ($$event) {
        if (this.detail.Mes == null || this.detail.Mes.toString() == 'null' || this.detail.Mes.toString() == '') {
            this.mesRequerido = true;
        }
        else {
            this.mesRequerido = false;
        }
    };
    CreateOrEditDiagramasInspectoresModalComponent.prototype.OnAnioInspectoresComboChanged = function ($event) {
        if ($event == null || $event == 'null' || $event == '') {
            this.anioRequerido = true;
            this.anioIncorrecto = false;
        }
        else {
            this.anioRequerido = false;
            if ($event.toString().length == 4) {
                this.anioIncorrecto = false;
            }
            else {
                this.anioIncorrecto = true;
            }
        }
    };
    CreateOrEditDiagramasInspectoresModalComponent.prototype.OnGrupoInspectoresComboChanged = function ($event) {
        if (this.detail.GrupoInspectoresId == null || this.detail.GrupoInspectoresId.toString() == '') {
            this.grupoInspRequerido = true;
        }
        else {
            this.grupoInspRequerido = false;
        }
    };
    __decorate([
        core_1.ViewChild('createOrEditChild', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], CreateOrEditDiagramasInspectoresModalComponent.prototype, "createOrEditChild", void 0);
    CreateOrEditDiagramasInspectoresModalComponent = __decorate([
        core_1.Component({
            selector: 'createOrEditDiagramasInspectoresDtoModal',
            templateUrl: './create-or-edit-diagramasinspectores-modal.component.html',
        }),
        __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef,
            diagramasinspectores_model_1.DiagramasInspectoresDto,
            core_1.Injector,
            diagramasinspectores_service_1.DiagramasInspectoresService,
            estadosdiagramainspectores_service_1.EstadosDiagramaInspectoresService,
            core_1.ChangeDetectorRef])
    ], CreateOrEditDiagramasInspectoresModalComponent);
    return CreateOrEditDiagramasInspectoresModalComponent;
}(detail_component_1.DetailAgregationComponent));
exports.CreateOrEditDiagramasInspectoresModalComponent = CreateOrEditDiagramasInspectoresModalComponent;
//# sourceMappingURL=create-or-edit-diagramasinspectores-modal.component.js.map
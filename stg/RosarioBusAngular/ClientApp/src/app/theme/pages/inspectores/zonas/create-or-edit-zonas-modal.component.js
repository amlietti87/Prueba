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
var zonas_service_1 = require("./zonas.service");
var zonas_model_1 = require("../model/zonas.model");
var material_1 = require("@angular/material");
var CreateOrEditZonasModalComponent = /** @class */ (function (_super) {
    __extends(CreateOrEditZonasModalComponent, _super);
    function CreateOrEditZonasModalComponent(dialogRef, data, injector, service) {
        var _this = _super.call(this, dialogRef, service, injector, data) || this;
        _this.dialogRef = dialogRef;
        _this.data = data;
        _this.service = service;
        _this.allowmodificarzona = false;
        //this.detail = new ZonasDto();
        _this.cfr = injector.get(core_1.ComponentFactoryResolver);
        _this.saveLocal = false;
        _this.IsInMaterialPopupMode = true;
        _this.allowmodificarzona = _this.permission.isGranted("Inspectores.Zonas.Modificar");
        return _this;
    }
    CreateOrEditZonasModalComponent.prototype.save = function (form) {
        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }
        this.saving = true;
        this.completedataBeforeSave(this.detail);
        if (!this.validateSave()) {
            this.saving = false;
            return;
        }
        this.SaveDetail();
    };
    __decorate([
        core_1.ViewChild('createOrEditChild', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], CreateOrEditZonasModalComponent.prototype, "createOrEditChild", void 0);
    CreateOrEditZonasModalComponent = __decorate([
        core_1.Component({
            selector: 'createOrEditZonasDtoModal',
            templateUrl: './create-or-edit-zonas-modal.component.html',
        }),
        __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef,
            zonas_model_1.ZonasDto,
            core_1.Injector,
            zonas_service_1.ZonasService])
    ], CreateOrEditZonasModalComponent);
    return CreateOrEditZonasModalComponent;
}(detail_component_1.DetailAgregationComponent));
exports.CreateOrEditZonasModalComponent = CreateOrEditZonasModalComponent;
//# sourceMappingURL=create-or-edit-zonas-modal.component.js.map
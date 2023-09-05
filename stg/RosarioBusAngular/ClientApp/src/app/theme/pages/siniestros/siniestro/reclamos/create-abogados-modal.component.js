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
var detail_component_1 = require("../../../../../shared/manager/detail.component");
var base_model_1 = require("../../../../../shared/model/base.model");
var localidad_service_1 = require("../../localidades/localidad.service");
var material_1 = require("@angular/material");
var abogados_model_1 = require("../../model/abogados.model");
var abogados_service_1 = require("../../abogados/abogados.service");
var CreateAbogadosModalComponent = /** @class */ (function (_super) {
    __extends(CreateAbogadosModalComponent, _super);
    function CreateAbogadosModalComponent(injector, service, localidadservice, dialogRef, data) {
        var _this = _super.call(this, service, injector) || this;
        _this.service = service;
        _this.localidadservice = localidadservice;
        _this.dialogRef = dialogRef;
        _this.data = data;
        _this.cfr = injector.get(core_1.ComponentFactoryResolver);
        _this.detail = new abogados_model_1.AbogadosDto();
        return _this;
    }
    CreateAbogadosModalComponent.prototype.save = function (form) {
        _super.prototype.save.call(this, form);
    };
    CreateAbogadosModalComponent.prototype.completedataBeforeShow = function (item) {
        if (this.viewMode == base_model_1.ViewMode.Modify) {
            this.localidadservice.getById(item.LocalidadId)
                //.finally(() => { this.isTableLoading = false; })
                .subscribe(function (t) {
                var findlocalidad = new base_model_1.ItemDto();
                findlocalidad.Id = item.LocalidadId;
                findlocalidad.Description = t.DataObject.DscLocalidad + ' - ' + t.DataObject.CodPostal;
                item.selectLocalidades = findlocalidad;
            });
        }
    };
    CreateAbogadosModalComponent.prototype.completedataBeforeSave = function (item) {
        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    };
    CreateAbogadosModalComponent.prototype.close = function () {
        _super.prototype.close.call(this);
        this.dialogRef.close(false);
    };
    CreateAbogadosModalComponent.prototype.SaveDetail = function () {
        var _this = this;
        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(function () { _this.saving = false; })
            .subscribe(function (t) {
            if (_this.viewMode = base_model_1.ViewMode.Add) {
                _this.detail.Id = t.DataObject;
                _this.detail.Description = _this.detail.Description;
            }
            _this.notify.info('Guardado exitosamente');
            _this.affterSave(_this.detail);
            _this.closeOnSave = true;
            _this.modalSave.emit(null);
            _this.dialogRef.close(_this.detail);
        });
    };
    CreateAbogadosModalComponent = __decorate([
        core_1.Component({
            selector: 'createAbogadosDtoModal',
            templateUrl: './create-abogados-modal.component.html',
        }),
        __param(4, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [core_1.Injector,
            abogados_service_1.AbogadosService,
            localidad_service_1.LocalidadesService,
            material_1.MatDialogRef, Object])
    ], CreateAbogadosModalComponent);
    return CreateAbogadosModalComponent;
}(detail_component_1.DetailEmbeddedComponent));
exports.CreateAbogadosModalComponent = CreateAbogadosModalComponent;
//# sourceMappingURL=create-abogados-modal.component.js.map
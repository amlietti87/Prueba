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
var coordenadas_service_1 = require("./coordenadas.service");
var coordenadas_model_1 = require("../model/coordenadas.model");
var material_1 = require("@angular/material");
var localidad_service_1 = require("../../siniestros/localidades/localidad.service");
var base_model_1 = require("../../../../shared/model/base.model");
var CreateOrEditCoordenadaModalComponent = /** @class */ (function (_super) {
    __extends(CreateOrEditCoordenadaModalComponent, _super);
    function CreateOrEditCoordenadaModalComponent(dialogRef, injector, service, localidadservice, data) {
        var _this = _super.call(this, dialogRef, service, injector, data) || this;
        _this.dialogRef = dialogRef;
        _this.service = service;
        _this.localidadservice = localidadservice;
        _this.data = data;
        if (!data) {
            _this.detail = new coordenadas_model_1.CoordenadasDto();
        }
        _this.title = "Sector";
        return _this;
    }
    CreateOrEditCoordenadaModalComponent.prototype.completedataBeforeShow = function (item) {
        if (this.viewMode == base_model_1.ViewMode.Modify) {
            if (item.LocalidadId) {
                if (item.Localidad) {
                    var findlocalidad = new base_model_1.ItemDto();
                    findlocalidad.Id = item.LocalidadId;
                    findlocalidad.Description = item.Localidad;
                    item.selectLocalidades = findlocalidad;
                }
                else {
                    this.localidadservice.getById(item.LocalidadId)
                        //.finally(() => { this.isTableLoading = false; })
                        .subscribe(function (t) {
                        var findlocalidad = new base_model_1.ItemDto();
                        findlocalidad.Id = item.LocalidadId;
                        findlocalidad.Description = t.DataObject.DscLocalidad + ' - ' + t.DataObject.CodPostal;
                        item.selectLocalidades = findlocalidad;
                    });
                }
            }
        }
    };
    CreateOrEditCoordenadaModalComponent.prototype.completedataBeforeSave = function (item) {
        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    };
    CreateOrEditCoordenadaModalComponent = __decorate([
        core_1.Component({
            selector: 'createOrEditCoordenadaDtoModal',
            templateUrl: './create-or-edit-coordenadas-modal.component.html',
        }),
        __param(4, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef,
            core_1.Injector,
            coordenadas_service_1.CoordenadasService,
            localidad_service_1.LocalidadesService,
            coordenadas_model_1.CoordenadasDto])
    ], CreateOrEditCoordenadaModalComponent);
    return CreateOrEditCoordenadaModalComponent;
}(detail_component_1.DetailAgregationComponent));
exports.CreateOrEditCoordenadaModalComponent = CreateOrEditCoordenadaModalComponent;
//# sourceMappingURL=create-or-edit-coordenadas-modal.component.js.map
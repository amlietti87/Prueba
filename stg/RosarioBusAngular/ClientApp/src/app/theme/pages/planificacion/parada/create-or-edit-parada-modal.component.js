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
var parada_service_1 = require("./parada.service");
var parada_model_1 = require("../model/parada.model");
var material_1 = require("@angular/material");
var localidad_service_1 = require("../../siniestros/localidades/localidad.service");
var base_model_1 = require("../../../../shared/model/base.model");
var selectmarker_maps_component_1 = require("../../../../components/rbmaps/selectmarker.maps.component");
var forms_1 = require("@angular/forms");
var CreateOrEditParadaModalComponent = /** @class */ (function (_super) {
    __extends(CreateOrEditParadaModalComponent, _super);
    function CreateOrEditParadaModalComponent(dialogRef, injector, service, localidadservice, cdr, data) {
        var _this = _super.call(this, dialogRef, service, injector, data) || this;
        _this.dialogRef = dialogRef;
        _this.service = service;
        _this.localidadservice = localidadservice;
        _this.cdr = cdr;
        _this.data = data;
        if (!data) {
            _this.detail = new parada_model_1.ParadaDto();
        }
        _this.title = "Parada";
        return _this;
    }
    CreateOrEditParadaModalComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    CreateOrEditParadaModalComponent.prototype.inicializarMapa = function () {
        var _this = this;
        var long;
        var lat;
        long = -60.655931;
        lat = -32.954517;
        this.mapaComponents.crearMapa(lat, long);
        if (this.data && this.data.Long && this.data.Lat) {
            this.mapaComponents.removeLayerPuntos();
            var marker = this.mapaComponents.AgregarMarcador_lat_lng(this.data.Lat, this.data.Long, true);
            this.mapaComponents.AfterAddMaker.emit(marker);
            setTimeout(function () { _this.mapaComponents.setCenter(_this.data.Lat, _this.data.Long); }, 100);
        }
        else {
            this.mapaComponents.setCenter(lat, long);
        }
        this.mapaComponents.AfterAddMaker.subscribe(function (e) {
            _this.data.Lat = e.lat;
            _this.data.Long = e.lng;
        });
    };
    CreateOrEditParadaModalComponent.prototype.cambiotipoparada = function () {
        if (this.detail.LocationType != null) {
            if (this.detail.LocationType == 2) {
                this.detailForm.controls['ParentStationId'].clearValidators();
                this.detailForm.controls['ParentStationId'].setValidators(forms_1.Validators.required);
                this.addEstacion = true;
            }
            else {
                this.detailForm.controls['ParentStationId'].clearValidators();
                this.detailForm.controls['ParentStationId'].setValidators(null);
                this.addEstacion = false;
            }
        }
        this.cdr.detectChanges();
    };
    CreateOrEditParadaModalComponent.prototype.searchGmap = function () {
        var Calle = this.detail.Calle;
        var Cruce = this.detail.Cruce;
        var localidad = this.detail.selectLocalidades ? this.detail.selectLocalidades.Description : "";
        localidad = (localidad.split(' - ')[0]).trim();
        var buscarpo = Calle + " y " + Cruce + " ,  " + localidad;
        this.mapaComponents.buscar_Gmaps(buscarpo);
    };
    CreateOrEditParadaModalComponent.prototype.completedataBeforeShow = function (item) {
        var _this = this;
        this.addEstacion = false;
        this.cantChange = false;
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
                if (item.LocationType == 1) {
                    debugger;
                    var f = { ParentStationId: item.Id };
                    this.service.requestAllByFilter(f)
                        .subscribe(function (e) {
                        var paradas = e.DataObject.Items;
                        if (paradas.length != 0) {
                            _this.cantChange = true;
                        }
                    });
                }
                if (item.LocationType == 2) {
                    this.addEstacion = true;
                }
                this.cambiotipoparada();
            }
            else {
                this.detail.LocationType = 0;
            }
        }
        this.inicializarMapa();
    };
    CreateOrEditParadaModalComponent.prototype.validateSave = function () {
        if (!this.detail.Lat) {
            this.message.error("Falta marcar un Punto en el mapa", "Localizacion es requeria");
            return false;
        }
        if (this.detail.LocationType == 2 && this.detail.ParentStation == null) {
            this.message.error("Falta agregar la estacion a la cual corresponde la entrada o salida", "Estacion es requeria");
            return false;
        }
        return true;
    };
    CreateOrEditParadaModalComponent.prototype.completedataBeforeSave = function (item) {
        if (item.ParentStation != null) {
            item.ParentStationId = item.ParentStation.Id;
        }
        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    };
    __decorate([
        core_1.ViewChild('mapaComponents'),
        __metadata("design:type", selectmarker_maps_component_1.SelectMarkerMapsComponent)
    ], CreateOrEditParadaModalComponent.prototype, "mapaComponents", void 0);
    CreateOrEditParadaModalComponent = __decorate([
        core_1.Component({
            selector: 'createOrEditParadaDtoModal',
            templateUrl: './create-or-edit-parada-modal.component.html',
        }),
        __param(5, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef,
            core_1.Injector,
            parada_service_1.ParadaService,
            localidad_service_1.LocalidadesService,
            core_1.ChangeDetectorRef,
            parada_model_1.ParadaDto])
    ], CreateOrEditParadaModalComponent);
    return CreateOrEditParadaModalComponent;
}(detail_component_1.DetailAgregationComponent));
exports.CreateOrEditParadaModalComponent = CreateOrEditParadaModalComponent;
//# sourceMappingURL=create-or-edit-parada-modal.component.js.map
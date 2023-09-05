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
var material_1 = require("@angular/material");
var detail_component_1 = require("../../shared/manager/detail.component");
var create_or_edit_coordenadas_modal_component_1 = require("../../theme/pages/planificacion/coordenadas/create-or-edit-coordenadas-modal.component");
var punto_model_1 = require("../../theme/pages/planificacion/model/punto.model");
var base_model_1 = require("../../shared/model/base.model");
var punto_service_1 = require("../../theme/pages/planificacion/punto/punto.service");
var tipoparada_service_1 = require("../../theme/pages/planificacion/tipoParada/tipoparada.service");
var coordenadas_service_1 = require("../../theme/pages/planificacion/coordenadas/coordenadas.service");
var sectoresTarifarios_service_1 = require("../../theme/pages/planificacion/sectoresTarifarios/sectoresTarifarios.service");
var coordenadas_model_1 = require("../../theme/pages/planificacion/model/coordenadas.model");
var coordenadas_component_1 = require("../../theme/pages/planificacion/coordenadas/coordenadas.component");
var parada_component_1 = require("../../theme/pages/planificacion/parada/parada.component");
var create_or_edit_parada_modal_component_1 = require("../../theme/pages/planificacion/parada/create-or-edit-parada-modal.component");
var parada_model_1 = require("../../theme/pages/planificacion/model/parada.model");
var CreateOrEditPuntoModalComponent = /** @class */ (function (_super) {
    __extends(CreateOrEditPuntoModalComponent, _super);
    function CreateOrEditPuntoModalComponent(injector, service, _servicetp, _coordenadasService, _sectoresTarifariosService) {
        var _this = _super.call(this, service, injector) || this;
        _this.service = service;
        _this._servicetp = _servicetp;
        _this._coordenadasService = _coordenadasService;
        _this._sectoresTarifariosService = _sectoresTarifariosService;
        _this.isSectormode = false;
        _this.modalSaveRuta = new core_1.EventEmitter();
        _this.ApplyPunto = new core_1.EventEmitter();
        _this.ApplySectores = new core_1.EventEmitter();
        _this.dialog = injector.get(material_1.MatDialog);
        //this.active = true;
        _this.active = false;
        _this.title = "Punto";
        _this.detail = new punto_model_1.PuntoDto();
        return _this;
    }
    //GetEditCoordenadaComponent() {
    //    if (!this.detailElement) { 
    //        var factory = this.cfr.resolveComponentFactory(CreateOrEditCoordenadaModalComponent);
    //        const ref = this['CreateOrEditCoordenadaModal'].createComponent(factory);
    //        ref.changeDetectorRef.detectChanges();
    //        this.detailElement = ref.instance;
    //        this.detailElement.modalClose.subscribe(e => {
    //            this.active = true;
    //        });
    //        this.detailElement.modalSave.subscribe(e => {
    //            this.active = true;
    //            this.Search(null);
    //        });
    //    }
    //    return this.detailElement;
    //}
    CreateOrEditPuntoModalComponent.prototype.ngOnInit = function () {
        var _this = this;
        _super.prototype.ngOnInit.call(this);
        this._servicetp.requestAllByFilter().subscribe(function (result) {
            _this.TiposParadas = result.DataObject.Items;
        });
        // this._sectoresTarifariosService.requestAllByFilter().subscribe(result => {
        //     this.SectoresTarifarios = result.DataObject.Items;
        // });
    };
    CreateOrEditPuntoModalComponent.prototype.getDescription = function (item) {
        return "Punto";
    };
    CreateOrEditPuntoModalComponent.prototype.showDto = function (item) {
        this.isSectormode = false;
        if (item.EsPuntoInicio) {
            item.EsCambioSectorTarifario = true;
        }
        _super.prototype.showDto.call(this, item);
        this.topbarAsideObj.show();
        $('#m_h_quick_sidebar_tabs_Puntos').click();
    };
    CreateOrEditPuntoModalComponent.prototype.showSectores = function (items) {
        this.isSectormode = true;
        this.sectoresdto = items;
        this.topbarAsideObj.show();
        $('#m_h_quick_sidebar_tabs_Sector').click();
    };
    CreateOrEditPuntoModalComponent.prototype.close = function () {
        _super.prototype.close.call(this);
        this.topbarAsideObj.hide();
    };
    CreateOrEditPuntoModalComponent.prototype.ngAfterViewInit = function () {
        mApp.initPortlets();
        this.initpuntosDetailsidebar();
    };
    CreateOrEditPuntoModalComponent.prototype.save = function (form) {
        _super.prototype.save.call(this, form);
    };
    CreateOrEditPuntoModalComponent.prototype.saveRuta = function (form) {
        this.modalSaveRuta.emit({});
    };
    CreateOrEditPuntoModalComponent.prototype.applyPunto = function () {
        //if (this.detail.CodigoNombre && this.detail.Abreviacion && (this.puntoOriginal.CodigoNombre != this.detail.CodigoNombre
        //    || this.puntoOriginal.Abreviacion != this.detail.Abreviacion)) {
        //    var filter = new CoordenadasFilter();
        //    filter.CodigoNombre = this.detail.CodigoNombre;
        //    filter.Abreviacion = this.detail.Abreviacion;
        //    this._coordenadasService.requestAllByFilter(filter)
        //        .subscribe(result => {
        //            console.log(result);
        //            if (result.DataObject.Items && result.DataObject.Items.length > 0) {
        //                var coordenadadto = result.DataObject.Items[0];
        //                if (this.detail.Lat != coordenadadto.Lat || this.detail.Long != coordenadadto.Long) {
        //                    this.message.confirm('Cambio de Coordenadas?', 'Confirmación', (a) => {
        //                        //this.isshowalgo = !this.isshowalgo;
        //                        if (a.value) {
        //                            this.detail.Lat = coordenadadto.Lat;
        //                            this.detail.Long = coordenadadto.Long;
        //                            this.ApplyPunto.emit(this.detail);
        //                            this.close();
        //                        }
        //                    });
        //                }
        //                else {
        //                    //si existe diferencia
        //                    this.ApplyPunto.emit(this.detail);
        //                    this.close();
        //                }
        //            }
        //            else {
        //                //si no existe el punto
        //                this.ApplyPunto.emit(this.detail);
        //                this.close();
        //            }
        //        });
        //}
        //else {
        //    //si no se completaron los datos
        //    this.ApplyPunto.emit(this.detail);
        //    this.close();
        //}
        if (this.detail.PlaCoordenadaAnulada == true) {
            this.message.error("La coordenada de este punto esta anulada. Por favor cambiarla.", 'Coordenada Anulada');
        }
        else {
            this.ApplyPunto.emit(this.detail);
            this.close();
        }
    };
    CreateOrEditPuntoModalComponent.prototype.applySectores = function () {
        console.log("sectoresdto", this.sectoresdto);
        this.ApplySectores.emit(this.sectoresdto);
        this.close();
    };
    CreateOrEditPuntoModalComponent.prototype.initpuntosDetailsidebar = function () {
        var topbarAside = $('#puntosDetailsidebar');
        var topbarAsideContent = topbarAside.find('.puntosDetailsidebar_content');
        this.topbarAsideObj = new mOffcanvas('puntosDetailsidebar', {
            overlay: false,
            baseClass: 'm-quick-sidebar',
        });
        // run once on first time dropdown shown
        this.topbarAsideObj.one('afterShow', function () {
            mApp.block(topbarAside);
            setTimeout(function () {
                mApp.unblock(topbarAside);
                topbarAsideContent.removeClass('m--hide');
                //                initOffcanvasTabs();
            }, 1000);
        });
    };
    CreateOrEditPuntoModalComponent.prototype.openSearchDialog = function () {
        var _this = this;
        var dialogConfig = new material_1.MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        var dialogRef = this.dialog.open(coordenadas_component_1.CoordenadasComponent, dialogConfig);
        dialogRef.componentInstance.showbreadcum = false;
        dialogRef.componentInstance.showDefaultBreadcum = false;
        dialogRef.componentInstance.allowSelect = true;
        dialogRef.componentInstance.showAnulado = true;
        dialogRef.componentInstance.onCreated.subscribe(function () {
            dialogRef.close(true);
            var dialogRef2 = _this.dialog.open(create_or_edit_coordenadas_modal_component_1.CreateOrEditCoordenadaModalComponent, dialogConfig);
            dialogRef2.componentInstance.IsInMaterialPopupMode = true;
            dialogRef2.componentInstance.saveLocal = false;
            //this.topbarAsideObj.hide();
            var dto = new coordenadas_model_1.CoordenadasDto();
            dialogRef2.componentInstance.showNew(dto);
            dialogRef2.afterClosed().subscribe(function (data) {
                _this.topbarAsideObj.show();
                if (data) {
                    console.log(data);
                    _this.active = true;
                    _this.detail.PlaCoordenadaId = data.Id;
                    var item = new base_model_1.ItemDto();
                    item.Description = data.Calle1 + " " + data.Calle2 + "";
                    item.Id = data.Id;
                    _this.detail.PlaCoordenadaItem = item;
                    _this.detail.PlaCoordenadaAnulada = data.Anulado;
                    _this.detail.NumeroExterno = data.NumeroExternoIVU;
                }
            });
        });
        dialogRef.componentInstance.onSelected.subscribe(function (data) {
            dialogRef.close();
            //this.topbarAsideObj.show();
            if (data) {
                console.log(data);
                _this.active = true;
                _this.detail.PlaCoordenadaId = data.Id;
                var item = new base_model_1.ItemDto();
                item.Description = data.Calle1 + " " + data.Calle2 + "";
                item.Id = data.Id;
                _this.detail.PlaCoordenadaItem = item;
                _this.detail.NumeroExterno = data.NumeroExternoIVU;
            }
        });
        dialogRef.afterClosed().subscribe(function (data) {
            if (!data) {
                _this.topbarAsideObj.show();
            }
        });
        this.topbarAsideObj.hide();
    };
    CreateOrEditPuntoModalComponent.prototype.OnChangeAbreviacion = function (event) {
        if (event) {
            if (event.NumeroExternoIVU)
                this.detail.NumeroExterno = event.NumeroExternoIVU;
        }
        else {
            this.detail.NumeroExterno = null;
        }
    };
    CreateOrEditPuntoModalComponent.prototype.openSearchDialogParada = function () {
        var _this = this;
        var dialogConfig = new material_1.MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        var dialogRef = this.dialog.open(parada_component_1.ParadaComponent, dialogConfig);
        dialogRef.componentInstance.showbreadcum = false;
        dialogRef.componentInstance.showDefaultBreadcum = false;
        dialogRef.componentInstance.allowSelect = true;
        dialogRef.componentInstance.inMapa = true;
        dialogRef.componentInstance.filter.Lat = this.detail.Lat;
        dialogRef.componentInstance.filter.Long = this.detail.Long;
        dialogRef.componentInstance.onCreated.subscribe(function () {
            dialogRef.close(true);
            var dialogConfigModel = new material_1.MatDialogConfig();
            dialogConfigModel.disableClose = false;
            dialogConfigModel.autoFocus = true;
            var dto = new parada_model_1.ParadaDto();
            if (_this.detail) {
                dto.Lat = _this.detail.Lat;
                dto.Long = _this.detail.Long;
            }
            dialogConfigModel.data = dto;
            var dialogRef2 = _this.dialog.open(create_or_edit_parada_modal_component_1.CreateOrEditParadaModalComponent, dialogConfigModel);
            dialogRef2.componentInstance.viewMode = base_model_1.ViewMode.Add;
            dialogRef2.componentInstance.IsInMaterialPopupMode = true;
            dialogRef2.componentInstance.saveLocal = false;
            dialogRef2.afterClosed().subscribe(function (data) {
                _this.topbarAsideObj.show();
                if (data) {
                    debugger;
                    console.log(data);
                    _this.active = true;
                    _this.detail.PlaParadaId = data.Id;
                    var item = new base_model_1.ItemDto();
                    item.Description = data.Codigo;
                    item.Id = data.Id;
                    _this.detail.PlaParadaItem = item;
                }
            });
        });
        dialogRef.componentInstance.onSelected.subscribe(function (data) {
            dialogRef.close();
            //this.topbarAsideObj.show();
            if (data) {
                console.log(data);
                _this.active = true;
                _this.detail.PlaParadaId = data.Id;
                var item = new base_model_1.ItemDto();
                item.Description = data.Codigo;
                item.Id = data.Id;
                _this.detail.PlaParadaItem = item;
            }
        });
        dialogRef.afterClosed().subscribe(function (data) {
            if (!data) {
                _this.topbarAsideObj.show();
            }
        });
        this.topbarAsideObj.hide();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CreateOrEditPuntoModalComponent.prototype, "modalSaveRuta", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CreateOrEditPuntoModalComponent.prototype, "ApplyPunto", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CreateOrEditPuntoModalComponent.prototype, "ApplySectores", void 0);
    CreateOrEditPuntoModalComponent = __decorate([
        core_1.Component({
            selector: 'createOrEditPuntoDtoModal',
            templateUrl: './create-or-edit-punto-modal.component.html',
            styleUrls: ['./create-or-edit-punto-modal.component.css']
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            punto_service_1.PuntoService,
            tipoparada_service_1.TipoParadaService,
            coordenadas_service_1.CoordenadasService,
            sectoresTarifarios_service_1.SectoresTarifariosService])
    ], CreateOrEditPuntoModalComponent);
    return CreateOrEditPuntoModalComponent;
}(detail_component_1.DetailEmbeddedComponent));
exports.CreateOrEditPuntoModalComponent = CreateOrEditPuntoModalComponent;
//# sourceMappingURL=create-or-edit-punto-modal.component.js.map
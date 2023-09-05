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
var crud_component_1 = require("../../../../shared/manager/crud.component");
var parada_service_1 = require("./parada.service");
var parada_model_1 = require("../model/parada.model");
var create_or_edit_parada_modal_component_1 = require("./create-or-edit-parada-modal.component");
var yesnoall_combo_component_1 = require("../../../../shared/components/yesnoall-combo.component");
var ParadaComponent = /** @class */ (function (_super) {
    __extends(ParadaComponent, _super);
    function ParadaComponent(injector, _RolesService, cdr) {
        var _this = _super.call(this, _RolesService, create_or_edit_parada_modal_component_1.CreateOrEditParadaModalComponent, injector) || this;
        _this._RolesService = _RolesService;
        _this.cdr = cdr;
        _this.allowSelect = false;
        _this.inMapa = false;
        _this.loadInMaterialPopup = true;
        _this.isFirstTime = true;
        _this.title = "Parada";
        _this.moduleName = "Administración";
        _this.icon = "flaticon-settings";
        _this.showbreadcum = false;
        _this.onCreated = new core_1.EventEmitter();
        _this.onSelected = new core_1.EventEmitter();
        return _this;
    }
    ParadaComponent.prototype.GetEditComponentType = function () {
        return create_or_edit_parada_modal_component_1.CreateOrEditParadaModalComponent;
    };
    ParadaComponent.prototype.ngAfterViewInit = function () {
        _super.prototype.ngAfterViewInit.call(this);
        this.ComboAnulado.writeValue(this.filter.Anulada);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    };
    ParadaComponent.prototype.getNewfilter = function () {
        var filter = new parada_model_1.ParadaFilter();
        filter.AnuladoCombo = 2;
        return filter;
    };
    ParadaComponent.prototype.getDescription = function (item) {
        return item.Codigo;
    };
    ParadaComponent.prototype.getNewItem = function (item) {
        var item = new parada_model_1.ParadaDto(item);
        // item.Activo = true;
        return item;
    };
    ParadaComponent.prototype.Search = function (event) {
        if (this.filter && this.filterselectLocalidades) {
            this.filter.LocalidadId = this.filterselectLocalidades.Id;
        }
        _super.prototype.Search.call(this, event);
    };
    ParadaComponent.prototype.onCreate = function () {
        if (this.allowSelect) {
            this.onCreated.emit();
        }
        else {
            _super.prototype.onCreate.call(this);
        }
    };
    ParadaComponent.prototype.onSelect = function (item) {
        this.onSelected.emit(item);
    };
    __decorate([
        core_1.ViewChild('ComboAnulado'),
        __metadata("design:type", yesnoall_combo_component_1.YesNoAllComboComponent)
    ], ParadaComponent.prototype, "ComboAnulado", void 0);
    ParadaComponent = __decorate([
        core_1.Component({
            templateUrl: "./parada.component.html",
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [core_1.Injector, parada_service_1.ParadaService, core_1.ChangeDetectorRef])
    ], ParadaComponent);
    return ParadaComponent;
}(crud_component_1.BaseCrudComponent));
exports.ParadaComponent = ParadaComponent;
//# sourceMappingURL=parada.component.js.map
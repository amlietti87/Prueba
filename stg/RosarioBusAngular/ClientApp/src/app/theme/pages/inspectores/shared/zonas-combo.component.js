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
var forms_1 = require("@angular/forms");
var comboBase_component_1 = require("../../../../shared/components/comboBase.component");
var zonas_model_1 = require("../model/zonas.model");
var zonas_service_1 = require("../zonas/zonas.service");
var create_or_edit_zonas_modal_component_1 = require("../zonas/create-or-edit-zonas-modal.component");
var ZonasComboComponent = /** @class */ (function (_super) {
    __extends(ZonasComboComponent, _super);
    function ZonasComboComponent(service, injector) {
        return _super.call(this, service, injector) || this;
    }
    ZonasComboComponent_1 = ZonasComboComponent;
    ZonasComboComponent.prototype.ngOnInit = function () {
        //super.ngOnInit();
    };
    ZonasComboComponent.prototype.onSearch = function () {
        if (!this.BuscarZona) {
            _super.prototype.onSearch.call(this);
        }
    };
    Object.defineProperty(ZonasComboComponent.prototype, "BuscarZona", {
        get: function () {
            return this._BuscarZona;
        },
        set: function (buscarZona) {
            this._BuscarZona = buscarZona;
            if (!buscarZona) {
                this.onSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ZonasComboComponent.prototype, "Anulado", {
        get: function () {
            return this._Anulado;
        },
        set: function (anulado) {
            this._Anulado = anulado;
            if (anulado) {
                this.onSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    ZonasComboComponent.prototype.GetFilter = function () {
        var f = { Anulado: this.Anulado };
        return f;
    };
    ZonasComboComponent.prototype.getIDetailComponent = function () {
        return create_or_edit_zonas_modal_component_1.CreateOrEditZonasModalComponent;
    };
    ZonasComboComponent.prototype.getNewDto = function () {
        return new zonas_model_1.ZonasDto();
    };
    var ZonasComboComponent_1;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean),
        __metadata("design:paramtypes", [Boolean])
    ], ZonasComboComponent.prototype, "BuscarZona", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number),
        __metadata("design:paramtypes", [Number])
    ], ZonasComboComponent.prototype, "Anulado", null);
    ZonasComboComponent = ZonasComboComponent_1 = __decorate([
        core_1.Component({
            selector: 'zonas-combo',
            template: "\n    <button *ngIf=\"showAddButton\"\n            type=\"button\"\n            class=\"smallesttext btn btn-primary blue\"\n            (click)=\"onAddButtonClick()\">\n            <i class=\"la la-plus\"></i>\n    </button>\n    <select #combobox\n            [(ngModel)]=\"value\"\n            [disabled]=\"IsDisabled || isLoading\"\n            (ngModelChange)=\"selectedItemChange.emit($event)\"\n            name=\"combobox\"\n            title=\"{{emptyText}}\"\n            data-container=\"body\"\n            style=\"width:100%;\">\n            <option *ngIf=\"allowNullable==true\" value=\"null\">\n                {{emptyText}}\n            </option>\n            <option *ngFor=\"let item of items\" [value]=\"item.Id\">{{item.Description}}</option>\n    </select>\n  ",
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return ZonasComboComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [zonas_service_1.ZonasService, core_1.Injector])
    ], ZonasComboComponent);
    return ZonasComboComponent;
}(comboBase_component_1.ComboBoxComponent));
exports.ZonasComboComponent = ZonasComboComponent;
//# sourceMappingURL=zonas-combo.component.js.map
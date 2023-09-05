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
var autocompleteBase_component_1 = require("../../../../shared/components/autocompleteBase.component");
var parada_service_1 = require("../parada/parada.service");
var ParadaAutoCompleteComponent = /** @class */ (function (_super) {
    __extends(ParadaAutoCompleteComponent, _super);
    function ParadaAutoCompleteComponent(service, injector) {
        return _super.call(this, service, injector) || this;
    }
    ParadaAutoCompleteComponent_1 = ParadaAutoCompleteComponent;
    ParadaAutoCompleteComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    ParadaAutoCompleteComponent.prototype.onSelect = function () {
        this.killTooltip();
    };
    ParadaAutoCompleteComponent.prototype.onBlur = function () {
        // this.killTooltip();
    };
    ParadaAutoCompleteComponent.prototype.onUnselect = function () {
        this.killTooltip();
    };
    ParadaAutoCompleteComponent.prototype.onKeyUp = function () {
        // this.killTooltip();
    };
    ParadaAutoCompleteComponent.prototype.onDropdownClick = function () {
        this.killTooltip();
    };
    ParadaAutoCompleteComponent.prototype.killTooltip = function () {
        //if (this.t) {
        //    $(this.t).tooltip('dispose');
        //}
    };
    Object.defineProperty(ParadaAutoCompleteComponent.prototype, "Lat", {
        get: function () {
            return this._lat;
        },
        set: function (lat) {
            this._lat = lat;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParadaAutoCompleteComponent.prototype, "Long", {
        get: function () {
            return this._long;
        },
        set: function (long) {
            this._long = long;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParadaAutoCompleteComponent.prototype, "LocationType", {
        get: function () {
            return this._LocationType;
        },
        set: function (locationtype) {
            this._LocationType = locationtype;
        },
        enumerable: true,
        configurable: true
    });
    ParadaAutoCompleteComponent.prototype.GetFilter = function (query) {
        debugger;
        var f = {
            FilterText: query,
            Anulado: false,
            Lat: this.Lat,
            Long: this.Long,
            LocationType: this.value ? null : this.LocationType
        };
        return f;
    };
    ParadaAutoCompleteComponent.prototype.filterItems = function (event) {
        var _this = this;
        var query = null;
        if (event != null) {
            query = event.query;
        }
        this.service.requestAllByFilter(this.GetFilter(query)).subscribe(function (x) {
            _this.items = [];
            for (var i in x.DataObject.Items) {
                var item = x.DataObject.Items[i];
                _this.items.push(item);
            }
            setTimeout(function () {
                //this.killTooltip();
                if (_this.items && _this.items.length > 2) {
                    //this.t = $('[data-toggle="tooltip"]').tooltip();
                }
            }, 10);
        });
    };
    var ParadaAutoCompleteComponent_1;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array),
        __metadata("design:paramtypes", [Array])
    ], ParadaAutoCompleteComponent.prototype, "Lat", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array),
        __metadata("design:paramtypes", [Array])
    ], ParadaAutoCompleteComponent.prototype, "Long", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number),
        __metadata("design:paramtypes", [Number])
    ], ParadaAutoCompleteComponent.prototype, "LocationType", null);
    ParadaAutoCompleteComponent = ParadaAutoCompleteComponent_1 = __decorate([
        core_1.Component({
            selector: 'parada-autocomplete',
            templateUrl: './parada-autocomplete.html',
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return ParadaAutoCompleteComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [parada_service_1.ParadaService, core_1.Injector])
    ], ParadaAutoCompleteComponent);
    return ParadaAutoCompleteComponent;
}(autocompleteBase_component_1.AutoCompleteComponent));
exports.ParadaAutoCompleteComponent = ParadaAutoCompleteComponent;
//# sourceMappingURL=parada-autocomplete.component.js.map
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
var coordenadas_service_1 = require("../coordenadas/coordenadas.service");
var CoordenadasAutoCompleteComponent = /** @class */ (function (_super) {
    __extends(CoordenadasAutoCompleteComponent, _super);
    function CoordenadasAutoCompleteComponent(service, injector) {
        return _super.call(this, service, injector) || this;
    }
    CoordenadasAutoCompleteComponent_1 = CoordenadasAutoCompleteComponent;
    CoordenadasAutoCompleteComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    CoordenadasAutoCompleteComponent.prototype.onSelect = function () {
        this.killTooltip();
    };
    CoordenadasAutoCompleteComponent.prototype.onBlur = function () {
        // this.killTooltip();
    };
    CoordenadasAutoCompleteComponent.prototype.onUnselect = function () {
        this.killTooltip();
    };
    CoordenadasAutoCompleteComponent.prototype.onKeyUp = function () {
        // this.killTooltip();
    };
    CoordenadasAutoCompleteComponent.prototype.onDropdownClick = function () {
        this.killTooltip();
    };
    CoordenadasAutoCompleteComponent.prototype.killTooltip = function () {
        //if (this.t) {
        //    $(this.t).tooltip('dispose');
        //}
    };
    CoordenadasAutoCompleteComponent.prototype.GetFilter = function (query) {
        var f = {
            FilterText: query,
            AnuladoCombo: 2
        };
        return f;
    };
    CoordenadasAutoCompleteComponent.prototype.filterItems = function (event) {
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
    var CoordenadasAutoCompleteComponent_1;
    CoordenadasAutoCompleteComponent = CoordenadasAutoCompleteComponent_1 = __decorate([
        core_1.Component({
            selector: 'coordenada-autocomplete',
            templateUrl: './coordenada-autocomplete.html',
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return CoordenadasAutoCompleteComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [coordenadas_service_1.CoordenadasService, core_1.Injector])
    ], CoordenadasAutoCompleteComponent);
    return CoordenadasAutoCompleteComponent;
}(autocompleteBase_component_1.AutoCompleteComponent));
exports.CoordenadasAutoCompleteComponent = CoordenadasAutoCompleteComponent;
//# sourceMappingURL=coordenada-autocomplete.component.js.map
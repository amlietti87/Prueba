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
var linea_service_1 = require("../linea/linea.service");
var LineaAutoCompleteComponent = /** @class */ (function (_super) {
    __extends(LineaAutoCompleteComponent, _super);
    function LineaAutoCompleteComponent(service, injector) {
        return _super.call(this, service, injector) || this;
    }
    LineaAutoCompleteComponent_1 = LineaAutoCompleteComponent;
    Object.defineProperty(LineaAutoCompleteComponent.prototype, "SucursalId", {
        get: function () {
            return this._SucursalId;
        },
        set: function (SucursalId) {
            this._SucursalId = SucursalId;
        },
        enumerable: true,
        configurable: true
    });
    LineaAutoCompleteComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    LineaAutoCompleteComponent.prototype.GetFilter = function (query) {
        var f = {
            SucursalId: this._SucursalId,
            FilterText: query
        };
        return f;
    };
    var LineaAutoCompleteComponent_1;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number),
        __metadata("design:paramtypes", [Number])
    ], LineaAutoCompleteComponent.prototype, "SucursalId", null);
    LineaAutoCompleteComponent = LineaAutoCompleteComponent_1 = __decorate([
        core_1.Component({
            selector: 'linea-autocomplete',
            templateUrl: '../../../../shared/components/autocompleteBase.component.html',
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return LineaAutoCompleteComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [linea_service_1.LineaService, core_1.Injector])
    ], LineaAutoCompleteComponent);
    return LineaAutoCompleteComponent;
}(autocompleteBase_component_1.AutoCompleteComponent));
exports.LineaAutoCompleteComponent = LineaAutoCompleteComponent;
//# sourceMappingURL=linea-autocomplete.component.js.map
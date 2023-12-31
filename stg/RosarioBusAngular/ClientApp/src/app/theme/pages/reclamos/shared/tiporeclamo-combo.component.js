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
var tiposreclamo_service_1 = require("../tiposreclamo/tiposreclamo.service");
var TipoReclamoComboComponent = /** @class */ (function (_super) {
    __extends(TipoReclamoComboComponent, _super);
    function TipoReclamoComboComponent(service, injector) {
        return _super.call(this, service, injector) || this;
    }
    TipoReclamoComboComponent_1 = TipoReclamoComboComponent;
    TipoReclamoComboComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    TipoReclamoComboComponent.prototype.GetFilter = function () {
        var f = {};
        return f;
    };
    var TipoReclamoComboComponent_1;
    TipoReclamoComboComponent = TipoReclamoComboComponent_1 = __decorate([
        core_1.Component({
            selector: 'tiporeclamo-combo',
            templateUrl: '../../../../shared/components/comboBase.component.html',
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return TipoReclamoComboComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [tiposreclamo_service_1.TiposReclamoService, core_1.Injector])
    ], TipoReclamoComboComponent);
    return TipoReclamoComboComponent;
}(comboBase_component_1.ComboBoxComponent));
exports.TipoReclamoComboComponent = TipoReclamoComboComponent;
//# sourceMappingURL=tiporeclamo-combo.component.js.map
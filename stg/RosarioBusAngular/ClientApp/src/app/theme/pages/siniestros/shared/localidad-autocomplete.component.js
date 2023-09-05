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
var localidad_model_1 = require("../model/localidad.model");
var localidad_service_1 = require("../localidades/localidad.service");
var create_or_edit_localidades_modal_component_1 = require("../../admin/localidades/create-or-edit-localidades-modal.component");
var LocalidadesAutoCompleteComponent = /** @class */ (function (_super) {
    __extends(LocalidadesAutoCompleteComponent, _super);
    function LocalidadesAutoCompleteComponent(service, injector) {
        return _super.call(this, service, injector) || this;
    }
    LocalidadesAutoCompleteComponent_1 = LocalidadesAutoCompleteComponent;
    LocalidadesAutoCompleteComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    LocalidadesAutoCompleteComponent.prototype.GetFilter = function (query) {
        var f = {
            FilterText: query
        };
        return f;
    };
    LocalidadesAutoCompleteComponent.prototype.getIDetailComponent = function () {
        return create_or_edit_localidades_modal_component_1.CreateOrEditLocalidadesModalComponent;
    };
    LocalidadesAutoCompleteComponent.prototype.getNewDto = function () {
        return new localidad_model_1.LocalidadesDto();
    };
    var LocalidadesAutoCompleteComponent_1;
    LocalidadesAutoCompleteComponent = LocalidadesAutoCompleteComponent_1 = __decorate([
        core_1.Component({
            selector: 'localidad-autocomplete',
            templateUrl: '../../../../shared/components/autocompleteBase.component.html',
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return LocalidadesAutoCompleteComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [localidad_service_1.LocalidadesService, core_1.Injector])
    ], LocalidadesAutoCompleteComponent);
    return LocalidadesAutoCompleteComponent;
}(autocompleteBase_component_1.AutoCompleteComponent));
exports.LocalidadesAutoCompleteComponent = LocalidadesAutoCompleteComponent;
//# sourceMappingURL=localidad-autocomplete.component.js.map
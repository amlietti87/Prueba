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
var base_model_1 = require("../../../../shared/model/base.model");
var PagaCambiaComboComponent = /** @class */ (function (_super) {
    __extends(PagaCambiaComboComponent, _super);
    function PagaCambiaComboComponent(injector) {
        var _this = _super.call(this, injector) || this;
        _this.injector = injector;
        var opc1 = new base_model_1.ItemDto();
        opc1.Id = 0;
        opc1.Description = 'Cambia';
        var opc2 = new base_model_1.ItemDto();
        opc2.Id = 1;
        opc2.Description = 'Paga';
        _this.items.push(opc1);
        _this.items.push(opc2);
        return _this;
    }
    PagaCambiaComboComponent_1 = PagaCambiaComboComponent;
    PagaCambiaComboComponent.prototype.onSearch = function () {
        var self = this;
        self.isLoading = false;
    };
    var PagaCambiaComboComponent_1;
    PagaCambiaComboComponent = PagaCambiaComboComponent_1 = __decorate([
        core_1.Component({
            selector: 'pagacambia-combo',
            template: "\n    <button *ngIf=\"showAddButton\"\n            type=\"button\"\n            class=\"smallesttext btn btn-primary blue\"\n            (click)=\"onAddButtonClick()\">\n            <i class=\"la la-plus\"></i>\n    </button>\n    <select #combobox\n            [(ngModel)]=\"value\"\n            [disabled]=\"IsDisabled || isLoading\"\n            (ngModelChange)=\"selectedItemChange.emit($event)\"\n            name=\"combobox\"\n            title=\"{{emptyText}}\"\n            data-container=\"body\"\n            style=\"width:100%;\">\n            <option *ngIf=\"allowNullable==true\" value=\"null\">\n                {{emptyText}}\n            </option>\n            <option *ngFor=\"let item of items\" [value]=\"item.Id\">{{item.Description}}</option>\n    </select>\n  ",
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return PagaCambiaComboComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [core_1.Injector])
    ], PagaCambiaComboComponent);
    return PagaCambiaComboComponent;
}(comboBase_component_1.ComboBoxYesNoAllComponent));
exports.PagaCambiaComboComponent = PagaCambiaComboComponent;
//# sourceMappingURL=pagacambia-combo.component.js.map
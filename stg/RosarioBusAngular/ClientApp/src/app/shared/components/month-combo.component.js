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
var comboBase_component_1 = require("./comboBase.component");
var base_model_1 = require("../model/base.model");
var MonthComboComponent = /** @class */ (function (_super) {
    __extends(MonthComboComponent, _super);
    function MonthComboComponent(injector) {
        var _this = _super.call(this, injector) || this;
        _this.injector = injector;
        var opc1 = new base_model_1.ItemDto();
        opc1.Id = 1;
        opc1.Description = 'Enero';
        var opc2 = new base_model_1.ItemDto();
        opc2.Id = 2;
        opc2.Description = 'Febrero';
        var opc3 = new base_model_1.ItemDto();
        opc3.Id = 3;
        opc3.Description = 'Marzo';
        var opc4 = new base_model_1.ItemDto();
        opc4.Id = 4;
        opc4.Description = 'Abril';
        var opc5 = new base_model_1.ItemDto();
        opc5.Id = 5;
        opc5.Description = 'Mayo';
        var opc6 = new base_model_1.ItemDto();
        opc6.Id = 6;
        opc6.Description = 'Junio';
        var opc7 = new base_model_1.ItemDto();
        opc7.Id = 7;
        opc7.Description = 'Julio';
        var opc8 = new base_model_1.ItemDto();
        opc8.Id = 8;
        opc8.Description = 'Agosto';
        var opc9 = new base_model_1.ItemDto();
        opc9.Id = 9;
        opc9.Description = 'Septiembre';
        var opc10 = new base_model_1.ItemDto();
        opc10.Id = 10;
        opc10.Description = 'Octubre';
        var opc11 = new base_model_1.ItemDto();
        opc11.Id = 11;
        opc11.Description = 'Noviembre';
        var opc12 = new base_model_1.ItemDto();
        opc12.Id = 12;
        opc12.Description = 'Diciembre';
        _this.items.push(opc1);
        _this.items.push(opc2);
        _this.items.push(opc3);
        _this.items.push(opc4);
        _this.items.push(opc5);
        _this.items.push(opc6);
        _this.items.push(opc7);
        _this.items.push(opc8);
        _this.items.push(opc9);
        _this.items.push(opc10);
        _this.items.push(opc11);
        _this.items.push(opc12);
        return _this;
    }
    MonthComboComponent_1 = MonthComboComponent;
    MonthComboComponent.prototype.onSearch = function () {
        var self = this;
        self.isLoading = false;
    };
    var MonthComboComponent_1;
    MonthComboComponent = MonthComboComponent_1 = __decorate([
        core_1.Component({
            selector: 'month-combo',
            templateUrl: './comboBase.component.html',
            providers: [
                {
                    provide: forms_1.NG_VALUE_ACCESSOR,
                    useExisting: core_1.forwardRef(function () { return MonthComboComponent_1; }),
                    multi: true
                }
            ]
        }),
        __metadata("design:paramtypes", [core_1.Injector])
    ], MonthComboComponent);
    return MonthComboComponent;
}(comboBase_component_1.ComboBoxYesNoAllComponent));
exports.MonthComboComponent = MonthComboComponent;
//# sourceMappingURL=month-combo.component.js.map
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var base_model_1 = require("../../../../shared/model/base.model");
var ElementosDto = /** @class */ (function (_super) {
    __extends(ElementosDto, _super);
    function ElementosDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ElementosDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return ElementosDto;
}(base_model_1.Dto));
exports.ElementosDto = ElementosDto;
var ElementosFilter = /** @class */ (function (_super) {
    __extends(ElementosFilter, _super);
    function ElementosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ElementosFilter;
}(base_model_1.FilterDTO));
exports.ElementosFilter = ElementosFilter;
var CroTipoDto = /** @class */ (function (_super) {
    __extends(CroTipoDto, _super);
    function CroTipoDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CroTipoDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return CroTipoDto;
}(base_model_1.Dto));
exports.CroTipoDto = CroTipoDto;
var CroTipoFilter = /** @class */ (function (_super) {
    __extends(CroTipoFilter, _super);
    function CroTipoFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CroTipoFilter;
}(base_model_1.FilterDTO));
exports.CroTipoFilter = CroTipoFilter;
//# sourceMappingURL=elemento.model.js.map
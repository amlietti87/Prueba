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
var ConsecuenciasDto = /** @class */ (function (_super) {
    __extends(ConsecuenciasDto, _super);
    function ConsecuenciasDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ConsecuenciasDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return ConsecuenciasDto;
}(base_model_1.Dto));
exports.ConsecuenciasDto = ConsecuenciasDto;
var ConsecuenciasFilter = /** @class */ (function (_super) {
    __extends(ConsecuenciasFilter, _super);
    function ConsecuenciasFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ConsecuenciasFilter;
}(base_model_1.FilterDTO));
exports.ConsecuenciasFilter = ConsecuenciasFilter;
var CategoriasDto = /** @class */ (function (_super) {
    __extends(CategoriasDto, _super);
    function CategoriasDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CategoriasDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return CategoriasDto;
}(base_model_1.Dto));
exports.CategoriasDto = CategoriasDto;
//# sourceMappingURL=consecuencias.model.js.map
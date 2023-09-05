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
var LocalidadesDto = /** @class */ (function (_super) {
    __extends(LocalidadesDto, _super);
    function LocalidadesDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    LocalidadesDto.prototype.getDescription = function () {
        return this.DscLocalidad;
    };
    return LocalidadesDto;
}(base_model_1.Dto));
exports.LocalidadesDto = LocalidadesDto;
var LocalidadesFilter = /** @class */ (function (_super) {
    __extends(LocalidadesFilter, _super);
    function LocalidadesFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return LocalidadesFilter;
}(base_model_1.FilterDTO));
exports.LocalidadesFilter = LocalidadesFilter;
var ProvinciasDto = /** @class */ (function (_super) {
    __extends(ProvinciasDto, _super);
    function ProvinciasDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ProvinciasDto.prototype.getDescription = function () {
        return this.DscProvincia;
    };
    return ProvinciasDto;
}(base_model_1.Dto));
exports.ProvinciasDto = ProvinciasDto;
var ProvinciasFilter = /** @class */ (function (_super) {
    __extends(ProvinciasFilter, _super);
    function ProvinciasFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ProvinciasFilter;
}(base_model_1.FilterDTO));
exports.ProvinciasFilter = ProvinciasFilter;
//# sourceMappingURL=localidad.model.js.map
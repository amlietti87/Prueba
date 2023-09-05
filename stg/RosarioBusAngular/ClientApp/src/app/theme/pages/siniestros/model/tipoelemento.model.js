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
var TipoElementoDto = /** @class */ (function (_super) {
    __extends(TipoElementoDto, _super);
    function TipoElementoDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.CroElemeneto = [];
        return _this;
    }
    TipoElementoDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return TipoElementoDto;
}(base_model_1.Dto));
exports.TipoElementoDto = TipoElementoDto;
var TipoElementoFilter = /** @class */ (function (_super) {
    __extends(TipoElementoFilter, _super);
    function TipoElementoFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return TipoElementoFilter;
}(base_model_1.FilterDTO));
exports.TipoElementoFilter = TipoElementoFilter;
//# sourceMappingURL=tipoelemento.model.js.map
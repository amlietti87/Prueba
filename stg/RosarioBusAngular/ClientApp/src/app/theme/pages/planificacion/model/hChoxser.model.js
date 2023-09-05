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
Object.defineProperty(exports, "__esModule", { value: true });
var base_model_1 = require("../../../../shared/model/base.model");
var HChoxser = /** @class */ (function (_super) {
    __extends(HChoxser, _super);
    function HChoxser() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    HChoxser.prototype.getDescription = function () {
        return "";
    };
    return HChoxser;
}(base_model_1.Dto));
exports.HChoxser = HChoxser;
var HChoxserExtendedDto = /** @class */ (function (_super) {
    __extends(HChoxserExtendedDto, _super);
    function HChoxserExtendedDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.isRequiredSale = false;
        _this.isRequiredSaleR = false;
        _this.isRequiredSaleA = false;
        _this.isRequiredLlega = false;
        _this.isRequiredLlegaR = false;
        _this.isRequiredLlegaA = false;
        return _this;
    }
    return HChoxserExtendedDto;
}(HChoxser));
exports.HChoxserExtendedDto = HChoxserExtendedDto;
var HChoxserFilter = /** @class */ (function () {
    function HChoxserFilter() {
    }
    return HChoxserFilter;
}());
exports.HChoxserFilter = HChoxserFilter;
var ImportadorDuracionResult = /** @class */ (function () {
    function ImportadorDuracionResult() {
        this.List = [];
    }
    return ImportadorDuracionResult;
}());
exports.ImportadorDuracionResult = ImportadorDuracionResult;
var ChofXServImportado = /** @class */ (function () {
    function ChofXServImportado() {
    }
    return ChofXServImportado;
}());
exports.ChofXServImportado = ChofXServImportado;
var HorarioDuracion = /** @class */ (function () {
    function HorarioDuracion() {
    }
    return HorarioDuracion;
}());
exports.HorarioDuracion = HorarioDuracion;
//# sourceMappingURL=hChoxser.model.js.map
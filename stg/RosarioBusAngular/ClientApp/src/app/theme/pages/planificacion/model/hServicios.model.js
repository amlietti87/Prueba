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
var HServiciosDto = /** @class */ (function (_super) {
    __extends(HServiciosDto, _super);
    function HServiciosDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    HServiciosDto.prototype.getDescription = function () {
        return this.Description;
    };
    return HServiciosDto;
}(base_model_1.ItemDto));
exports.HServiciosDto = HServiciosDto;
var HServiciosFilter = /** @class */ (function (_super) {
    __extends(HServiciosFilter, _super);
    function HServiciosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return HServiciosFilter;
}(base_model_1.FilterDTO));
exports.HServiciosFilter = HServiciosFilter;
var ExportarExcelDto = /** @class */ (function () {
    function ExportarExcelDto() {
    }
    return ExportarExcelDto;
}());
exports.ExportarExcelDto = ExportarExcelDto;
//# sourceMappingURL=hServicios.model.js.map
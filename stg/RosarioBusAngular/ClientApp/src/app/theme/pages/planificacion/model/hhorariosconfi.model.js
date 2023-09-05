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
var HHorariosConfiDto = /** @class */ (function (_super) {
    __extends(HHorariosConfiDto, _super);
    function HHorariosConfiDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    HHorariosConfiDto.prototype.getDescription = function () {
        return "";
    };
    return HHorariosConfiDto;
}(base_model_1.Dto));
exports.HHorariosConfiDto = HHorariosConfiDto;
var HHorariosConfiFilter = /** @class */ (function (_super) {
    __extends(HHorariosConfiFilter, _super);
    function HHorariosConfiFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return HHorariosConfiFilter;
}(base_model_1.FilterDTO));
exports.HHorariosConfiFilter = HHorariosConfiFilter;
var DetalleSalidaRelevosFilter = /** @class */ (function (_super) {
    __extends(DetalleSalidaRelevosFilter, _super);
    function DetalleSalidaRelevosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return DetalleSalidaRelevosFilter;
}(base_model_1.FilterDTO));
exports.DetalleSalidaRelevosFilter = DetalleSalidaRelevosFilter;
var ReporteHorarioPasajerosFilter = /** @class */ (function (_super) {
    __extends(ReporteHorarioPasajerosFilter, _super);
    function ReporteHorarioPasajerosFilter() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.BanderasIda = [];
        _this.BanderasVueltas = [];
        return _this;
    }
    return ReporteHorarioPasajerosFilter;
}(base_model_1.FilterDTO));
exports.ReporteHorarioPasajerosFilter = ReporteHorarioPasajerosFilter;
var ReporteDistribucionCochesFilter = /** @class */ (function (_super) {
    __extends(ReporteDistribucionCochesFilter, _super);
    function ReporteDistribucionCochesFilter() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.lineList = [];
        return _this;
    }
    return ReporteDistribucionCochesFilter;
}(base_model_1.FilterDTO));
exports.ReporteDistribucionCochesFilter = ReporteDistribucionCochesFilter;
var ReportePasajerosFilter = /** @class */ (function (_super) {
    __extends(ReportePasajerosFilter, _super);
    function ReportePasajerosFilter() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.Banderas = [];
        return _this;
    }
    return ReportePasajerosFilter;
}(base_model_1.FilterDTO));
exports.ReportePasajerosFilter = ReportePasajerosFilter;
//# sourceMappingURL=hhorariosconfi.model.js.map
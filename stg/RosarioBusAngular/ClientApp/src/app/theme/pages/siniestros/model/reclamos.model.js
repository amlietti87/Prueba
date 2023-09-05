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
var ReclamosDto = /** @class */ (function (_super) {
    __extends(ReclamosDto, _super);
    function ReclamosDto(data) {
        var _this = _super.call(this, data) || this;
        _this.ReclamoCuotas = [];
        _this.itemsHistorial = [];
        _this.ActualizarConductor = false;
        return _this;
    }
    ReclamosDto.prototype.getDescription = function () {
        return this.NroExpediente;
    };
    return ReclamosDto;
}(base_model_1.Dto));
exports.ReclamosDto = ReclamosDto;
var ReclamosHistoricosDto = /** @class */ (function (_super) {
    __extends(ReclamosHistoricosDto, _super);
    function ReclamosHistoricosDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ReclamosHistoricosDto.prototype.getDescription = function () {
        return this.NroExpediente;
    };
    return ReclamosHistoricosDto;
}(ReclamosDto));
exports.ReclamosHistoricosDto = ReclamosHistoricosDto;
var ReclamosFilter = /** @class */ (function (_super) {
    __extends(ReclamosFilter, _super);
    function ReclamosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ReclamosFilter;
}(base_model_1.FilterDTO));
exports.ReclamosFilter = ReclamosFilter;
var ReclamosHistoricosFilter = /** @class */ (function (_super) {
    __extends(ReclamosHistoricosFilter, _super);
    function ReclamosHistoricosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ReclamosHistoricosFilter;
}(base_model_1.FilterDTO));
exports.ReclamosHistoricosFilter = ReclamosHistoricosFilter;
var ReclamoCuotasDto = /** @class */ (function (_super) {
    __extends(ReclamoCuotasDto, _super);
    function ReclamoCuotasDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ReclamoCuotasDto.prototype.getDescription = function () {
        return this.Concepto;
    };
    return ReclamoCuotasDto;
}(base_model_1.Dto));
exports.ReclamoCuotasDto = ReclamoCuotasDto;
var EstadosItemDto = /** @class */ (function (_super) {
    __extends(EstadosItemDto, _super);
    function EstadosItemDto(data) {
        var _this = _super.call(this, data) || this;
        _this.animate = false;
        return _this;
    }
    EstadosItemDto.prototype.getDescription = function () {
        return this.Description;
    };
    return EstadosItemDto;
}(base_model_1.Dto));
exports.EstadosItemDto = EstadosItemDto;
//# sourceMappingURL=reclamos.model.js.map
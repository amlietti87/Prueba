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
var SiniestrosDto = /** @class */ (function (_super) {
    __extends(SiniestrosDto, _super);
    function SiniestrosDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.Reclamos = [];
        _this.ActualizarConductor = false;
        return _this;
    }
    SiniestrosDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return SiniestrosDto;
}(base_model_1.Dto));
exports.SiniestrosDto = SiniestrosDto;
var SiniestroHistorialEmpleado = /** @class */ (function (_super) {
    __extends(SiniestroHistorialEmpleado, _super);
    function SiniestroHistorialEmpleado() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SiniestroHistorialEmpleado.prototype.getDescription = function () {
        return '';
    };
    return SiniestroHistorialEmpleado;
}(base_model_1.Dto));
exports.SiniestroHistorialEmpleado = SiniestroHistorialEmpleado;
var GrillaConductor = /** @class */ (function (_super) {
    __extends(GrillaConductor, _super);
    function GrillaConductor() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    GrillaConductor.prototype.getDescription = function () {
        return '';
    };
    return GrillaConductor;
}(base_model_1.Dto));
exports.GrillaConductor = GrillaConductor;
var SiniestrosConsecuenciasDto = /** @class */ (function (_super) {
    __extends(SiniestrosConsecuenciasDto, _super);
    function SiniestrosConsecuenciasDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SiniestrosConsecuenciasDto.prototype.getDescription = function () {
        return 'err';
    };
    return SiniestrosConsecuenciasDto;
}(base_model_1.Dto));
exports.SiniestrosConsecuenciasDto = SiniestrosConsecuenciasDto;
var SiniestrosFilter = /** @class */ (function (_super) {
    __extends(SiniestrosFilter, _super);
    function SiniestrosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SiniestrosFilter;
}(base_model_1.FilterDTO));
exports.SiniestrosFilter = SiniestrosFilter;
var SiniestroMapDto = /** @class */ (function (_super) {
    __extends(SiniestroMapDto, _super);
    function SiniestroMapDto(data) {
        return _super.call(this, data) || this;
    }
    SiniestroMapDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return SiniestroMapDto;
}(base_model_1.Dto));
exports.SiniestroMapDto = SiniestroMapDto;
//# sourceMappingURL=siniestro.model.js.map
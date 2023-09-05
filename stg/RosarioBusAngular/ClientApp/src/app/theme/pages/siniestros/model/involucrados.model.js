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
var conductor_model_1 = require("./conductor.model");
var vehiculo_model_1 = require("./vehiculo.model");
var lesionado_model_1 = require("./lesionado.model");
var tipoinvolucrado_model_1 = require("./tipoinvolucrado.model");
var InvolucradosDto = /** @class */ (function (_super) {
    __extends(InvolucradosDto, _super);
    function InvolucradosDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.TipoInvolucrado = new tipoinvolucrado_model_1.TipoInvolucradoDto(null);
        _this.Conductor = new conductor_model_1.ConductorDto(null);
        _this.Vehiculo = new vehiculo_model_1.VehiculoDto(null);
        _this.Lesionado = new lesionado_model_1.LesionadoDto(null);
        _this.MuebleInmueble = new MuebleInmuebleDto(null);
        _this.DetalleLesion = [];
        return _this;
    }
    InvolucradosDto.prototype.getDescription = function () {
        return this.Description;
    };
    return InvolucradosDto;
}(base_model_1.Dto));
exports.InvolucradosDto = InvolucradosDto;
var InvolucradosFilter = /** @class */ (function (_super) {
    __extends(InvolucradosFilter, _super);
    function InvolucradosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return InvolucradosFilter;
}(base_model_1.FilterDTO));
exports.InvolucradosFilter = InvolucradosFilter;
var DetalleLesionDto = /** @class */ (function (_super) {
    __extends(DetalleLesionDto, _super);
    function DetalleLesionDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DetalleLesionDto.prototype.getDescription = function () {
        return this.LugarAtencion;
    };
    return DetalleLesionDto;
}(base_model_1.Dto));
exports.DetalleLesionDto = DetalleLesionDto;
var MuebleInmuebleDto = /** @class */ (function (_super) {
    __extends(MuebleInmuebleDto, _super);
    function MuebleInmuebleDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MuebleInmuebleDto.prototype.getDescription = function () {
        return this.Lugar + ' ' + this.selectLocalidades != null ? this.selectLocalidades.Description : "";
    };
    return MuebleInmuebleDto;
}(base_model_1.Dto));
exports.MuebleInmuebleDto = MuebleInmuebleDto;
var HistorialInvolucrados = /** @class */ (function (_super) {
    __extends(HistorialInvolucrados, _super);
    function HistorialInvolucrados() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    HistorialInvolucrados.prototype.getDescription = function () {
        return '';
    };
    return HistorialInvolucrados;
}(base_model_1.Dto));
exports.HistorialInvolucrados = HistorialInvolucrados;
//# sourceMappingURL=involucrados.model.js.map
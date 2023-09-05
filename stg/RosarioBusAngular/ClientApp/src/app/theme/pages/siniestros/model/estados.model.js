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
var EstadosDto = /** @class */ (function (_super) {
    __extends(EstadosDto, _super);
    function EstadosDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    EstadosDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return EstadosDto;
}(base_model_1.Dto));
exports.EstadosDto = EstadosDto;
var EstadosFilter = /** @class */ (function (_super) {
    __extends(EstadosFilter, _super);
    function EstadosFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return EstadosFilter;
}(base_model_1.FilterDTO));
exports.EstadosFilter = EstadosFilter;
var SubEstadosDto = /** @class */ (function (_super) {
    __extends(SubEstadosDto, _super);
    function SubEstadosDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SubEstadosDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return SubEstadosDto;
}(base_model_1.Dto));
exports.SubEstadosDto = SubEstadosDto;
//# sourceMappingURL=estados.model.js.map
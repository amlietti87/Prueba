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
var TipoParadaDto = /** @class */ (function (_super) {
    __extends(TipoParadaDto, _super);
    function TipoParadaDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TipoParadaDto.prototype.getDescription = function () {
        return this.Description;
    };
    return TipoParadaDto;
}(base_model_1.Dto));
exports.TipoParadaDto = TipoParadaDto;
var TiempoEsperadoDeCargaDto = /** @class */ (function (_super) {
    __extends(TiempoEsperadoDeCargaDto, _super);
    function TiempoEsperadoDeCargaDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TiempoEsperadoDeCargaDto.prototype.getDescription = function () {
        return "";
    };
    return TiempoEsperadoDeCargaDto;
}(base_model_1.Dto));
exports.TiempoEsperadoDeCargaDto = TiempoEsperadoDeCargaDto;
//# sourceMappingURL=tipoParada.model.js.map
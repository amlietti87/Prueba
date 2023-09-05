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
var RutaDto = /** @class */ (function (_super) {
    __extends(RutaDto, _super);
    function RutaDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    RutaDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return RutaDto;
}(base_model_1.Dto));
exports.RutaDto = RutaDto;
var RutaFilter = /** @class */ (function (_super) {
    __extends(RutaFilter, _super);
    function RutaFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return RutaFilter;
}(base_model_1.FilterDTO));
exports.RutaFilter = RutaFilter;
var MinutosPorSectorFilter = /** @class */ (function (_super) {
    __extends(MinutosPorSectorFilter, _super);
    function MinutosPorSectorFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return MinutosPorSectorFilter;
}(RutaFilter));
exports.MinutosPorSectorFilter = MinutosPorSectorFilter;
//# sourceMappingURL=ruta.model.js.map
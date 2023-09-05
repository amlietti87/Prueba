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
var SancionSugeridaDto = /** @class */ (function (_super) {
    __extends(SancionSugeridaDto, _super);
    function SancionSugeridaDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SancionSugeridaDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return SancionSugeridaDto;
}(base_model_1.Dto));
exports.SancionSugeridaDto = SancionSugeridaDto;
var SancionSugeridaFilter = /** @class */ (function (_super) {
    __extends(SancionSugeridaFilter, _super);
    function SancionSugeridaFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SancionSugeridaFilter;
}(base_model_1.FilterDTO));
exports.SancionSugeridaFilter = SancionSugeridaFilter;
//# sourceMappingURL=sancionsugerida.model.js.map
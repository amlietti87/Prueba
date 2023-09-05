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
var TipoDniDto = /** @class */ (function (_super) {
    __extends(TipoDniDto, _super);
    function TipoDniDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TipoDniDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return TipoDniDto;
}(base_model_1.Dto));
exports.TipoDniDto = TipoDniDto;
var TipoDniFilter = /** @class */ (function (_super) {
    __extends(TipoDniFilter, _super);
    function TipoDniFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return TipoDniFilter;
}(base_model_1.FilterDTO));
exports.TipoDniFilter = TipoDniFilter;
//# sourceMappingURL=tipodni.model.js.map
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
var CoordenadasDto = /** @class */ (function (_super) {
    __extends(CoordenadasDto, _super);
    function CoordenadasDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CoordenadasDto.prototype.getDescription = function () {
        return this.Description;
    };
    return CoordenadasDto;
}(base_model_1.Dto));
exports.CoordenadasDto = CoordenadasDto;
var CoordenadasFilter = /** @class */ (function (_super) {
    __extends(CoordenadasFilter, _super);
    function CoordenadasFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CoordenadasFilter;
}(base_model_1.FilterDTO));
exports.CoordenadasFilter = CoordenadasFilter;
//# sourceMappingURL=coordenadas.model.js.map
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
var PlaTalleresIvuDto = /** @class */ (function (_super) {
    __extends(PlaTalleresIvuDto, _super);
    function PlaTalleresIvuDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PlaTalleresIvuDto.prototype.getDescription = function () {
        return this.CodGalNavigation.DesGal;
    };
    return PlaTalleresIvuDto;
}(base_model_1.Dto));
exports.PlaTalleresIvuDto = PlaTalleresIvuDto;
var PlaTalleresIvuFilter = /** @class */ (function (_super) {
    __extends(PlaTalleresIvuFilter, _super);
    function PlaTalleresIvuFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return PlaTalleresIvuFilter;
}(base_model_1.FilterDTO));
exports.PlaTalleresIvuFilter = PlaTalleresIvuFilter;
//# sourceMappingURL=talleresivu.model.js.map
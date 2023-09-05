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
var base_model_1 = require("./base.model");
var CroquisDto = /** @class */ (function (_super) {
    __extends(CroquisDto, _super);
    function CroquisDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CroquisDto.prototype.getDescription = function () {
        return this.Description;
    };
    return CroquisDto;
}(base_model_1.Dto));
exports.CroquisDto = CroquisDto;
var CrosquisFilter = /** @class */ (function (_super) {
    __extends(CrosquisFilter, _super);
    function CrosquisFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CrosquisFilter;
}(base_model_1.FilterDTO));
exports.CrosquisFilter = CrosquisFilter;
//# sourceMappingURL=croqui.model.js.map
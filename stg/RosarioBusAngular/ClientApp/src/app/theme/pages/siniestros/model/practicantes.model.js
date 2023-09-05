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
var PracticantesDto = /** @class */ (function (_super) {
    __extends(PracticantesDto, _super);
    function PracticantesDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PracticantesDto.prototype.getDescription = function () {
        return this.ApellidoNombre;
    };
    return PracticantesDto;
}(base_model_1.Dto));
exports.PracticantesDto = PracticantesDto;
var PracticanteFilter = /** @class */ (function (_super) {
    __extends(PracticanteFilter, _super);
    function PracticanteFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return PracticanteFilter;
}(base_model_1.FilterDTO));
exports.PracticanteFilter = PracticanteFilter;
//# sourceMappingURL=practicantes.model.js.map
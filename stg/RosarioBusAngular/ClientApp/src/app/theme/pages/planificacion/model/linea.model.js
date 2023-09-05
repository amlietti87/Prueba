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
var LineaDto = /** @class */ (function (_super) {
    __extends(LineaDto, _super);
    function LineaDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.PlaLineaLineaHoraria = [];
        return _this;
    }
    LineaDto.prototype.getDescription = function () {
        return this.DesLin;
    };
    return LineaDto;
}(base_model_1.Dto));
exports.LineaDto = LineaDto;
var LineaFilter = /** @class */ (function (_super) {
    __extends(LineaFilter, _super);
    function LineaFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return LineaFilter;
}(base_model_1.FilterDTO));
exports.LineaFilter = LineaFilter;
var LineasFilter = /** @class */ (function (_super) {
    __extends(LineasFilter, _super);
    function LineasFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return LineasFilter;
}(base_model_1.FilterDTO));
exports.LineasFilter = LineasFilter;
var RutasViewFilter = /** @class */ (function (_super) {
    __extends(RutasViewFilter, _super);
    function RutasViewFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return RutasViewFilter;
}(base_model_1.FilterDTO));
exports.RutasViewFilter = RutasViewFilter;
var BanderaItemLongDto = /** @class */ (function (_super) {
    __extends(BanderaItemLongDto, _super);
    function BanderaItemLongDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return BanderaItemLongDto;
}(base_model_1.ItemDto));
exports.BanderaItemLongDto = BanderaItemLongDto;
//# sourceMappingURL=linea.model.js.map
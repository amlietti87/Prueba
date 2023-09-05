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
var SubGalponDto = /** @class */ (function (_super) {
    __extends(SubGalponDto, _super);
    function SubGalponDto(data) {
        var _this = _super.call(this, data) || this;
        _this.Configu = [];
        return _this;
    }
    SubGalponDto.prototype.getDescription = function () {
        return this.DesSubg;
    };
    return SubGalponDto;
}(base_model_1.Dto));
exports.SubGalponDto = SubGalponDto;
var ConfiguDto = /** @class */ (function (_super) {
    __extends(ConfiguDto, _super);
    function ConfiguDto(data) {
        return _super.call(this, data) || this;
    }
    ConfiguDto.prototype.getDescription = function () {
        return "";
    };
    return ConfiguDto;
}(base_model_1.Dto));
exports.ConfiguDto = ConfiguDto;
var GruposDto = /** @class */ (function (_super) {
    __extends(GruposDto, _super);
    function GruposDto(data) {
        return _super.call(this, data) || this;
    }
    GruposDto.prototype.getDescription = function () {
        return "";
    };
    return GruposDto;
}(base_model_1.Dto));
exports.GruposDto = GruposDto;
var GalponDto = /** @class */ (function (_super) {
    __extends(GalponDto, _super);
    function GalponDto(data) {
        return _super.call(this, data) || this;
    }
    GalponDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return GalponDto;
}(base_model_1.Dto));
exports.GalponDto = GalponDto;
var PlanCamDto = /** @class */ (function (_super) {
    __extends(PlanCamDto, _super);
    function PlanCamDto(data) {
        return _super.call(this, data) || this;
    }
    PlanCamDto.prototype.getDescription = function () {
        return "";
    };
    return PlanCamDto;
}(base_model_1.Dto));
exports.PlanCamDto = PlanCamDto;
var SubGalponFilter = /** @class */ (function (_super) {
    __extends(SubGalponFilter, _super);
    function SubGalponFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SubGalponFilter;
}(base_model_1.FilterDTO));
exports.SubGalponFilter = SubGalponFilter;
//# sourceMappingURL=subgalpon.model.js.map
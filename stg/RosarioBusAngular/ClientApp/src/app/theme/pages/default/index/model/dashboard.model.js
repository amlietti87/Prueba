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
var base_model_1 = require("../../../../../shared/model/base.model");
var DashboardDto = /** @class */ (function (_super) {
    __extends(DashboardDto, _super);
    function DashboardDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.Selected = false;
        return _this;
    }
    DashboardDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return DashboardDto;
}(base_model_1.Dto));
exports.DashboardDto = DashboardDto;
var DashboardFilter = /** @class */ (function (_super) {
    __extends(DashboardFilter, _super);
    function DashboardFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return DashboardFilter;
}(base_model_1.FilterDTO));
exports.DashboardFilter = DashboardFilter;
var UsuarioDashboardItemDto = /** @class */ (function (_super) {
    __extends(UsuarioDashboardItemDto, _super);
    function UsuarioDashboardItemDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    UsuarioDashboardItemDto.prototype.getDescription = function () {
        return "";
    };
    return UsuarioDashboardItemDto;
}(base_model_1.Dto));
exports.UsuarioDashboardItemDto = UsuarioDashboardItemDto;
var UsuarioDashboardInput = /** @class */ (function (_super) {
    __extends(UsuarioDashboardInput, _super);
    function UsuarioDashboardInput() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.Items = [];
        return _this;
    }
    UsuarioDashboardInput.prototype.getDescription = function () {
        return "";
    };
    return UsuarioDashboardInput;
}(base_model_1.Dto));
exports.UsuarioDashboardInput = UsuarioDashboardInput;
//# sourceMappingURL=dashboard.model.js.map
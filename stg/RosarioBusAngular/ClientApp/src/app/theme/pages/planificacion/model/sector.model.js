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
var SectorFilter = /** @class */ (function (_super) {
    __extends(SectorFilter, _super);
    function SectorFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SectorFilter;
}(base_model_1.FilterDTO));
exports.SectorFilter = SectorFilter;
var SectorDto = /** @class */ (function (_super) {
    __extends(SectorDto, _super);
    function SectorDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SectorDto.prototype.getDescription = function () {
        return this.Descripcion;
    };
    return SectorDto;
}(base_model_1.Dto));
exports.SectorDto = SectorDto;
var SectorViewDto = /** @class */ (function (_super) {
    __extends(SectorViewDto, _super);
    function SectorViewDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SectorViewDto.prototype.getDescription = function () {
        return this.desc;
    };
    return SectorViewDto;
}(base_model_1.Dto));
exports.SectorViewDto = SectorViewDto;
var ItemSectorViewDto = /** @class */ (function (_super) {
    __extends(ItemSectorViewDto, _super);
    function ItemSectorViewDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ItemSectorViewDto.prototype.getDescription = function () {
        return this.desc;
    };
    return ItemSectorViewDto;
}(base_model_1.Dto));
exports.ItemSectorViewDto = ItemSectorViewDto;
var RutaSectoresDto = /** @class */ (function (_super) {
    __extends(RutaSectoresDto, _super);
    function RutaSectoresDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    RutaSectoresDto.prototype.getDescription = function () {
        return this.Description;
    };
    return RutaSectoresDto;
}(base_model_1.Dto));
exports.RutaSectoresDto = RutaSectoresDto;
//# sourceMappingURL=sector.model.js.map
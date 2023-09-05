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
var PuntoFilter = /** @class */ (function (_super) {
    __extends(PuntoFilter, _super);
    function PuntoFilter() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.CodRecs = [];
        return _this;
    }
    return PuntoFilter;
}(base_model_1.FilterDTO));
exports.PuntoFilter = PuntoFilter;
var PuntoDto = /** @class */ (function (_super) {
    __extends(PuntoDto, _super);
    function PuntoDto() {
        var _this = _super.call(this) || this;
        _this.Polylines = [];
        _this.Steps = [];
        _this.CodigoNombre = "";
        return _this;
    }
    PuntoDto.prototype.getDescription = function () {
        return this.CodigoNombre;
    };
    PuntoDto.prototype.setGmapMarker = function (marker) {
        this.Lat = marker.lat;
        this.Long = marker.lng;
        this.Id = marker.id;
    };
    return PuntoDto;
}(base_model_1.Dto));
exports.PuntoDto = PuntoDto;
//# sourceMappingURL=punto.model.js.map
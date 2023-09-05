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
var TallerDto = /** @class */ (function (_super) {
    __extends(TallerDto, _super);
    function TallerDto(data) {
        return _super.call(this, data) || this;
    }
    TallerDto.prototype.getDescription = function () {
        return this.Nombre;
    };
    return TallerDto;
}(base_model_1.Dto));
exports.TallerDto = TallerDto;
//export class TallerPuntoDto extends Dto<string> {
//    getDescription(): string {
//        return '';
//    }
//    taller: TallerDto;
//    puntoTaller: PuntoDto;
//    ruta: RutaDto;
//}
var TallerFilter = /** @class */ (function (_super) {
    __extends(TallerFilter, _super);
    function TallerFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return TallerFilter;
}(base_model_1.FilterDTO));
exports.TallerFilter = TallerFilter;
//# sourceMappingURL=taller.model.js.map
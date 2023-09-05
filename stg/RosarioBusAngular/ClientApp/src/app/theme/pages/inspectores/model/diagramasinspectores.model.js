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
var DiagramasInspectoresDto = /** @class */ (function (_super) {
    __extends(DiagramasInspectoresDto, _super);
    function DiagramasInspectoresDto() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.InspDiagramaInspectoresTurnos = [];
        return _this;
    }
    DiagramasInspectoresDto.prototype.getDescription = function () {
        return this.Mes.toString() + '-' + this.Anio.toString();
    };
    return DiagramasInspectoresDto;
}(base_model_1.Dto));
exports.DiagramasInspectoresDto = DiagramasInspectoresDto;
var InspDiagramaInspectoresTurnosDto = /** @class */ (function () {
    function InspDiagramaInspectoresTurnosDto() {
    }
    return InspDiagramaInspectoresTurnosDto;
}());
exports.InspDiagramaInspectoresTurnosDto = InspDiagramaInspectoresTurnosDto;
var DiagramasInspectoresFilter = /** @class */ (function (_super) {
    __extends(DiagramasInspectoresFilter, _super);
    function DiagramasInspectoresFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return DiagramasInspectoresFilter;
}(base_model_1.FilterDTO));
exports.DiagramasInspectoresFilter = DiagramasInspectoresFilter;
var DiagramaMesAnioDto = /** @class */ (function () {
    function DiagramaMesAnioDto() {
        this.DiasMes = [];
        this.DiasMesAP = [];
    }
    return DiagramaMesAnioDto;
}());
exports.DiagramaMesAnioDto = DiagramaMesAnioDto;
var DiasMesDto = /** @class */ (function () {
    function DiasMesDto() {
        this.Inspectores = [];
    }
    return DiasMesDto;
}());
exports.DiasMesDto = DiasMesDto;
var InspectorDiaDto = /** @class */ (function () {
    function InspectorDiaDto() {
        this.validations = [];
    }
    Object.defineProperty(InspectorDiaDto.prototype, "HoraDesde", {
        get: function () {
            return this._HoraDesde;
        },
        set: function (value) {
            debugger;
            this._HoraDesde = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InspectorDiaDto.prototype, "HoraHasta", {
        get: function () {
            return this._HoraHasta;
        },
        set: function (value) {
            debugger;
            this._HoraHasta = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InspectorDiaDto.prototype, "HoraDesdeModificada", {
        get: function () {
            return this._HoraDesdeModificada;
        },
        set: function (value) {
            debugger;
            this._HoraDesdeModificada = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InspectorDiaDto.prototype, "HoraHastaModificada", {
        get: function () {
            return this._HoraHastaModificada;
        },
        set: function (value) {
            debugger;
            this._HoraHastaModificada = value;
        },
        enumerable: true,
        configurable: true
    });
    return InspectorDiaDto;
}());
exports.InspectorDiaDto = InspectorDiaDto;
var EstadosDiagrama;
(function (EstadosDiagrama) {
    EstadosDiagrama[EstadosDiagrama["Borrador"] = 1] = "Borrador";
    EstadosDiagrama[EstadosDiagrama["Publicado"] = 2] = "Publicado";
})(EstadosDiagrama = exports.EstadosDiagrama || (exports.EstadosDiagrama = {}));
var ValidationResult = /** @class */ (function () {
    function ValidationResult() {
        this.isValid = false;
        this.Messages = [];
    }
    return ValidationResult;
}());
exports.ValidationResult = ValidationResult;
//# sourceMappingURL=diagramasinspectores.model.js.map
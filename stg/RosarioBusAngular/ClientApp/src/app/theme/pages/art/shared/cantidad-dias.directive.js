"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var moment = require("moment");
var CantidadDiasDirective = /** @class */ (function () {
    function CantidadDiasDirective(injector, cdRef) {
        this.cdRef = cdRef;
        this.cantidadDiasChange = new core_1.EventEmitter();
    }
    Object.defineProperty(CantidadDiasDirective.prototype, "FechaDesde", {
        get: function () {
            return this._FechaDesde;
        },
        set: function (value) {
            this._FechaDesde = value;
            this.CalcularCantidadDias();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CantidadDiasDirective.prototype, "FechaHasta", {
        get: function () {
            return this._FechaHasta;
        },
        set: function (value) {
            this._FechaHasta = value;
            this.CalcularCantidadDias();
        },
        enumerable: true,
        configurable: true
    });
    CantidadDiasDirective.prototype.CalcularCantidadDias = function () {
        if (!this._FechaDesde) {
            this.value = 0;
            return;
        }
        if (this._FechaDesde == null) {
            this.value = 0;
            return;
        }
        var begin = moment(this._FechaDesde);
        var end = moment(new Date());
        if (this.FechaHasta) {
            if (this.FechaHasta != null) {
                end = moment(this._FechaHasta);
            }
        }
        if (begin > end) {
            this.value = 0;
            return;
        }
        var duration = moment.duration(end.diff(begin));
        var days = Math.floor(duration.asDays());
        this.value = days;
        //this.cantidadDias = days;
        //this.cdRef.detectChanges();
        this.cantidadDiasChange.emit(this.value);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CantidadDiasDirective.prototype, "cantidadDiasChange", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Date),
        __metadata("design:paramtypes", [Date])
    ], CantidadDiasDirective.prototype, "FechaDesde", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Date),
        __metadata("design:paramtypes", [Date])
    ], CantidadDiasDirective.prototype, "FechaHasta", null);
    CantidadDiasDirective = __decorate([
        core_1.Directive({
            selector: '[CantidadDias]'
        }),
        __metadata("design:paramtypes", [core_1.Injector, core_1.ChangeDetectorRef])
    ], CantidadDiasDirective);
    return CantidadDiasDirective;
}());
exports.CantidadDiasDirective = CantidadDiasDirective;
//# sourceMappingURL=cantidad-dias.directive.js.map
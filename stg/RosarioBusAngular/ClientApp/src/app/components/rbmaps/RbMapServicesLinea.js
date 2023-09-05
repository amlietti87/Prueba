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
var RbMapLinea_1 = require("./RbMapLinea");
var RbMapServicesLinea = /** @class */ (function () {
    function RbMapServicesLinea() {
    }
    RbMapServicesLinea.prototype.save = function (linea) {
        var lineas = this.getAll();
        var found = false;
        for (var i = 0; i < lineas.length && !found; i++) {
            var element = lineas[i];
            if (element.id == linea.id) {
                element.points = linea.points;
                element.nombre = linea.nombre;
                element.diametro = linea.diametro;
                element.color = linea.color;
                element.grupoId = linea.grupoId;
                found = true;
            }
        }
        if (!found)
            lineas.push(linea);
        localStorage.setItem('lineas', JSON.stringify(lineas));
    };
    RbMapServicesLinea.prototype.getAll = function () {
        var _this = this;
        var resultado = new Array();
        var lineas = localStorage.getItem('lineas');
        if (lineas) {
            var list = JSON.parse(lineas);
            list.forEach(function (element) {
                var l = _this.getLinea(element);
                resultado.push(l);
            });
        }
        return resultado;
    };
    RbMapServicesLinea.prototype.get = function (id) {
        var linea;
        var lineas_ls = localStorage.getItem('lineas');
        if (lineas_ls) {
            var lineas = JSON.parse(lineas_ls);
            var result = lineas.filter(function (item) { return item.id == id; });
            if (result && result.length > 0)
                linea = this.getLinea(result[0]);
        }
        return linea;
    };
    RbMapServicesLinea.prototype.getLinea = function (element) {
        var l = new RbMapLinea_1.RbMapLinea();
        l.color = element.color;
        l.diametro = element.diametro;
        l.grupoId = element.grupoId;
        l.id = element.id;
        l.nombre = element.nombre;
        l.points = element.points;
        return l;
    };
    RbMapServicesLinea = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], RbMapServicesLinea);
    return RbMapServicesLinea;
}());
exports.RbMapServicesLinea = RbMapServicesLinea;
//# sourceMappingURL=RbMapServicesLinea.js.map
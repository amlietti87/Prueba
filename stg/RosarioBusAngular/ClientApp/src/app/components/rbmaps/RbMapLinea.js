"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RbMapServices_1 = require("../rbmaps/RbMapServices");
var RbMapLinea = /** @class */ (function () {
    function RbMapLinea() {
        this.id = RbMapServices_1.RbMapServices.guid();
        this.diametro = 2;
        this.points = new Array();
        this.color = "#000";
    }
    RbMapLinea.prototype.agregarPunto = function (punto) {
        this.points.push(punto);
    };
    return RbMapLinea;
}());
exports.RbMapLinea = RbMapLinea;
//# sourceMappingURL=RbMapLinea.js.map
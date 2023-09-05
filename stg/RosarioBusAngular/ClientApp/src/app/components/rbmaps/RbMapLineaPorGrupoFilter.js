"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var RbMapLineaPorGrupoFilter = /** @class */ (function () {
    function RbMapLineaPorGrupoFilter() {
    }
    RbMapLineaPorGrupoFilter.prototype.transform = function (items, filter) {
        if (!items || !filter) {
            return new Array();
        }
        // filter items array, items which match and return true will be
        // kept, false will be filtered out
        return items.filter(function (item) { return item.grupoId == filter; });
    };
    RbMapLineaPorGrupoFilter = __decorate([
        core_1.Pipe({
            name: 'lineaPorGrupo',
            pure: false
        })
    ], RbMapLineaPorGrupoFilter);
    return RbMapLineaPorGrupoFilter;
}());
exports.RbMapLineaPorGrupoFilter = RbMapLineaPorGrupoFilter;
//# sourceMappingURL=RbMapLineaPorGrupoFilter.js.map
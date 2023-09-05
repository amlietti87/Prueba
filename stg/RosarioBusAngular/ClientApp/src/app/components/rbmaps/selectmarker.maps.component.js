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
var RbMapServices_1 = require("./RbMapServices");
var baseMaps_component_1 = require("./baseMaps.component");
var SelectMarkerMapsComponent = /** @class */ (function (_super) {
    __extends(SelectMarkerMapsComponent, _super);
    function SelectMarkerMapsComponent(injector, cdRef, _service) {
        var _this = _super.call(this, injector, cdRef, _service) || this;
        _this.cdRef = cdRef;
        _this.mapId = "SelectMarkerMapr ";
        _this.loadOnInit = false;
        _this.DefaultIconID = 4;
        _this.markers = [];
        return _this;
    }
    SelectMarkerMapsComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    SelectMarkerMapsComponent.prototype.AgregarMarcador = function (latlng, center) {
        this.removeLayerPuntos();
        _super.prototype.AgregarMarcador.call(this, latlng, center);
    };
    SelectMarkerMapsComponent.prototype.successfullyGeocode = function (results) {
        _super.prototype.successfullyGeocode.call(this, results);
        var latlng = results[0].geometry.location;
        this.AgregarMarcador(latlng, true);
    };
    SelectMarkerMapsComponent.prototype.removeLayerPuntos = function () {
        var _this = this;
        for (var i = 0; i < this.map.markers.length; i++) {
            var index = this.map.markers.findIndex(function (e) { return e.id == _this.map.markers[i].id; });
            if (index >= 0) {
                this.map.markers[index].setMap(null);
                this.map.markers.splice(index, 1);
            }
        }
    };
    SelectMarkerMapsComponent = __decorate([
        core_1.Component({
            selector: 'select-marker-maps',
            templateUrl: './selectmarker.maps.component.html',
            styleUrls: ['./rbmaps.component.css']
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            core_1.ChangeDetectorRef, RbMapServices_1.RbMapServices])
    ], SelectMarkerMapsComponent);
    return SelectMarkerMapsComponent;
}(baseMaps_component_1.BaseMapsComponent));
exports.SelectMarkerMapsComponent = SelectMarkerMapsComponent;
//# sourceMappingURL=selectmarker.maps.component.js.map
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
var tipoelemento_model_1 = require("../../theme/pages/siniestros/model/tipoelemento.model");
var tipoelemento_service_1 = require("../../theme/pages/siniestros/tipoelemento/tipoelemento.service");
var environment_1 = require("../../../environments/environment");
var siniestro_model_1 = require("../../theme/pages/siniestros/model/siniestro.model");
var croqui_service_1 = require("./croqui.service");
var croqui_model_1 = require("../model/croqui.model");
var base_model_1 = require("../model/base.model");
var detail_component_1 = require("../manager/detail.component");
var CroquiComponent = /** @class */ (function (_super) {
    __extends(CroquiComponent, _super);
    function CroquiComponent(serviceTipoElemento, service, injector) {
        var _this = _super.call(this, service, injector) || this;
        _this.serviceTipoElemento = serviceTipoElemento;
        _this.service = service;
        _this.firstHandled = false;
        _this.defaultSvg = '<svg name="svgout" _ngcontent-c2="" height="600px" id="svgout" width="800px"><defs></defs><image id="background" href="" preserveAspectRatio = "none" x = "0" y = "0" width = "600" height = "800" ></image></svg>';
        _this.dataElementos = [];
        _this._value = '200';
        _this._angulo = '0';
        _this._isflip = false;
        _this._istext = false;
        _this._colorText = '#000000';
        _this._IsClick = false;
        if (!_this.detail) {
            _this.detail = new croqui_model_1.CroquisDto();
        }
        return _this;
    }
    Object.defineProperty(CroquiComponent.prototype, "detailSiniestro", {
        get: function () {
            return this._Siniestro;
        },
        set: function (value) {
            if (value) {
                this._Siniestro = value;
                if (this._Siniestro.CroquiId && this._Siniestro.CroquiId != 0) {
                    this.GetCroqui(this._Siniestro.CroquiId);
                }
                else {
                    this.CroquiDefault();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CroquiComponent.prototype.Render = function () {
        this.svg = Snap("#svgout");
        var selft = this;
        this.appDownloadUrl = environment_1.environment.siniestrosUrl + '/Adjuntos/DownloadFiles';
        if (this.svg) {
            this.svg.selectAll('g').forEach(function (element) {
                selft.handershape(element);
            });
        }
        var clickCallback = function (event) {
            if (!selft.firstHandled) {
                selft.clearselecion(selft);
            }
            selft.firstHandled = false;
        };
        this.svg.click(clickCallback);
        this.textToAdd = "";
        this.completarPanel();
    };
    CroquiComponent.prototype.completarPanel = function () {
        var _this = this;
        var f = new tipoelemento_model_1.TipoElementoFilter();
        this.serviceTipoElemento.requestAllByFilter(f)
            .subscribe(function (t) {
            if (t.DataObject) {
                _this.dataElementos = t.DataObject.Items;
            }
        });
    };
    CroquiComponent.prototype.GetCroqui = function (id) {
        var _this = this;
        this.service.getById(id).subscribe(function (t) {
            if (t.DataObject) {
                _this.detail = t.DataObject;
                _this.viewMode = base_model_1.ViewMode.Modify;
                _this.dataContainer.nativeElement.innerHTML = _this.detail.Svg;
                _this.Render();
            }
        });
    };
    CroquiComponent.prototype.CroquiDefault = function () {
        this.detail = new croqui_model_1.CroquisDto();
        this.dataContainer.nativeElement.innerHTML = this.defaultSvg;
        this.viewMode = base_model_1.ViewMode.Add;
        this.Render();
    };
    CroquiComponent.prototype.int = function () {
        var selft = this;
        this.clearselecion(selft);
    };
    Object.defineProperty(CroquiComponent.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            this._value = v;
            this.applyChange();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CroquiComponent.prototype, "angulo", {
        get: function () {
            return this._angulo;
        },
        set: function (v) {
            this._angulo = v;
            this.applyChange();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CroquiComponent.prototype, "isflip", {
        get: function () {
            return this._isflip;
        },
        set: function (v) {
            this._isflip = v;
            this.applyChange();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CroquiComponent.prototype, "isText", {
        get: function () {
            return this._istext;
        },
        set: function (v) {
            this._istext = v;
            this.applyChange();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CroquiComponent.prototype, "colorText", {
        get: function () {
            return this._colorText;
        },
        set: function (v) {
            this._colorText = v;
            //this.applyChange();
        },
        enumerable: true,
        configurable: true
    });
    CroquiComponent.prototype.applyChange = function () {
        var _this = this;
        if (!this._IsClick) {
            if (this.current) {
                if (this.isText) {
                    if (this.isflip) {
                        this.current.selectAll('text')[0].transform('s-1,1t0,0r' + this.angulo);
                        var text = this.current.selectAll('text')[0];
                        var bb = text.getBBox();
                        this.current.selectAll('rect')[0].attr({ 'x': bb.x });
                        this.current.selectAll('rect')[0].attr({ 'y': bb.y });
                        this.current.selectAll('rect')[0].attr({ 'width': bb.width });
                        this.current.selectAll('rect')[0].attr({ 'height': bb.height });
                    }
                    else {
                        this.current.selectAll('text')[0].transform('t0,0r' + this.angulo);
                        var text = this.current.selectAll('text')[0];
                        var bb = text.getBBox();
                        this.current.selectAll('rect')[0].attr({ 'x': bb.x });
                        this.current.selectAll('rect')[0].attr({ 'y': bb.y });
                        this.current.selectAll('rect')[0].attr({ 'width': bb.width });
                        this.current.selectAll('rect')[0].attr({ 'height': bb.height });
                    }
                    var valuechange = 0.375;
                    var valuenuevo = +this.value * valuechange;
                    var text = this.current.selectAll('text')[0];
                    this.current.selectAll('text')[0].attr({ 'font-size': valuenuevo });
                    var bb = text.getBBox();
                    this.current.selectAll('rect')[0].attr({ 'x': bb.x });
                    this.current.selectAll('rect')[0].attr({ 'y': bb.y });
                    this.current.selectAll('rect')[0].attr({ 'width': bb.width });
                    this.current.selectAll('rect')[0].attr({ 'height': bb.height });
                }
                else {
                    if (this.isflip) {
                        this.current.selectAll('image')[0].transform('s-1,1t0,0r' + this.angulo);
                        this.current.selectAll('rect')[0].transform('s-1,1t0,0r' + this.angulo);
                    }
                    else {
                        this.current.selectAll('image')[0].transform('t0,0r' + this.angulo);
                        this.current.selectAll('rect')[0].transform('t0,0r' + this.angulo);
                    }
                    if (this.current.selectAll('rect')[0].attr('width') != this.value) {
                        var ratio = this.current.selectAll('rect')[0].attr('width') / this.current.selectAll('rect')[0].attr('height');
                        var nuevaaltura = this.nuevaAltura(this.value, ratio);
                        this.current.selectAll('*').forEach(function (element) {
                            element.attr('width', _this.value);
                            element.attr('height', nuevaaltura);
                        });
                    }
                }
                this.current.attr({ 'isflip': this.isflip });
                this.current.attr({ 'angulo': this.angulo });
                this.current.attr({ 'istext': this.isText });
                this.current.attr({ 'fill': this.colorText });
            }
        }
    };
    CroquiComponent.prototype.borrar = function () {
        if (this.current) {
            this.current.remove();
        }
        this.current = null;
    };
    CroquiComponent.prototype.clearselecion = function (c) {
        c.svg.selectAll('rect').attr({ 'strokeWidth': 0 });
        this.current = null;
        this.isText = false;
    };
    CroquiComponent.prototype.AgregarTexto = function () {
        var s = this.svg;
        var g = s.g();
        var text = g.text(70, 135, this.textToAdd);
        text.attr({
            'font-size': '50px',
            'fill': "#000000"
        });
        var bb = text.getBBox();
        var rect = g.rect(bb.x, bb.y, bb.width, bb.height);
        rect.attr({ 'fill': null, 'stroke': 'black', 'strokeWidth': 0, 'fill-opacity': 0.0 });
        g.attr({
            'angulo': 0, 'isflip': false, 'istext': true
        });
        this.handershape(g);
    };
    CroquiComponent.prototype.agregarsvg = function (detail) {
        if (!(detail.TipoElementoId == 1)) {
            var s = this.svg;
            var img = this.appDownloadUrl + '\?id=' + detail.Id + '&c=' + this.time;
            var g = s.g();
            var title = Snap.parse('<title>This is a title 2</title>');
            var igenoriginal = new Image();
            igenoriginal.src = img;
            var ratio = igenoriginal.width / igenoriginal.height;
            var nuevaaltura = this.nuevaAltura(200, ratio);
            var image = g.image(img, 0, 0, 200, nuevaaltura);
            var rect = g.rect(0, 0, 200, nuevaaltura);
            rect.attr({ 'fill': null, 'stroke': 'black', 'strokeWidth': 0, 'fill-opacity': 0.0 });
            g.attr({ 'angulo': 0, 'isflip': false, 'istext': false });
            image.append(title);
            this.handershape(g);
        }
        else {
            var urlimage = this.appDownloadUrl + '\?id=' + detail.Id + '&c=' + this.time;
            var imageobj = new Image();
            imageobj.src = urlimage;
            document.getElementById('background').setAttribute("href", urlimage);
            document.getElementById('background').setAttribute("width", imageobj.width.toString() + "px");
            document.getElementById('background').setAttribute("height", imageobj.height.toString() + "px");
        }
    };
    CroquiComponent.prototype.nuevaAltura = function (width, aspectRatio) {
        var height = width / aspectRatio;
        return height;
    };
    CroquiComponent.prototype.handershape = function (g) {
        var selft = this;
        var move = function (dx, dy) {
            this.attr({
                transform: this.data('origTransform') + (this.data('origTransform') ? "T" : "t") + [dx, dy]
            });
        };
        var start = function () {
            this.data('origTransform', this.transform().local);
        };
        var stop = function () {
            console.log('finished dragging');
        };
        var click = function () {
            selft.firstHandled = true;
            selft.svg.selectAll('rect').attr({ 'strokeWidth': 0 });
            g.selectAll('rect').attr({ 'strokeWidth': 1 });
            selft.current = this;
            selft._IsClick = true;
            selft.value = g.selectAll('rect')[0].attr('width');
            selft.angulo = selft.current.attr('angulo') || 0;
            selft.isflip = selft.convertToBoolean(selft.current.attr('isflip')) || false;
            selft.isText = selft.convertToBoolean(selft.current.attr('istext')) || false;
            if (selft.isText) {
                selft.colorText = g.selectAll('text')[0].attr().fill;
            }
            selft._IsClick = false;
        };
        g.drag(move, start, stop);
        var selft = this;
        g.click(click);
    };
    CroquiComponent.prototype.ChangeColor = function (event) {
        this.current.selectAll('text')[0].attr({ 'fill': this.colorText });
    };
    CroquiComponent.prototype.convertToBoolean = function (input) {
        try {
            return JSON.parse(input);
        }
        catch (e) {
            return undefined;
        }
    };
    CroquiComponent.prototype.GuardarCroqui = function () {
        var _this = this;
        debugger;
        this.clearselecion(this);
        this.detail.TipoId = 1;
        this.detail.SiniestroId = this.detailSiniestro.Id;
        this.detail.Svg = new XMLSerializer().serializeToString(document.getElementById("svgout"));
        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(function () { _this.saving = false; })
            .subscribe(function (t) {
            if (_this.viewMode = base_model_1.ViewMode.Add) {
                _this.detail.Id = t.DataObject;
                _this.detailSiniestro.CroquiId = _this.detail.Id;
            }
            _this.notify.info('Guardado exitosamente');
            _this.closeOnSave = false;
            if (_this.closeOnSave) {
                _this.close();
            }
            ;
            _this.affterSave(_this.detail);
            _this.modalSave.emit(null);
        });
    };
    __decorate([
        core_1.ViewChild('dataContainer'),
        __metadata("design:type", core_1.ElementRef)
    ], CroquiComponent.prototype, "dataContainer", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CroquiComponent.prototype, "allowAdd", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CroquiComponent.prototype, "allowModify", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", siniestro_model_1.SiniestrosDto),
        __metadata("design:paramtypes", [siniestro_model_1.SiniestrosDto])
    ], CroquiComponent.prototype, "detailSiniestro", null);
    CroquiComponent = __decorate([
        core_1.Component({
            selector: 'app-croqui',
            templateUrl: './croqui.component.html',
            styleUrls: ['./croqui.component.css']
        }),
        __metadata("design:paramtypes", [tipoelemento_service_1.TipoElementoService,
            croqui_service_1.CroquiService,
            core_1.Injector])
    ], CroquiComponent);
    return CroquiComponent;
}(detail_component_1.DetailEmbeddedComponent));
exports.CroquiComponent = CroquiComponent;
//# sourceMappingURL=croqui.component.js.map
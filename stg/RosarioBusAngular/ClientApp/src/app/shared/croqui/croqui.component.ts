import { Component, OnInit, Injector, Input, ViewChild, ElementRef } from '@angular/core';
import { AppComponentBase } from '../common/app-component-base';
import * as SNAPSVG_TYPE from "snapsvg"
import { TipoElementoDto, TipoElementoFilter } from '../../theme/pages/siniestros/model/tipoelemento.model';
import { TipoElementoService } from '../../theme/pages/siniestros/tipoelemento/tipoelemento.service';
import { environment } from '../../../environments/environment';
import { ElementosDto } from '../../theme/pages/siniestros/model/elemento.model';
import { xml } from 'd3';
import { SiniestrosDto } from '../../theme/pages/siniestros/model/siniestro.model';
import { CroquiService } from './croqui.service';
import { CroquisDto } from '../model/croqui.model';
import { ViewMode } from '../model/base.model';
import { DetailEmbeddedComponent } from '../manager/detail.component';
import { CrudService } from '../common/services/crud.service';

declare var Snap: typeof SNAPSVG_TYPE
declare var mina: any;

@Component({
    selector: 'app-croqui',
    templateUrl: './croqui.component.html',
    styleUrls: ['./croqui.component.css']
})
export class CroquiComponent extends DetailEmbeddedComponent<CroquisDto> {

    @ViewChild('dataContainer') dataContainer: ElementRef;

    appDownloadUrl: string;
    current: any;
    svg: any;
    time: string;
    firstHandled: boolean = false;
    _Siniestro: SiniestrosDto;
    textToAdd: string;

    @Input() allowAdd: boolean;
    @Input() allowModify: boolean;

    @Input()
    get detailSiniestro(): SiniestrosDto {

        return this._Siniestro;
    }

    set detailSiniestro(value: SiniestrosDto) {
        if (value) {
            this._Siniestro = value;
            if (this._Siniestro.CroquiId && this._Siniestro.CroquiId != 0) {
                this.GetCroqui(this._Siniestro.CroquiId);
            }
            else {
                this.CroquiDefault();

            }
        }
    }


    defaultSvg: string = '<svg name="svgout" _ngcontent-c2="" height="600px" id="svgout" width="800px"><defs></defs><image id="background" href="" preserveAspectRatio = "none" x = "0" y = "0" width = "600" height = "800" ></image></svg>';

    constructor(
        protected serviceTipoElemento: TipoElementoService,
        protected service: CroquiService,
        injector: Injector

    ) {
        super(service, injector);
        if (!this.detail) {
            this.detail = new CroquisDto();
        }
    }

    dataElementos: TipoElementoDto[] = [];
    Render() {

        this.svg = Snap("#svgout");

        var selft = this;
        this.appDownloadUrl = environment.siniestrosUrl + '/Adjuntos/DownloadFiles';

        if (this.svg) {
            this.svg.selectAll('g').forEach(element => {
                selft.handershape(element);
            });
        }



        var clickCallback = function(event) {
            if (!selft.firstHandled) {

                selft.clearselecion(selft);
            }
            selft.firstHandled = false;
        };

        this.svg.click(clickCallback);

        this.textToAdd = "";
        this.completarPanel();
    }

    completarPanel() {
        var f = new TipoElementoFilter();
        this.serviceTipoElemento.requestAllByFilter(f)
            .subscribe((t) => {
                if (t.DataObject) {
                    this.dataElementos = t.DataObject.Items;
                }
            })
    }


    GetCroqui(id: number): any {
        this.service.getById(id).subscribe
            ((t) => {
                if (t.DataObject) {
                    this.detail = t.DataObject;
                    this.viewMode = ViewMode.Modify;
                    this.dataContainer.nativeElement.innerHTML = this.detail.Svg;
                    this.Render();
                }
            });
    }
    CroquiDefault() {
        this.detail = new CroquisDto();
        this.dataContainer.nativeElement.innerHTML = this.defaultSvg;
        this.viewMode = ViewMode.Add;
        this.Render();
    }


    int(): void {
        var selft = this;
        this.clearselecion(selft);
    }

    _value: string = '200';
    _angulo: string = '0';
    _isflip: boolean = false;
    _istext: boolean = false;
    _colorText: string = '#000000';

    get value(): string {

        return this._value;
    }

    set value(v: string) {
        this._value = v;
        this.applyChange();
    }


    get angulo(): string {

        return this._angulo;
    }

    set angulo(v: string) {
        this._angulo = v;
        this.applyChange();
    }


    set isflip(v: boolean) {
        this._isflip = v;
        this.applyChange();
    }

    get isflip(): boolean {

        return this._isflip;
    }

    set isText(v: boolean) {
        this._istext = v;
        this.applyChange();
    }

    get isText(): boolean {

        return this._istext;
    }

    set colorText(v: string) {
        this._colorText = v;
        //this.applyChange();
    }

    get colorText(): string {

        return this._colorText;
    }



    _IsClick: boolean = false;

    applyChange(): void {

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
                    this.current.selectAll('text')[0].attr({ 'font-size': valuenuevo })
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
                        this.current.selectAll('*').forEach(element => {

                            element.attr('width', this.value);
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


    }

    borrar(): void {
        if (this.current) {
            this.current.remove();
        }

        this.current = null;
    }



    clearselecion(c: CroquiComponent): void {

        c.svg.selectAll('rect').attr({ 'strokeWidth': 0 });
        this.current = null;
        this.isText = false;
    }

    AgregarTexto(): void {
        var s = this.svg;
        var g = s.g();

        var text = g.text(70, 135, this.textToAdd);
        text.attr({
            'font-size': '50px',
            'fill': "#000000"
        });


        var bb = text.getBBox();
        var rect = g.rect(bb.x, bb.y, bb.width, bb.height)
        rect.attr({ 'fill': null, 'stroke': 'black', 'strokeWidth': 0, 'fill-opacity': 0.0 });
        g.attr({
            'angulo': 0, 'isflip': false, 'istext': true
        });


        this.handershape(g);
    }


    agregarsvg(detail: ElementosDto): void {
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
    }


    nuevaAltura(width: any, aspectRatio: any) {
        var height = width / aspectRatio;

        return height;

    }

    handershape(g: any) {

        var selft = this;

        var move = function(dx, dy) {

            this.attr({

                transform: this.data('origTransform') + (this.data('origTransform') ? "T" : "t") + [dx, dy]
            });
        }

        var start = function() {
            this.data('origTransform', this.transform().local);
        }
        var stop = function() {
            console.log('finished dragging');
        }

        var click = function() {

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

        }


        g.drag(move, start, stop);
        var selft = this;


        g.click(click);



    }

    ChangeColor(event: any) {
        this.current.selectAll('text')[0].attr({ 'fill': this.colorText });
    }

    convertToBoolean(input: string): boolean | undefined {
        try {
            return JSON.parse(input);
        }
        catch (e) {
            return undefined;
        }
    }

    GuardarCroqui() {
        this.clearselecion(this);
        this.detail.TipoId = 1;
        this.detail.SiniestroId = this.detailSiniestro.Id;
        this.detail.Svg = new XMLSerializer().serializeToString(document.getElementById("svgout"));
        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {

                if (this.viewMode = ViewMode.Add) {
                    this.detail.Id = t.DataObject;
                    this.detailSiniestro.CroquiId = this.detail.Id;
                }

                this.notify.info('Guardado exitosamente');
                this.closeOnSave = false;
                if (this.closeOnSave) {
                    this.close();
                };
                this.affterSave(this.detail);
                this.modalSave.emit(null);
            })
    }



}

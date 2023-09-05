import { Directive, Input, ElementRef, Renderer2, OnInit, Injector, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import * as moment from 'moment';
import { ChangeDetectionStrategy } from '@angular/compiler/src/core';

@Directive({
    selector: '[CantidadDias]'
})
export class CantidadDiasDirective {


    constructor(injector: Injector, private cdRef: ChangeDetectorRef) {

    }

    @Output() cantidadDiasChange = new EventEmitter();

    value: number;


    _FechaDesde: Date;
    @Input()
    set FechaDesde(value: Date) {
        this._FechaDesde = value;
        this.CalcularCantidadDias();
    }
    get FechaDesde(): Date {

        return this._FechaDesde;
    }

    _FechaHasta: Date;
    @Input()
    set FechaHasta(value: Date) {
        this._FechaHasta = value;
        this.CalcularCantidadDias();
    }
    get FechaHasta(): Date {

        return this._FechaHasta;
    }

    CalcularCantidadDias(): void {

        if (!this._FechaDesde) { this.value = 0; return; }
        if (this._FechaDesde == null) { this.value = 0; return; }
        var begin = moment(this._FechaDesde);
        var end = moment(new Date());
        if (this.FechaHasta) {
            if (this.FechaHasta != null) {
                end = moment(this._FechaHasta);
            }
        }
        if (begin > end) { this.value = 0; return; }
        var duration = moment.duration(end.diff(begin));
        var days = Math.floor(duration.asDays());
        this.value = days;
        //this.cantidadDias = days;
        //this.cdRef.detectChanges();
        this.cantidadDiasChange.emit(this.value);

    }
}

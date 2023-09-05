import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { SucursalDto } from '../model/sucursal.model';
import { SucursalService } from '../sucursal/sucursal.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { BanderaDto, BanderaFilter } from '../model/bandera.model';
import { BanderaService } from '../bandera/bandera.service';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { ItemDto } from '../../../../shared/model/base.model';

@Component({
    selector: 'bandera-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => BanderaComboComponent),
            multi: true
        }
    ]
})
export class BanderaComboComponent extends ComboBoxComponent<BanderaDto> implements OnInit {

    constructor(protected serviceBandera: BanderaService, injector: Injector) {
        super(serviceBandera, injector);
    }

    @Input() IsSabanaMode: boolean;


    ngOnInit(): void {
        //super.ngOnInit();
    }

    _CodHfecha: number;

    @Input()
    get CodHfecha(): number {
        return this._CodHfecha;
    }

    set CodHfecha(value: number) {
        this._CodHfecha = value;

        if (value) {
            super.onSearch();
        }

    }


    _CodTdia: number;

    @Input()
    get CodTdia(): number {
        return this._CodTdia;
    }

    set CodTdia(value: number) {
        this._CodTdia = value;

        if (value) {
            super.onSearch();
        }

    }

    protected GetFilter(): any {
        var f = {
            CodHfecha: this.CodHfecha
        };

        return f;
    }

    _linea: ItemDto;
    get linea(): ItemDto {
        return this._linea;
    }

    @Input()
    set linea(linea: ItemDto) {
        this._linea = linea;
        this.buscarBanderas();
    }

    _sentido: number;
    get sentido(): number {
        return this._sentido;
    }

    @Input()
    set sentido(sentido: number) {
        this._sentido = sentido;
        this.buscarBanderas();
    }


    _fecha: Date;
    get fecha(): Date {
        return this._fecha;
    }

    @Input()
    set fecha(fecha: Date) {
        this._fecha = fecha;
        this.buscarBanderas();
    }

    _servicio: number;

    get servicio(): number {
        return this._servicio
    }

    @Input()
    set servicio(servicio: number) {
        this._servicio = servicio;
        this.buscarBanderasServicio();
    }

    buscarBanderasServicio() {
        if (this.linea && this.servicio != 0) {
            var filtro = new BanderaFilter();
            filtro.CodTdia = this.CodTdia;
            filtro.LineaId = this.linea.Id;
            filtro.CodHfecha = this.CodHfecha;
            filtro.cod_servicio = this.servicio;

            this.serviceBandera.RecuperarBanderasPorServicio(filtro).subscribe(e => {
                var self = this;
                var _items = [];
                e.DataObject.forEach(d => {
                    _items.push(new BanderaDto(d));
                });

                this.items = _items;
                self.isLoading = false;
                setTimeout(() => {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }, 200);
            })
        }

    }



    buscarBanderas() {
        if (this.linea && this.sentido && this.sentido != 0) {
            var filtro = new BanderaFilter();
            filtro.SentidoBanderaId = this.sentido;
            filtro.LineaId = this.linea.Id;
            filtro.Fecha = moment(this.fecha).toISOString();
            filtro.CodTdia = this.CodTdia;

            this.serviceBandera.RecuperarLineasActivasPorFecha(filtro).subscribe(e => {

                var self = this;

                var _items = [];

                e.DataObject.forEach(d => {
                    _items.push(new BanderaDto(d));
                });

                this.items = _items;

                self.isLoading = false;
                setTimeout(() => {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }, 200);


            });
        } else {
            this.items = [];
            this.isLoading = false;
            setTimeout(() => {
                $(this.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        }
    }

}

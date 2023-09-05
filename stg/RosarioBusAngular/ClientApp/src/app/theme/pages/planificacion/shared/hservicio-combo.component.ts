import { Component, OnInit, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { ItemDto } from '../../../../shared/model/base.model';
import { HServiciosDto, HServiciosFilter } from '../model/hServicios.model';
import { HServiciosService } from '../hservicio/servicio.service';

@Component({
    selector: 'servicio-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => HServicioComboComponent),
            multi: true
        }
    ]
})

export class HServicioComboComponent extends ComboBoxComponent<HServiciosDto> implements OnInit {

    constructor(protected hServiceServicios: HServiciosService, injector: Injector) {
        super(hServiceServicios, injector);
    }
    @Input() mode: string = "";

    onSearch(): void {

    }

    protected GetFilter(): any {
        return new HServiciosFilter();
    }

    _linea: ItemDto;
    get linea(): ItemDto {
        return this._linea;
    }
    @Input()
    set linea(linea: ItemDto) {
        this._linea = linea;
        this.buscarServicios();
    }


    _fecha: Date;
    get fecha(): Date {
        return this._fecha;
    }
    @Input()
    set fecha(fecha: Date) {
        this._fecha = fecha;
        this.buscarServicios();
    }



    _CodHfecha: number;

    @Input()
    get CodHfecha(): number {
        return this._CodHfecha;
    }

    set CodHfecha(value: number) {
        this._CodHfecha = value;

        if (value) {
            this.buscarServicios();
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
            this.buscarServicios();
        }

    }

    buscarServicios() {

        if (this.mode == "sabana.servicio") {
            if (this.CodTdia && this.CodHfecha && this._linea) {
                var filtro = new HServiciosFilter();
                filtro.LineaId = this._linea.Id;
                //filtro.Fecha = moment(this.fecha).toISOString();
                filtro.CodTdia = this.CodTdia;
                filtro.CodHfecha = this.CodHfecha;
                this.hServiceServicios.RecuperarServiciosPorLinea(filtro).subscribe((response) => {

                    var self = this;
                    self.isLoading = false;
                    var _items = [];
                    if (response.Status === "Ok") {
                        response.DataObject.forEach(item => {
                            _items.push(new HServiciosDto(item));
                        });
                    };

                    this.items = _items;

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
        else {
            this.items = [];
            this.isLoading = false;
            setTimeout(() => {
                $(this.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        }


    }
}



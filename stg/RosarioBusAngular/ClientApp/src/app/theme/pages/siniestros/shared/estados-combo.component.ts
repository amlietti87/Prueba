import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoDanioService } from '../tipodedanio/tipodanio.service';
import { TipoDanioDto } from '../model/tipodanio.model';
import { TipoInvolucradoDto } from '../model/tipoinvolucrado.model';
import { TipoInvolucradoService } from '../tipoinvolucrado/tipoinvolucrado.service';
import { EstadosDto } from '../model/estados.model';
import { EstadosService } from '../estados/estados.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditEstadosModalComponent } from '../estados/create-or-edit-estados-modal.component';

@Component({
    selector: 'estados-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EstadosComboComponent),
            multi: true
        }
    ]
})
export class EstadosComboComponent extends ComboBoxComponent<EstadosDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: EstadosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    onSearch(): void {
        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        });
    }

    _EstadoId: number;

    @Input()
    get EstadoId(): number {

        return this._EstadoId;
    }

    set EstadoId(value: number) {
        this._EstadoId = value;
        if (value) {
            this.onSearch();
        }
    }

    _OrdenCambioEstado: number;

    @Input()
    get OrdenCambioEstado(): number {

        return this._OrdenCambioEstado;
    }

    set OrdenCambioEstado(value: number) {
        this._OrdenCambioEstado = value;
        if (value) {
            this.onSearch();
        }
    }

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            EstadoId: this.EstadoId,
            OrdenCambioEstado: this.OrdenCambioEstado
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<EstadosDto>> {
        return CreateOrEditEstadosModalComponent;
    }

    getNewDto(): EstadosDto {
        return new EstadosDto();
    }


}

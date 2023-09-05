import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { TiposAcuerdoDto } from '../model/tiposacuerdo.model';
import { TiposAcuerdoService } from '../tiposacuerdo/tiposacuerdo.service';
import { CreateOrEditTiposAcuerdoModalComponent } from '../tiposacuerdo/create-or-edit-tiposacuerdo-modal.component';

@Component({
    selector: 'tiposacuerdo-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TiposAcuerdoComboComponent),
            multi: true
        }
    ]
})
export class TiposAcuerdoComboComponent extends ComboBoxComponent<TiposAcuerdoDto> implements OnInit {


    Anulado: boolean = false;

    constructor(service: TiposAcuerdoService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _TipoAcuerdoId: number;

    @Input()
    get TipoAcuerdoId(): number {

        return this._TipoAcuerdoId;
    }

    set TipoAcuerdoId(value: number) {
        this._TipoAcuerdoId = value;
        var find = this.items.find(e => e.Id == value);
        if (!find) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            TipoAcuerdoId: this.TipoAcuerdoId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<TiposAcuerdoDto>> {
        return CreateOrEditTiposAcuerdoModalComponent;
    }

    getNewDto(): TiposAcuerdoDto {
        return new TiposAcuerdoDto();
    }


}

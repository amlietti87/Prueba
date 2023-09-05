import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { TipoDniDto, TipoDniFilter } from '../model/tipodni.model';
import { TipoDniService } from '../tipodni/tipodni.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditTipoDniModalComponent } from '../../admin/tipodni/create-or-edit-tipodni-modal.component';

@Component({
    selector: 'tipoDocId-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoDocIdComboComponent),
            multi: true
        }
    ]
})
export class TipoDocIdComboComponent extends ComboBoxComponent<TipoDniDto> implements OnInit {
    Anulado: boolean = false;

    constructor(service: TipoDniService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _TipoDocId: number;

    @Input()
    get TipoDocId(): number {

        return this._TipoDocId;
    }

    set TipoDocId(value: number) {
        this._TipoDocId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }

    getIDetailComponent(): ComponentType<DetailComponent<TipoDniDto>> {
        return CreateOrEditTipoDniModalComponent;
    }

    getNewDto(): TipoDniDto {
        return new TipoDniDto();
    }

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            SeguroId: this.TipoDocId
        };
        return f;
    }


}

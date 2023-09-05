import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { CausasReclamoDto } from '../model/causasreclamo.model';
import { CausasReclamoService } from '../causasreclamo/causasreclamo.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditCausasReclamoModalComponent } from '../causasreclamo/create-or-edit-causasreclamo-modal.component';

@Component({
    selector: 'causareclamo-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CausaReclamoComboComponent),
            multi: true
        }
    ]
})
export class CausaReclamoComboComponent extends ComboBoxComponent<CausasReclamoDto> implements OnInit {


    Anulado: boolean = false;

    constructor(service: CausasReclamoService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _CausaReclamoId: number;

    @Input()
    get CausaReclamoId(): number {

        return this._CausaReclamoId;
    }

    set CausaReclamoId(value: number) {
        this._CausaReclamoId = value;
        var find = this.items.find(e => e.Id == value);
        if (!find) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            CausaReclamoId: this.CausaReclamoId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<CausasReclamoDto>> {
        return CreateOrEditCausasReclamoModalComponent;
    }

    getNewDto(): CausasReclamoDto {
        return new CausasReclamoDto();
    }


}

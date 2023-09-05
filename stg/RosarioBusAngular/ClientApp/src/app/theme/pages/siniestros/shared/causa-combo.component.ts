import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { CausasService } from '../causas/causas.service';
import { CausasDto } from '../model/causas.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditCausasModalComponent } from '../causas/create-or-edit-causas-modal.component';

@Component({
    selector: 'causas-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CausaComboComponent),
            multi: true
        }
    ]
})
export class CausaComboComponent extends ComboBoxComponent<CausasDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: CausasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _causaid: number;
    @Input()
    get CausaId(): number {
        return this._causaid;
    }
    set CausaId(value: number) {
        this._causaid = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            CausaId: this.CausaId
        };
        return f;
    }
    getIDetailComponent(): ComponentType<DetailComponent<CausasDto>> {
        return CreateOrEditCausasModalComponent;
    }
    getNewDto(): CausasDto {
        return new CausasDto();
    }
}
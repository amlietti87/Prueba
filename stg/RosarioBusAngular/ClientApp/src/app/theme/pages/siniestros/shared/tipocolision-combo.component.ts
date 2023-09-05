import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay';
import { TipoColisionDto } from '../model/tipocolision.model';
import { TipoColisionService } from '../tipocolision/tipocolision.service';
import { CreateOrEditTipoColisionModalComponent } from '../tipocolision/create-or-edit-tipocolision-modal.component';

@Component({
    selector: 'tipocolision-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoColisionComboComponent),
            multi: true
        }
    ]
})
export class TipoColisionComboComponent extends ComboBoxComponent<TipoColisionDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: TipoColisionService, injector: Injector) {
        super(service, injector);

    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<TipoColisionDto>> {
        return CreateOrEditTipoColisionModalComponent;
    }

    getNewDto(): TipoColisionDto {
        return new TipoColisionDto();
    }
}

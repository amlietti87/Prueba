import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ConsecuenciasService } from '../consecuencias/consecuencias.service';
import { ConsecuenciasDto } from '../model/consecuencias.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditConsecuenciasModalComponent } from '../consecuencias/create-or-edit-consecuencias-modal.component';

@Component({
    selector: 'consecuencia-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ConsecuenciasComboComponent),
            multi: true
        }
    ]
})
export class ConsecuenciasComboComponent extends ComboBoxComponent<ConsecuenciasDto> implements OnInit {

    constructor(service: ConsecuenciasService, injector: Injector) {
        super(service, injector);
    }
    ngOnInit(): void {
        super.ngOnInit();
    }
    protected GetFilter(): any {
        var f = {
            FiltrarAnulado: true
        };
        return f;
    }
    getIDetailComponent(): ComponentType<DetailComponent<ConsecuenciasDto>> {
        return CreateOrEditConsecuenciasModalComponent;
    }
    getNewDto(): ConsecuenciasDto {
        return new ConsecuenciasDto();
    }
}
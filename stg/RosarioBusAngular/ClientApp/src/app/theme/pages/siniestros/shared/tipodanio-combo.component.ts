import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoDanioService } from '../tipodedanio/tipodanio.service';
import { TipoDanioDto } from '../model/tipodanio.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay';
import { CreateOrEditTipoDanioModalComponent } from '../tipodedanio/create-or-edit-tipodanio-modal.component';

@Component({
    selector: 'tipodanio-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoDanioComboComponent),
            multi: true
        }
    ]
})
export class TipoDanioComboComponent extends ComboBoxComponent<TipoDanioDto> implements OnInit {


    constructor(service: TipoDanioService, injector: Injector) {
        super(service, injector);

    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    getIDetailComponent(): ComponentType<DetailComponent<TipoDanioDto>> {
        return CreateOrEditTipoDanioModalComponent;
    }

    getNewDto(): TipoDanioDto {
        return new TipoDanioDto();
    }
}

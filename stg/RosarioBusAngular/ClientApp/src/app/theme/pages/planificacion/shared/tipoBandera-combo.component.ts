import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { TipoBanderaDto } from '../model/tipoBandera.model';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoBanderaService } from '../tipoBandera/tipoBandera.service';

@Component({
    selector: 'tipoBandera-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoBanderaComboComponent),
            multi: true
        }
    ]
})
export class TipoBanderaComboComponent extends ComboBoxComponent<TipoBanderaDto> implements OnInit {


    constructor(service: TipoBanderaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }



}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { BandaSiniestralDto } from '../model/bandasiniestral.model';
import { BandaSiniestralService } from '../bandasiniestral/bandasiniestral.service';

@Component({
    selector: 'bandasiniestral-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => BandaSiniestralComboComponent),
            multi: true
        }
    ]
})
export class BandaSiniestralComboComponent extends ComboBoxComponent<BandaSiniestralDto> implements OnInit {


    constructor(service: BandaSiniestralService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

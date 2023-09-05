import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { TipoBanderaDto } from '../model/tipoBandera.model';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';

import { BanderaTupService } from '../banderaTup/banderaTup.service';
import { BanderaTupDto } from '../model/banderaTup.model';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'bandera-tup-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => BanderaTupComboComponent),
            multi: true
        }
    ]
})
export class BanderaTupComboComponent extends ComboBoxAsync<BanderaTupDto> implements OnInit {


    constructor(service: BanderaTupService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

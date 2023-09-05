import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { ResponsableInformesDto } from '../model/responsableInformes.model';
import { ResponsableInformesService } from '../responsableInformes/ResponsableInformes.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'responsableInformes-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ResponsableInformesComboComponent),
            multi: true
        }
    ]
})
export class ResponsableInformesComboComponent extends ComboBoxAsync<ResponsableInformesDto> implements OnInit {


    constructor(service: ResponsableInformesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}
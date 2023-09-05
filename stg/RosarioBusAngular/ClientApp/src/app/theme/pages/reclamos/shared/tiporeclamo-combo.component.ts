import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TiposReclamoDto } from '../model/tiposreclamo.model';
import { TiposReclamoService } from '../tiposreclamo/tiposreclamo.service';

@Component({
    selector: 'tiporeclamo-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoReclamoComboComponent),
            multi: true
        }
    ]
})
export class TipoReclamoComboComponent extends ComboBoxComponent<TiposReclamoDto> implements OnInit {


    constructor(service: TiposReclamoService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(): any {

        var f = {

        };
        return f;
    }


}

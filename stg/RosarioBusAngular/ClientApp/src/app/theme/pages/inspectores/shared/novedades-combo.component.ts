import { NovedadesService } from './../novedades/novedades.service';
import { Component, OnInit, Injector, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { NovedadesDto } from '../model/Novedades.model';

@Component({
    selector: 'novedades-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => NovedadesComboComponent),
            multi: true
        }
    ]
})
export class NovedadesComboComponent extends ComboBoxComponent<NovedadesDto> implements OnInit {

    constructor(service: NovedadesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

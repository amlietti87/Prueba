import { Component, OnInit, Injector, forwardRef, ViewEncapsulation } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SucursalDto } from '../../planificacion/model/sucursal.model';
import { SucursalService } from '../../planificacion/sucursal/sucursal.service';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'sucursal-combo-denuncia',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SucursalComboDenunciaComponent),
            multi: true
        }
    ]
})
export class SucursalComboDenunciaComponent extends ComboBoxAsync<SucursalDto> implements OnInit {

    constructor(service: SucursalService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

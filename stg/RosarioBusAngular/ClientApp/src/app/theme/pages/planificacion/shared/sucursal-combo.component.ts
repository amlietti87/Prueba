import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { SucursalDto } from '../model/sucursal.model';
import { SucursalService } from '../sucursal/sucursal.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateAbogadosModalComponent } from '../../siniestros/siniestro/reclamos/create-abogados-modal.component';

@Component({
    selector: 'sucursal-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SucursalComboComponent),
            multi: true
        }
    ]
})
export class SucursalComboComponent extends ComboBoxComponent<SucursalDto> implements OnInit {



    constructor(service: SucursalService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

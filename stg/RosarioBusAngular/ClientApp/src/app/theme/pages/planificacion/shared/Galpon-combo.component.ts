import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateAbogadosModalComponent } from '../../siniestros/siniestro/reclamos/create-abogados-modal.component';
import { GalponDto } from '../model/subgalpon.model';
import { GalponService } from '../galpon/galpon.service';

@Component({
    selector: 'galpon-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => GalponComboComponent),
            multi: true
        }
    ]
})
export class GalponComboComponent extends ComboBoxComponent<GalponDto> implements OnInit {



    constructor(service: GalponService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

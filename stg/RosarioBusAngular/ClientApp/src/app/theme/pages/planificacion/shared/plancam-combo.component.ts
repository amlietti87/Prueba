import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { PlanCamDto } from '../model/subgalpon.model';
import { PlanCamService } from '../PlaCam/PlanCam.service';

@Component({
    selector: 'plancam-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PlanCamComboComponent),
            multi: true
        }
    ]
})
export class PlanCamComboComponent extends ComboBoxComponent<PlanCamDto> implements OnInit {



    constructor(service: PlanCamService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

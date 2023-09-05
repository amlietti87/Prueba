import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateAbogadosModalComponent } from '../../siniestros/siniestro/reclamos/create-abogados-modal.component';
import { GruposDto } from '../model/subgalpon.model';
import { GruposService } from '../grupos/grupo.service';
import { TipoViajeDto } from '../model/tipoviaje.model';
import { TipoViajeService } from '../../../../components/rbmaps/tipoviaje.service';

@Component({
    selector: 'tipoviaje-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoViajeComboComponent),
            multi: true
        }
    ]
})
export class TipoViajeComboComponent extends ComboBoxComponent<TipoViajeDto> implements OnInit {



    constructor(service: TipoViajeService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}

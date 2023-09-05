import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoDanioService } from '../tipodedanio/tipodanio.service';
import { TipoDanioDto } from '../model/tipodanio.model';
import { TipoInvolucradoDto } from '../model/tipoinvolucrado.model';
import { TipoInvolucradoService } from '../tipoinvolucrado/tipoinvolucrado.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditTipoInvolucradoModalComponent } from '../tipoinvolucrado/create-or-edit-tipoinvolucrado-modal.component';

@Component({
    selector: 'tipoinvolucrado-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoInvolucradoComboComponent),
            multi: true
        }
    ]
})
export class TipoInvolucradoComboComponent extends ComboBoxComponent<TipoInvolucradoDto> implements OnInit {


    constructor(service: TipoInvolucradoService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    getIDetailComponent(): ComponentType<DetailComponent<TipoInvolucradoDto>> {

        return CreateOrEditTipoInvolucradoModalComponent;
    }

    getNewDto(): TipoInvolucradoDto {
        return new TipoInvolucradoDto();
    }

}

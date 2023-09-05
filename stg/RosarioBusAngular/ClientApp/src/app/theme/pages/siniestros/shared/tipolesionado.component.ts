import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoLesionadoDto } from '../model/tipolesionado.model';
import { TipoLesionadoService } from '../tipolesionado/tipolesionado.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditTipoLesionadoModalComponent } from '../tipolesionado/create-or-edit-tipolesionado-modal.component';

@Component({
    selector: 'tipolesionado-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoLesionadoComboComponent),
            multi: true
        }
    ]
})
export class TipoLesionadoComboComponent extends ComboBoxComponent<TipoLesionadoDto> implements OnInit {

    createDefaultDetail: any;

    constructor(service: TipoLesionadoService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        ;
        super.ngOnInit();
    }

    getIDetailComponent(): ComponentType<DetailComponent<TipoLesionadoDto>> {
        return CreateOrEditTipoLesionadoModalComponent;
    }

    getNewDto(): TipoLesionadoDto {
        return new TipoLesionadoDto();
    }


}

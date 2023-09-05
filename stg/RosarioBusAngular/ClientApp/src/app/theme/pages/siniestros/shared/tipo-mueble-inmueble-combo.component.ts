import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoMuebleDto } from '../model/tipomueble.model';
import { TipoMuebleService } from '../tipoMuebleInmueble/tipomueble.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditTipoMuebleModalComponent } from '../tipoMuebleInmueble/create-or-edit-tipomueble-modal.component';


@Component({
    selector: 'tipo-mueble-inmueble-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoMuebleInmuebleComboComponent),
            multi: true
        }
    ]
})
export class TipoMuebleInmuebleComboComponent extends ComboBoxComponent<TipoMuebleDto> implements OnInit {


    constructor(service: TipoMuebleService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
    getIDetailComponent(): ComponentType<DetailComponent<TipoMuebleDto>> {
        return CreateOrEditTipoMuebleModalComponent;
    }
    onSearch(): void {
        super.onSearch();
    }
    getNewDto(): TipoMuebleDto {
        return new TipoMuebleDto();
    }
}

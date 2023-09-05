import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoElementoDto } from '../model/tipoelemento.model';
import { TipoElementoService } from '../tipoelemento/tipoelemento.service';
import { ComponentType } from '@angular/cdk/overlay/index';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditTipoElementosModalComponent } from '../tipoelemento/create-or-edit-tipoelementos-modal.component';

@Component({
    selector: 'tipoelementos-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoElementoComboComponent),
            multi: true
        }
    ]
})
export class TipoElementoComboComponent extends ComboBoxComponent<TipoElementoDto> implements OnInit {


    constructor(service: TipoElementoService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _TipoElementoId: number;

    @Input()
    get TipoElementoId(): number {

        return this._TipoElementoId;
    }

    set TipoElementoId(value: number) {
        this._TipoElementoId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<TipoElementoDto>> {
        return CreateOrEditTipoElementosModalComponent;
    }

    getNewDto(): TipoElementoDto {
        return new TipoElementoDto();
    }
}

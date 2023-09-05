import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { PrestadoresMedicosDto } from '../model/prestadoresmedicos.model';
import { PrestadoresMedicosService } from '../prestadores/prestadores.service';
import { ComponentType } from '@angular/cdk/overlay/index';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditPrestadoresMedicosModalComponent } from '../prestadores/create-or-edit-prestadores-modal.component';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';


@Component({
    selector: 'prestadores-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PrestadoresMedicosComboComponent),
            multi: true
        }
    ]
})
export class PrestadoresMedicosComboComponent extends ComboBoxAsync<PrestadoresMedicosDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: PrestadoresMedicosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _PrestadorMedicoId: number;

    @Input()
    get PrestadorMedicoId(): number {

        return this._PrestadorMedicoId;
    }

    set PrestadorMedicoId(value: number) {
        this._PrestadorMedicoId = value;
        if (value) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            PrestadorMedicoId: this.PrestadorMedicoId
        };
        return f;
    }


    getIDetailComponent(): ComponentType<DetailComponent<PrestadoresMedicosDto>> {
        return CreateOrEditPrestadoresMedicosModalComponent;
    }

    getNewDto(): PrestadoresMedicosDto {
        return new PrestadoresMedicosDto();
    }
}

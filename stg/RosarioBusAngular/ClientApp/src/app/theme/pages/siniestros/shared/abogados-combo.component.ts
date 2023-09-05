import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { BandaSiniestralDto } from '../model/bandasiniestral.model';
import { BandaSiniestralService } from '../bandasiniestral/bandasiniestral.service';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { CiaSegurosService } from '../seguros/seguros.service';
import { CiaSegurosDto } from '../model/seguros.model';
import { AbogadosDto } from '../model/abogados.model';
import { AbogadosService } from '../abogados/abogados.service';
import { CreateAbogadosModalComponent } from '../siniestro/reclamos/create-abogados-modal.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';

@Component({
    selector: 'abogados-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => AbogadosComboComponent),
            multi: true
        }
    ]
})
export class AbogadosComboComponent extends ComboBoxComponent<AbogadosDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: AbogadosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _AbogadoId: number;

    @Input()
    get AbogadoId(): number {

        return this._AbogadoId;
    }

    set AbogadoId(value: number) {
        this._AbogadoId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            AbogadoId: this.AbogadoId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<AbogadosDto>> {
        return CreateAbogadosModalComponent;
    }
    getNewDto(): AbogadosDto {
        return new AbogadosDto();
    }
}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoDanioService } from '../tipodedanio/tipodanio.service';
import { TipoDanioDto } from '../model/tipodanio.model';
import { TipoInvolucradoDto } from '../model/tipoinvolucrado.model';
import { TipoInvolucradoService } from '../tipoinvolucrado/tipoinvolucrado.service';
import { EstadosDto } from '../model/estados.model';
import { EstadosService } from '../estados/estados.service';
import { InvolucradosDto } from '../model/involucrados.model';
import { InvolucradosService } from '../involucrados/involucrados.service';
import { JuzgadosDto } from '../model/juzgados.model';
import { JuzgadosService } from '../juzgados/juzgados.service';
import { CreateOrEditJuzgadosModalComponent } from '../juzgados/create-or-edit-juzgados-modal.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { DetailComponent } from '../../../../shared/manager/detail.component';

@Component({
    selector: 'juzgados-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => JuzgadosComboComponent),
            multi: true
        }
    ]
})
export class JuzgadosComboComponent extends ComboBoxComponent<JuzgadosDto> implements OnInit {


    constructor(service: JuzgadosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _JuzgadoId: number;

    @Input()
    get JuzgadoId(): number {

        return this._JuzgadoId;
    }

    set JuzgadoId(value: number) {
        this._JuzgadoId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }

    protected GetFilter(): any {
        var f = {
            JuzgadoId: this.JuzgadoId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<JuzgadosDto>> {
        return CreateOrEditJuzgadosModalComponent;
    }

    getNewDto(): JuzgadosDto {
        return new JuzgadosDto();
    }

}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ContingenciasDto } from '../model/contingencias.model';
import { ContingenciasService } from '../contingencias/contingencias.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditContingenciasModalComponent } from '../contingencias/create-or-edit-contingencias-modal.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'contingencias-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ContingenciasComboComponent),
            multi: true
        }
    ]
})
export class ContingenciasComboComponent extends ComboBoxAsync<ContingenciasDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: ContingenciasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _ContingenciaId: number;

    @Input()
    get ContingenciaId(): number {

        return this._ContingenciaId;
    }

    set ContingenciaId(value: number) {
        this._ContingenciaId = value;
        if (value) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            ContingenciaId: this.ContingenciaId
        };
        return f;
    }


    getIDetailComponent(): ComponentType<DetailComponent<ContingenciasDto>> {
        return CreateOrEditContingenciasModalComponent;
    }

    getNewDto(): ContingenciasDto {
        return new ContingenciasDto();
    }
}

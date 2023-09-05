import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { BandaSiniestralDto } from '../model/bandasiniestral.model';
import { BandaSiniestralService } from '../bandasiniestral/bandasiniestral.service';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { FactoresIntervinientesService } from '../factores-intervinientes/factoresintervinientes.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditFactoresIntervinientesModalComponent } from '../factores-intervinientes/create-or-edit-factoresintervinientes-modal.component';

@Component({
    selector: 'factores-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FactoresComboComponent),
            multi: true
        }
    ]
})
export class FactoresComboComponent extends ComboBoxComponent<FactoresIntervinientesDto> implements OnInit {


    constructor(service: FactoresIntervinientesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    Anulado: boolean = false;

    _factoresId: number;

    @Input()
    get FactoresId(): number {

        return this._factoresId;
    }

    set FactoresId(value: number) {
        this._factoresId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }

    }


    onSearch(): void {
        super.onSearch();
    }

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            FactoresId: this.FactoresId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<FactoresIntervinientesDto>> {
        return CreateOrEditFactoresIntervinientesModalComponent;
    }

    getNewDto(): FactoresIntervinientesDto {
        return new FactoresIntervinientesDto();
    }
}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { BandaSiniestralDto } from '../model/bandasiniestral.model';
import { BandaSiniestralService } from '../bandasiniestral/bandasiniestral.service';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { CiaSegurosService } from '../seguros/seguros.service';
import { CiaSegurosDto } from '../model/seguros.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditSegurosModalComponent } from '../seguros/create-or-edit-seguros-modal.component';

@Component({
    selector: 'seguros-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SegurosComboComponent),
            multi: true
        }
    ]
})
export class SegurosComboComponent extends ComboBoxComponent<CiaSegurosDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: CiaSegurosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _SeguroId: number;

    @Input()
    get SeguroId(): number {

        return this._SeguroId;
    }

    set SeguroId(value: number) {
        this._SeguroId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            SeguroId: this.SeguroId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<CiaSegurosDto>> {
        return CreateOrEditSegurosModalComponent;
    }

    getNewDto(): CiaSegurosDto {
        return new CiaSegurosDto();
    }
}

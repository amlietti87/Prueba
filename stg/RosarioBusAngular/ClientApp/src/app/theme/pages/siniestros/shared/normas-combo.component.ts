import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ConductasNormasDto } from '../model/normas.model';
import { ConductasNormasService } from '../normas/normas.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditConductasNormasModalComponent } from '../normas/create-or-edit-normas-modal.component';

@Component({
    selector: 'normas-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => NormasComboComponent),
            multi: true
        }
    ]
})
export class NormasComboComponent extends ComboBoxComponent<ConductasNormasDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: ConductasNormasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _ConductaId: number;

    @Input()
    get ConductaId(): number {

        return this._ConductaId;
    }

    set ConductaId(value: number) {
        this._ConductaId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            ConductaId: this.ConductaId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<ConductasNormasDto>> {
        return CreateOrEditConductasNormasModalComponent;
    }

    getNewDto(): ConductasNormasDto {
        return new ConductasNormasDto();
    }
}

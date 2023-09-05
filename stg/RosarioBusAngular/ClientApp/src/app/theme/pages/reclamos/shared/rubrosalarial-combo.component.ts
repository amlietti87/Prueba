import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { RubrosSalarialesDto } from '../model/rubrossalariales.model';
import { RubrosSalarialesService } from '../rubrossalariales/rubrossalariales.service';
import { CreateOrEditRubrosSalarialesModalComponent } from '../rubrossalariales/create-or-edit-rubrossalariales-modal.component';

@Component({
    selector: 'rubrosalarial-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RubroSalarialComboComponent),
            multi: true
        }
    ]
})
export class RubroSalarialComboComponent extends ComboBoxComponent<RubrosSalarialesDto> implements OnInit {


    Anulado: boolean = false;

    constructor(service: RubrosSalarialesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _RubroSalarialId: number;

    @Input()
    get RubroSalarialId(): number {

        return this._RubroSalarialId;
    }

    set RubroSalarialId(value: number) {
        this._RubroSalarialId = value;
        var find = this.items.find(e => e.Id == value);
        if (!find) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            RubroSalarialId: this.RubroSalarialId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<RubrosSalarialesDto>> {
        return CreateOrEditRubrosSalarialesModalComponent;
    }

    getNewDto(): RubrosSalarialesDto {
        return new RubrosSalarialesDto();
    }


}

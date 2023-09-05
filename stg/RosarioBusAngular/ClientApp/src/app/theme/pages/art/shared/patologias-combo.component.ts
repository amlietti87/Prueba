import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { PatologiasDto } from '../model/patologias.model';
import { PatologiasService } from '../patologias/patologias.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditPatologiasModalComponent } from '../patologias/create-or-edit-patologias-modal.component';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'patologias-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PatologiasComboComponent),
            multi: true
        }
    ]
})
export class PatologiasComboComponent extends ComboBoxAsync<PatologiasDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: PatologiasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _PatologiaId: number;

    @Input()
    get PatologiaId(): number {

        return this._PatologiaId;
    }

    set PatologiaId(value: number) {
        this._PatologiaId = value;
        if (value) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            PatologiaId: this.PatologiaId
        };
        return f;
    }


    getIDetailComponent(): ComponentType<DetailComponent<PatologiasDto>> {
        return CreateOrEditPatologiasModalComponent;
    }

    getNewDto(): PatologiasDto {
        return new PatologiasDto();
    }
}

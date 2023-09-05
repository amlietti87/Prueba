import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { EmpleadosService } from '../empleados/empleados.service';
import { EmpleadosDto } from '../model/empleado.model';
import { PracticantesService } from '../practicantes/practicantes.service';
import { PracticantesDto } from '../model/practicantes.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditPracticantesModalComponent } from '../practicantes/create-or-edit-practicantes-modal.component';

@Component({
    selector: 'practicante-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PracticantesAutoCompleteComponent),
            multi: true
        }
    ]
})
export class PracticantesAutoCompleteComponent extends AutoCompleteComponent<PracticantesDto> implements OnInit {


    Anulado: boolean = false;

    constructor(service: PracticantesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(query: any): any {
        var f = {
            FilterText: query,
            Anulado: this.Anulado
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<PracticantesDto>> {
        return CreateOrEditPracticantesModalComponent;
    }

    getNewDto(): PracticantesDto {
        return new PracticantesDto();
    }

}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { LocalidadesDto } from '../model/localidad.model';
import { LocalidadesService } from '../localidades/localidad.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditLocalidadesModalComponent } from '../../admin/localidades/create-or-edit-localidades-modal.component';


@Component({
    selector: 'localidad-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => LocalidadesAutoCompleteComponent),
            multi: true
        }
    ]
})
export class LocalidadesAutoCompleteComponent extends AutoCompleteComponent<LocalidadesDto> implements OnInit {

    constructor(service: LocalidadesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(query: any): any {
        var f = {
            FilterText: query
        };

        return f;
    }


    getIDetailComponent(): ComponentType<DetailComponent<LocalidadesDto>> {
        return CreateOrEditLocalidadesModalComponent;
    }

    getNewDto(): LocalidadesDto {
        return new LocalidadesDto();
    }

}

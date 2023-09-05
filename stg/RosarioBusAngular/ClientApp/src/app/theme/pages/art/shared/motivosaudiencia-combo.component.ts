import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { MotivosAudienciasDto } from '../model/motivosaudencias.model';
import { MotivosAudienciasService } from '../motivosaudiencias/motivosaudiencias.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditMotivosAudienciasModalComponent } from '../motivosaudiencias/create-or-edit-motivosaudiencias-modal.component';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';


@Component({
    selector: 'motivosaudiencia-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => MotivosAudienciaComboComponent),
            multi: true
        }
    ]
})
export class MotivosAudienciaComboComponent extends ComboBoxAsync<MotivosAudienciasDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: MotivosAudienciasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _MotivoAudienciaId: number;

    @Input()
    get MotivoAudienciaId(): number {

        return this._MotivoAudienciaId;
    }

    set MotivoAudienciaId(value: number) {
        this._MotivoAudienciaId = value;
        if (value) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            MotivoAudienciaId: this.MotivoAudienciaId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<MotivosAudienciasDto>> {
        return CreateOrEditMotivosAudienciasModalComponent;
    }

    getNewDto(): MotivosAudienciasDto {
        return new MotivosAudienciasDto();
    }
}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { MotivosNotificacionesDto } from '../model/motivosnotificaciones.model';
import { MotivosNotificacionesService } from '../motivosnotificaciones/motivosnotificaciones.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditMotivosNotificacionesModalComponent } from '../motivosnotificaciones/create-or-edit-motivosnotificaciones-modal.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'motivosnotificacion-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => MotivosNotificacionComboComponent),
            multi: true
        }
    ]
})
export class MotivosNotificacionComboComponent extends ComboBoxAsync<MotivosNotificacionesDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: MotivosNotificacionesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _MotivoNotificacionId: number;

    @Input()
    get MotivoNotificacionId(): number {
        return this._MotivoNotificacionId;
    }

    set MotivoNotificacionId(value: number) {
        this._MotivoNotificacionId = value;
        if (value) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            MotivoNotificacionId: this.MotivoNotificacionId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<MotivosNotificacionesDto>> {
        return CreateOrEditMotivosNotificacionesModalComponent;
    }

    getNewDto(): MotivosNotificacionesDto {
        return new MotivosNotificacionesDto();
    }
}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { CreateOrEditGruposInspectoresModalComponent } from '../gruposinspectores/create-or-edit-gruposinspectores-modal.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { EstadosDiagramaInspectoresService } from '../estadosdiagramainspectores/estadosdiagramainspectores.service';
import { EstadosDiagramaInspectoresDto } from '../model/estadosdiagramainspectores.model';


@Component({
    selector: 'estadosdiagramas-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EstadosDiagramaInspectoresComboComponent),
            multi: true
        }
    ]
})
export class EstadosDiagramaInspectoresComboComponent extends ComboBoxComponent<EstadosDiagramaInspectoresDto> implements OnInit {


    constructor(service: EstadosDiagramaInspectoresService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    private _EsBorrador: number;
    get EsBorrado(): number {
        return this._EsBorrador;
    }

    @Input()
    set EsBorrador(borrador: number) {
        this._EsBorrador = borrador;
        if (borrador) {
            this.onSearch();
        }
    }

    protected GetFilter(): any {
        var f = { EsBorrador: this.EsBorrador };

        return f;
    }

}

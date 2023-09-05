import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { PersTurnosService } from '../turnos/persturnos.service';
import { PersTurnosDto } from '../model/persturnos.model';


@Component({
    selector: 'persturnos-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PersTurnosComboComponent),
            multi: true
        }
    ]
})
export class PersTurnosComboComponent extends ComboBoxComponent<PersTurnosDto> implements OnInit {


    constructor(service: PersTurnosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    @Input() filtrarPorGrupos: boolean = false;

    private _GrupoInspectorId: number;
    get GrupoInspectorId(): number {
        return this._GrupoInspectorId;
    }

    @Input()
    set GrupoInspectorId(grupoInspectorId: number) {
        this._GrupoInspectorId = grupoInspectorId;
        if (grupoInspectorId) {
            this.onSearch();
        }
    }

    protected GetFilter(): any {

        var f = { GrupoInspectorId: null };

        if (this.filtrarPorGrupos) {
            f.GrupoInspectorId = this.GrupoInspectorId || 0;
        }

        return f;
    }
}

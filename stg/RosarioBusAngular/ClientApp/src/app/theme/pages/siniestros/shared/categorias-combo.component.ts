import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { CategoriasDto } from '../model/consecuencias.model';
import { CategoriasService } from '../consecuencias/categorias.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';

@Component({
    selector: 'categorias-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CategoriasComboComponent),
            multi: true
        }
    ]
})
export class CategoriasComboComponent extends ComboBoxComponent<CategoriasDto> implements OnInit {

    _ConsecuenciaId: number;
    @Input()
    get ConsecuenciaId(): number {
        return this._ConsecuenciaId;
    }
    set ConsecuenciaId(value: number) {
        this._ConsecuenciaId = value;
        if (this._ConsecuenciaId) {
            super.onSearch();
        }
    }


    constructor(service: CategoriasService, injector: Injector) {
        super(service, injector);
    }
    protected GetFilter(): any {
        var f = {
            ConsecuenciaId: this.ConsecuenciaId,
            Anulado: false
        };
        return f;
    }
}
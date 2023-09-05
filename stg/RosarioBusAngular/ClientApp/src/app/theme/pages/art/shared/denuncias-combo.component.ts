import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { DenunciasDto } from '../../art/model/denuncias.model';
import { DenunciasService } from '../../art/denuncias/denuncias.service';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'denuncias-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DenunciasComboComponent),
            multi: true
        }
    ]
})
export class DenunciasComboComponent extends ComboBoxAsync<DenunciasDto> implements OnInit {

    Anulado: number = 2;

    constructor(service: DenunciasService, injector: Injector) {
        super(service, injector);
    }

    @Input() ReclamosSearch: boolean = false;

    ngOnInit(): void {
        super.ngOnInit();
    }

    onSearch(): void {
        if (this.ReclamosSearch) {
            if (this.Empleado && this.Empleado != null) {
                this.isLoading = true;
                super.onSearch();
            }
        }
        else {
            if (this.Empleado && this.Empleado != null && this.PrestadorMedicoId && this.PrestadorMedicoId != null) {
                this.isLoading = true;
                super.onSearch();
            }
        }
    }

    _Empleado: any;
    @Input()
    get Empleado(): any {
        return this._Empleado;
    }
    set Empleado(value: any) {
        this._Empleado = value;
        if (this._Empleado != null) {
            this.onSearch();
        }
    }

    _Id: number;
    @Input()
    get Id(): number {
        return this._Id;
    }
    set Id(value: number) {
        this._Id = value;
    }


    _PrestadorMedicoId: number;

    @Input()
    get PrestadorMedicoId(): number {

        return this._PrestadorMedicoId;
    }

    set PrestadorMedicoId(value: number) {
        this._PrestadorMedicoId = value;
        if (this._PrestadorMedicoId != null) {
            this.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            EmpleadoId: this.Empleado.Id,
            NotId: this.Id,
            PrestadorMedicoId: this.PrestadorMedicoId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<DenunciasDto>> {
        return null;
    }

    getNewDto(): DenunciasDto {
        return new DenunciasDto();
    }
}

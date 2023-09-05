import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { BandaSiniestralDto } from '../model/bandasiniestral.model';
import { BandaSiniestralService } from '../bandasiniestral/bandasiniestral.service';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { FactoresIntervinientesService } from '../factores-intervinientes/factoresintervinientes.service';
import { SancionSugeridaDto } from '../model/sancionsugerida.model';
import { SancionSugeridaService } from '../sancionsugerida/sancion.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditSancionModalComponent } from '../sancionsugerida/create-or-edit-sancion-modal.component';

@Component({
    selector: 'sancion-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SancionComboComponent),
            multi: true
        }
    ]
})
export class SancionComboComponent extends ComboBoxComponent<SancionSugeridaDto> implements OnInit {


    constructor(service: SancionSugeridaService, injector: Injector) {
        super(service, injector);

    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    Anulado: boolean = false;

    _sancionId: number;

    @Input()
    get SancionId(): number {

        return this._sancionId;
    }

    set SancionId(value: number) {
        this._sancionId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }

    }


    onSearch(): void {
        super.onSearch();
    }

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            SancionId: this.SancionId
        };
        return f;
    }


    getIDetailComponent(): ComponentType<DetailComponent<SancionSugeridaDto>> {
        return CreateOrEditSancionModalComponent;
    }

    getNewDto(): SancionSugeridaDto {
        return new SancionSugeridaDto();
    }

}

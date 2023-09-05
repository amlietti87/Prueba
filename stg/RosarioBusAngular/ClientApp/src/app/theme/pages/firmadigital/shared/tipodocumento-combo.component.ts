import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { FDTiposDocumentosDto } from '../model/fdtiposdocumentos.model';
import { FDTiposDocumentosService } from '../services/fdtiposdocumentos.service';

@Component({
    selector: 'fdtiposdocumentos-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FDTipoDocumentoComboComponent),
            multi: true
        }
    ]
})
export class FDTipoDocumentoComboComponent extends ComboBoxComponent<FDTiposDocumentosDto> implements OnInit {


    constructor(service: FDTiposDocumentosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    Anulado: boolean = false;

    _TipoDocumentoId: number;

    @Input()
    get TipoDocumentoId(): number {

        return this._TipoDocumentoId;
    }

    set TipoDocumentoId(value: number) {
        this._TipoDocumentoId = value;
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
            TipoDocumentoId: this.TipoDocumentoId
        };
        return f;
    }

    //getIDetailComponent(): ComponentType<DetailComponent<FactoresIntervinientesDto>> {
    //    return CreateOrEditFactoresIntervinientesModalComponent;
    //}

    //getNewDto(): FactoresIntervinientesDto {
    //    return new FactoresIntervinientesDto();
    //}
}

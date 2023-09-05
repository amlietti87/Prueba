import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChange, SimpleChanges } from '@angular/core';


import { TipoParadaDto } from '../model/tipoParada.model';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { TipoParadaService } from '../tipoParada/tipoparada.service';
import { RamalColorDto } from '../model/ramalcolor.model';
import { RamalColorService } from '../ramalcolor/ramalcolor.service';
import { GroupItemDto } from '../../../../shared/model/base.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditRamaColorModalComponent } from '../ramalcolor/create-or-edit-ramalcolor-modal.component';
import { ComponentType } from '@angular/cdk/overlay/index';

@Component({
    selector: 'ramalcolor-combocomun',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RamalColorComboComunComponent),
            multi: true
        }
    ]
})
export class RamalColorComboComunComponent extends ComboBoxComponent<RamalColorDto> implements OnInit {

    constructor(service: RamalColorService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    Anulado: boolean = false;

    _lineaId: number;

    @Input()
    get LineaId(): number {

        return this._lineaId;
    }

    set LineaId(value: number) {
        this._lineaId = value;
        debugger;
        super.onSearch()

    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            LineaId: this.LineaId
        };
        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<RamalColorDto>> {
        return CreateOrEditRamaColorModalComponent;
    }

    getNewDto(): RamalColorDto {
        return new RamalColorDto();
    }



}

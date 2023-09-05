import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChange, SimpleChanges } from '@angular/core';


import { TipoParadaDto } from '../model/tipoParada.model';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { TipoParadaService } from '../tipoParada/tipoparada.service';

@Component({
    selector: 'tipoParada-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoParadaComboComponent),
            multi: true
        }
    ]
})
export class TipoParadaComboComponent extends ComboBoxComponent<TipoParadaDto> implements OnInit, OnChanges {


    @Input() ItemsModel: TipoParadaDto[];

    constructor(service: TipoParadaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    onSearch(): void {

    }

    ngOnChanges(changes: SimpleChanges) {

        const itemmodel: SimpleChange = changes.ItemsModel;
        var self = this;

        if (itemmodel && itemmodel.previousValue != itemmodel.currentValue) {

            this.items = this.ItemsModel;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 0);
        }
    }



}

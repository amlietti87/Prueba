import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChange, SimpleChanges } from '@angular/core';


import { TipoParadaDto } from '../model/tipoParada.model';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { TipoParadaService } from '../tipoParada/tipoparada.service';
import { RamalColorDto } from '../model/ramalcolor.model';
import { RamalColorService } from '../ramalcolor/ramalcolor.service';
import { GroupItemDto } from '../../../../shared/model/base.model';

@Component({
    selector: 'ramalcolor-combo',
    templateUrl: '../../../../shared/components/comboGroupBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RamalColorComboComponent),
            multi: true
        }
    ]
})
export class RamalColorComboComponent extends ComboBoxComponent<RamalColorDto> implements OnInit, OnChanges {


    @Input() ItemsModel: GroupItemDto[];

    constructor(service: RamalColorService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    onSearch(): void {
        var self = this;
        setTimeout(() => {
            $(self.comboboxElement.nativeElement).selectpicker('refresh');
        }, 0);

    }

    ngOnChanges(changes: SimpleChanges) {

        const itemmodel: SimpleChange = changes.ItemsModel;
        var self = this;

        if (itemmodel && itemmodel.previousValue != itemmodel.currentValue) {

            //this.items = this.ItemsModel;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 0);
        }
    }



}

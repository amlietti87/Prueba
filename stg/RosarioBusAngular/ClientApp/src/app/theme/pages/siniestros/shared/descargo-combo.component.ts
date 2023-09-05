import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { TipoDniDto } from '../model/tipodni.model';
import { TipoDniService } from '../tipodni/tipodni.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent, ComboBoxBaseComponent } from '../../../../shared/components/comboBase.component';
import { ItemDto } from '../../../../shared/model/base.model';
import { SiniestroService } from '../siniestro/siniestro.service';
import { DescargoDto } from '../model/descargo.model';

@Component({
    selector: 'descargo-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DescargoComboComponent),
            multi: true
        }
    ]
})


export class DescargoComboComponent extends ComboBoxComponent<DescargoDto> implements OnInit {



    constructor(protected service: SiniestroService,
        protected injector: Injector) {

        super(service, injector);
        var opc1 = new DescargoDto();
        opc1.Id = 2;
        opc1.Description = 'No';
        var opc2 = new DescargoDto();
        opc2.Id = 1;
        opc2.Description = 'Si';

        this.livesearch = false;


        this.items.push(opc1);
        this.items.push(opc2);
    }

    ngOnInit(): void {
        this.onSearch();
    }



    onSearch(): void {
        var self = this;
        self.isLoading = false;
    }
}

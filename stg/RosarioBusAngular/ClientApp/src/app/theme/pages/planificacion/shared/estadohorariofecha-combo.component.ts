import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChange, SimpleChanges } from '@angular/core';




import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { EstadoHorarioFechaDto } from '../model/estadohorariofecha.model';
import { EstadoHorarioFechaService } from '../estadohorariofecha/estadohorariofecha.service';


@Component({
    selector: 'estadohorariofecha-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EstadoHorarioFechaComboComponent),
            multi: true
        }
    ]
})
export class EstadoHorarioFechaComboComponent extends ComboBoxComponent<EstadoHorarioFechaDto> implements OnInit, OnChanges {




    constructor(service: EstadoHorarioFechaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    ngOnChanges(changes: SimpleChanges) {


    }

}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { GrupoLineasDto } from '../model/grupolineas.model';
import { GrupoLineasService } from '../grupolineas/grupolineas.service';


@Component({
    selector: 'grupolineas-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => GrupoLineasComboComponent),
            multi: true
        }
    ]
})
export class GrupoLineasComboComponent extends ComboBoxComponent<GrupoLineasDto> implements OnChanges, OnInit {


    @Input() name: string;
    private _name: string;

    constructor(service: GrupoLineasService, injector: Injector) {
        super(service, injector);
    }


    private _SucursalId: number;
    get SucursalId(): number {
        return this._SucursalId;
    }

    @Input()
    set SucursalId(SucursalId: number) {
        this._SucursalId = SucursalId;
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    ngOnChanges(changes: SimpleChanges) {

        const SucursalId: SimpleChange = changes.SucursalId;

        if (SucursalId && SucursalId.previousValue != SucursalId.currentValue) {
            this.onSearch();
        }
    }


    onSearch(): void {
        var self = this;
        this.isLoading = true;

        var filter = {
            SucursalId: this.SucursalId
        }
        this.service.requestAllByFilter(filter).subscribe(result => {
            this.items = result.DataObject.Items;
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 0);
        });
    }

}

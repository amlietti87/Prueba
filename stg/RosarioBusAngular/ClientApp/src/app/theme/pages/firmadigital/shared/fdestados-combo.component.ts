import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { FDEstadosDto } from '../model/fdestados.model';
import { FDEstadosService } from '../services/fdestados.service';

@Component({
    selector: 'fdestados-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FDEstadosComboComponent),
            multi: true
        }
    ]
})
export class FDEstadosComboComponent extends ComboBoxComponent<FDEstadosDto> implements OnInit {


    constructor(service: FDEstadosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _mostrarBDEmpleado: boolean;
    predeterminedState: number;

    @Input()
    get MostrarBDEmpleado(): boolean {
        return this._mostrarBDEmpleado;
    }
    set MostrarBDEmpleado(val: boolean) {
        this._mostrarBDEmpleado = val;
        if (val != null) {
            this.onSearch();
        }
    }

    async setPredeterminedState() {
        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(e => e.VpDBDEmpleado == true).finally(() => {
            this.predeterminedState = self.predeterminedState;
            setTimeout(() => {
                self.isLoading = false;
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 3000);
        })
            .subscribe(result => {
                var Item = result.DataObject.Items.find(e => e.VpDBDEmpleado == true);
                setTimeout(() => {
                    self.predeterminedState = Item.Id;
                }, 50);
            });
    }

    onSearch(): void {
        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            self.isLoading = false;
            if (this.MostrarBDEmpleado == true) {
                var find = this.items.find(e => e.VpDBDEmpleado == true);
                if (find != null) {
                    this.writeValue(find.Id);
                }
            }
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        });
    }

    protected GetFilter(): any {
        var f = {
            MostrarBDEmpleado: this.MostrarBDEmpleado
        };
        return f;
    }


}

import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { DenunciasEstadosService } from '../estados/estados.service';
import { DenunciasEstadosDto } from '../model/denunciasestados.model';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditDenunciasEstadosModalComponent } from '../estados/create-or-edit-estados-modal.component';
import { EstadosDto } from '../model/estados.model';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';


@Component({
    selector: 'denunciaestados-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EstadosDenunciaComboComponent),
            multi: true
        }
    ]
})
export class EstadosDenunciaComboComponent extends ComboBoxAsync<EstadosDto> implements OnInit {

    Anulado: boolean = false;
    constructor(service: DenunciasEstadosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _EstadoId: number;

    @Input()
    get EstadoId(): number {

        return this._EstadoId;
    }

    set EstadoId(value: number) {
        this._EstadoId = value;
        /*
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
        */
        if (value) {
            super.onSearch();
        }
    }

    /*
    onSearch(): void {

        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            if (this.items) {
                if (!this.value) {
                    var defaultItem = this.items.filter(e => e.Predeterminado);
                    if (defaultItem && defaultItem.length > 0) {
                        //this.value = defaultItem[0].Id; 
                        //this.writeValue(defaultItem[0].Id);
                    }
                }
            }
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        });

    }
    */

    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            EstadoId: this.EstadoId
        };
        return f;
    }
    getIDetailComponent(): ComponentType<DetailComponent<EstadosDto>> {
        return CreateOrEditDenunciasEstadosModalComponent;
    }

    getNewDto(): EstadosDto {
        return new EstadosDto();
    }

}

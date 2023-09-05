import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { PatologiasDto } from '../model/patologias.model';
import { PatologiasService } from '../patologias/patologias.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditPatologiasModalComponent } from '../patologias/create-or-edit-patologias-modal.component';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';
import { SiniestrosDto } from '../../siniestros/model/siniestro.model';
import { SiniestroService } from '../../siniestros/siniestro/siniestro.service';
import { ItemDto } from '../../../../shared/model/base.model';

@Component({
    selector: 'siniestro-combobyempleado',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SiniestroEmpleadoComboComponent),
            multi: true
        }
    ]
})
export class SiniestroEmpleadoComboComponent extends ComboBoxAsync<SiniestrosDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: SiniestroService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _PatologiaId: number;

    _SiniestroId: number;
    _SucursalId: number;
    _selectEmpleados: ItemDto;

    @Input()
    get SiniestroId(): number {

        return this._SiniestroId;
    }

    set SiniestroId(value: number) {
        this._SiniestroId = value;
        if (value) {
            this.onSearch();
        }
    }

    @Input() ReclamosSearch: boolean = false;

    @Input()
    get SucursalId(): number {

        return this._SucursalId;
    }

    set SucursalId(value: number) {
        this._SucursalId = value;
        if (value) {
            this.onSearch();
        }
    }

    @Input()
    get selectEmpleados(): ItemDto {

        return this._selectEmpleados;
    }

    set selectEmpleados(value: ItemDto) {
        this._selectEmpleados = value;
        if (value) {
            this.ConductorId = value.Id;
            this.onSearch();
        }
    }

    ConductorId: number = null;

    protected GetFilter(): any {
        var f = {
            SiniestroId: this.SiniestroId,
            ConductorId: this.ConductorId,
            Anulado: false,
            SucursalId: this.SucursalId,
            ReclamosSearch: this.ReclamosSearch
        };
        return f;
    }

    onSearch(): void {
        if (this.ConductorId && this.ConductorId != null) {

            var self = this;
            this.isLoading = true;
            this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {

                this.Items.next(result.DataObject.Items);
                this.data = result.DataObject.Items;
                self.isLoading = false;
                this.detectChanges();

            });
        }
    }

}

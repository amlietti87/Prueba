import { Component, OnInit, Injector, Input, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ProvinciasService } from '../provincias/provincias.service';
import { ProvinciasDto } from '../model/localidad.model';
import { CreateOrEditProvinciasModalComponent } from '../provincias/create-or-edit-provincias-modal.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { DetailComponent } from '../../../../shared/manager/detail.component';

@Component({
    selector: 'provincias-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ProvinciasComboComponent),
            multi: true
        }
    ]
})
export class ProvinciasComboComponent extends ComboBoxComponent<ProvinciasDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: ProvinciasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _ProvinciaId: number;

    @Input()
    get ProvinciaId(): number {

        return this._ProvinciaId;
    }

    set ProvinciaId(value: number) {
        this._ProvinciaId = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            ProvinciaId: this.ProvinciaId
        };
        return f;
    }

    onSearch(): void {
        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        });
    }


    getIDetailComponent(): ComponentType<DetailComponent<ProvinciasDto>> {
        return CreateOrEditProvinciasModalComponent;
    }

    getNewDto(): ProvinciasDto {
        return new ProvinciasDto();
    }
}

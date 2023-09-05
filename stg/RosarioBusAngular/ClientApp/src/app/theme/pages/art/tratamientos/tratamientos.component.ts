import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ContingenciasDto } from '../model/contingencias.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { TratamientosDto } from '../model/tratamientos.model';
import { TratamientosService } from './tratamientos.service';
import { CreateOrEditTratamientosModalComponent } from './create-or-edit-tratamientos-modal.component';

@Component({

    templateUrl: "./tratamientos.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TratamientosComponent extends CrudComponent<TratamientosDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTratamientosModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: TratamientosService) {
        super(_RolesService, CreateOrEditTratamientosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tratamientos"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: TratamientosDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TratamientosDto): TratamientosDto {

        var item = new TratamientosDto();
        item.Anulado = false;
        return item;
    }






}
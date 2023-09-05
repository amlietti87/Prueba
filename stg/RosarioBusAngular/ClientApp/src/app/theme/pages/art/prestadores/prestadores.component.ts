import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ContingenciasDto } from '../model/contingencias.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { PrestadoresMedicosDto } from '../model/prestadoresmedicos.model';
import { PrestadoresMedicosService } from './prestadores.service';
import { CreateOrEditPrestadoresMedicosModalComponent } from './create-or-edit-prestadores-modal.component';

@Component({

    templateUrl: "./prestadores.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class PrestadoresMedicosComponent extends CrudComponent<PrestadoresMedicosDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditPrestadoresMedicosModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: PrestadoresMedicosService) {
        super(_RolesService, CreateOrEditPrestadoresMedicosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Prestadores Medicos"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: PrestadoresMedicosDto): string {
        return item.Descripcion;
    }
    getNewItem(item: PrestadoresMedicosDto): PrestadoresMedicosDto {

        var item = new PrestadoresMedicosDto();
        item.Anulado = false;
        return item;
    }






}
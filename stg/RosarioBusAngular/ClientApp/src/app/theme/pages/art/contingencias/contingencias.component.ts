import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ContingenciasDto } from '../model/contingencias.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditContingenciasModalComponent } from './create-or-edit-contingencias-modal.component';
import { ContingenciasService } from './contingencias.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./contingencias.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ContingenciasComponent extends CrudComponent<ContingenciasDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditContingenciasModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: ContingenciasService) {
        super(_RolesService, CreateOrEditContingenciasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Contingencias"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: ContingenciasDto): string {
        return item.Descripcion;
    }
    getNewItem(item: ContingenciasDto): ContingenciasDto {

        var item = new ContingenciasDto();
        item.Anulado = false;
        return item;
    }






}
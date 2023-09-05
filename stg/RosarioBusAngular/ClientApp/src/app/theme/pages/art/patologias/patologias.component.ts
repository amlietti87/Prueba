import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ContingenciasDto } from '../model/contingencias.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { PatologiasService } from './patologias.service';
import { PatologiasDto } from '../model/patologias.model';
import { CreateOrEditPatologiasModalComponent } from './create-or-edit-patologias-modal.component';

@Component({

    templateUrl: "./patologias.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class PatologiasComponent extends CrudComponent<PatologiasDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditPatologiasModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: PatologiasService) {
        super(_RolesService, CreateOrEditPatologiasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Patologias"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: PatologiasDto): string {
        return item.Descripcion;
    }
    getNewItem(item: PatologiasDto): PatologiasDto {

        var item = new PatologiasDto();
        item.Anulado = false;
        return item;
    }






}
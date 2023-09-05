import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ConductasNormasDto } from '../model/normas.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditConductasNormasModalComponent } from './create-or-edit-normas-modal.component';
import { ConductasNormasService } from './normas.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./normas.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ConductasNormasComponent extends CrudComponent<ConductasNormasDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditConductasNormasModalComponent
    }
    constructor(injector: Injector, protected _RolesService: ConductasNormasService) {
        super(_RolesService, CreateOrEditConductasNormasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Normas/Conductas incumplidas";
        this.moduleName = "Siniestros";
        this.moduleWithinTitle = true;
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }


    getDescription(item: ConductasNormasDto): string {
        return item.Descripcion;
    }
    getNewItem(item: ConductasNormasDto): ConductasNormasDto {
        var item = new ConductasNormasDto(item);
        item.Anulado = false;
        return item;
    }






}
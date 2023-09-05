import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';

import { CrudComponent } from '../../../../shared/manager/crud.component';

import { IDetailComponent } from '../../../../shared/manager/detail.component';

import { CreateOrEditLocalidadesModalComponent } from './create-or-edit-localidades-modal.component';
import { LocalidadesService } from '../../siniestros/localidades/localidad.service';
import { LocalidadesDto } from '../../siniestros/model/localidad.model';

@Component({

    templateUrl: "./localidades.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class LocalidadesComponent extends CrudComponent<LocalidadesDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditLocalidadesModalComponent
    }

    constructor(injector: Injector, protected _RolesService: LocalidadesService) {
        super(_RolesService, CreateOrEditLocalidadesModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Localidades"
        this.moduleName = "Admin";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: LocalidadesDto): string {
        return item.DscLocalidad
    }
    getNewItem(item: LocalidadesDto): LocalidadesDto {

        var item = new LocalidadesDto(item);
        return item;
    }






}
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { DenunciasEstadosDto } from '../model/denunciasestados.model';
import { DenunciasEstadosService } from './estados.service';
import { CreateOrEditDenunciasEstadosModalComponent } from './create-or-edit-estados-modal.component';
import { EstadosDto } from '../model/estados.model';


@Component({

    templateUrl: "./estados.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class DenunciasEstadosComponent extends CrudComponent<EstadosDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditDenunciasEstadosModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: DenunciasEstadosService) {
        super(_RolesService, CreateOrEditDenunciasEstadosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Estados"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: EstadosDto): string {
        return item.Description;
    }
    getNewItem(item: EstadosDto): EstadosDto {

        var item = new EstadosDto();
        return item;
    }






}
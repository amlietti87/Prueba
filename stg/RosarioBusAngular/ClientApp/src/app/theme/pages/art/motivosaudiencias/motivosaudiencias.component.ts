import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { MotivosAudienciasDto } from '../model/motivosaudencias.model';
import { MotivosAudienciasService } from './motivosaudiencias.service';
import { CreateOrEditMotivosAudienciasModalComponent } from './create-or-edit-motivosaudiencias-modal.component';

@Component({

    templateUrl: "./motivosaudiencias.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class MotivosAudienciasComponent extends CrudComponent<MotivosAudienciasDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditMotivosAudienciasModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: MotivosAudienciasService) {
        super(_RolesService, CreateOrEditMotivosAudienciasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Motivos Audiencias"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: MotivosAudienciasDto): string {
        return item.Description;
    }
    getNewItem(item: MotivosAudienciasDto): MotivosAudienciasDto {

        var item = new MotivosAudienciasDto();
        item.Anulado = false;
        return item;
    }






}
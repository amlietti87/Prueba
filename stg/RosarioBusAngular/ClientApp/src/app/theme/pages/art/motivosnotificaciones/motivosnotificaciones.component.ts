import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { MotivosNotificacionesDto } from '../model/motivosnotificaciones.model';
import { MotivosNotificacionesService } from './motivosnotificaciones.service';
import { CreateOrEditMotivosNotificacionesModalComponent } from './create-or-edit-motivosnotificaciones-modal.component';

@Component({

    templateUrl: "./motivosnotificaciones.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class MotivosNotificacionesComponent extends CrudComponent<MotivosNotificacionesDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditMotivosNotificacionesModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: MotivosNotificacionesService) {
        super(_RolesService, CreateOrEditMotivosNotificacionesModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Motivos Notificaciones"
        this.moduleName = "ART";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: MotivosNotificacionesDto): string {
        return item.Description;
    }
    getNewItem(item: MotivosNotificacionesDto): MotivosNotificacionesDto {

        var item = new MotivosNotificacionesDto();
        item.Anulado = false;
        return item;
    }






}
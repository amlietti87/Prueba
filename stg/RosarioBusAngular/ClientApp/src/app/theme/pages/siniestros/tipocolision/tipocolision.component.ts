import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoColisionDto } from '../model/tipocolision.model';
import { TipoColisionService } from './tipocolision.service';
import { CreateOrEditTipoColisionModalComponent } from './create-or-edit-tipocolision-modal.component';

@Component({

    templateUrl: "./tipocolision.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoColisionComponent extends CrudComponent<TipoColisionDto> implements AfterViewInit {



    constructor(injector: Injector, protected _RolesService: TipoColisionService) {
        super(_RolesService, CreateOrEditTipoColisionModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de colisión"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }


    getDescription(item: TipoColisionDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TipoColisionDto): TipoColisionDto {
        var item = new TipoColisionDto(item);
        item.Anulado = false;
        return item;
    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoColisionModalComponent;
    }





}
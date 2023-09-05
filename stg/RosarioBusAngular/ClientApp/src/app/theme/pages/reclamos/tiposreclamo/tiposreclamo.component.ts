import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TiposReclamoDto } from '../model/tiposreclamo.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTiposReclamoModalComponent } from './create-or-edit-tiposreclamo-modal.component';
import { TiposReclamoService } from './tiposreclamo.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./tiposreclamo.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TiposReclamoComponent extends CrudComponent<TiposReclamoDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTiposReclamoModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: TiposReclamoService) {
        super(_RolesService, CreateOrEditTiposReclamoModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de Reclamo"
        this.moduleName = "Reclamos";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: TiposReclamoDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TiposReclamoDto): TiposReclamoDto {

        var item = new TiposReclamoDto();
        item.Anulado = false;
        return item;
    }






}
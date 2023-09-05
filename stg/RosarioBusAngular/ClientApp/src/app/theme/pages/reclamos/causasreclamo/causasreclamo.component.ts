import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditCausasReclamoModalComponent } from './create-or-edit-causasreclamo-modal.component';
import { CausasReclamoService } from './causasreclamo.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { CausasReclamoDto } from '../model/causasreclamo.model';

@Component({

    templateUrl: "./causasreclamo.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class CausasReclamoComponent extends CrudComponent<CausasReclamoDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditCausasReclamoModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: CausasReclamoService) {
        super(_RolesService, CreateOrEditCausasReclamoModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Causas de Reclamos"
        this.moduleName = "Reclamos";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: CausasReclamoDto): string {
        return item.Descripcion;
    }
    getNewItem(item: CausasReclamoDto): CausasReclamoDto {

        var item = new CausasReclamoDto();
        item.Anulado = false;
        return item;
    }






}
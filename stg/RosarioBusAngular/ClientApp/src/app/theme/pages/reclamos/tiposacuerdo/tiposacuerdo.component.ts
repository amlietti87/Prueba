import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TiposAcuerdoDto } from '../model/tiposacuerdo.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTiposAcuerdoModalComponent } from './create-or-edit-tiposacuerdo-modal.component';
import { TiposAcuerdoService } from './tiposacuerdo.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./tiposacuerdo.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TiposAcuerdoComponent extends CrudComponent<TiposAcuerdoDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTiposAcuerdoModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: TiposAcuerdoService) {
        super(_RolesService, CreateOrEditTiposAcuerdoModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de Acuerdo"
        this.moduleName = "Reclamos";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: TiposAcuerdoDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TiposAcuerdoDto): TiposAcuerdoDto {

        var item = new TiposAcuerdoDto();
        item.Anulado = false;
        return item;
    }






}
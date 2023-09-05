import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoDanioService } from './tipodanio.service';
import { TipoDanioDto } from '../model/tipodanio.model';
import { CreateOrEditTipoDanioModalComponent } from './create-or-edit-tipodanio-modal.component';

@Component({

    templateUrl: "./tipodanio.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoDanioComponent extends CrudComponent<TipoDanioDto> implements AfterViewInit {



    constructor(injector: Injector, protected _RolesService: TipoDanioService) {
        super(_RolesService, CreateOrEditTipoDanioModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de daño"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }


    getDescription(item: TipoDanioDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TipoDanioDto): TipoDanioDto {
        var item = new TipoDanioDto(item);
        item.Anulado = false;
        return item;
    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoDanioModalComponent;
    }





}
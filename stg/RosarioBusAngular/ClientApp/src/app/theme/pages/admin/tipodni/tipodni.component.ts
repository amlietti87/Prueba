import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent, DetailComponent } from '../../../../shared/manager/detail.component';

import { CreateOrEditTipoDniModalComponent } from './create-or-edit-tipodni-modal.component';
import { TipoDniDto } from '../../siniestros/model/tipodni.model';
import { TipoDniService } from '../../siniestros/tipodni/tipodni.service';
import { ComponentType } from '@angular/cdk/overlay/index';

@Component({

    templateUrl: "./tipodni.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoDniComponent extends CrudComponent<TipoDniDto> implements AfterViewInit {



    constructor(injector: Injector, protected _RolesService: TipoDniService) {
        super(_RolesService, CreateOrEditTipoDniModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de documento";
        this.moduleName = "Administración General";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }


    getDescription(item: TipoDniDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TipoDniDto): TipoDniDto {
        var item = new TipoDniDto(item);
        item.Anulado = false;
        return item;
    }
    getIDetailComponent(): ComponentType<DetailComponent<TipoDniDto>> {
        return CreateOrEditTipoDniModalComponent;
    }

    getNewDto(): TipoDniDto {
        return new TipoDniDto();
    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoDniModalComponent
    }





}
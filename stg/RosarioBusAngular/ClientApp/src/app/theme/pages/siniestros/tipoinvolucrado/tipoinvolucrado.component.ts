import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TipoInvolucradoDto } from '../model/tipoinvolucrado.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTipoInvolucradoModalComponent } from './create-or-edit-tipoinvolucrado-modal.component';
import { TipoInvolucradoService } from './tipoinvolucrado.service';
import { IDetailComponent, DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';

@Component({

    templateUrl: "./tipoinvolucrado.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoInvolucradoComponent extends CrudComponent<TipoInvolucradoDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoInvolucradoModalComponent
    }

    constructor(injector: Injector, protected _RolesService: TipoInvolucradoService) {
        super(_RolesService, CreateOrEditTipoInvolucradoModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de involucrados"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: TipoInvolucradoDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TipoInvolucradoDto): TipoInvolucradoDto {

        var item = new TipoInvolucradoDto(item)
        item.Reclamo = false;
        item.Conductor = false;
        item.Vehiculo = false;
        item.MuebleInmueble = false;
        item.Lesionado = false;
        item.Anulado = false;
        return item;
    }








}
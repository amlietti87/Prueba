import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TipoLineaDto } from '../model/tipoLinea.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTipoLineaModalComponent } from './create-or-edit-tipoLinea-modal.component';
import { TipoLineaService } from './tipoLinea.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./tipoLinea.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoLineaComponent extends CrudComponent<TipoLineaDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoLineaModalComponent
    }

    constructor(injector: Injector, protected _RolesService: TipoLineaService) {
        super(_RolesService, CreateOrEditTipoLineaModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipo de linea"
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }






    getDescription(item: TipoLineaDto): string {
        return item.Nombre;
    }
    getNewItem(item: TipoLineaDto): TipoLineaDto {


        var item = new TipoLineaDto(item)
        // item.Activo = true;
        return new TipoLineaDto(item);

    }






}
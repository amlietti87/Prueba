import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TipoMuebleDto } from '../model/tipomueble.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTipoMuebleModalComponent } from './create-or-edit-tipomueble-modal.component';
import { TipoMuebleService } from './tipomueble.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./tipomueble.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoMuebleComponent extends CrudComponent<TipoMuebleDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoMuebleModalComponent
    }

    constructor(injector: Injector, protected _RolesService: TipoMuebleService) {
        super(_RolesService, CreateOrEditTipoMuebleModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipo de mueble/inmueble"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }


    ngOnInit() {
        super.ngOnInit();


    }



    getDescription(item: TipoMuebleDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TipoMuebleDto): TipoMuebleDto {

        var item = new TipoMuebleDto(item)
        item.Anulado = false;
        return item;
    }






}
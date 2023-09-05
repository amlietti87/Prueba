import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TipoLesionadoDto } from '../model/tipolesionado.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTipoLesionadoModalComponent } from './create-or-edit-tipolesionado-modal.component';
import { TipoLesionadoService } from './tipolesionado.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./tipolesionado.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoLesionadoComponent extends CrudComponent<TipoLesionadoDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoLesionadoModalComponent
    }

    constructor(injector: Injector, protected _RolesService: TipoLesionadoService) {
        super(_RolesService, CreateOrEditTipoLesionadoModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipo de lesionados"
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



    getDescription(item: TipoLesionadoDto): string {
        return item.Descripcion;
    }
    getNewItem(item: TipoLesionadoDto): TipoLesionadoDto {

        var item = new TipoLesionadoDto(item)
        item.Anulado = false;
        return item;
    }






}
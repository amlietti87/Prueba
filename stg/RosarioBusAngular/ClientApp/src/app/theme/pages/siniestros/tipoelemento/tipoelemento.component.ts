import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoElementoDto } from '../model/tipoelemento.model';
import { CreateOrEditTipoElementosModalComponent } from './create-or-edit-tipoelementos-modal.component';
import { TipoElementoService } from './tipoelemento.service';

@Component({

    templateUrl: "./tipoelemento.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoElementosComponent extends CrudComponent<TipoElementoDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoElementosModalComponent;
    }

    constructor(injector: Injector, protected _service: TipoElementoService) {
        super(_service, CreateOrEditTipoElementosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipos de elementos"
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



    getDescription(item: TipoElementoDto): string {
        return item.Nombre;
    }
    getNewItem(item: TipoElementoDto): TipoElementoDto {

        var item = new TipoElementoDto(item);
        return item;
    }






}
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';




import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { TipoDiaDto } from '../model/tipoDia.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTipoDiaModalComponent } from './create-or-edit-tipodia-modal.component';
import { TipoDiaService } from './tipodia.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./tipodia.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoDiaComponent extends CrudComponent<TipoDiaDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoDiaModalComponent
    }

    constructor(injector: Injector, protected _RolesService: TipoDiaService) {
        super(_RolesService, CreateOrEditTipoDiaModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipo de día";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }






    getDescription(item: TipoDiaDto): string {
        return item.DesTdia;
    }
    getNewItem(item: TipoDiaDto): TipoDiaDto {


        var item = new TipoDiaDto(item)
        item.Activo = true;
        return new TipoDiaDto(item);

    }






}
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';




import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { SucursalDto } from '../model/sucursal.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditSucursalModalComponent } from './create-or-edit-sucursal-modal.component';
import { SucursalService } from './sucursal.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./sucursal.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class SucursalComponent extends CrudComponent<SucursalDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditSucursalModalComponent
    }

    constructor(injector: Injector, protected _RolesService: SucursalService) {
        super(_RolesService, CreateOrEditSucursalModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Unidad de negocio"
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;

    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }






    getDescription(item: SucursalDto): string {
        return item.DscSucursal;
    }
    getNewItem(item: SucursalDto): SucursalDto {
        return new SucursalDto(item);
    }





}
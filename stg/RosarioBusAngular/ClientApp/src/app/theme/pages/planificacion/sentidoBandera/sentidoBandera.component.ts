import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';

import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { SentidoBanderaDto } from '../model/sentidoBandera.model';
import { CreateOrEditSentidoBanderaModalComponent } from './create-or-edit-sentidobandera-modal.component';
import { SentidoBanderaService } from './sentidoBandera.service';

@Component({

    templateUrl: "./sentidoBandera.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class SentidoBanderaComponent extends CrudComponent<SentidoBanderaDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditSentidoBanderaModalComponent
    }

    constructor(injector: Injector, protected _Service: SentidoBanderaService) {
        super(_Service, CreateOrEditSentidoBanderaModalComponent, injector);
        this.isFirstTime = true;
        this.title = " Sentido Bandera";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }

    getDescription(item: SentidoBanderaDto): string {
        return item.Descripcion;
    }
    getNewItem(item: SentidoBanderaDto): SentidoBanderaDto {
        var item = new SentidoBanderaDto(item);
        return new SentidoBanderaDto(item);

    }






}
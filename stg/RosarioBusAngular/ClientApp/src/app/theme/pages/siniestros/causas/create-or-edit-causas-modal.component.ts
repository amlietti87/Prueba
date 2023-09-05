import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { CausasService } from './causas.service';
import { CausasDto } from '../model/causas.model';
import { SubCausasDto } from '../model/causas.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode } from '../../../../shared/model/base.model';
import { ViewEncapsulation } from '@angular/compiler/src/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditCausasDtoModal',
    templateUrl: './create-or-edit-causas-modal.component.html'

})
export class CreateOrEditCausasModalComponent extends DetailAgregationComponent<CausasDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(
        protected dialogRef: MatDialogRef<CreateOrEditCausasModalComponent>,
        injector: Injector,
        protected service: CausasService,
        @Inject(MAT_DIALOG_DATA) public data: CausasDto
    ) {

        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
    }


    addNewSubCausas() {
        let lista = [...this.detail.SubCausas];
        lista.push(this.getNewItem(null, lista.length * -1));
        this.detail.SubCausas = lista;
    }

    getNewItem(item: SubCausasDto, id: number): SubCausasDto {
        var item = new SubCausasDto(item);
        item.CausaId = this.detail.Id;
        item.Id = id;
        item.Descripcion = null;
        item.Anulado = false;
        return item;

    }

    onCargaDelete(row: SubCausasDto): void {
        var index = this.detail.SubCausas.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.SubCausas];
            lista.splice(index, 1);
            this.detail.SubCausas = lista;
        }
    }

    save(form: NgForm): void {
        super.save(form);
    }

}


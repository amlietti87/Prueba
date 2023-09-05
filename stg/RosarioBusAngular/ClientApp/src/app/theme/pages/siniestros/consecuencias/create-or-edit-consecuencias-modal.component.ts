import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { ConsecuenciasService } from './consecuencias.service';
import { ConsecuenciasDto } from '../model/consecuencias.model';
import { CategoriasDto } from '../model/consecuencias.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditConsecuenciasDtoModal',
    templateUrl: './create-or-edit-consecuencias-modal.component.html',

})
export class CreateOrEditConsecuenciasModalComponent extends DetailAgregationComponent<ConsecuenciasDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditConsecuenciasModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ConsecuenciasDto,
        injector: Injector,
        protected service: ConsecuenciasService) {

        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
    }


    addNewCategorias() {
        if (!this.detail.Categorias) {
            this.detail.Categorias = new Array<CategoriasDto>();
        }
        let lista = [...this.detail.Categorias];
        lista.push(this.getNewItem(null, lista.length * -1));
        this.detail.Categorias = lista;
    }

    getNewItem(item: CategoriasDto, id: number): CategoriasDto {
        var item = new CategoriasDto(item);
        item.ConsecuenciaId = this.detail.Id;
        item.Id = id;
        item.Descripcion = null;
        item.InfoAdicional = null;
        item.Anulado = false;
        return item;
    }

    onCargaDelete(row: CategoriasDto): void {
        var index = this.detail.Categorias.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.Categorias];
            lista.splice(index, 1);
            this.detail.Categorias = lista;
        }
    }


}


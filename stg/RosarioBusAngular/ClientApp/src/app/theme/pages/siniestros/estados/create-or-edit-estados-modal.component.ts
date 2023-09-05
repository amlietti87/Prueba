import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { EstadosService } from './estados.service';
import { EstadosDto } from '../model/estados.model';
import { SubEstadosDto } from '../model/estados.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditEstadosDtoModal',
    templateUrl: './create-or-edit-estados-modal.component.html',

})
export class CreateOrEditEstadosModalComponent extends DetailAgregationComponent<EstadosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(
        injector: Injector,
        protected service: EstadosService,
        protected dialogRef: MatDialogRef<CreateOrEditEstadosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: EstadosDto,
    ) {

        super(dialogRef, service, injector, data);

        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
    }


    addNewSubEstados() {
        if (!this.detail.SubEstados) {
            this.detail.SubEstados = [];
        }
        let lista = [...this.detail.SubEstados];
        lista.push(this.getNewItem(null, lista.length * -1));
        this.detail.SubEstados = lista;
    }

    getNewItem(item: SubEstadosDto, id: number): SubEstadosDto {
        var item = new SubEstadosDto(item);
        item.EstadoId = this.detail.Id;
        item.Id = id;
        item.Descripcion = null;
        item.Cierre = false;
        item.Anulado = false;
        return item;

    }

    onCargaDelete(row: SubEstadosDto): void {
        var index = this.detail.SubEstados.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.SubEstados];
            lista.splice(index, 1);
            this.detail.SubEstados = lista;
        }
    }

    save(form: NgForm): void {

        super.save(form);
    }

}


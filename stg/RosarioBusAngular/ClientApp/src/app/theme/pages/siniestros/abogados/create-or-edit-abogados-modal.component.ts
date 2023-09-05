import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { AbogadosService } from './abogados.service';
import { AbogadosDto } from '../model/abogados.model';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { LocalidadesService } from '../localidades/localidad.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditAbogadosDtoModal',
    templateUrl: './create-or-edit-abogados-modal.component.html',

})
export class CreateOrEditAbogadosModalComponent extends DetailAgregationComponent<AbogadosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditAbogadosModalComponent>,
        injector: Injector,
        protected service: AbogadosService,
        protected localidadservice: LocalidadesService,
        @Inject(MAT_DIALOG_DATA) public data: AbogadosDto) {

        super(dialogRef, service, injector, data);

        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
        this.SetAllowPermission();
    }

    allowAddLocalidades: boolean = false;

    SetAllowPermission() {
        this.allowAddLocalidades = this.permission.isGranted('Admin.Localidad.Agregar');
    }

    completedataBeforeSave(item: AbogadosDto): any {
        if (item.Email == "") {
            item.Email = null;
        }
        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }

    }

    completedataBeforeShow(item: AbogadosDto): any {

        if (this.viewMode == ViewMode.Modify && item.LocalidadId && item.LocalidadId != 0) {

            this.localidadservice.getById(item.LocalidadId)
                //.finally(() => { this.isTableLoading = false; })
                .subscribe((t) => {
                    var findlocalidad = new ItemDto();
                    findlocalidad.Id = item.LocalidadId;
                    findlocalidad.Description = t.DataObject.DscLocalidad + ' - ' + t.DataObject.CodPostal;
                    item.selectLocalidades = findlocalidad;
                })
        }

    }


}


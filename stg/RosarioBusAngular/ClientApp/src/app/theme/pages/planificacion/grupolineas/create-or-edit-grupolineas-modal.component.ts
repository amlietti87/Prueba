import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';

import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { GrupoLineasService } from './grupolineas.service';
import { GrupoLineasDto } from '../model/grupolineas.model';
import { LineaService } from '../linea/linea.service';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
declare let mApp: any;



@Component({
    selector: 'createOrEditGrupoLineasDtoModal',
    templateUrl: './create-or-edit-grupolineas-modal.component.html',

})
export class CreateOrEditGrupoLineasModalComponent extends DetailEmbeddedComponent<GrupoLineasDto> implements IDetailComponent {

    @Input() Sucursalid: number;
    @Input() Sucursal: string;
    selectItem: ItemDto;

    getDescription(item: GrupoLineasDto): string {
        return item.Nombre;
    }

    constructor(
        injector: Injector,
        protected service: GrupoLineasService, protected _lineaservice: LineaService,
    ) {
        super(service, injector);
        this.detail = new GrupoLineasDto();
        this.icon = 'flaticon-layers';
        this.title = 'Grupo de Líneas';

    }

    ngAfterViewInit(): void {
        mApp.initPortlets();
    }

    setSucursal(sucursalid: number, sucursal: string): void {
        this.Sucursalid = sucursalid;
        this.Sucursal = sucursal;

    }

    initFirtTab(): void {
        $('#m_heder_portlet_tab_glGeneral').click();

    }


    completedataBeforeShow(item: GrupoLineasDto): any {


        if (this.viewMode == ViewMode.Add) {
            this.initFirtTab();
            item.Lineas = [];
            item.SucursalId = this.Sucursalid;
        }
        else {

        }
    }


    search(event) {
        this.service.requestAllByFilter().subscribe(data => {
            //this.results = [];
            //for (var i in data.DataObject.Items) {
            //    this.results.push(data.DataObject.Items[i].Description);
            //}

        });
    }



    addLinea(item: ItemDto) {
        if (item.Id) {
            if (!this.detail.Lineas.some(x => x.Id == item.Id)) {

                item.IsSelected = true;
                this.detail.Lineas.push(item);
            }
            else {
                var c = this.detail.Lineas.find(x => x.Id == item.Id);
                this.notificationService.warn("La linea ya fue agregada", c.Description);
                c.animate = true;

                setTimeout(() => {
                    c.animate = false;
                }, 1000);
            }

        }


    }

    removeLinea(item: ItemDto) {

        var index = this.detail.Lineas.findIndex(x => x.Id == item.Id);
        if (index > 0) {
            this.detail.Lineas.splice(index, 1);
        }

    }


    Unselect(event) {

    }




}

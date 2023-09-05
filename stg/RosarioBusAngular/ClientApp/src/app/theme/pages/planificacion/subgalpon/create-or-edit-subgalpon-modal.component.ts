import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, ViewContainerRef, OnInit, ComponentFactoryResolver } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent, DetailEmbeddedComponent } from '../../../../shared/manager/detail.component';
import { SubGalponDto, ConfiguDto } from '../model/subgalpon.model';
import { SubGalponSinCacheService } from './subgalpon.service';
import { GrupoComboComponent } from '../shared/grupo-combo.component';
import { EmpresaComboComponent } from '../shared/empresa-combo.component';
import { SucursalComboComponent } from '../shared/sucursal-combo.component';
import { LineaAutoCompleteComponent } from '../shared/linea-autocomplete.component';
import { GalponComboComponent } from '../shared/Galpon-combo.component';
import { PlanCamComboComponent } from '../shared/plancam-combo.component';
import { AddConfiguModalComponent } from './add-configu-modal.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ViewMode } from '../../../../shared/model/base.model';


@Component({
    selector: 'createOrEditSubGalponDtoModal',
    templateUrl: './create-or-edit-subgalpon-modal.component.html',
    styleUrls: ['./create-or-edit-subgalpon-modal.component.css']
})
export class CreateOrEditSubGalponModalComponent extends DetailEmbeddedComponent<SubGalponDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    @ViewChild('GrupoCombo') GrupoCombo: GrupoComboComponent;
    @ViewChild('EmpresaCombo') EmpresaCombo: EmpresaComboComponent;
    @ViewChild('SucursalCombo') SucursalCombo: SucursalComboComponent;
    @ViewChild('LineaAutoComplete') LineaAutoComplete: LineaAutoCompleteComponent;
    @ViewChild('GalponCombo') GalponCombo: GalponComboComponent;
    @ViewChild('PlanCamCombo') PlanCamCombo: PlanCamComboComponent;
    constructor(
        injector: Injector,
        protected service: SubGalponSinCacheService,
        public dialog: MatDialog
    ) {
        super(service, injector);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.detail = new SubGalponDto();
        this.icon = "fa fa-map-signs";
        this.title = "Sub Galpon";
    }

    completedataBeforeShow(item: SubGalponDto): any {


        if (item.Balanceo && item.Balanceo != null && item.Balanceo.trim().toLocaleUpperCase() == 'S') {
            item.BalanceoCheck = true;
        }
        else {
            item.BalanceoCheck = false;
        }

        if (item.FltComodines && item.FltComodines != null && item.FltComodines.trim().toLocaleUpperCase() == 'S') {
            item.ComodinesCheck = true;
        }
        else {
            item.ComodinesCheck = false;
        }

    }

    completedataBeforeSave(item: SubGalponDto): any {
        if (item.BalanceoCheck) {
            item.Balanceo = 'S';
        }
        else {
            item.Balanceo = 'N';
        }
        if (item.ComodinesCheck) {
            item.FltComodines = 'S';
        }
        else {
            item.FltComodines = 'N';
        }
        this.detail.Configu.forEach(function(value) {

            value.Empresa = null;
            value.Galpon = null;
            value.PlanCamNav = null;
            value.Grupo = null;
            value.Sucursal = null;
            value.Linea = null;
            if (value.selectLinea && value.selectLinea != null) {
                value.CodLin = value.selectLinea.Id;
            }

        });
    }


    getNewItem(item: ConfiguDto, id: number): ConfiguDto {
        var item = new ConfiguDto(item);
        item.CodSubg = this.detail.Id;
        item.Id = id;
        return item;
    }

    onCargaDelete(row: ConfiguDto): void {
        var index = this.detail.Configu.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.Configu];
            lista.splice(index, 1);
            this.detail.Configu = lista;
        }
    }

    AgregarConfigu() {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        let lista = [...this.detail.Configu];

        dialogConfig.data = {
            ConfiguDefault: this.getNewItem(null, lista.length * -1),
            viewMode: ViewMode.Add
        };

        const dialogRef = this.dialog.open(AddConfiguModalComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(res => {

            if (res) {
                lista.push(res);
                this.detail.Configu = lista;
            }
        });

    }

    EditarConfigu(row: ConfiguDto) {

        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        let lista = [...this.detail.Configu];
        var index = lista.findIndex(e => e.CodGru == row.CodGru && e.CodEmpr == row.CodEmpr && e.CodSuc == row.CodSuc && e.CodLin == row.CodLin && e.CodGal == row.CodGal && e.FecBaja == row.FecBaja);

        dialogConfig.data = {
            ConfiguDefault: row,
            viewMode: ViewMode.Modify
        };

        const dialogRef = this.dialog.open(AddConfiguModalComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(res => {

            if (res) {
                lista[index] = res;
                this.detail.Configu = lista;
            }
        });

    }
}

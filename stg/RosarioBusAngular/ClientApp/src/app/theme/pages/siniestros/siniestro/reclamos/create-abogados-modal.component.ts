import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent } from '../../../../../shared/manager/detail.component';
import { CiaSegurosDto } from '../../model/seguros.model';
import { CiaSegurosService } from '../../seguros/seguros.service';
import { ViewMode, ItemDto } from '../../../../../shared/model/base.model';
import { LocalidadesService } from '../../localidades/localidad.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AbogadosDto } from '../../model/abogados.model';
import { AbogadosService } from '../../abogados/abogados.service';

@Component({
    selector: 'createAbogadosDtoModal',
    templateUrl: './create-abogados-modal.component.html',

})
export class CreateAbogadosModalComponent extends DetailEmbeddedComponent<AbogadosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;

    constructor(
        injector: Injector,
        protected service: AbogadosService,
        protected localidadservice: LocalidadesService,
        public dialogRef: MatDialogRef<CreateAbogadosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        super(service, injector);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.detail = new AbogadosDto();
    }

    save(form: NgForm): void {
        super.save(form);
    }



    completedataBeforeShow(item: AbogadosDto): any {

        if (this.viewMode == ViewMode.Modify) {

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

    completedataBeforeSave(item: AbogadosDto): any {

        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    }

    close(): void {
        super.close();
        this.dialogRef.close(false);
    }


    SaveDetail(): any {
        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {

                if (this.viewMode = ViewMode.Add) {
                    this.detail.Id = t.DataObject;
                    this.detail.Description = this.detail.Description;
                }

                this.notify.info('Guardado exitosamente');
                this.affterSave(this.detail);
                this.closeOnSave = true;
                this.modalSave.emit(null);
                this.dialogRef.close(this.detail);
            })
    }
}


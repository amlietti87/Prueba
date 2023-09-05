import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ReclamosDto } from '../../siniestros/model/reclamos.model';
import { DetailEmbeddedComponent } from '../../../../shared/manager/detail.component';
import { ReclamosService } from '../../siniestros/reclamos/reclamos.service';


@Component({
    selector: 'AnularReclamoModal',
    templateUrl: './anular-modal.component.html',

})
export class AnularReclamoModalComponent extends DetailEmbeddedComponent<ReclamosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;


    constructor(
        injector: Injector,
        protected service: ReclamosService,
        public dialogRef: MatDialogRef<AnularReclamoModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        super(service, injector);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.detail = data;
    }


    save(form: NgForm): void {
        super.save(form);
    }


    completedataBeforeShow(item: ReclamosDto): any {

    }

    completedataBeforeSave(item: ReclamosDto): any {
    }

    close(): void {
        this.dialogRef.close(false);
        super.close();
    }


    SaveDetail(): any {
        this.detail.Anulado = true;
        this.service.Anular(this.detail)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {

                this.notify.info('Guardado exitosamente');
                this.affterSave(this.detail);
                this.closeOnSave = true;
                this.modalSave.emit(null);
                this.dialogRef.close(this.detail);
            })
    }
}


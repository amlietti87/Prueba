import { Component, Input, Type, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, DebugElement, Injector } from '@angular/core';
import { SiniestrosDto } from '../../model/siniestro.model';
import { ItemDto, ItemDtoStr, ViewMode } from '../../../../../shared/model/base.model';
import { ActivatedRoute } from '@angular/router';
import { PermissionCheckerService } from '../../../../../shared/common/permission-checker.service';
import { IDetailComponent } from '../../../../../shared/manager/detail.component';
import { Subscription } from 'rxjs';
import { forEach } from '@angular/router/src/utils/collection';
import { NgModel, FormBuilder, Validators } from '@angular/forms';
import { SiniestroMapsComponent } from '../../../../../components/rbmaps/siniestro.maps.component';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { MapaModalComponent } from './mapa-modal.component';

@Component({
    selector: 'datoslugar-acc',
    templateUrl: './datoslugar.component.html',
})
export class DatosLugarComponent {



    lugarForm: any;

    _detail: SiniestrosDto;
    _viewmode: ViewMode;

    @Input()
    get detail(): SiniestrosDto {

        return this._detail;
    }

    set detail(value: SiniestrosDto) {

        this._detail = value;

    }




    constructor(
        injector: Injector,
        private lugarFB: FormBuilder,
        public dialog: MatDialog
    ) {
        this.lugarFB = injector.get(FormBuilder);
        this.createForm();
    }


    VerMapa() {


        const dialogRef = this.dialog.open(MapaModalComponent, {
            width: '600px',
            data: {
                Latitud: this.detail.Latitud,
                Longitud: this.detail.Longitud,
                Localidad: this.detail.Localidad,
                Direccion: this.detail.Direccion,
                SucursalId: this.detail.SucursalId
            }
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.detail.Latitud = res.Latitud;
                this.detail.Longitud = res.Longitud;
                this.detail.Localidad = res.Localidad;
                this.detail.Direccion = res.Direccion;

            }
        });

    }


    createForm() {
        if (this.detail == null) {
            this.detail = new SiniestrosDto();
        }
        this.lugarForm =
            this.lugarFB.group({
                Lugar: [this.detail.Lugar, Validators.required],
                Localidad: [this.detail.Lugar, Validators.required]
            });

    }

}
import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, SimpleChanges } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material';

import { NgForm } from '@angular/forms';
import { DetailAgregationComponent } from '../../../../../shared/manager/detail.component';
import { InvolucradosDto, HistorialInvolucrados, MuebleInmuebleDto } from '../../model/involucrados.model';
import { InvolucradosService } from '../../involucrados/involucrados.service';
import { TipoInvolucradoService } from '../../tipoinvolucrado/tipoinvolucrado.service';
import { StatusResponse } from '../../../../../shared/model/base.model';
import { TipoInvolucradoDto } from '../../model/tipoinvolucrado.model';
import { VehiculoDto } from '../../model/vehiculo.model';
import { ConductorDto } from '../../model/conductor.model';
import { LesionadoDto } from '../../model/lesionado.model';


declare let mApp: any;

@Component({
    selector: 'involucrado-detail-modal',
    templateUrl: './involucrado-detail-modal.component.html',
    styleUrls: ['./involucrado-detail-modal.component.css']
})
export class InvolucradoDetailComponent extends DetailAgregationComponent<InvolucradosDto> implements OnInit, AfterViewInit {

    @ViewChild('detailForm') detailForm: NgForm;

    HistorialSiniestros: HistorialInvolucrados = new HistorialInvolucrados();
    public dialog: MatDialog;

    constructor(protected dialogRef: MatDialogRef<InvolucradoDetailComponent>,
        protected serviceInvolucrados: InvolucradosService,
        private tipoInvolucradoService: TipoInvolucradoService,
        injector: Injector,
        @Inject(MAT_DIALOG_DATA) public data: InvolucradosDto) {

        super(dialogRef, serviceInvolucrados, injector, data);

        this.dialog = injector.get(MatDialog);
        this.saveLocal = false;
        this.SetAllowPermission();
    }

    InitializeDetail(data: InvolucradosDto) {
        super.InitializeDetail(data);
        this.onTipoInvolucradoChanged(null);
        this.RefrescarHistorialSiniestros(null, null);
    }

    // Permissions
    allowAdd: boolean = false;
    allowModify: boolean = false;
    allowAddTipoInvolucrado: boolean = false;
    allowAddTipoMueInmueble: boolean = false;
    allowAddTipoLesionado: boolean = false;
    allowAddCiaSeguro: boolean = false;
    allowAddLocalidades: boolean = false;
    allowAddTipoDni: boolean = false;
    bMostrarHistorialSiniestros: boolean = false;
    viewDetalleDanio: boolean = false;

    SetAllowPermission() {
        this.allowAdd = this.permission.isGranted('Siniestro.Involucrado.Agregar');
        this.allowModify = this.permission.isGranted('Siniestro.Involucrado.Modificar');
        this.allowAddTipoInvolucrado = this.permission.isGranted('Siniestro.TipoInvolucrado.Agregar');
        this.allowAddTipoMueInmueble = this.permission.isGranted('Siniestro.TipoMuebleInmueble.Agregar');
        this.allowAddTipoLesionado = this.permission.isGranted('Siniestro.TipoLesionado.Agregar');
        this.allowAddCiaSeguro = this.permission.isGranted('Siniestro.Seguro.Agregar');
        this.allowAddLocalidades = this.permission.isGranted('Admin.Localidad.Agregar');
        this.allowAddTipoDni = this.permission.isGranted('Admin.TipoDni.Agregar');
    }

    // Variables
    bMostrarMuebleInmueble: boolean = false;
    bMostrarVehiculo: boolean = false;
    bMostrarConductor: boolean = false;
    bMostrarLesionado: boolean = false;

    // Methods & Functions
    RefrescarHistorialSiniestros(tipoDocId: number, nroDoc: string): void {

        var filtroTipoDocId: number;
        if (tipoDocId && tipoDocId != null) {
            filtroTipoDocId = tipoDocId;
        } else {
            filtroTipoDocId = this.detail.TipoDocId;
        }

        var filtroNroDoc: string;
        if (nroDoc && nroDoc != null) {
            filtroNroDoc = nroDoc;
        } else {
            if (this.detail.NroDoc && this.detail.NroDoc != null) {
                filtroNroDoc = this.detail.NroDoc.toString();
            }
        }
        if (filtroTipoDocId != null && filtroNroDoc != null && filtroNroDoc != "") {
            this.serviceInvolucrados.HistorialSiniestros(filtroTipoDocId, filtroNroDoc)
                .subscribe((t) => {
                    if (t.DataObject) {
                        this.bMostrarHistorialSiniestros = true;
                        this.HistorialSiniestros = t.DataObject;
                    } else {
                        this.bMostrarHistorialSiniestros = false;
                        this.HistorialSiniestros = new HistorialInvolucrados();
                    }
                });
        } else {
            this.HistorialSiniestros = new HistorialInvolucrados();
            this.bMostrarHistorialSiniestros = false;
        }
    }

    // Events
    onDocumentoChanged(event) {
        this.RefrescarHistorialSiniestros(this.detail.TipoDocId, event);
    }
    onTipoDocumentoChanged(event) {
        this.RefrescarHistorialSiniestros(event, this.detail.NroDoc);
    }
    onTipoInvolucradoChanged(event: any) {

        if (this.detail == null) {
            return;
        }

        if (this.detail.TipoInvolucradoId == null && (event == null || event == "")) {
            return;
        }
        if (event == null || event == "") {
            event = this.detail.TipoInvolucradoId
        }
        this.tipoInvolucradoService.getById(event).subscribe(result => {
            if (result.Status == StatusResponse.Ok) {
                this.detail.TipoInvolucrado = result.DataObject;

                this.bMostrarMuebleInmueble = this.detail.TipoInvolucrado.MuebleInmueble;
                if (this.bMostrarMuebleInmueble && this.detail.MuebleInmueble == null) {
                    this.detail.MuebleInmueble = new MuebleInmuebleDto();
                }
                this.bMostrarVehiculo = this.detail.TipoInvolucrado.Vehiculo;
                if (this.bMostrarVehiculo && this.detail.Vehiculo == null) {
                    this.detail.Vehiculo = new VehiculoDto();
                }
                this.bMostrarConductor = this.detail.TipoInvolucrado.Conductor;
                if (this.bMostrarConductor && this.detail.Conductor == null) {
                    this.detail.Conductor = new ConductorDto();
                }
                this.bMostrarLesionado = this.detail.TipoInvolucrado.Lesionado;

                if (this.bMostrarLesionado) {
                    if (this.detail.Lesionado == null) {
                        this.detail.Lesionado = new LesionadoDto();
                        this.detail.TipoLesionadoId = null;
                    }
                    else {
                        this.detail.TipoLesionadoId = this.detail.Lesionado.TipoLesionadoId;
                    }
                }
                else {
                    this.detail.TipoLesionadoId = null;
                }

                if (this.detail.TipoInvolucrado.Conductor || this.detail.TipoInvolucrado.Vehiculo || this.detail.TipoInvolucrado.MuebleInmueble || this.detail.TipoInvolucrado.Lesionado) {
                    this.viewDetalleDanio = true;
                }
                else {
                    this.viewDetalleDanio = false;
                }

            }
            else {
                this.detail.TipoInvolucrado = new TipoInvolucradoDto();
                this.bMostrarMuebleInmueble = false;
                this.bMostrarVehiculo = false;
                this.bMostrarConductor = false;
                this.bMostrarLesionado = false;
                this.viewDetalleDanio = false;
                this.detail.TipoLesionadoId = null;
            }
        })
    }

    // Overrides
    ngOnInit() {
        super.ngOnInit();
    }
    ngAfterViewInit() {
        mApp.initComponents();
        //this.RefrescarHistorialSiniestros();
    }
    completedataBeforeShow(item: InvolucradosDto) {
        this.onTipoInvolucradoChanged(null);
    }
    save(form: NgForm): void {

        if (this.detailForm && this.detailForm.form.invalid) {
            this.notificationService.warn("Debe completar los datos obligatorios");
            return;
        }

        this.completedataBeforeSave(this.detail);
        var validate = this.validateSave();
        if (!validate) {
            this.saving = false;
            return;
        } else {
            this.SaveDetail(); // Este metodo pone el this.saving en true
        }

    }
    completedataBeforeSave(item: InvolucradosDto): any {
        if (!item.TipoInvolucrado.Conductor) {
            item.Conductor = null;
        }
        if (!item.TipoInvolucrado.Lesionado) {
            item.Lesionado = null;
        } else {
            //Limpiar Lesionado si no seleccion� TipoLesionadoId
            if (item.TipoLesionadoId == null || item.TipoLesionadoId == 0) {
                item.Lesionado = null;
            } else {
                if (item.Lesionado == null) {
                    item.Lesionado = new LesionadoDto();
                }
                item.Lesionado.TipoLesionadoId = item.TipoLesionadoId;
            }
        }
        if (!item.TipoInvolucrado.MuebleInmueble) {
            item.MuebleInmueble = null;
        }
        if (!item.TipoInvolucrado.Vehiculo) {
            item.Vehiculo = null;
        }
        if (item.Lesionado) {
            if (item.Lesionado.TipoLesionadoId == null || item.Lesionado.TipoLesionadoId == 0) {
                item.LesionadoId = null;
            }
        }

        if (this.detail.Vehiculo && this.detail.Vehiculo.Dominio != null && this.detail.Vehiculo.Dominio.indexOf(' ') != -1) {
            this.detail.Vehiculo.Dominio = this.detail.Vehiculo.Dominio.replace(/\s+/g, "");
            return false;
        }

    }
    validateSave() {

        // Validar que si tiene mueble/inmueble, tenga TipoInmuebleId
        if (this.detail.MuebleInmueble && this.detail.MuebleInmueble.TipoInmuebleId == null) {
            this.notificationService.warn("Debe agregar el Tipo de Mueble/Inmueble");
            return false;
        }
        // Validar que si tiene Vehiculo, tenga algun dato del vehiculo
        if (this.detail.Vehiculo && (this.detail.Vehiculo.Marca == null && this.detail.Vehiculo.Dominio == null && this.detail.Vehiculo.Modelo == null && this.detail.Vehiculo.Poliza == null)) {
            this.notificationService.warn("Debe agregar al menos un dato en Vehiculo");
            return false;
        }
        // Validar que si tiene Lesionado y ningun otro, tenga el TipoLesionadoId
        if (!(this.detail.TipoInvolucrado.MuebleInmueble || this.detail.TipoInvolucrado.Vehiculo || this.detail.TipoInvolucrado.Conductor) && this.detail.TipoInvolucrado.Lesionado) {
            if ((this.detail.TipoLesionadoId as any) == "" || this.detail.TipoLesionadoId == undefined || this.detail.TipoLesionadoId == null || this.detail.TipoLesionadoId == 0) {
                this.notificationService.warn("Debe agregar el tipo de Lesionado");
                return false;
            }
            else if (!this.detail.Lesionado) {
                this.notificationService.warn("Debe agregar el tipo de Lesionado");
                return false;
            }
        }

        if (this.detail.DetalleLesion && this.detail.DetalleLesion.length > 0 && !this.detail.Lesionado) {
            this.notificationService.warn("Si existe Detalle de lesion debe agregar el tipo de Lesionado");
            return false;
        }
        return true;
    }
}
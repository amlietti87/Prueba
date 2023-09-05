import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent } from '../../../../shared/manager/detail.component';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ReclamosHistoricosDto, ReclamosDto } from '../../siniestros/model/reclamos.model';
import { ReclamosHistoricosService } from '../../siniestros/reclamos/reclamoshistoricos.service';
import { ReclamosService } from '../../siniestros/reclamos/reclamos.service';
@Component({
    selector: 'EstadosDtoModal',
    templateUrl: './estados-modal.component.html',

})
export class EstadosReclamosModalComponent extends DetailEmbeddedComponent<ReclamosHistoricosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    cargado: boolean = false;
    Historial: ReclamosHistoricosDto[] = [];
    detailTablaReclamos: ReclamosDto;
    constructor(
        injector: Injector,
        protected service: ReclamosHistoricosService,
        protected serviceReclamos: ReclamosService,
        public dialogRef: MatDialogRef<EstadosReclamosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        super(service, injector);
        this.cfr = injector.get(ComponentFactoryResolver);
        if (data) {
            this.detail = data;

            this.GetReclamoPrincipal(this.detail.ReclamoId);
        }
        else {
            this.detail = new ReclamosHistoricosDto();
        }
        this.detailTablaReclamos = new ReclamosDto();
        this.detailTablaReclamos.Fecha = new Date();

    }

    BuscarHistorial(detail: ReclamosHistoricosDto) {
        var f = {
            ReclamoId: detail.ReclamoId
        };
        this.service.requestAllByFilter(f).subscribe(x => {
            this.Historial = [];
            this.Historial = x.DataObject.Items;
        });
    }

    save(form: NgForm): void {
        super.save(form);
    }

    GetReclamoPrincipal(ReclamoId: number) {
        this.serviceReclamos.getById(ReclamoId)
            .subscribe((t) => {
                if (t.DataObject) {
                    this.detailTablaReclamos = t.DataObject;
                    this.detailTablaReclamos.Fecha = new Date();
                    if (this.detailTablaReclamos.SubEstado) {
                        if (!this.detailTablaReclamos.SubEstado.Cierre) {
                            this.detailTablaReclamos.SubEstadoId = null;
                        }
                    }
                    this.cargado = true;
                }
            })
    }

    completedataBeforeShow(item: ReclamosHistoricosDto): any {
        item.Fecha = new Date();
    }

    completedataBeforeSave(item: ReclamosHistoricosDto): any {
    }

    close(): void {
        super.close();
        this.dialogRef.close(false);
    }

    NullExtraProperties(object: ReclamosDto): void {
        object.AbogadoEmpresaId = null;
        object.AbogadoId = null;
        object.Autos = null;
        object.Cuotas = null;
        object.Description = null;
        object.FechaOfrecimiento = null;
        object.FechaPago = null;
        object.JuntaMedica = null;
        object.JuzgadoId = null;
        object.MontoDemandado = null;
        object.MontoFranquicia = null;
        object.MontoHonorariosAbogado = null;
        object.MontoHonorariosMediador = null;
        object.MontoHonorariosPerito = null;
        object.MontoOfrecido = null;
        object.MontoPagado = null;
        object.MontoReconsideracion = null;
        object.NroExpediente = null;
        object.ObsConvenioExtrajudicial = null;
        object.Observaciones = null;
        object.PorcentajeIncapacidad = null;
        object.ReclamoCuotas = null;
        object.MontoTasasJudiciales = null;
        object.CausaId = null;
        object.TipoAcuerdoId = null;
        object.RubroSalarialId = null;
        object.Hechos = null;
    }

    SaveDetail(): any {

        var nuevoestado = this.detail.EstadoId;
        var nuevosubestado = this.detail.SubEstadoId;
        var historicosubestado = this.detailTablaReclamos.SubEstadoId;
        var historicoestado = this.detailTablaReclamos.EstadoId;

        this.detailTablaReclamos.EstadoId = nuevoestado;
        this.detailTablaReclamos.SubEstadoId = nuevosubestado;
        this.detail.EstadoId = historicoestado;
        this.detail.SubEstadoId = historicosubestado;

        this.NullExtraProperties(this.detailTablaReclamos);

        this.serviceReclamos.CambioEstado(this.detailTablaReclamos, this.detail)
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


import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { FDEstadosDto } from '../model/fdestados.model';
import { FDEstadosService } from '../services/fdestados.service';
import { FDAccionesPermitidasDto } from '../model/fdaccionespermitidas.model';
import { FdAccionesPermitidasService } from '../services/fdaccionespermitidas.service';
import { FDAccionesDto } from '../model/fdacciones.model';
import { PermissionCheckerService } from '../../../../shared/common/permission-checker.service';
import { DocumentosProcesadosDto } from '../model/documentosprocesados.model';

@Component({
    selector: 'accionesgrilla-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FDAccionesGrillaComboComponent),
            multi: true
        }
    ]
})
export class FDAccionesGrillaComboComponent extends ComboBoxComponent<FDAccionesPermitidasDto> implements OnInit {

    permission: PermissionCheckerService;

    constructor(service: FdAccionesPermitidasService, injector: Injector, permisoService: PermissionCheckerService) {
        super(service, injector);
        this.permission = injector.get(PermissionCheckerService);
    }

    itemsClone: FDAccionesPermitidasDto[] = [];

    ngOnInit(): void {
        super.ngOnInit();
    }

    _DocumentosGrilla: DocumentosProcesadosDto[];

    @Input()
    get DocumentosGrilla(): DocumentosProcesadosDto[] {
        return this._DocumentosGrilla;
    }
    set DocumentosGrilla(val: DocumentosProcesadosDto[]) {
        this._DocumentosGrilla = val;
        if (val && val != null && val.length > 0 && (this.MostrarBDEmpleado == true || this.MostrarBDEmpleado == false)) {
            this.LlenarComboCustom();
        }
    }

    _mostrarBDEmpleado: boolean;

    @Input()
    get MostrarBDEmpleado(): boolean {
        return this._mostrarBDEmpleado;
    }
    set MostrarBDEmpleado(val: boolean) {
        this._mostrarBDEmpleado = val;
        if (val != null && this.DocumentosGrilla && this.DocumentosGrilla != null && this.DocumentosGrilla.length > 0) {
            this.LlenarComboCustom();
        }
    }

    itemsPorDefecto(): FDAccionesPermitidasDto[] {
        var acciones: FDAccionesPermitidasDto[] = [];

        var accion1 = new FDAccionesPermitidasDto();
        accion1.Id = -1;
        accion1.DisplayName = 'Abrir archivo';
        accion1.PermissionId = 278;
        accion1.Description = accion1.DisplayName;
        if (this.MostrarBDEmpleado == false) {
            accion1.TokenPermission = 'FirmaDigital.BD-Empleador.Abrir';
        }
        else {
            accion1.TokenPermission = 'FirmaDigital.BD-Empleado.Abrir';
        }

        if (this.permission.isGranted(accion1.TokenPermission)) {
            acciones.push(accion1);
        }

        var accion2 = new FDAccionesPermitidasDto();
        accion2.Id = -2;
        accion2.DisplayName = 'Descargar archivo';
        accion2.PermissionId = 274;
        accion2.Description = accion2.DisplayName;
        if (this.MostrarBDEmpleado == false) {
            accion2.TokenPermission = 'FirmaDigital.BD-Empleador.Descargar';
        }
        else {
            accion2.TokenPermission = 'FirmaDigital.BD-Empleado.Descargar';
        }

        if (this.permission.isGranted(accion2.TokenPermission)) {
            acciones.push(accion2);
        }

        //ACCION -6 ESTÁ OCUPADA POR REVISAR ERRORES QUE NO ESTÁ EN ESTE COMBO


        if (this.MostrarBDEmpleado == false) {

            var accion3 = new FDAccionesPermitidasDto();
            accion3.Id = -3;
            accion3.DisplayName = 'Rechazar documento';
            accion3.PermissionId = 275;
            accion3.Description = accion3.DisplayName;
            accion3.TokenPermission = 'FirmaDigital.BD-Empleador.Rechazar';

            var findDocGrilla = this.DocumentosGrilla.find(e => e.Rechazado == false && e.PermiteRechazo == true);

            if (this.permission.isGranted(accion3.TokenPermission) && findDocGrilla != null) {
                acciones.push(accion3);
            }


            var accion4 = new FDAccionesPermitidasDto();
            accion4.Id = -4;
            accion4.DisplayName = 'Ver detalle documento';
            accion4.PermissionId = 276;
            accion4.Description = accion4.DisplayName;
            accion4.TokenPermission = 'FirmaDigital.BD-Empleador.Ver';

            if (this.permission.isGranted(accion4.TokenPermission)) {
                acciones.push(accion4);
            }
        }

        if (this.MostrarBDEmpleado == true) {
            var accion7 = new FDAccionesPermitidasDto();
            accion7.Id = -7;
            accion7.TokenPermission = 'FirmaDigital.BD-Empleado.EnviarCorreo';
            accion7.PermissionId = 288;
            accion7.DisplayName = 'Enviar correo';
            accion7.PermissionId = null;
            accion7.Description = accion7.DisplayName;

            if (this.permission.isGranted(accion7.TokenPermission)) {
                acciones.push(accion7);
            }

        }



        return acciones;
    }

    LlenarComboCustom() {
        var self = this;
        this.isLoading = true;
        this.itemsClone = [];
        this.itemsClone = this.itemsPorDefecto();
        this.DocumentosGrilla.forEach((e) => {

            e.AccionesDisponibles.forEach((z) => {

                var find = this.itemsClone.find(f => f.Id == z.AccionPermitida.Id);

                if (find == null) {

                    var okPermission = this.permission.isGranted(z.AccionPermitida.TokenPermission);

                    if (okPermission) {
                        this.itemsClone.push(z.AccionPermitida);
                    }

                }
            })

        });
        this.items = this.itemsClone;
        self.isLoading = false;
        setTimeout(() => {
            $(self.comboboxElement.nativeElement).selectpicker('refresh');
        }, 200);
    }

    onSearch(): void {

    }

    protected GetFilter(): any {
        var f = {
            MostrarBDEmpleado: this.MostrarBDEmpleado
        };
        return f;
    }


}

import { Component, OnInit, Input, Injector, ViewChild, Inject } from '@angular/core';
import { SiniestrosDto, SiniestroHistorialEmpleado } from '../../model/siniestro.model';
import { ItemDto, ViewMode } from '../../../../../shared/model/base.model';
import { EmpleadosService } from '../../empleados/empleados.service';
import { EmpleadosDto, LegajosEmpleadoDto } from '../../model/empleado.model';
import { PracticantesDto } from '../../model/practicantes.model';
import { PracticantesService } from '../../practicantes/practicantes.service';
import { LegajosEmpleadoService } from '../../legajoempleado/legajosempleado.service';
import { SiniestroService } from '../siniestro.service';
import { EmpresaService } from '../../../planificacion/empresa/empresa.service';
import { EmpresaDto } from '../../../planificacion/model/empresa.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmpleadosAutoCompleteComponent } from '../../shared/empleado-autocomplete.component';
import { DetalleSiniestroComponent } from '../detallesiniestro/detallesiniestro.component';
import { EmpPractComboComponent } from '../../shared/EmPract-combo.component';
import { MatDialog, MAT_DIALOG_DATA, MatDialogConfig } from '@angular/material';
import { PracticantesAutoCompleteComponent } from '../../shared/practicante-autocomplete.component';
import { CreatePracticantesModalComponent } from './create-practicante-modal.component';

@Component({
    selector: 'conductor',
    templateUrl: './conductor.component.html',
})
export class ConductorComponent {
    _viewMode: ViewMode;
    conductorForm: any;
    empleadoExiste: boolean = true;


    get EsConductor(): boolean {

        return this._detail.EmpPractConId == 1;
    }

    @Input() allowAddPracticante: boolean;
    @Input() CurrentConductor: EmpleadosDto;
    @Input() CurrentConductorLegajo: LegajosEmpleadoDto;
    @Input() CurrentPracticante: PracticantesDto;
    @Input() CurrentHistorial: SiniestroHistorialEmpleado;
    @Input() CurrentConductorEmpresa: EmpresaDto;
    _detail: SiniestrosDto;
    @ViewChild('PracticanteAutocomplete') PracticanteAutocomplete: PracticantesAutoCompleteComponent;
    public dialog: MatDialog
    constructor(
        protected serviceEmpleado: EmpleadosService,
        protected servicePracticante: PracticantesService,
        protected serviceLegajoEmpleado: LegajosEmpleadoService,
        protected serviceSiniestro: SiniestroService,
        protected serviceEmpresa: EmpresaService,
        injector: Injector,
        private conductorFB: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: PracticantesDto
    ) {
        this.conductorFB = injector.get(FormBuilder);
        this.dialog = injector.get(MatDialog);
        this.createForm();
    }

    @ViewChild('ConductorId') ConductorId: EmpleadosAutoCompleteComponent;
    @ViewChild('EmpPractConId') EmpPractConId: EmpPractComboComponent;

    @Input()
    get detail(): SiniestrosDto {

        return this._detail;
    }

    set detail(value: SiniestrosDto) {
        this._detail = value;

        if (value.ConductorId) {
            this.GetCurrentConductor();
        }
        else if (value.PracticanteId) {
            this.GetCurrentPracticante();
        }
    }


    @Input()
    get selectEmpleados(): ItemDto {

        return this.detail.selectEmpleados;
    }

    set selectEmpleados(value: ItemDto) {
        this.detail.selectEmpleados = value;

        if (value) {
            if (this.detail.ConductorId != value.Id) {
                this.detail.ActualizarConductor = true;
            }
            this.detail.ConductorId = value.Id;
            this.GetCurrentConductor();
        }
    }

    @Input()
    get selectPracticantes(): ItemDto {

        return this.detail.selectPracticantes;
    }

    set selectPracticantes(value: ItemDto) {
        this.detail.selectPracticantes = value;

        if (value) {
            this.detail.PracticanteId = value.Id;
            this.GetCurrentPracticante();
        }



    }

    @Input()
    get ViewMode(): ViewMode {

        return this._viewMode;
    }

    set ViewMode(value: ViewMode) {

        this._viewMode = value;
    }

    private GetCurrentConductor(): void {

        if (this.detail.ConductorId) {
            this.serviceEmpleado.ExisteEmpleado(this.detail.ConductorId).subscribe((t) => {

                if (t.DataObject) {

                    this.empleadoExiste = true;
                    if (this.ConductorId) {
                        this.ConductorId.setDisabledState(false);
                    }
                    if (this.EmpPractConId) {
                        this.EmpPractConId.setDisabledState(false);
                    }
                    this.serviceEmpleado.getById(this.detail.ConductorId)
                        .subscribe((t) => {
                            this.CurrentConductor = t.DataObject;
                            this.detail.CuilEmpleado = this.CurrentConductor.Cuil;
                        })
                    this.serviceLegajoEmpleado.GetMaxById(this.detail.ConductorId)
                        .subscribe((t) => {
                            this.CurrentConductorLegajo = t.DataObject;
                            if (this.selectEmpleados && !this.detail.ActualizarConductor && this.ViewMode == ViewMode.Modify) {
                                this.CurrentConductorLegajo.LegajoSap = this.detail.ConductorLegajo;
                                this.CurrentConductorEmpresa = this.detail.ConductorEmpresa;
                                this.detail.ActualizarConductor = false;
                            }
                            else {

                                this.serviceEmpresa.getById(this.CurrentConductorLegajo.CodEmpresa)
                                    .subscribe((t) => {
                                        this.detail.ConductorEmpresaId = this.CurrentConductorLegajo.CodEmpresa;
                                        this.CurrentConductorEmpresa = t.DataObject;
                                        this.detail.ConductorEmpresa = this.CurrentConductorEmpresa;
                                        this.detail.ConductorLegajo = this.CurrentConductorLegajo.LegajoSap;
                                        this.detail.ActualizarConductor = false;
                                    })
                            }
                        })
                    this.serviceSiniestro.GetHistorialEmpPract(true, this.detail.ConductorId)
                        .subscribe((t) => {
                            this.CurrentHistorial = t.DataObject;
                        })
                }
                else {
                    if (this.ConductorId) {
                        this.ConductorId.setDisabledState(true);
                    }
                    if (this.EmpPractConId) {
                        this.EmpPractConId.setDisabledState(true);
                    }
                    this.detail.EmpPractConId = 1;
                    this.empleadoExiste = false;
                }
            })
        }

    }

    private GetCurrentPracticante(): void {

        if (this.detail.PracticanteId) {
            this.servicePracticante.getById(this.detail.PracticanteId)
                .subscribe((t) => {
                    this.CurrentPracticante = t.DataObject;
                })
            this.serviceSiniestro.GetHistorialEmpPract(false, this.detail.PracticanteId)
                .subscribe((t) => {
                    this.CurrentHistorial = t.DataObject;
                })

        }

    }

    createForm() {
        if (this.detail == null) {
            this.detail = new SiniestrosDto();
        }
        this.conductorForm =
            this.conductorFB.group({
                EmpPractConId: [this.detail.EmpPractConId, Validators.required],
                ConductorId: [this.detail.ConductorId, null],
                PracticanteId: [this.detail.PracticanteId, null]
            });

        this.formControlValueChanged();

    }


    openDialogPracticante() {

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;


        let dialogRef = this.dialog.open(CreatePracticantesModalComponent, dialogConfig);
        dialogRef.componentInstance.showNew(new PracticantesDto());
        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {

                    //Opcion 1 recargar  el combo
                    // this.SegurosCombo.onSearch();
                    //this.detail.PracticanteId = data.Id;

                    //opcion 2 agregar el item a al combo
                    this.PracticanteAutocomplete.items.push(data);
                    this.detail.PracticanteId = data.Id;
                    var itemdto = new ItemDto();
                    itemdto.Id = data.Id;
                    itemdto.Description = data.ApellidoNombre;
                    itemdto.IsSelected = true;
                    this.detail.selectPracticantes = itemdto;
                }

            }
        );
    }


    formControlValueChanged() {

        this.conductorForm.get('EmpPractConId').valueChanges.subscribe(
            (EmpPractConId: number) => {

                this.conductorForm.get('PracticanteId').clearValidators();
                this.conductorForm.get('ConductorId').clearValidators();

                if (EmpPractConId == 2) {
                    this.conductorForm.get('PracticanteId').setValidators([Validators.required]);
                    this.detail.ConductorId = null;
                    this.detail.selectEmpleados = null;
                    this.detail.EmpPract = 'P';

                }
                else if (EmpPractConId == 1) {
                    this.conductorForm.get('ConductorId').setValidators([Validators.required]);
                    this.detail.PracticanteId = null;
                    this.detail.selectPracticantes = null;
                    this.detail.EmpPract = 'E';
                }
                //this.conductorForm.get('ConductorId').updateValueAndValidity();
                //this.conductorForm.get('PracticanteId').updateValueAndValidity();
            }

        )
    }



}
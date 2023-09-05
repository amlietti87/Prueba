import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, OnDestroy, ComponentFactoryResolver, ElementRef, Inject, Input, ChangeDetectorRef } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { SelectItem, Dropdown } from 'primeng/primeng';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { HMinxtipoService } from '../../hminxtipo/hminxtipo.service';
import { CopiarHMinxtipoInput } from '../../model/hminxtipo.model';
import { TipoDiaComboComponent } from '../../shared/tipoDia-combo.component';
import { BanderaComboComponent } from '../../shared/bandera-combo.component';
import { BanderaDto } from '../../model/bandera.model';

@Component({
    selector: 'importarhorariofecha',
    templateUrl: "./copiar-minutosporsector.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class CopiarMinutosPorSectorComponent extends AppComponentBase implements OnInit {



    active = true;
    autoLoad: boolean = false;
    isLoading: boolean;
    htposhorasSelectItem: SelectItem[];
    saving: boolean = false;
    disableTipoHoraDestino: boolean = false;
    BanderasSource: BanderaDto[];
    Banderastarget: BanderaDto[] = [];
    @ViewChild('codTipoDiaComboOrigen') codTipoDiaCombo: TipoDiaComboComponent;

    @ViewChild('codTipoDiaComboDetino') codTipoDiaComboDetino: TipoDiaComboComponent;


    @ViewChild('copiarMinutosForm') filterForm: NgForm;

    @Input() LineaId: number;


    constructor(
        protected dialogRef: MatDialogRef<CopiarMinutosPorSectorComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CopiarHMinxtipoInput,
        protected service: HMinxtipoService,
        protected cdr: ChangeDetectorRef,
        injector: Injector) {
        super(injector);
    }


    ngOnInit(): void {

        if (this.data.CodHfechaOrigen) {
            this.codTipoDiaCombo.CodHfecha = this.data.CodHfechaOrigen;
            this.codTipoDiaComboDetino.CodHfecha = this.data.CodHfechaOrigen;
            this.LineaId = this.data.LineaId;
        }

    }


    close(): void {
        this.dialogRef.close(false);
    }

    TipoHoraOrigenChanged(event: any) {
        if (event == '') {
            this.data.TipoHoraDestino = '';
            this.disableTipoHoraDestino = true;
            this.cdr.detectChanges();
        }
        else {
            this.disableTipoHoraDestino = false;
        }
    }



    Procesar() {
        this.filterForm.onSubmit(null);

        if (this.Banderastarget.length == 0) {
            this.notify.warn("selecione banderas");
            return;
        }

        if (this.data.TipoHoraDestino == '' && this.data.TipoHoraOrigen != '') {
            this.notify.warn("No puede seleccionar tipo de hora de destino en Todos cuando el tipo de hora de origen no es igual a Todos");
            return;
        }

        if (this.data.TipoHoraDestino != '' && this.data.TipoHoraOrigen == '') {
            this.notify.warn("No puede seleccionar tipo de hora de Origen en Todos cuando el tipo de hora de Destino no es igual a Todos");
            return;
        }

        if (this.data.TipoHoraDestino == '' && this.data.TipoHoraOrigen == '') {
            this.data.TipoHoraDestino = 'all';
            this.data.TipoHoraOrigen = 'all';
        }

        if (this.filterForm.valid) {

            this.data.BanderasId = [];

            this.Banderastarget.forEach(f => {
                this.data.BanderasId.push(f.Id);
            });

            this.saving = true
            this.service.CopiarMinutos(this.data).finally(() => {
                this.saving = false;
            }).subscribe(e => {
                if (e.DataObject) {
                    this.notificationService.success("Se actualizaron las bandera: " + e.DataObject, "Se Copio con exito!");
                    this.dialogRef.close(true);
                }
                else {
                    this.notify.warn("no se actualizo ninguna bandera", "no se encontro bandera para actualizar");
                }


            });
        }
    }


}
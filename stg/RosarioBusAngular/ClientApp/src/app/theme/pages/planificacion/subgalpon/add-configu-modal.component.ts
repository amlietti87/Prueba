import { Component, ViewChild, Injector, ViewContainerRef, Inject, OnInit } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { NgForm } from '@angular/forms';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { ConfiguDto } from '../model/subgalpon.model';
import { GrupoComboComponent } from '../shared/grupo-combo.component';
import { EmpresaComboComponent } from '../shared/empresa-combo.component';
import { SucursalComboComponent } from '../shared/sucursal-combo.component';
import { LineaAutoCompleteComponent } from '../shared/linea-autocomplete.component';
import { GalponComboComponent } from '../shared/Galpon-combo.component';
import { PlanCamComboComponent } from '../shared/plancam-combo.component';
import { ViewMode } from '../../../../shared/model/base.model';




@Component({
    selector: 'add-configu-modal',
    templateUrl: './add-configu-modal.component.html'
})
export class AddConfiguModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('detailForm') detailForm: NgForm;

    @ViewChild('GrupoCombo') GrupoCombo: GrupoComboComponent;
    @ViewChild('EmpresaCombo') EmpresaCombo: EmpresaComboComponent;
    @ViewChild('SucursalCombo') SucursalCombo: SucursalComboComponent;
    @ViewChild('LineaAutoComplete') LineaAutoComplete: LineaAutoCompleteComponent;
    @ViewChild('GalponCombo') GalponCombo: GalponComboComponent;
    @ViewChild('PlanCamCombo') PlanCamCombo: PlanCamComboComponent;

    Configu: ConfiguDto = new ConfiguDto();

    viewMode: ViewMode;

    constructor(
        injector: Injector,
        public dialogRef: MatDialogRef<AddConfiguModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        super(injector);

        this.Configu = data.ConfiguDefault;
        this.viewMode = data.viewMode;
    }

    ngOnInit() {
        this.dialogRef.updateSize('50%');
    }


    save(): void {


        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }


        this.Configu.GrupoGrilla = this.GrupoCombo.items.find(e => e.Id == this.Configu.CodGru).DesGru;


        this.Configu.EmpresaGrilla = this.EmpresaCombo.items.find(e => e.Id == this.Configu.CodEmpr).DesEmpr;


        this.Configu.SucursalGrilla = this.SucursalCombo.items.find(e => e.Id == this.Configu.CodSuc).DscSucursal;



        this.Configu.GalponGrilla = this.GalponCombo.items.find(e => e.Id == this.Configu.CodGal).Description;

        this.Configu.PlanCam = 1;
        // Configu.PlanCamNavGrilla = this.PlanCamCombo.items.find(e => e.Id == row.PlanCam).DesPlan;


        this.dialogRef.close(this.Configu);
    }



    close(): void {
        this.dialogRef.close(false);
    }
}

import { Component, Input, Type, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, DebugElement, Injector } from '@angular/core';
import { SiniestrosDto } from '../../model/siniestro.model';
import { CochesDto } from '../../model/coche.model';
import { ItemDto, ItemDtoStr } from '../../../../../shared/model/base.model';
import { LineaDto } from '../../../planificacion/model/linea.model';
import { CausasService } from '../../causas/causas.service';
import { ActivatedRoute } from '@angular/router';
import { PermissionCheckerService } from '../../../../../shared/common/permission-checker.service';
import { IDetailComponent } from '../../../../../shared/manager/detail.component';
import { forEach } from '@angular/router/src/utils/collection';
import { NgModel } from '@angular/forms';
import { ConductasNormasService } from '../../normas/normas.service';

@Component({
    selector: 'normas-acc',
    templateUrl: './normas.component.html',
})
export class NormasComponent {


    constructor(
        protected serviceNormas: ConductasNormasService) {
    }


    _detail: SiniestrosDto;

    @Input() allowAddNorma: boolean;

    @Input()
    get detail(): SiniestrosDto {

        return this._detail;
    }

    set detail(value: SiniestrosDto) {

        this._detail = value;
    }











}
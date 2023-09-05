import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { PersTurnosDto } from '../model/persturnos.model';

@Injectable()
export class PersTurnosService extends CrudService<PersTurnosDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/PersTurnos';
        this.endpoint = this.planificacionUrl;
    }
}


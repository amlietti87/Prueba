import { RangosHorariosDto } from './../model/rangoshorarios.model';
import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

@Injectable()
export class RangosHorariosService extends CrudService<RangosHorariosDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspRangosHorarios';
        this.endpoint = this.planificacionUrl;
    }
}


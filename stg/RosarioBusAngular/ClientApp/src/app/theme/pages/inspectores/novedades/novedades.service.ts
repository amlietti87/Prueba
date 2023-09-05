import { NovedadesDto } from './../model/novedades.model';
import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

@Injectable()
export class NovedadesService extends CrudService<NovedadesDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/Novedades';
        this.endpoint = this.planificacionUrl;
    }

}


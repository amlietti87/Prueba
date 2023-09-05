import { ZonasDto } from './../model/zonas.model';
import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';

@Injectable()
export class ZonasService extends CrudService<ZonasDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspZonas';
        this.endpoint = this.planificacionUrl;
    }


}


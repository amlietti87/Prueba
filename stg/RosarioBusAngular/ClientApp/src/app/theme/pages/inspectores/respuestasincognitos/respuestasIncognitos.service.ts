import { RespuestasIncognitosDto } from '../model/respuestasIncognitos.model';
import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';


@Injectable()
export class RespuestasIncognitosService extends CrudService<RespuestasIncognitosDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspRespuestasIncognitos';
        this.endpoint = this.planificacionUrl;
    }
}


import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { EstadosDiagramaInspectoresDto } from '../model/estadosdiagramainspectores.model';

@Injectable()
export class EstadosDiagramaInspectoresService extends CrudService<EstadosDiagramaInspectoresDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspEstadosDiagramaInspectores';
        this.endpoint = this.planificacionUrl;
    }

}


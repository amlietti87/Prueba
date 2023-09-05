import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { DiagramasInspectoresDto, DiagramaMesAnioDto } from '../model/diagramasinspectores.model';
import { ResponseModel } from '../../../../shared/model/base.model';
import { Observable } from 'rxjs/Observable';
import { PersTopesHorasExtrasDto } from '../model/pers-topes-horas-extras.model';

@Injectable()
export class PersTopesHorasExtrasService extends CrudService<PersTopesHorasExtrasDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/PersTopesHorasExtras';
        this.endpoint = this.planificacionUrl;
    }


}


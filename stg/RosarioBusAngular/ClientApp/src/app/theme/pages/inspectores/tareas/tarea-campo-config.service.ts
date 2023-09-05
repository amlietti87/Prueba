import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { TareaDto } from '../model/tarea.model';
import { TareaCampoConfigDto } from '../model/tarea-campo-config.model';

@Injectable()
export class TareaCampoConfigService extends CrudService<TareaCampoConfigDto> {

    private resourceUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.resourceUrl = environment.planificacionUrl + '/TareaCampoConfig';
        this.endpoint = this.resourceUrl;
    }


}


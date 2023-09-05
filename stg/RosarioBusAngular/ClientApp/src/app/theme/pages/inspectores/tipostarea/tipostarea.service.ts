import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { TiposTareaDto } from '../model/tipostarea.model';

@Injectable()
export class TiposTareaService extends CrudService<TiposTareaDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspTiposTarea';
        this.endpoint = this.planificacionUrl;
    }
}


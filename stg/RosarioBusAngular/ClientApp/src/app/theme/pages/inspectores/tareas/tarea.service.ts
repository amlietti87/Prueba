import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { TareaDto } from '../model/tarea.model';

@Injectable()
export class TareaService extends CrudService<TareaDto> {

    private resourceUrl: string = '';
    constructor(
        protected http: HttpClient) {
        super(http);
        this.resourceUrl = environment.planificacionUrl + '/Tarea';
        this.endpoint = this.resourceUrl;
    }


}


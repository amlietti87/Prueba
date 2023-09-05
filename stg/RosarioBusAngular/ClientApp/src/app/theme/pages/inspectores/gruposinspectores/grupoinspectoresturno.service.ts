import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';

import { InspGrupoInspectoresTurnoDto } from '../model/gruposinspectores.model';

@Injectable()
export class GrupoInspectoresTurnoService extends CrudService<InspGrupoInspectoresTurnoDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspGrupoInspectoresTurnos';
        this.endpoint = this.planificacionUrl;
    }


}


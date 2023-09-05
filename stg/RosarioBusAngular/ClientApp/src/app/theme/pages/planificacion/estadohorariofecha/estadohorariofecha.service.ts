import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { FileService } from '../../../../shared/common/file.service';
import { EstadoHorarioFechaDto } from '../model/estadohorariofecha.model';

@Injectable()
export class EstadoHorarioFechaService extends CrudService<EstadoHorarioFechaDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,

    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/PlaEstadoHorarioFecha';
        this.endpoint = this.identityUrl;
    }


}


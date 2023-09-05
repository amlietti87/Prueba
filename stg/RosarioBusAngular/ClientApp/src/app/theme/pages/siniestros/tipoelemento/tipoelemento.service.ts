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
import { } from '../model/user.model';
import { TipoElementoDto } from '../model/tipoelemento.model';

@Injectable()
export class TipoElementoService extends CrudService<TipoElementoDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.planificacionUrl + '/CroTipoElemento';
        this.endpoint = this.siniestrosUrl;
    }


}


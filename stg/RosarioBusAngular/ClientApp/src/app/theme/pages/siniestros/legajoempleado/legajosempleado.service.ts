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

import { EmpleadosDto, LegajosEmpleadoDto } from '../model/empleado.model';

@Injectable()
export class LegajosEmpleadoService extends CrudService<LegajosEmpleadoDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/LegajosEmpleado';
        this.endpoint = this.siniestrosUrl;
    }

    GetMaxById(id: any): Observable<ResponseModel<LegajosEmpleadoDto>> {

        let url = this.endpoint + '/GetMaxById?id=' + id;
        return this.http.get<ResponseModel<LegajosEmpleadoDto>>(url);
    }
}


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
import { ElementosDto } from '../model/elemento.model';
import { AdjuntosDto } from '../model/adjuntos.model';

@Injectable()
export class ElementosService extends CrudService<ElementosDto> {


    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/CroElemeneto';
        this.endpoint = this.identityUrl;
    }


    GetAdjunto(id: string): Observable<ResponseModel<AdjuntosDto>> {

        let url = this.endpoint + '/GetAdjunto?Id=' + id;
        return this.http.get<ResponseModel<AdjuntosDto>>(url);
    }



}


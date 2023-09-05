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
import { SectorDto, SectorFilter, SectorViewDto, RutaSectoresDto } from '../model/sector.model';

@Injectable()
export class SectorService extends CrudService<SectorDto> {

    getSectorView(filter: SectorFilter): Observable<ResponseModel<RutaSectoresDto>> {

        let url = this.endpoint + '/GetSectorView';
        let data = filter;
        return this.http.post<ResponseModel<RutaSectoresDto>>(url, data);
    }
    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Sector';
        this.endpoint = this.identityUrl;
    }
}


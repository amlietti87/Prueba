import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { CrudService } from '../../../../../shared/common/services/crud.service';
import { DashboardDto, UsuarioDashboardInput, UsuarioDashboardItemDto } from '../model/dashboard.model';
import { AuthService } from '../../../../../auth/auth.service';
import { environment } from '../../../../../../environments/environment';
import { ResponseModel } from '../../../../../shared/model/base.model';


@Injectable()
export class DashboarService extends CrudService<DashboardDto> {
    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/DshDashboard';
        this.endpoint = this.identityUrl;
    }

    guardarDashboard(input: UsuarioDashboardInput): Observable<ResponseModel<UsuarioDashboardItemDto[]>> {
        return this.http.post<ResponseModel<UsuarioDashboardItemDto[]>>(this.endpoint + '/GuardarDashboard', input);
    }

    RecuperarDshUsuarioDashboardItems(): Observable<ResponseModel<UsuarioDashboardInput>> {
        return this.http.post<ResponseModel<UsuarioDashboardInput>>(this.endpoint + '/RecuperarDshUsuarioDashboardItems', {});
    }

}


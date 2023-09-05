import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService, CacheCrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { DBLocalStorageService } from '../../../../shared/utils/local-storage.service';
import { HChoxser, HChoxserFilter, ImportadorDuracionResult, ChofXServImportado, HChoxserExtendedDto, HorarioDuracion } from '../model/hChoxser.model';
import { HMinxtipoFilter } from '../model/hminxtipo.model';
import { HHorariosConfiFilter, HHorariosConfiDto } from '../model/hhorariosconfi.model';



@Injectable()
export class HChoxserService extends CrudService<HChoxser> {


    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/HChoxser';
        this.endpoint = this.identityUrl;
    }


    uploadPlanilla(data: any): Observable<ResponseModel<string>> {

        let testData: FormData = new FormData();
        testData.append('file', data, data.name);
        let url = this.endpoint + '/uploadPlanilla';
        return this.http.post<ResponseModel<string>>(url, testData);
    }

    RecuperarPlanilla(filter: HChoxserFilter): Observable<ResponseModel<ImportadorDuracionResult>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/RecuperarPlanilla';
        return this.http.get<ResponseModel<ImportadorDuracionResult>>(url, { params: params });
    }


    ImportarMinutos(input: HChoxserFilter): Observable<ResponseModel<string>> {

        let url = this.endpoint + '/ImportarDuraciones';
        return this.http.post<ResponseModel<string>>(url, input);
    }


    RecuperarDuraciones(filter: HHorariosConfiFilter): Observable<ResponseModel<HChoxserExtendedDto[]>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/RecuperarDuraciones';
        return this.http.get<ResponseModel<HChoxserExtendedDto[]>>(url, { params: params });
    }

    createOrUpdateDurYSer(data: HorarioDuracion): Observable<ResponseModel<any>> {

        let url = this.endpoint + '/UpdateDurYSer';
        // if (mode == ViewMode.Add) {
        //     url = this.endpoint + '/SaveNewEntity';
        // }
        return this.http.post<ResponseModel<any>>(url, data);
    }

}
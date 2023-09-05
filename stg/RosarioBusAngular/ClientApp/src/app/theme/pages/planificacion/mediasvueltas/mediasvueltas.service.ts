import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


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
import { MediasVueltasDto, MediasVueltasFilter, HMediasVueltasImportadaView } from '../model/mediasvueltas.model';
import { FileService } from '../../../../shared/common/file.service';



@Injectable()
export class MediasVueltasService extends CrudService<MediasVueltasDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/HMediasVueltas';
        this.endpoint = this.identityUrl;
    }

    GenerateReportPuntaPunta(data: MediasVueltasFilter): any {

        let url = this.endpoint + '/GenerateReportPuntaPunta';
        this.fileService.dowloadAuthenticatedByPost(url, data);
    }

    LeerMediasVueltasIncompletas(data: MediasVueltasFilter): Observable<ResponseModel<HMediasVueltasImportadaView[]>> {
        var params = new HttpParams();
        if (data) {
            Object.keys(data).forEach(function(item) {
                params = params.set(item, data[item]);
            });
        }
        let url = this.endpoint + '/LeerMediasVueltasIncompletas';
        return this.http.get<ResponseModel<HMediasVueltasImportadaView[]>>(url, { params: params });
    }


}


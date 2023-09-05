import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { HServiciosDto } from '../model/hServicios.model';
import { Observable } from 'rxjs/Rx';
import { HMinxtipoFilter, MinutosPorSectorDto, HMinxtipoDto, HSectoresDto, HMinxtipoImportado, ImportadorHMinxtipoResult, CopiarHMinxtipoInput } from '../model/hminxtipo.model';
import { ResponseModel } from '../../../../shared/model/base.model';
import { FileService } from '../../../../shared/common/file.service';
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';

@Injectable()
export class HMinxtipoService extends CrudService<HMinxtipoDto> {





    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private fileService: FileService

    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/HMinxtipo';
        this.endpoint = this.identityUrl;
    }


    GetMinutosPorSector(filter: HMinxtipoFilter): Observable<ResponseModel<MinutosPorSectorDto>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {

                if (Array.isArray(filter[item])) {
                    filter[item].forEach(element => {
                        params = params.append(item, element);
                    });

                } else {
                    params = params.set(item, filter[item]);
                }
            });
        }
        return this.http.get<ResponseModel<MinutosPorSectorDto>>(this.endpoint + '/GetMinutosPorSector', { params: params });
    }

    GetHSectores(filter: HMinxtipoFilter): Observable<ResponseModel<HSectoresDto[]>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {

                if (Array.isArray(filter[item])) {
                    filter[item].forEach(element => {
                        params = params.append(item, element);
                    });

                } else {
                    params = params.set(item, filter[item]);
                }
            });
        }
        return this.http.get<ResponseModel<HSectoresDto[]>>(this.endpoint + '/GetHSectores', { params: params });
    }

    SetHSectores(data: HSectoresDto[]): Observable<ResponseModel<HSectoresDto[]>> {
        return this.http.post<ResponseModel<HSectoresDto[]>>(this.endpoint + '/SetHSectores', data);
    }

    UpdateHMinxtipo(items: HMinxtipoDto[]): Observable<ResponseModel<string>> {
        return this.http.post<ResponseModel<string>>(this.endpoint + '/UpdateHMinxtipo', items);
    }


    uploadPlanilla(data: any): Observable<ResponseModel<string>> {

        let testData: FormData = new FormData();
        testData.append('file', data, data.name);
        let url = this.endpoint + '/uploadPlanilla';
        return this.http.post<ResponseModel<string>>(url, testData);
    }

    CopiarMinutos(data: CopiarHMinxtipoInput): Observable<ResponseModel<string>> {
        return this.http.post<ResponseModel<string>>(this.endpoint + '/CopiarMinutos', data);
    }





    RecuperarPlanilla(filter: HMinxtipoFilter): Observable<ResponseModel<ImportadorHMinxtipoResult>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/RecuperarPlanilla';
        return this.http.get<ResponseModel<ImportadorHMinxtipoResult>>(url, { params: params });
    }

    ImportarMinutos(input: HMinxtipoFilter): any {
        let url = this.endpoint + '/ImportarMinutos';
        return this.http.post<ResponseModel<string>>(url, input);
    }

    GetReporteExcelMinxSec(filter: HMinxtipoFilter, callbackFn: Function): any {
        let url = this.endpoint + '/GenerarExcelMinXSec';
        this.fileService.dowloadListAuthenticatedByPost(url, filter).subscribe(data => {
            data.DataObject.map(data => {
                var url = this.getBlobURL(data);
                if (data.ForceDownload) {
                    var a = window.document.createElement('a');
                    a.href = url;
                    a.download = data.FileName;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                }
                else {
                    window.open(url, "_blank");
                }
            });
            if (callbackFn) {
                callbackFn();
            }
        }
        );
    }

    getBlobURL(file: FileDTO) {
        var raw = window.atob(file.ByteArray);
        var rawLength = raw.length;
        var array = new Uint8Array(new ArrayBuffer(rawLength));
        for (var i = 0; i < rawLength; i++) {
            array[i] = raw.charCodeAt(i);
        }
        var url = window.URL.createObjectURL(new Blob([array], { type: file.FileType }));
        return url;
    }
}
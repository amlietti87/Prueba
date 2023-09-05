import { Injectable } from '@angular/core';
import { ResponseModel } from '../model/base.model';
import { HttpClient } from '@angular/common/http';
import { FileDTO } from './models/fileDTO.model';
import { Service } from './services/crud.service';
import { Helpers } from '../../helpers';


@Injectable()
export class FileService implements Service {
    endpoint: string;

    constructor(protected http: HttpClient) {

    }

    downloadAnonymousFileByUrl(url: string) {
        window.open(url, '_blank');
    }

    dowloadAuthenticatedByPost(url: string, params: any) {

        Helpers.setLoading(true);
        return this.http.post<ResponseModel<FileDTO>>(url, params).finally(() =>
            Helpers.setLoading(false)
        ).subscribe(ret => {
            var url = this.getBlobURL(ret.DataObject);
            if (ret.DataObject.ForceDownload) {
                var a = window.document.createElement('a');
                a.href = url;
                a.download = ret.DataObject.FileName;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            } else {
                window.open(url, "_blank");
            }
        }
            )
    }

    dowloadSabanAuthenticatedByPost(url: string, params: any) {
        return this.http.post<ResponseModel<FileDTO>>(url, params);
    }
    dowloadListAuthenticatedByPost(url: string, params: any) {
        return this.http.post<ResponseModel<FileDTO[]>>(url, params);
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

    dowloadAuthenticatedByGet(url: string, params: any) {

    }
}
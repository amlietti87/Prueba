import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FilterDTO, PaginListResultDto, ResponseModel, ADto, Dto, ViewMode, ItemDto } from '../../model/base.model';
import { DBLocalStorageService } from '../../utils/local-storage.service';
import { LocatorService } from './locator.service';
import { MessageService } from '../message.service';


export interface Service {
    endpoint: string;
}

@Injectable()
export abstract class CrudService<T extends ADto> implements Service {
    


    endpoint: string;
    private _messageService: MessageService;


    constructor(protected http: HttpClient) {
        this._messageService = LocatorService.injector.get(MessageService);
    }

    requestAllByFilter(reqParams?: any): Observable<ResponseModel<PaginListResultDto<T>>> {

        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {

                if (Array.isArray(reqParams[item])) {
                    reqParams[item].forEach(element => {
                        params = params.append(item, element);
                    });

                } else {
                    params = params.set(item, reqParams[item]);
                }
            });
        }
        return this.http.get<ResponseModel<PaginListResultDto<T>>>(this.endpoint + '/GetAllAsync', { params: params });
    }


    GetItemsAsync(reqParams?: any): Observable<ResponseModel<ItemDto[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {

                if (Array.isArray(reqParams[item])) {
                    reqParams[item].forEach(element => {
                        params = params.append(item, element);
                    });

                } else {
                    params = params.set(item, reqParams[item]);
                }
            });
        }

        return this.http.get<ResponseModel<ItemDto[]>>(this.endpoint + '/GetItemsAsync', { params: params });
    }


    FindItemsAsync(filter: any): Observable<ResponseModel<ItemDto[]>> {
        let url = this.endpoint + '/FindItemsAsync';
        let data = filter;

        return this.http.post<ResponseModel<ItemDto[]>>(url, data);
    }

    getById(id: any, blockEntity: boolean = true): Observable<ResponseModel<T>> {
        let url = this.endpoint + '/GetByIdAsync?id=' + id + '&blockEntity=' + blockEntity;
        return this.http.get<ResponseModel<T>>(url);
    }

    createOrUpdate(data: T, mode: ViewMode): Observable<ResponseModel<any>> {

        let url = this.endpoint + '/UpdateEntity';
        if (mode == ViewMode.Add) {
            url = this.endpoint + '/SaveNewEntity';
        }
        return this.http.post<ResponseModel<T>>(url, data);
    }

    search(filter: FilterDTO): Observable<ResponseModel<PaginListResultDto<T>>> {

        let url = this.endpoint + '/GetPagedList';
        let data = filter;

        return this.http.post<ResponseModel<PaginListResultDto<T>>>(url, data);
    }

    delete(id: number): Observable<any> {
        let url = this.endpoint + '/DeleteById?id=' + id
        return this.http.post(url, null);
    }

    unBlockEntity(id: any): Observable<any> {
        let url = this.endpoint + '/UnBlockEntity?id=' + id
        return this.http.post(url, null);
    }


   
    

}

@Injectable()
export abstract class CacheCrudService<T extends ADto> extends CrudService<T>
{
    storage: DBLocalStorageService;

    constructor(protected http: HttpClient) {
        super(http);

        this.storage = LocatorService.getInstance(DBLocalStorageService);

    }

    requestAllByFilterCached(): Promise<T[]> {

        return this.storage.getItem<T[]>(this.endpoint, null).then(r => {
            if (r != null) {
                return r;
            }
            else {
                return this.SetCache();
            }
        });
    }

    getByIdCached(id: any): Promise<T> {
        return this.storage.getItem<T[]>(this.endpoint, null).then(r => {
            if (r != null) {
                return r.find(e => e.Id == id);
            }
            else {
                return this.SetCache().then(s => {
                    return s.find(l => l.Id == id);
                })
            }
        });
    }

    SetCache(): Promise<T[]> {
        return this.requestAllByFilter().toPromise().then(r => {
            return this.storage.setItem<T[]>(this.endpoint, r.DataObject.Items).then(e => {
                return r.DataObject.Items;
            })
        });
    }
}

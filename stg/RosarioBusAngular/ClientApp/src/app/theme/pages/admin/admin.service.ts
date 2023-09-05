//import { Injectable } from '@angular/core';
//import { Response } from '@angular/http';
//import { DataService } from '../../../shared/common/services/data.service';
//import { ConfigurationService } from '../../../shared/common/services/configuration.service';

//import { UserFilter, ResponseModel, PaginListResultDto } from '../../../shared/model/base.model';





//import 'rxjs/Rx';
//import { Observable } from 'rxjs/Observable';
//import 'rxjs/add/observable/throw';
//import { Observer } from 'rxjs/Observer';
//import 'rxjs/add/operator/map';
//import { ListResultDtoOfUserListDto, UserDto } from './model/user.model';
//import { RolFilter, RolDto } from './model/rol.model';



//@Injectable()
//export class AdminService {


//    private identityUrl: string = '';
//    constructor(
//        private service: DataService,
//        private configurationService: ConfigurationService) {
//        if (this.configurationService.isReady)
//            this.identityUrl = this.configurationService.serverSettings.identityUrl;
//        else
//            this.configurationService.settingsLoaded$.subscribe(x => this.identityUrl = this.configurationService.serverSettings.identityUrl);

//    }

//    getUsers(pageIndex: number, pageSize: number, sort: string): Observable<ResponseModel<ListResultDtoOfUserListDto>> {


//        let url = this.identityUrl + '/User/GetPagedList';
//        let data = new UserFilter();
//        data.Page = pageIndex;
//        data.PageSize = pageSize;
//        data.Sort = sort;





//        return this.service.post(url, data).map((response: Response) => {
//            return response.json();
//        });
//    }

//    getRolesByUser(permission: string): Observable<RolDto[]> {

//        let url = this.identityUrl + '/Roles';



//        let fakeResponse = [];


//        //var item1 = new RoleListDto();
//        //item1.Id = 1;
//        //item1.displayName = "admin";
//        //item1.name = "admin";

//        //fakeResponse.items.push(item1);

//        //var item2 = new RoleListDto();
//        //item2.id = 2;
//        //item2.displayName = "otro";
//        //item2.name = "otro";
//        //fakeResponse.items.push(item2);


//        let delayedObservable = Observable.of(fakeResponse).delay(10);
//        return delayedObservable;
//        //return this.service.get(url).map((response: Response) => {
//        //    return response.json();
//        //});
//    }

//    getRoles(pageIndex: number, pageSize: number, sort: string): Observable<ResponseModel<RolDto>> {

//        let url = this.identityUrl + '/Roles/GetPagedList';
//        let data = new RolFilter();
//        data.Page = pageIndex;
//        data.PageSize = pageSize;
//        data.Sort = sort;


//        return this.service.post(url, data).map((response: Response) => {
//            return response.json();
//        });
//    }

//    getUserForEdit(id: number): Observable<ResponseModel<UserDto>> {

//        let url = this.identityUrl + '/User/GetByIdAsync?id=' + id;

//        return this.service.get(url).map((response: Response) => {
//            return response.json();
//        });
//    }

//    getRolForEdit(id: number): Observable<ResponseModel<RolDto>> {

//        let url = this.identityUrl + '/Roles/GetByIdAsync?id=' + id;

//        return this.service.get(url).map((response: Response) => {
//            return response.json();
//        });
//    }

//    createOrUpdateUser(data: UserDto): Observable<ResponseModel<UserDto>> {

//        let url = this.identityUrl + '/User/UpdateEntity';
//        if (data.Id <= 0) {
//            url = this.identityUrl + '/User/SaveNewEntity';
//        }

//        return this.service.post(url, data).map((response: Response) => {
//            return response.json();
//        });
//    }

//    createOrUpdateRol(data: RolDto): Observable<ResponseModel<RolDto>> {

//        let url = this.identityUrl + '/Roles/UpdateEntity';
//        if (data.Id <= 0) {
//            url = this.identityUrl + '/Roles/SaveNewEntity';
//        }

//        return this.service.post(url, data).map((response: Response) => {
//            return response.json();
//        });
//    }

//    deleteUser(id: number): Observable<void> {
//        let url = this.identityUrl + '/User/DeleteById?id=' + id;

//        return this.service.post(url, null).map((response: Response) => {
//            return response.json();
//        });
//    }

//    deleteRol(id: number): Observable<void> {
//        let url = this.identityUrl + '/Roles/DeleteById?id=' + id;

//        return this.service.post(url, null).map((response: Response) => {
//            return response.json();
//        });
//    }

//    findUser(userName: string): Observable<ResponseModel<UserDto>> {
//        let url = this.identityUrl + '/User/GetByUserLdapAsync?userName=' + userName;

//        return this.service.get(url).map((response: Response) => {
//            return response.json();
//        });
//    }

//}




import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../shared/model/base.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { AuthService } from '../auth/auth.service';
import { UserDto, ListResultDtoOfUserListDto } from '../theme/pages/admin/model/user.model';
import { GetPermissionsForEditOutput, UpdatePermissionsInput } from '../theme/pages/admin/model/permission.model';
import { UpdateLineasForEdit } from '../theme/pages/admin/model/usuariolineas.model';
import { environment } from '../../environments/environment';

@Injectable()
export class UserService extends CrudService<UserDto> {



    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);

        this.identityUrl = environment.identityUrl + '/User';
        this.endpoint = this.identityUrl;

    }

    getUsers(pageIndex: number, pageSize: number, sort: string): Observable<ResponseModel<ListResultDtoOfUserListDto>> {


        let url = this.identityUrl + '/GetPagedList';
        let data = new UserFilter();
        data.Page = pageIndex;
        data.PageSize = pageSize;
        data.Sort = sort;

        return this.http.post<ResponseModel<ListResultDtoOfUserListDto>>(url, data, )
    }


    getUserForEdit(id: number): Observable<ResponseModel<UserDto>> {

        let url = this.identityUrl + '/GetByIdAsync?id=' + id;
        return this.http.get(url).map((response: Response) => {
            return response.json();
        });
    }


    PermiteLoginManualCurrentUser(): Observable<ResponseModel<boolean>> {
        let url = this.identityUrl + '/PermiteLoginManualCurrentUser';
        return this.http.get<ResponseModel<boolean>>(url);
    }

    createOrUpdateUser(data: UserDto): Observable<ResponseModel<UserDto>> {

        let url = this.identityUrl + '/UpdateEntity';
        if (data.Id <= 0) {
            url = this.identityUrl + '/SaveNewEntity';
        }

        return this.http.post<ResponseModel<UserDto>>(url, data)
    }

    getUserPermissionsForEdit(id: number): Observable<ResponseModel<GetPermissionsForEditOutput>> {

        let url = this.identityUrl + '/GetUserPermissionsForEdit?id=' + id;
        return this.http.get<ResponseModel<GetPermissionsForEditOutput>>(url);
    }

    resetPassword(id: number): Observable<ResponseModel<string>> {

        let url = this.identityUrl + '/ResetPassword?id=' + id;
        return this.http.post<ResponseModel<string>>(url, { id: id });

    }

    updateUserPermissions(input: UpdatePermissionsInput): Observable<ResponseModel<string>> {
        let url = this.identityUrl + '/UpdateUserPermissions';
        return this.http.post<ResponseModel<string>>(url, input)
    }




    GetUserLineasForEdit(id: number): Observable<ResponseModel<UpdateLineasForEdit>> {
        let url = this.identityUrl + '/GetUserLineasForEdit?id=' + id;
        return this.http.get<ResponseModel<UpdateLineasForEdit>>(url);
    }

    SetUserLineasForEdit(input: UpdateLineasForEdit): Observable<ResponseModel<string>> {
        let url = this.identityUrl + '/SetUserLineasForEdit';
        return this.http.post<ResponseModel<string>>(url, input);
    }




    findUser(userName: string) {
        let url = this.identityUrl + '/GetByUserLdapAsync?userName=' + userName;


        return this.http.get<ResponseModel<UserDto>>(url);
    }

}


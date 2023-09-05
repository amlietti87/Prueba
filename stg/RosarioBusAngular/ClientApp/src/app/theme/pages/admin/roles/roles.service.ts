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
import { RolDto, RolFilter } from '../model/rol.model';
import { } from '../model/user.model';
import { GetPermissionsForEditOutput, UpdatePermissionsInput } from '../model/permission.model';

@Injectable()
export class RolesService extends CrudService<RolDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Roles';
        this.endpoint = this.identityUrl;
    }

    getRolePermissionsForEdit(id: number): Observable<ResponseModel<GetPermissionsForEditOutput>> {

        let url = this.identityUrl + '/GetRolePermissionsForEdit?id=' + id;
        return this.http.get<ResponseModel<GetPermissionsForEditOutput>>(url);
    }

    updateRolePermissions(input: UpdatePermissionsInput): Observable<ResponseModel<string>> {
        let url = this.identityUrl + '/UpdateRolePermissions';
        return this.http.post<ResponseModel<string>>(url, input)
    }
}


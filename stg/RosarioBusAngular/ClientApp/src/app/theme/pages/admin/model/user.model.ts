import { Dto, PaginListResultDto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";

import * as moment from 'moment';

export class UserDto extends Dto<number> {
    getDescription(): string {
        return this.NomUsuario;
    }

    constructor(data?: any) {
        super(data);
    }

    NomUsuario: string;
    LogonName: string;
    LogicalLogon: string;
    DisplayName: string;
    Mail: string;
    CanonicalName: string;
    DistinguishedName: string;
    UserPrincipalName: string;
    Area: string;
    TpoNroDoc: string;
    TpoDoc: string;
    NroDoc: string;
    TelTrabajo: string;
    TelPersonal: string;
    Baja: string;
    CodEmp: string;
    PermiteLoginManual: boolean;
    SucursalId: number;
    UserRoles: UserRoleDto[];
    EsInspector: boolean;
    GruposInspectoresId: number;
    EmpleadoId: number;
    TurnoId: number;
    DescEmpleado: string;
    displayName: string;
}

export class UserRoleDto {
    RoleId: number;
    RoleName: string;
    RoleDisplayName: string;
    IsAssigned: boolean;
}

export class UserFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    RoleId: number;
    GruposInspectoresId: number;
    TurnoId: number;
    selectEmpleados: ItemDto;
}

export class ListResultDtoOfUserListDto extends PaginListResultDto<UserDto> implements IListResultDtoOfUserListDto {

}

export interface IListResultDtoOfUserListDto {
    Items: UserDto[];
    TotalCount: number;
}
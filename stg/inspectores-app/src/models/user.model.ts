import { Dto, PaginListResultDto } from "./Base/base.model";

export class UserDto extends Dto<number> {
    getDescription(): string {
        return this.NomUsuario;
    }

    constructor(data?: any) {
        super(data);
    }

    NomUsuario: string;
    LogonName: string;
    // LogicalLogon: string;
    // DisplayName: string;
    // Mail: string;
    // CanonicalName: string;
    // DistinguishedName: string;
    // UserPrincipalName: string;
    // Area: string;
    // TpoNroDoc: string;
    // TpoDoc: string;
    // NroDoc: string;
    // TelTrabajo: string;
    // TelPersonal: string;
    // Baja: string;
    // CodEmp: string;
    // PermiteLoginManual: boolean;
     SucursalId: number;
    // UserRoles: UserRoleDto[];

}


export class ListResultDtoOfUserListDto extends PaginListResultDto<UserDto> implements IListResultDtoOfUserListDto {

}

export interface IListResultDtoOfUserListDto {
    Items: UserDto[];
    TotalCount: number;
}
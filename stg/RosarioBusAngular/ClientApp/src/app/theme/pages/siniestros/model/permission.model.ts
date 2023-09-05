

export class FlatPermissionDto implements IFlatPermissionDto {
    ParentName: string;
    Name: string;
    DisplayName: string;
    Description: string;
    IsGrantedByDefault: boolean;
}

export interface IFlatPermissionDto {
    ParentName: string;
    Name: string;
    DisplayName: string;
    Description: string;
    IsGrantedByDefault: boolean;
}


export class GetPermissionsForEditOutput implements IGetPermissionsForEditOutput {
    Permissions: FlatPermissionDto[];
    GrantedPermissionNames: string[];

}

export interface IGetPermissionsForEditOutput {
    Permissions: FlatPermissionDto[];
    GrantedPermissionNames: string[];
}


export class UpdatePermissionsInput implements IUpdatePermissionsInput {
    Id: number;
    GrantedPermissionNames: string[] = [];

}

export interface IUpdatePermissionsInput {
    Id: number;
    GrantedPermissionNames: string[];
}
import { FlatPermissionDto } from "../model/permission.model";



export interface PermissionTreeEditModel {

    Permissions: FlatPermissionDto[];

    GrantedPermissionNames: string[];


}



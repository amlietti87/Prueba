import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class FDAccionesPermitidasDto extends Dto<number> {
    getDescription(): string {
        return this.DisplayName;
    }

    PermissionId: number;
    DisplayName: string;
    TokenPermission: string;
}


export class FDAccionesPermitidasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

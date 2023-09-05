import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { GalponDto } from "../../planificacion/model/subgalpon.model";


export class PlaTalleresIvuDto extends Dto<number> {
    getDescription(): string {
        return this.CodGalNavigation.DesGal;
    }
    CodGal: number;
    CodGalIvu: number;
    CodGalNavigation: GalponDto;
}


export class PlaTalleresIvuFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
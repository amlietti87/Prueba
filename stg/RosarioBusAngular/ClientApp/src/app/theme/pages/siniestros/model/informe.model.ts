import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class InformeDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
}


export class InformeFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

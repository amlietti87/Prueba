import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TiposTareaDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
}


export class TiposTareaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
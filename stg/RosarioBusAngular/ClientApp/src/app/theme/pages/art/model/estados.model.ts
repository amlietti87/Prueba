import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class EstadosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
    Predeterminado: boolean;

}


export class EstadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

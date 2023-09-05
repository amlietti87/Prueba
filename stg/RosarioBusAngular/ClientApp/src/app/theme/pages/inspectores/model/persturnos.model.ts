import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class PersTurnosDto extends Dto<number> {
    getDescription(): string {
        return this.DscTurno;
    }
    DscTurno: string;
    Orden: number;
    isSelected: boolean = false;

}


export class PersTurnosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Anulado: number;
    GrupoInspectoresId: number;
}
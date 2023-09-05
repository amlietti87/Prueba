import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class EstadosDiagramaInspectoresDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    EsBorrador: boolean;
}


export class EstadosDiagramaInspectoresDtoFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    EsBorrador: boolean;
}
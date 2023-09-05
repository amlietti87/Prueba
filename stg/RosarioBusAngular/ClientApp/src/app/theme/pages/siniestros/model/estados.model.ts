import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class EstadosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    OrdenCambioEstado: number;
    Judicial: boolean;
    Anulado: boolean;
    SubEstados: SubEstadosDto[];

}


export class EstadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}


export class SubEstadosDto extends Dto<number> {



    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    EstadoId: number;
    Cierre: boolean;
    Anulado: boolean;
    EstadoNombre: string;
}

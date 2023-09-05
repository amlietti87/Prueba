import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TipoInvolucradoDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Reclamo: boolean;
    Conductor: boolean;
    Vehiculo: boolean;
    MuebleInmueble: boolean;
    Lesionado: boolean;
    Anulado: boolean;
}


export class TipoInvolucradoFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class FDEstadosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    PermiteRechazo: boolean;
    ImportarDocumentoOK: boolean;
    ImagenGrilla: string;
    VpDBDEmpleado: boolean;
}


export class FDEstadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

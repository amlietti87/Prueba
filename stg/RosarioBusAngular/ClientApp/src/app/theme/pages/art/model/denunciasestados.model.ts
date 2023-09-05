import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class DenunciasEstadosDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    DenunciaId: number;
    EstadoId: number;
    Fecha: number;
}


export class DenunciasEstadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

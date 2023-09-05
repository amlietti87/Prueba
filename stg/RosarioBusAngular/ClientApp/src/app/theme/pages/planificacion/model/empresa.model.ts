import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class EmpresaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Nombre: string;
    DesEmpr: string;
    Activo: boolean;
}

export class EmpresaFilter extends FilterDTO {
    EmpresaId: number;
    FecBaja: boolean;

}

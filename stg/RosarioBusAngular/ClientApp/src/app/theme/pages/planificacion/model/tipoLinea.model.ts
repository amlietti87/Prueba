import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';

export class TipoLineaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Nombre: string
    CantidadConductoresPorServicio: number

}

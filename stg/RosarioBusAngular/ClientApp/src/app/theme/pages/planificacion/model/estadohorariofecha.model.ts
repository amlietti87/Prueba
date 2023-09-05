import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class EstadoHorarioFechaDto extends Dto<number> {
    getDescription(): string {
        return "";
    }

    Descripcion: string;

}


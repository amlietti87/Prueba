import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class MotivosNotificacionesDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;

    constructor() {
        super();
        this.Id = 0;
    }
}


export class MotivosNotificacionesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

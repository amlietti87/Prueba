import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { MotivosNotificacionesDto } from "./motivosnotificaciones.model";
import { MotivosNotificacionComboComponent } from "../shared/motivosnotificacion-combo.component";


export class DenunciaNotificacionesDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    DenunciaId: number;
    MotivoId: number;
    Fecha: Date;
    Observaciones: string;
    Anulado: boolean;

    MotivoNotificacion: MotivosNotificacionesDto;

    constructor() {
        super();
        this.MotivoNotificacion = new MotivosNotificacionesDto();
    }

}


export class DenunciasNotificacionesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class LesionadoDto extends Dto<number> {
    getDescription(): string {
        return this.TipoLesionadoDescripcion;
    }

    TipoLesionadoId: number;
    TipoLesionadoDescripcion: string;

}






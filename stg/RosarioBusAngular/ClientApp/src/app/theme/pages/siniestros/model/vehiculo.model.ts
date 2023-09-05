import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { EmpresaDto } from "../../planificacion/model/empresa.model";


export class VehiculoDto extends Dto<number> {
    getDescription(): string {
        return this.Marca + ' ' + this.Modelo + ' ' + this.Dominio;
    }
    Marca: string;
    Modelo: string;
    Dominio: string;
    SeguroId: number;
    Poliza: string;
}






import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { LocalidadesDto } from "./localidad.model";


export class CiaSegurosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Domicilio: string;
    LocalidadId: number;
    selectLocalidades: ItemDto;
    Telefono: string;
    Encargado: string;
    Email: string;
    Anulado: boolean;
}


export class CiaSegurosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    selectLocalidades: ItemDto;
    Localidades: ItemDto[];
    LocalidadesSelect: LocalidadesDto[];
}

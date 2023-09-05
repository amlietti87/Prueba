import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { LocalidadesDto } from "./localidad.model";


export class JuzgadosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    LocalidadId: number;
    selectLocalidades: ItemDto;
    Anulado: boolean;
}


export class JuzgadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    selectLocalidades: ItemDto;
    Localidades: ItemDto[];
    LocalidadesSelect: LocalidadesDto[];
}

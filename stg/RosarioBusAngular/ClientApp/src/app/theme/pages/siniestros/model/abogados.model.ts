import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { LocalidadesDto } from "./localidad.model";


export class AbogadosDto extends Dto<number> {
    getDescription(): string {
        return this.ApellidoNombre;
    }

    ApellidoNombre: string;
    Domicilio: string;
    LocalidadId: number;
    selectLocalidades: ItemDto;
    Telefono: string;
    Celular: string;
    Email: string;
    Anulado: boolean;
}


export class AbogadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    selectLocalidades: ItemDto;
    Localidades: ItemDto[];
    LocalidadesSelect: LocalidadesDto[];
}

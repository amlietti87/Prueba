import { FilterDTO, Dto, ItemDto } from "../../../../shared/model/base.model";
import { MediasVueltasDto } from "./mediasvueltas.model";
import { SelectItem } from "primeng/primeng";


export class HServiciosDto extends ItemDto {
    getDescription(): string {
        return this.Description;
    }
    Id: number;
    CodHconfi: number;
    NumSer: string;
    NroInterno: string;
    Duracion: number;

    HMediasVueltas: MediasVueltasDto[];
    EliminarMV: MediasVueltasDto[];
}

export class HServiciosFilter extends FilterDTO {
    Fecha: string;
    LineaId: number;
    ServicioId: number;
    DescripcionServicio: string;
    CodTdia: number;
    DescTdia: string;
    CodSubg: number;
    CodHfecha: number;
    CodBan: number;



    //para minutos-por-sector.component
    TipoHora: string;
    TipoDeHora: any;
    PlanillaId: string;
    BanderasId: number[];
}

export class ExportarExcelDto {
    CodTdia: number;
    CodSubg: number;
    CodHfecha: number;
    CodBan: number;
    LineaId: number;
    TipoHora: string;
}
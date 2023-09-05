import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import { PuntoDto } from "./punto.model";
import { SectorDto } from "./sector.model";

export class RutaDto extends Dto<number> {

    getDescription(): string {
        return this.Nombre;
    }
    Nombre: string;
    BanderaNombre: null
    AbrBan: string;
    CodigoVarianteLinea: string;
    BanderaId: number;
    EstadoRutaId: number;
    EstadoRutaNombre: string;
    FechaVigenciaDesde: Date;
    FechaVigenciaHasta: Date;
    Puntos: PuntoDto[];
    Sectores: SectorDto[];
    CopyFromRutaId: number;
    EsOriginal: number;
    Calles: string;
    Selected: boolean;
    TipoBanderaId: number;
    CodSec: number;
    CodLin: number;
    SucursalId: number;
    Vigente: boolean;
    Instructions: string;
}


export class RutaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    BanderaId: number;
    EstadoRutaId: number;
}

export class MinutosPorSectorFilter extends RutaFilter {
    SectoresIds: number[];
    TipoDiaId: number;
} 

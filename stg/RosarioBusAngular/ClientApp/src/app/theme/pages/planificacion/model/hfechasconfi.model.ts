import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { retry } from "rxjs/operator/retry";
import { Numeric } from "d3";
import { BanderasCartelDetalleDto, BanderaCartelDto } from "./banderacartel.model";
import { HServiciosDto } from "./hServicios.model";


export class HFechasConfiDto extends Dto<number> {
    getDescription(): string {
        return "";
    }

    FechaDesde: Date;
    FechaHasta: Date;
    CodLinea: number;
    Linea: ItemDto;
    PlaEstadoHorarioFechaId: number;
    DescripcionEstado: number;
    TiposDeDias: string;
    DistribucionDeCochesPorTipoDeDia: PlaDistribucionDeCochesPorTipoDeDiaDto[] = [];
    HBasec: HBasecDto[] = [];
    BeforeMigration: boolean;
    CodTDia: number;
}


export class PlaHorarioFechaLineaListView {
    CodLinea: number;
    DescripcionLinea: Date;
    FechaUltimaModificacion: number;
    Activo: boolean;
}





export class HorarioFechaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    LineaId: number;

}


export class PlaDistribucionDeCochesPorTipoDeDiaDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    CodHfecha: number;
    Motivo: string;
    CodTdia: number;
    CantidadDeCochesEstimados: number;
    CantidadDeConductoresEstimados: number;


    TipoDediaDescripcion: string;
    Estado: number;
    IsNew: boolean;
    Descripcion: string;

}



export class PlaDistribucionDeCochesPorTipoDeDiaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    CodHfecha: number;
    CodTdia: number;
    PlanillaId: String;

}

export class HMediasVueltasImportadaDto {

    NumServicio: string;

    Sale: Date;
    Llega: Date;
    DifMin: number;
    CodBan: number;
    DescripcionBandera: string;
    CodTpoHora: string;
    DescripcionTpoHora: string;
    CodSubGalpon: number;
    DescripcionSubGalpon: string;
    Errors: string[];
    IsValid: boolean;

}


export class ImportarServiciosInput {
    PlanillaId: string;
    // BolBanderasCartelDetalle: BanderasCartelDetalleDto[];
    BolBanderasCartel: BanderaCartelDto;
    CodHfecha: number;
    CodLinea: number;
    desde: number;
    hasta: number;
    CodTdia: number;
}



export class HBasecDto {

    CodHfecha: number;
    CodBan: number;
    CodSec: number;
    CodGal: number;
    VendeBoletos: boolean;
    CodSecbsas: number;
    CodRec: number;
    DescripcionBandera: string;
    DescripcionSentido: string;
    DescripcionAbreviatura: string;
    Recorido: string;
    NroSecuencia: string;
    Selected: boolean;
    Kmr: number;
    Km: number;
    CodBanderaColor: number;
    CodBanderaTup: number;
    DescripcionBanderaColor: string;
    DescripcionBanderaTup: string;
    TextoBandera: string;
    Movible: string;
    ObsBandera: string;
}




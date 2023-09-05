import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { retry } from "rxjs/operator/retry";
import { HServiciosDto } from "./hServicios.model";
import { BanderaDto } from "./bandera.model";


export class HHorariosConfiDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    CodHfecha: number;
    CodTdia: number;
    CodSubg: number;
    CantCoches: number;
    CantConductores: number;
    CantidadCochesReal: number;
    CantidadConductoresReal: number;
    CodHconfibsas: number;

    CurrentServicio: HServiciosDto;
}





export class HHorariosConfiFilter extends FilterDTO {
    CodTdia: number;
    CodSubg: number;
    CodHfecha: number;
    ServicioId: number;

}




export class DetalleSalidaRelevosFilter extends FilterDTO {
    cod_lin: number;
    cod_hfecha: number;
    codTdia: number;

}



export class ReporteHorarioPasajerosFilter extends FilterDTO {
    cod_lin: number;
    codHfecha: number;
    codTdia: number;
    BanderasIda: number[] = [];
    BanderasVueltas: number[] = [];
    SentidoOrigen: string;
    SentidoDestino: string;

}


export class ReporteDistribucionCochesFilter extends FilterDTO {

    lineList: number[] = [];
    codTdia: number;
    fecha: Date;

}

export class ReportePasajerosFilter extends FilterDTO {

    LineaId: number;
    Banderas: number[] = [];

}



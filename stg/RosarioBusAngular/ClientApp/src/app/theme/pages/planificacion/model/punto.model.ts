import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { RbMapMarker } from "../../../../components/rbmaps/RbMapMarker";



export class PuntoFilter extends FilterDTO {
    Id: string;
    Page: number;
    PageSize: number;
    Sort: String;
    RutaId: number;

    CodRec: number;
    CambioSectoresMapa: boolean;
    ParadasMapa: boolean;
    CodRecs: number[] = [];
}


export class PuntoDto extends Dto<string> {

    constructor() {
        super();

        this.Polylines = [];
        this.Steps = [];
        this.CodigoNombre = "";

    }

    getDescription(): string {
        return this.CodigoNombre;
    }

    setGmapMarker(marker: RbMapMarker) {
        this.Lat = marker.lat;
        this.Long = marker.lng;
        this.Id = marker.id;
    }

    Lat: number;
    Long: number;
    CodigoNombre: string;
    Data: string;
    RutaId: number;
    EsPuntoInicio: boolean;
    EsPuntoTermino: boolean;
    EsParada: boolean;
    EsCambioSector: boolean;
    EsPuntoRelevo: boolean;
    EsCambioSectorTarifario: boolean;
    MostrarEnReporte: boolean;
    Orden: number;
    TipoParadaId: number;
    NumeroExterno: string;
    Abreviacion: string;
    TipoBanderaId: number;
    Polylines: any[];
    Steps: any[];
    Instructions: any[];
    LineColor: string;
    Color: string;
    PlaCoordenadaId: number;
    PlaCoordenadaItem: ItemDto;
    PlaCoordenadaAnulada: boolean;
    PlaCoordenadaCalle1: string;
    PlaCoordenadaCalle2: string;
    CodSectorTarifario: number;
    //SectoresTarifarioId: number;
    SectoresTarifariosItem: ItemDto;
    PlaParadaItem: ItemDto;
    PlaParadaId: number;
    PlaTipoViajeId: number;
}


import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class HorariosPorSectorDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    Colulmnas: ColumnasDto[];
    Items: RowHorariosPorSectorDto[];
    Linea: string;
    LineaId: number;
    TipoDia: string;
    FechaHorario: string;
    FechaDesde: Date;
    LabelBandera: string;
    EmpresaId: number;
    Empresa: string;
    TipoInforme: number;
}


export class ColumnasDto {
    EsFija: boolean
    Key: string
    Label: string
}


export class RowHorariosPorSectorDto {
    Sale: string;
    Llega: string;
    Servicio: string;
    TotalDeMinutos: string;
    TipoHora: string;
    Bandera: string;
    Diferencia: string;
    cod_mvuelta: number;
    CodSubGalpon: number;
    ColumnasDinamicas: ColumnasDataDto[]
}

export class ColumnasDataDto {
    EsFija: boolean
    Key: string
    value: any
    Orden: number
    Hora: string
    HoraFormated: string
    DescripcionSector: string
    EsRelevo: boolean
}



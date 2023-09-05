import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import { DataTypeDto } from "./dataType.model";

export const RealTimeTracking = 'Inspectores_Geo_RealTimeTracking';
export const TiemposEntreHistorico = 'Inspectores_Geo_TiemposEntreHistorico';
export const TiempoEntreEnvios = 'Inspectores_Geo_TiempoEntreEnvios';
export const CantidadIntentosLoginKey = 'Loguin_CantidadIntentos';

export class ParametersDto extends Dto<number>{
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    Description: string;
    Value: string;
    Token: string;
    DataTypeId: number;
}


export class ParametersFilter extends FilterDTO {
    Token: string;
}
import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import { RespuestasIncognitosDto } from "./respuestasIncognitos.model";


export class PreguntasIncognitosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    RespuestaRequerida: boolean;
    MostrarObservacion: boolean;
    Orden: number;
    Anulado: boolean;
    InspPreguntasIncognitosRespuestas: InspPreguntasIncognitosRespuestasDto[];
}

export class PreguntasIncognitosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Anulado: number;
}

export class InspPreguntasIncognitosRespuestasDto extends Dto<number>
{
    Id: number;

    PreguntaIncognitoId: number;
    RespuestaId: number;
    Orden: number;
    Respuesta: RespuestasIncognitosDto;
    RespuestaNombre: string;

    getDescription(): string {
        return 'err';
    }
}


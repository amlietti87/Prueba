import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class PersTopesHorasExtrasDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    CodTopeHorasExtras: number;
    CodGalpon: number;
    Hs50Persona: number;
    Hs50Taller: number;
    FrancosTrabajadosPersona: number;
    FrancosTrabajadosTaller: number;
    MinutosNocturnosPersona: number;
    MinutosNocturnosTaller: number;
    FeriadosPersona: number;
    FeriadosTaller: number;
    HoraFeriado: Date;
    HoraFranco: Date;
    CodArea: number;
    HorasComunes: Date;
    IdGrupoInspectores: number;
    PermiteHsExtrasFeriado: boolean;
    PermiteFrancosTrabajadosFeriado: boolean;

    MinutosFeriados: number;
    MinutosFrancos: number;
    MinutosComunes: number;
}


export class PersTopesHorasExtrasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;

}
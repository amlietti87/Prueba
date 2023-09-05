import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class FDTiposDocumentosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    Prefijo: string;
    RequiereLider: boolean;
    EsPredeterminado: boolean;
    EsPredeterminadoOriginal: boolean;
    Anulado: boolean;
}


export class FDTiposDocumentosFilter extends FilterDTO {
    EsPredeterminado: boolean;
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

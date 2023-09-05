import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class PlaLineaLineaHorariaDto extends Dto<string> {
    getDescription(): string {
        return this.Description;
    }



    PlaLineaId: number;
    LineaId: number;

    DescripcionLinea: string;
    DescripcionPlaLinea: string;


    animate: boolean;
    IsSelected: boolean;

} 

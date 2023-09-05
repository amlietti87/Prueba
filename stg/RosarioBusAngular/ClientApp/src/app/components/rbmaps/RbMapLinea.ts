import { RbMapServices } from "../rbmaps/RbMapServices";
import { CustomMarker } from "./RbMapMarker";

export class RbMapLinea {
    id: string;
    grupoId: string;
    nombre: string;
    color: string;
    diametro: number;
    points: CustomMarker[];

    constructor() {
        this.id = RbMapServices.guid();
        this.diametro = 2;
        this.points = new Array() as CustomMarker[];
        this.color = "#000";
    }

    agregarPunto(punto: CustomMarker): void {
        this.points.push(punto);
    }
}
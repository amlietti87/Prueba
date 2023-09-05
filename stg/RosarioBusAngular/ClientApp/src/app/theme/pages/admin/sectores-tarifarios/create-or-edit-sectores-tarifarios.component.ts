import { Component, Injector } from "@angular/core";
import { SectoresTarifariosDto } from "../../planificacion/model/sectoresTarifarios.model";
import { DetailModalComponent } from "../../../../shared/manager/detail.component";
import { SectoresTarifariosService } from "../../planificacion/sectoresTarifarios/sectoresTarifarios.service";

@Component({
    selector: 'create-or-edit-sectores-tarifarios',
    templateUrl: './create-or-edit-sectores-tarifarios.component.html'
})
export class CreateOrEditSectoresTarifariosComponent extends DetailModalComponent<SectoresTarifariosDto> {
    constructor(private sectoresTarifariosService: SectoresTarifariosService, injector: Injector) {
        super(sectoresTarifariosService, injector);
        this.title = "Sectores Tarifarios";
        this.icon = "flaticon-settings";
    }
}
import { Component, Injector, AfterViewInit, ViewEncapsulation } from "@angular/core";
import { CrudComponent } from "../../../../shared/manager/crud.component";
import { SectoresTarifariosDto } from "../../planificacion/model/sectoresTarifarios.model";
import { SectoresTarifariosService } from "../../planificacion/sectoresTarifarios/sectoresTarifarios.service";
import { CreateOrEditSectoresTarifariosComponent } from "./create-or-edit-sectores-tarifarios.component";

@Component({
    templateUrl: "./sectores-tarifarios.component.html"
})
export class SectoresTarifariosComponent extends CrudComponent<SectoresTarifariosDto> implements AfterViewInit {
    constructor(private sectoresTarifariosService: SectoresTarifariosService, injector: Injector) {
        super(sectoresTarifariosService, CreateOrEditSectoresTarifariosComponent, injector);
        this.title = "Sectores Tarifarios";
        this.moduleName = "sectoresTarifarios";
        this.isFirstTime = true;
        this.showbreadcum = false;
        this.icon = "flaticon-settings";
    }

    ngAfterViewInit(): void {
        super.ngAfterViewInit();
    }

    SetAllowPermission(): void {
        super.SetAllowPermission();
    }

    getNewItem(item: SectoresTarifariosDto): SectoresTarifariosDto {
        return new SectoresTarifariosDto(item);
    }
}
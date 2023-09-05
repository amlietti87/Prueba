import { InspRespuestasIncognitosFilter } from './../model/respuestasIncognitos.model';
import { RespuestasIncognitosService } from './respuestasIncognitos.service';
import { RespuestasIncognitosDto } from '../model/respuestasIncognitos.model';
import { Component, ViewEncapsulation, AfterViewInit, Injector, Type, ViewChild, ChangeDetectorRef } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditRespuestasModalComponent } from './create-or-edit-respuestas-modal.component';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { ViewMode } from '../../../../shared/model/base.model';

@Component({

    templateUrl: "./respuestasIncognitos.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class RespuestasIncognitosComponent extends BaseCrudComponent<RespuestasIncognitosDto, InspRespuestasIncognitosFilter> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditRespuestasModalComponent;
    }

    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(injector: Injector, protected _respuestaService: RespuestasIncognitosService, protected cdr: ChangeDetectorRef) {
        super(_respuestaService, CreateOrEditRespuestasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Respuesta Incógnitos"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.Anulado);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
    }

    getDescription(item: RespuestasIncognitosDto): string {
        return item.Description;
    }
    getNewItem(item: RespuestasIncognitosDto): RespuestasIncognitosDto {

        var item = new RespuestasIncognitosDto();
        return item;
    }

    getNewfilter(): InspRespuestasIncognitosFilter {
        var filter = new InspRespuestasIncognitosFilter();
        filter.Anulado = 2;
        return filter;
    }

    onEdit(row: RespuestasIncognitosDto) {
        this.onEditID(row.Id);
    }

    onEditID(id: any) {
        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(e => this.Opendialog(e.DataObject, ViewMode.Modify));
        }
        else {
            this.GetEditComponent().show(id);
        }
    }
}
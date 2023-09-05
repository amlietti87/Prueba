import { PreguntasIncognitosService } from './preguntasincognitos.service';
import { PreguntasIncognitosDto, PreguntasIncognitosFilter } from './../model/preguntasincognitos.model';
import { Component, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';
import { BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { ViewMode } from '../../../../shared/model/base.model';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { CreateOrEditPreguntasIncognitosModalComponent } from './create-or-edit-preguntasincognitos-modal.component';

@Component({

    templateUrl: "./preguntasincognitos.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class PreguntasIncognitosComponent extends BaseCrudComponent<PreguntasIncognitosDto, PreguntasIncognitosFilter> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditPreguntasIncognitosModalComponent
    }
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(injector: Injector, protected _preguntasService: PreguntasIncognitosService, protected cdr: ChangeDetectorRef) {
        super(_preguntasService, CreateOrEditPreguntasIncognitosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Pregunta Incógnitos"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: PreguntasIncognitosDto): string {
        return item.Description
    }
    getNewItem(item: PreguntasIncognitosDto): PreguntasIncognitosDto {

        var item = new PreguntasIncognitosDto(item);
        return item;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.Anulado);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    ngOnInit() {
        super.ngOnInit();
    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
    }

    getNewfilter(): PreguntasIncognitosFilter {
        var filter = new PreguntasIncognitosFilter();
        filter.Anulado = 2;
        return filter;
    }

    onEdit(row: PreguntasIncognitosDto) {
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
import { OnInit, Component, ViewEncapsulation, Injector, AfterViewInit, Type, ViewChild, ChangeDetectorRef } from "@angular/core";
import { BaseCrudComponent } from "../../../../shared/manager/crud.component";
import { TareaDto, TareaFilter } from "../model/tarea.model";
import { TareaService } from "./tarea.service";
import { CreateOrEditTareaModalComponent } from "./create-or-edit-tarea-modal.component";
import { IDetailComponent } from "../../../../shared/manager/detail.component";
import { LazyLoadEventData } from "../../../../shared/helpers/PrimengDatatableHelper";
import { YesNoAllComboComponent } from "../../../../shared/components/yesnoall-combo.component";
import { ViewMode } from "../../../../shared/model/base.model";


@Component({
    templateUrl: "./tareas.component.html",
    encapsulation: ViewEncapsulation.None,
    exportAs: 'Tareas',
    styleUrls: ["./tareas.component.css"]
})
export class TareasComponent extends BaseCrudComponent<TareaDto, TareaFilter> implements AfterViewInit {
        
    filter : TareaFilter;
    allowVisualizar: boolean;
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(service: TareaService, injector: Injector, protected cdr: ChangeDetectorRef) {
        super(service, CreateOrEditTareaModalComponent, injector);        
        this.isFirstTime = true;
        this.title = "Tareas"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
        this.SetAllowPermission();
    }
    ngOnInit(): void {
        this.generateFilter();
        super.ngOnInit();
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.cdr.detectChanges();
    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTareaModalComponent
    }

    generateFilter() {
        this.filter = new TareaFilter();
        this.filter.Anulado = false;
        this.filter.Descripcion = "";
    }
    
    onClearFilters() : void{
       this.generateFilter();
    }

    SetAllowPermission() {
        this.allowAdd = this.permission.isGranted('Inspectores.Tareas.Agregar');
        this.allowModify = this.permission.isGranted('Inspectores.Tareas.Modificar');
        this.allowVisualizar = this.permission.isGranted('Inspectores.Tareas.Visualizar');
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
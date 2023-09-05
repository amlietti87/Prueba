import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { FDTiposDocumentosDto, FDTiposDocumentosFilter } from '../model/fdtiposdocumentos.model';
import { FDTiposDocumentosService } from '../services/fdtiposdocumentos.service';
import { UserDto } from '../../admin/model/user.model';
import { EditUserLineasModalComponent } from '../../admin/users/edit-user-lineas-modal.component';
import { EditUserPermissionsModalComponent } from '../../admin/users/edit-user-permissions-modal.component';
import { CreateOrEditTipoDocumentoModalComponent } from './create-or-edit-tipoDocumento-modal.component';
import { ViewMode } from '../../../../shared/model/base.model';
import { FdAccionesService } from '../services/fdacciones.service';
import { FDAccionesDto } from '../model/fdacciones.model';

@Component({

    templateUrl: "./tipodocumento.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoDocumentoComponent extends CrudComponent<FDTiposDocumentosDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoDocumentoModalComponent
    }

    constructor(injector: Injector, protected _TipoDocumentoService: FDTiposDocumentosService, protected fdAccionesService: FdAccionesService) {
        super(_TipoDocumentoService, CreateOrEditTipoDocumentoModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Tipo de Documentos"
        this.moduleName = "Firma Digital";
        this.icon = "flaticon-settings";
        // this.loadInMaterialPopup = false;
        this.advancedFiltersAreShown = true;
    }

    getDescription(item: FDTiposDocumentosDto): string {
        return item.Descripcion;
    }

    getNewItem(item: FDTiposDocumentosDto): FDTiposDocumentosDto {
        return new FDTiposDocumentosDto(item);
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }


    ngOnInit() {
        super.ngOnInit();
    }

    getNewfilter(): FDTiposDocumentosFilter {
        return new FDTiposDocumentosFilter();
    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
    }

    onDelete(item: any) {
        if (!this.allowDelete) {
            return;
        }

        if (item.EsPredeterminado) {

            this.message.info("No se puede eliminar el Tipo de Documento, es predeterminado", "Aviso");
        }
        else {

            var strindto = this.getDescription(item);
            //var aa = this.getNewItem(item);
            //var stringentity = aa.getDescription();

            this.message.confirm('¿Está seguro de que desea eliminar el registro?', strindto || 'Confirmación', (a) => {

                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    this.service.delete(item.Id)
                        .subscribe(() => {
                            this.Search();
                            this.notify.success(this.l('Registro eliminado correctamente'));
                        });
                }
            });
        }
    }
}
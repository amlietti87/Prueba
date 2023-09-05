import { OnInit, Component, ViewEncapsulation, Injector, Type } from "@angular/core";
import { BaseCrudComponent } from "../../../../shared/manager/crud.component";
import { IDetailComponent } from "../../../../shared/manager/detail.component";
import { CreateOrEditUserModalComponent } from "../../admin/users/create-or-edit-user-modal.component";
import { FdCertificadosService } from "../services/fdcertificados.service";
import { FDCertificadosFilter, FDCertificadosDto } from '../model/fdcertificados.model';
import { MatDialog, MatDialogConfig } from "@angular/material";
import { UserDto } from '../../admin/model/user.model';
import { FileDTO } from "../../../../shared/common/models/fileDTO.model";
import { FileService } from "../../../../shared/common/file.service";
import { CertificadoEmailComponent } from "./certificado-email.component";

@Component({

    templateUrl: "./mi-certificado.component.html",
    encapsulation: ViewEncapsulation.None
})
export class MiCertificadoComponent extends BaseCrudComponent<FDCertificadosDto, FDCertificadosFilter> implements OnInit {


    GetEditComponent(): IDetailComponent {
        return null;
    }

    public certificadoFilter: FDCertificadosFilter;
    public allowDownload: boolean = false;
    public allowSendByEmail: boolean = false;
    public allowSendEmail: boolean = false;
    public dialog: MatDialog;
    public currentUser: any;
    public showTable: boolean = true;
    constructor(injector: Injector, protected certificadosService: FdCertificadosService, protected fileService: FileService) {
        super(certificadosService, null, injector);

        this.title = "Mi Certificado"
        this.moduleName = "Mi Certificado";
        this.icon = "flaticon-users";
        this.showbreadcum = false;
        this.dialog = injector.get(MatDialog);
        this.allowDownload = this.permission.isGranted("FirmaDigital.MiCertificado.Descargar");
        this.allowSendEmail = this.permission.isGranted("FirmaDigital.MiCertificado.EnviarCorreo");
    }

    ngOnInit() {

        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.currentUser = currentUser;
        this.searhCertificate();
    }

    searhCertificate() {

        var filter = new FDCertificadosFilter()
        filter.Activo = true;
        this.primengDatatableHelper.isLoading = true;
        this.certificadosService.searhCertificate(filter)
            .subscribe(e => {
                if (e.DataObject.Items.length == 1) {
                    this.showTable = true;
                    this.primengDatatableHelper.records = e.DataObject.Items;
                } else {
                    this.showTable = false;
                }
                this.primengDatatableHelper.isLoading = false;
            });
    }

    downloadCertificate(certificado: FDCertificadosDto) {
        var filter = new FDCertificadosFilter();
        filter.ArchivoId = certificado.ArchivoId;
        this.certificadosService.downloadCertificate(filter)
            .subscribe(e => this.DownloadFile(e.DataObject))
    }

    DownloadFile(file: FileDTO) {
        var url = this.fileService.getBlobURL(file);
        if (file.ForceDownload) {
            var a = window.document.createElement('a');
            a.href = url;
            a.download = file.FileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        } else {
            window.open(url, "_blank");
        }
    }

    sendEmail(certificado: FDCertificadosDto) {
        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new FDCertificadosFilter();
        dialogConfig.data.ArchivoId = certificado.ArchivoId;
        dialogConfig.data.UserEmail = this.currentUser.email
        dialogConfig.width = '60%';

        //Declarar el historialcertificadoscomponent en el modulo de firma.
        let dialogRef = this.dialog.open(CertificadoEmailComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(
            data => {
                if (data == "OK") {
                    this.message.success("Correo enviado exitosamente");
                }
            }
        )
    }
}
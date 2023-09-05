import { ParametersDto } from "../model/parameters.model";
import { Component, Injector, OnInit } from "@angular/core";
import { DetailModalComponent, DetailEmbeddedComponent } from "../../../../shared/manager/detail.component";
import { ParametersService } from "./parameters.service";

@Component(
    {
        selector: 'createOrEditParametersModal',
        templateUrl: './create-or-edit-parameters-modal.component.html',

    }
)
export class CreateOrEditParametersModalComponent extends DetailEmbeddedComponent<ParametersDto> implements OnInit {
    constructor(injector: Injector, protected service: ParametersService
    ) {
        super(service, injector);
        this.detail = new ParametersDto();
    }
}
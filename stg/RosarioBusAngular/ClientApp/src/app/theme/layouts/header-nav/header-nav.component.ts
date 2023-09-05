import { Component, OnInit, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { Helpers } from '../../../helpers';
import { User } from '../../../auth/models/index';
import { AuthService } from '../../../auth/auth.service';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ResponseModel, Dto, PaginListResultDto, StatusResponse, ItemDto } from '../../../shared/model/base.model';
import { Observable } from 'rxjs/Observable';


declare let mQuicksearch: any;
declare let mLayout: any;
declare let mUtil: any;

@Component({
    selector: "app-header-nav",
    templateUrl: "./header-nav.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class HeaderNavComponent implements OnInit, AfterViewInit {

    //TODO ver de tipar con otro objeto
    user: User = new User();
    quicksearch: any;

    itemResult: SearchResultDto;


    constructor(private _authService: AuthService, protected http: HttpClient) {

    }
    ngOnInit() {

        var ud = this._authService.GetUserData();

        this.user.email = ud.email;
        this.user.fullName = ud.displayName;

    }
    ngAfterViewInit() {
        mLayout.initHeader();



        this.quicksearch = new mQuicksearch('m_quicksearch_2', {
            mode: mUtil.attr('m_quicksearch', 'm-quicksearch-mode'), // quick search type
            minLength: 1
        });

        //<div class="m-search-results m-search-results--skin-light"><span class="m-search-result__message">Something went wrong</div></div>



        var url = environment.identityUrl + '/search';

        var self = this;
        this.quicksearch.on('search', function(the) {
            the.showProgress();
            if (the.query) {
                self.http.get<ResponseModel<SearchResultDto>>(url + '?FilterText=' + the.query).subscribe(result => {
                    the.hideProgress();
                    var htmlresult = '';
                    if (result.Status == StatusResponse.Ok) {
                        self.itemResult = result.DataObject;
                    }
                }, err => {
                    the.hideProgress();
                });
            }
            else {
                self.itemResult = new SearchResultDto();
            }

        });
    }





    search(FilterText: string): Observable<ResponseModel<SearchResultDto>> {

        let url = environment.identityUrl + '/search?FilterText=' + FilterText;
        return this.http.get<ResponseModel<SearchResultDto>>(url);
    }

}


export class SearchResultDto {

    TotalRegistrosEncontrados: number;
    Banderas: SearchItemDto[];
    Lineas: SearchItemDto[];
    Ramales: RamalItemDto[];
}

export class SearchItemDto extends ItemDto {
    SucursalId: number
}

export class RamalItemDto extends SearchItemDto {
    LineaID: number;
}




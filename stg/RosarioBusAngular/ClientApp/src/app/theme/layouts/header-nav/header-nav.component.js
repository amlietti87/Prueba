"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var index_1 = require("../../../auth/models/index");
var auth_service_1 = require("../../../auth/auth.service");
var environment_1 = require("../../../../environments/environment");
var http_1 = require("@angular/common/http");
var base_model_1 = require("../../../shared/model/base.model");
var HeaderNavComponent = /** @class */ (function () {
    function HeaderNavComponent(_authService, http) {
        this._authService = _authService;
        this.http = http;
        //TODO ver de tipar con otro objeto
        this.user = new index_1.User();
    }
    HeaderNavComponent.prototype.ngOnInit = function () {
        var ud = this._authService.GetUserData();
        this.user.email = ud.email;
        this.user.fullName = ud.displayName;
    };
    HeaderNavComponent.prototype.ngAfterViewInit = function () {
        mLayout.initHeader();
        this.quicksearch = new mQuicksearch('m_quicksearch_2', {
            mode: mUtil.attr('m_quicksearch', 'm-quicksearch-mode'),
            minLength: 1
        });
        //<div class="m-search-results m-search-results--skin-light"><span class="m-search-result__message">Something went wrong</div></div>
        var url = environment_1.environment.identityUrl + '/search';
        var self = this;
        this.quicksearch.on('search', function (the) {
            the.showProgress();
            if (the.query) {
                self.http.get(url + '?FilterText=' + the.query).subscribe(function (result) {
                    the.hideProgress();
                    var htmlresult = '';
                    if (result.Status == base_model_1.StatusResponse.Ok) {
                        self.itemResult = result.DataObject;
                    }
                }, function (err) {
                    the.hideProgress();
                });
            }
            else {
                self.itemResult = new SearchResultDto();
            }
        });
    };
    HeaderNavComponent.prototype.search = function (FilterText) {
        var url = environment_1.environment.identityUrl + '/search?FilterText=' + FilterText;
        return this.http.get(url);
    };
    HeaderNavComponent = __decorate([
        core_1.Component({
            selector: "app-header-nav",
            templateUrl: "./header-nav.component.html",
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [auth_service_1.AuthService, http_1.HttpClient])
    ], HeaderNavComponent);
    return HeaderNavComponent;
}());
exports.HeaderNavComponent = HeaderNavComponent;
var SearchResultDto = /** @class */ (function () {
    function SearchResultDto() {
    }
    return SearchResultDto;
}());
exports.SearchResultDto = SearchResultDto;
var SearchItemDto = /** @class */ (function (_super) {
    __extends(SearchItemDto, _super);
    function SearchItemDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SearchItemDto;
}(base_model_1.ItemDto));
exports.SearchItemDto = SearchItemDto;
var RamalItemDto = /** @class */ (function (_super) {
    __extends(RamalItemDto, _super);
    function RamalItemDto() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return RamalItemDto;
}(SearchItemDto));
exports.RamalItemDto = RamalItemDto;
//# sourceMappingURL=header-nav.component.js.map
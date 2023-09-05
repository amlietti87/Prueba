"use strict";
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
var http_1 = require("@angular/common/http");
var helpers_1 = require("../../helpers");
var FileService = /** @class */ (function () {
    function FileService(http) {
        this.http = http;
    }
    FileService.prototype.downloadAnonymousFileByUrl = function (url) {
        window.open(url, '_blank');
    };
    FileService.prototype.dowloadAuthenticatedByPost = function (url, params) {
        var _this = this;
        helpers_1.Helpers.setLoading(true);
        return this.http.post(url, params).finally(function () {
            return helpers_1.Helpers.setLoading(false);
        }).subscribe(function (ret) {
            var url = _this.getBlobURL(ret.DataObject);
            if (ret.DataObject.ForceDownload) {
                var a = window.document.createElement('a');
                a.href = url;
                a.download = ret.DataObject.FileName;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            }
            else {
                window.open(url, "_blank");
            }
        });
    };
    FileService.prototype.dowloadSabanAuthenticatedByPost = function (url, params) {
        return this.http.post(url, params);
    };
    FileService.prototype.dowloadListAuthenticatedByPost = function (url, params) {
        return this.http.post(url, params);
    };
    FileService.prototype.getBlobURL = function (file) {
        var raw = window.atob(file.ByteArray);
        var rawLength = raw.length;
        var array = new Uint8Array(new ArrayBuffer(rawLength));
        for (var i = 0; i < rawLength; i++) {
            array[i] = raw.charCodeAt(i);
        }
        var url = window.URL.createObjectURL(new Blob([array], { type: file.FileType }));
        return url;
    };
    FileService.prototype.dowloadAuthenticatedByGet = function (url, params) {
    };
    FileService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient])
    ], FileService);
    return FileService;
}());
exports.FileService = FileService;
//# sourceMappingURL=file.service.js.map
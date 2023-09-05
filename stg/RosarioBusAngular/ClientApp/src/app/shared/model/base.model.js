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
Object.defineProperty(exports, "__esModule", { value: true });
var IFilterDTO = /** @class */ (function () {
    function IFilterDTO() {
    }
    return IFilterDTO;
}());
exports.IFilterDTO = IFilterDTO;
var FilterDTO = /** @class */ (function () {
    function FilterDTO() {
    }
    return FilterDTO;
}());
exports.FilterDTO = FilterDTO;
var ADto = /** @class */ (function () {
    function ADto(data) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    this[property] = data[property];
            }
        }
    }
    return ADto;
}());
exports.ADto = ADto;
var Dto = /** @class */ (function (_super) {
    __extends(Dto, _super);
    function Dto(data) {
        return _super.call(this, data) || this;
    }
    return Dto;
}(ADto));
exports.Dto = Dto;
var ItemDto = /** @class */ (function (_super) {
    __extends(ItemDto, _super);
    function ItemDto(data) {
        var _this = _super.call(this, data) || this;
        _this.animate = false;
        return _this;
    }
    ItemDto.prototype.getDescription = function () {
        return this.Description;
    };
    return ItemDto;
}(Dto));
exports.ItemDto = ItemDto;
var ItemDtoStr = /** @class */ (function (_super) {
    __extends(ItemDtoStr, _super);
    function ItemDtoStr(data) {
        var _this = _super.call(this, data) || this;
        _this.animate = false;
        return _this;
    }
    ItemDtoStr.prototype.getDescription = function () {
        return this.Description;
    };
    return ItemDtoStr;
}(Dto));
exports.ItemDtoStr = ItemDtoStr;
var GroupItemDto = /** @class */ (function (_super) {
    __extends(GroupItemDto, _super);
    function GroupItemDto(data) {
        var _this = _super.call(this, data) || this;
        _this.Items = [];
        return _this;
    }
    GroupItemDto.prototype.getDescription = function () {
        return this.Description;
    };
    return GroupItemDto;
}(Dto));
exports.GroupItemDto = GroupItemDto;
var PaginListResultDto = /** @class */ (function () {
    function PaginListResultDto() {
    }
    return PaginListResultDto;
}());
exports.PaginListResultDto = PaginListResultDto;
var PagedRequestDto = /** @class */ (function () {
    function PagedRequestDto() {
    }
    return PagedRequestDto;
}());
exports.PagedRequestDto = PagedRequestDto;
var ResponseModel = /** @class */ (function () {
    function ResponseModel() {
        this.Messages = [];
    }
    return ResponseModel;
}());
exports.ResponseModel = ResponseModel;
var UserFilter = /** @class */ (function (_super) {
    __extends(UserFilter, _super);
    function UserFilter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return UserFilter;
}(FilterDTO));
exports.UserFilter = UserFilter;
var Result;
(function (Result) {
    Result[Result["Ok"] = 0] = "Ok";
    Result[Result["Default"] = 1] = "Default";
})(Result = exports.Result || (exports.Result = {}));
var ViewMode;
(function (ViewMode) {
    ViewMode[ViewMode["Undefined"] = 0] = "Undefined";
    ViewMode[ViewMode["Add"] = 1] = "Add";
    ViewMode[ViewMode["Modify"] = 2] = "Modify";
    ViewMode[ViewMode["Delete"] = 3] = "Delete";
    ViewMode[ViewMode["List"] = 4] = "List";
    ViewMode[ViewMode["View"] = 5] = "View";
    ViewMode[ViewMode["Historico"] = 6] = "Historico";
})(ViewMode = exports.ViewMode || (exports.ViewMode = {}));
var SabanaViewMode;
(function (SabanaViewMode) {
    SabanaViewMode[SabanaViewMode["Sabana"] = 0] = "Sabana";
    SabanaViewMode[SabanaViewMode["Servicio"] = 1] = "Servicio";
    SabanaViewMode[SabanaViewMode["Bandera"] = 2] = "Bandera";
})(SabanaViewMode = exports.SabanaViewMode || (exports.SabanaViewMode = {}));
var StatusResponse;
(function (StatusResponse) {
    StatusResponse["Ok"] = "Ok";
    StatusResponse["Fail"] = "Fail";
    StatusResponse["Other"] = "Other";
})(StatusResponse = exports.StatusResponse || (exports.StatusResponse = {}));
//# sourceMappingURL=base.model.js.map
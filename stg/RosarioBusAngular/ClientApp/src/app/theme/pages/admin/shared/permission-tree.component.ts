import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter } from '@angular/core';


import * as _ from 'lodash';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { PermissionTreeEditModel } from './permission-tree-edit.model';


@Component({
    selector: 'permission-tree',
    template:
    `<div class="permission-tree"></div>`
})
export class PermissionTreeComponent extends AppComponentBase implements OnInit, AfterViewInit, AfterViewChecked {

    set editData(val: PermissionTreeEditModel) {
        this._editData = val;
        this.refreshTree();

    }

    private _$tree: JQuery;
    private _editData: PermissionTreeEditModel;
    private _createdTreeBefore;

    constructor(private _element: ElementRef,
        injector: Injector
    ) {
        super(injector);
    }

    ngOnInit(): void {
    }

    ngAfterViewInit(): void {
        this._$tree = $(this._element.nativeElement);

        this.refreshTree();
    }

    ngAfterViewChecked(): void {

    }

    getGrantedPermissionNames(): string[] {

        if (!this._$tree || !this._createdTreeBefore) {
            return [];
        }

        let permissionNames = [];

        let selectedPermissions = this._$tree.jstree('get_selected', true);
        for (let i = 0; i < selectedPermissions.length; i++) {
            permissionNames.push(selectedPermissions[i].original.id);
        }

        return permissionNames;
    }

    refreshTree(): void {

        let self = this;

        if (this._createdTreeBefore) {
            this._$tree.jstree('destroy');
        }

        this._createdTreeBefore = false;

        if (!this._editData || !this._$tree) {
            return;
        }


        let treeData = _.map(this._editData.Permissions, function(item) {
            return {
                id: item.Name,
                parent: item.ParentName ? item.ParentName : '#',
                text: item.DisplayName,
                state: {
                    opened: true,
                    selected: _.includes(self._editData.GrantedPermissionNames, item.Name)
                }
            };
        });


        this._$tree.jstree({
            "core": {
                //"multiple": true,
                "data": treeData,
            },
            "checkbox": {
                "keep_selected_style": true
            },
            "plugins": ["wholerow", "checkbox"],
        });


        this._createdTreeBefore = true;

        let inTreeChangeEvent = false;

        //this._$tree.jstree({
        //    'core': {
        //        data: treeData
        //    },
        //    'types': {
        //        'default': {
        //            'icon': 'fa fa-folder m--font-warning'
        //        },
        //        'file': {
        //            'icon': 'fa fa-file m--font-warning'
        //        }
        //    },
        //    'checkbox': {
        //        keep_selected_style: false,
        //        three_state: true,
        //        //cascade: ''
        //    },
        //    plugins: ['checkbox', 'types']
        //});

        //this._createdTreeBefore = true;

        //let inTreeChangeEvent = false;

        //function selectNodeAndAllParents(node) {
        //    self._$tree.jstree('select_node', node, true);
        //    let parent = self._$tree.jstree('get_parent', node);
        //    if (parent) {
        //        selectNodeAndAllParents(parent);
        //    }
        //}

        //this._$tree.on('changed.jstree', function (e, data) {
        //    
        //    if (!data.node) {
        //        return;
        //    }

        //    let wasInTreeChangeEvent = inTreeChangeEvent;
        //    if (!wasInTreeChangeEvent) {
        //        inTreeChangeEvent = true;
        //    }

        //    let childrenNodes;

        //    if (data.node.state.selected) {
        //        selectNodeAndAllParents(self._$tree.jstree('get_parent', data.node));

        //        childrenNodes = $.makeArray(self._$tree.jstree('get_children_dom', data.node));
        //        self._$tree.jstree('select_node', childrenNodes);

        //    } else {
        //        childrenNodes = $.makeArray(self._$tree.jstree('get_children_dom', data.node));
        //        self._$tree.jstree('deselect_node', childrenNodes);
        //    }

        //    if (!wasInTreeChangeEvent) {
        //        inTreeChangeEvent = false;
        //    }
        //});
    }
}

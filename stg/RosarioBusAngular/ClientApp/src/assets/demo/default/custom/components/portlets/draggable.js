var PortletDraggable = {
    init: function () {
        $("#m_sortable_portlets").sortable({
            connectWith: ".m-portlet__head",
            items: ".m-portlet",
            opacity: .8,
            handle: ".m-portlet__head",
            coneHelperSize: !0,
            placeholder: "m-portlet--sortable-placeholder",
            forcePlaceholderSize: !0,
            tolerance: "pointer",
            helper: "clone",
            tolerance: "pointer",
            forcePlaceholderSize: !0,
            helper: "clone",
            cancel: ".m-portlet--sortable-empty",
            revert: 250,
            update: function (e, t) {
                t.item.prev().hasClass("m-portlet--sortable-empty") && t.item.prev().before(t.item)
            }
        }
        )
    }
}

    ;
jQuery(document).ready(function () {
    PortletDraggable.init()
}

);
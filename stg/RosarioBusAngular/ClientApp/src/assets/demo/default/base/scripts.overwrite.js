$.extend(mApp,
    {
        initPortlets: function () {
            
            $('[m-portlet="true"]').each(function () {
                var el = $(this);

                if (el.data('portlet-initialized') !== true) {


                    var options = {
                        bodyToggleSpeed: 400,
                        tooltips: true,
                        tools: {
                            toggle: {
                                collapse: 'Colapsar',
                                expand: 'Expandir'
                            },
                            reload: 'Recargar',
                            remove: 'Eliminar',
                            fullscreen: {
                                on: 'Pantalla completa',
                                off: 'Salir de pantalla completa'
                            }
                        }
                    };


                    mApp.initPortlet(el, options);
                    el.data('portlet-initialized', true);
                }
            });
        }  
    } 
); 

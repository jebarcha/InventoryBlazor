; (function (app) {

    var ServicioDatos = {
        existsUsername: function (username) {
            return app.serviciosAjax.get('Users/Create?handler=ExistsUsername', { username });
        },
    };

    app.ServicioDatos = ServicioDatos;

}(window.app));

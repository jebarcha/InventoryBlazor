; (function (app) {

    const $inputUsername = $('#User_Username');
    $inputUsername.on('change', validateUser);

    function validateUser() {

        var valor = $(this).val();

        app.ServicioDatos.existsUsername(valor).then(response => {
            if (response.existsUser) {
                $(':input[type="submit"]').prop('disabled', true);
                var notyf = new Notyf();
                notyf.error('Username already exists.');
                $inputUsername.addClass('border-danger');
            }
            else {
                $(':input[type="submit"]').prop('disabled', false);
                $inputUsername.removeClass('border-danger');
            }
        });
    }

}(window.app));

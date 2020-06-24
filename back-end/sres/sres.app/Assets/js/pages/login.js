$(document).ready(() => {
    $('#frmLogin').submit(sendLogin);
    $('#btnIniciarSesionMRV').on('click', validarUsuarioMRV);
    $('#btnValidarUsuarioMRV').on('click', sendLoginWithMRV);
});

var sendLogin = (e) => {
    let tokenValue = $('#token').val();

    if (tokenValue == "") {
        e.preventDefault();

        let correo = $('#correo').val().trim();

        let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

        fetch(urlVerificarCorreo)
        .then(r => r.json())
        .then(j => {
            if (j) {
                $('#txtCorreoMRV').val(correo);
                $('#viewLoginMRV').show();
            } else {
                grecaptcha.ready(() => {
                    grecaptcha.execute(key).then((token) => {
                        $('#token').val(token);
                        $('#frmLogin').submit();
                    });
                });
            }
        })
    }
}

var sendLoginWithMRV = (e) => {
    let tokenValue = $('#tokenMRV').val();

    if (tokenValue == "") {
        e.preventDefault();

        let correo = $('#txtCorreoMRV').val().trim();

        let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

        fetch(urlVerificarCorreo)
        .then(r => r.json())
        .then(j => {
            if (j) {
                grecaptcha.ready(() => {
                    grecaptcha.execute(key).then((token) => {
                        $('#tokenMRV').val(token);
                        $('#frmLoginMRV').submit();
                    });
                });
            } else {
                alert('Correo no registrado en MRV');
            }
        })
    }
}

var validarUsuarioMRV = (e) => {
    e.preventDefault();

    let correo = $('#correo').val().trim();
    let contraseña = $('#contraseña').val().trim();

    $('#txtCorreoMRV').val(correo);
    $('#txtContraseñaMRV').val(contraseña);

    $('#viewLoginMRV').show();
}
$(document).ready(() => {
    $('#frmLogin').submit((e) => sendLogin());
    $('#btnIniciarSesionMRV').on('click', (e) => {
    });
});

var sendLogin = () => {
    let tokenValue = $('#token').val();

    if (tokenValue == "") {
        e.preventDefault();

        grecaptcha.ready(() => {
            grecaptcha.execute(key).then((token) => {
                $('#token').val(token);
                $('#frmLogin').submit();
            });
        });
    }
}

var validarUsuarioLogin = () => {
    let ruc = $('#txtRucMRV').val().trim();
    let correo = $('#txtCorreoMRV').val().trim();
    let contraseña = $('#txtContraseñaMRV').val().trim();

    if (ruc == '' || correo == '' || contraseña == '') return;

    let init = { method: 'POST' };
    let params = { ruc, correo, contraseña };
    let paramsString = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');
    let urlObtenerUsuarioRucCorreo = `/api/mrv/usuario/validarloginusuario?${paramsString}`;

    fetch(urlObtenerUsuarioRucCorreo, init)
    .then(r => r.json())
    .then(j => cargarDatosUsuarioMRV(j));
}
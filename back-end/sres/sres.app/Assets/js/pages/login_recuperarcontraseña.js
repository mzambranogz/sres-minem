var enviarMail = (e) => {
    debugger;
    e.preventDefault();
    let correo = $('#txt-user').val();

    let url = `${baseUrl}api/usuario/enviarlinkrecuperarcontraseña?correo=${correo}`;

    //$('#verificacion-mrv').modal('show');

    fetch(url)
    .then(r => r.json())
    .then(responseEnvioMail);
};

var responseEnvioMail = (data) => {
    //$('#verificacion-mrv').modal('hide');
    let success = data.success;
    let message = data.message;

    if (success == true) $('form > *:last').alert({ type: 'success', title: 'Validando credenciales', message, close: { time: 3000 } });
    else $('form > *:last').alert({ type: 'danger', title: 'Validando credenciales', message, close: { time: 3000 } });

};

$(document).ready(() => {
    $('#frmLogin').submit(enviarMail);
});
$(document).ready(() => {
    $('#frmLogin').submit(sendLogin);
    //$('#mrvBtn').on('click', validarUsuarioMRV);
    $('#mrvBtn').on('click', sendLoginWithMRV);
});

var sendLogin = (e) => {
    e.preventDefault();

    $('#modalValidacionSres').modal('show');

    grecaptcha.ready(() => {
        grecaptcha.execute(key).then(iniciarSesionConCaptcha);
    });
}

var iniciarSesionConCaptcha = (token) => {
    let correo = $('#txt-user').val().trim();
    let contraseña = $('#txt-pswd').val().trim();
    let data = { correo, contraseña, token };

    let url = `${baseUrl}Login/Validar`;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(validarInicioSesion)
}

var validarInicioSesion = (data) => {
    if (data.success == true) {
        $('#modalValidacionSres .modal-content .modal-body .row').alert({ type: 'success', title: 'Validación correcta', message: data.message });
        $('#modalValidacionSres .modal-content .modal-body .row > *:last > *:last').remove();
        $('#redireccionarText').show();
        $('#txtSegundosRedirigir').counter({ start: 5, end: 0, time: 1000, callback: () => location.reload() });
    }
    else {
        let correo = $('#txt-user').val().trim();

        let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

        fetch(urlVerificarCorreo)
        .then(r => r.json())
        .then((j) => validacionCorreoMRV(j, data.message))
    }
}

var validacionCorreoMRV = (data, message) => {
    if (data == true) $('#mrvBtn')[0].click();
    else {
        $('#modalValidacionSres').modal('hide');
        $('form .form-group:last').alert({ type: 'danger', title: 'Error de acceso', message: message, close: { time: 3000 } });
    }
}

var sendLoginWithMRV = (e) => {
    e.preventDefault();
    
    $('#modalValidacionSres').modal('show');

    let correo = $('#txt-user').val().trim();

    let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

    fetch(urlVerificarCorreo)
    .then(r => r.json())
    .then(validarCorreoMRV);
}

var validarCorreoMRV = (data) =>  {
    if (data == true) {
        grecaptcha.ready(() => {
            grecaptcha.execute(key).then(iniciarSesionMRVConCaptcha);
        });
    } else {

        
        //do {
        //} while ($('#modalValidacionSres').is(':visible'));
        $('#modalValidacionSres').modal('hide');
        //$("#modalValidacionSres").removeClass("show");
        //$("#modalValidacionSres").removeAttr("style");
        //$('body').removeClass('modal-open');
        //$('.modal-backdrop').remove();
        //$('#modalValidacionSres').modal('hide');
        //$('#modalValidacionSres').modal('hide');

        $('form .form-group:last').alert({ type: 'danger', title: 'Error de acceso', message: 'Las credenciales MRV no son válidas', close: { time: 3000 } });
        //console.log($('#modalValidacionSres')[0]);
    }
}

var iniciarSesionMRVConCaptcha = (token) => {
    let correo = $('#txt-user').val().trim();
    let contraseña = $('#txt-pswd').val().trim();
    let data = { correo, contraseña, token };

    let url = `${baseUrl}Login/ValidarMRV`;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };


    fetch(url, init)
    .then(r => r.json())
    .then(validarInicioSesionConMRV)
}

var validarInicioSesionConMRV = (data) => {
    if (data.success == true) {
        $('#modalValidacionSres .modal-content .modal-body .row').alert({ type: 'success', title: 'Validación correcta', message: data.message });
        $('#modalValidacionSres .modal-content .modal-body .row > *:last > *:last').remove();
        $('#redireccionarText').show();
        $('#txtSegundosRedirigir').counter({ start: 5, end: 0, time: 1000, callback: () => location.reload() });
    }
    else {
        $('#modalValidacionSres').modal('hide');
        $('form .form-group:last').alert({ type: 'danger', title: 'Error de acceso', message: 'Las credenciales MRV no son válidas', close: { time: 3000 } });
    }
}
$(document).ready(() => {
    $('#frmLogin').submit(sendLogin);
    $('#btnIniciarSesionMRV').on('click', validarUsuarioMRV);
    $('#btnValidarUsuarioMRV').on('click', sendLoginWithMRV);
});

var sendLogin = (e) => {
    e.preventDefault();

    let correo = $('#txt-user').val().trim();

    let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

    fetch(urlVerificarCorreo)
    .then(r => r.json())
    .then(validacionCorreoMRV)
}

var validacionCorreoMRV = (data) => {
    if (data == true) {
        $('#txtCorreoMRV').val(correo);
        $('#viewLoginMRV').show();
    } else {
        grecaptcha.ready(() => {
            grecaptcha.execute(key).then(iniciarSesionConCaptcha);
        });
    }
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
        
        setTimeout(() => { location.reload() }, 5000);
    }
    else {
        mostrarAlerta('#frmLogin .form-group:last', 'success', 'Error de acceso', data.message, 3000);
    }
}



var mostrarAlerta = (selector, type, title, message, closeTime = 1000) => {
    let element = $(`<div class="alert alert-${type} d-flex align-items-stretch" role="alert"></div>`);
    let optionsContent = `<div class="alert-wrap mr-3"><div class="sa"><div class="sa-error"><div class="sa-error-x"><div class="sa-error-left"></div><div class="sa-error-right"></div></div><div class="sa-error-placeholder"></div><div class="sa-error-fix"></div></div></div></div>`;
    let titleContent = `<h6 class="estilo-02">${title}</h6>`;
    let messageContent = `<small class="mb-0 estilo-01">${message}</small>`;
    let content = `${optionsContent}<div class="alert-wrap">${titleContent}<hr class="my-1">${messageContent}</div>`;
    element.html(content);
    $(selector).after(element);
    setTimeout(() => { element.remove(); }, closeTime);
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
﻿$(document).ready(() => {
    cargarInformacionInicial(inicializar());
});

var cargarInformacionInicial = (fn) => {
    let urlListarSectorPorEstado = `/api/sector/listarsectorporestado?flagEstado=1`;

    Promise.all([
        fetch(urlListarSectorPorEstado)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([listaSector]) => {
        cargarComboSector('#cbo-sector', listaSector);
        fn;
    })
}

var inicializar = () => {
    $('#sresBtn').on('click', (e) => consultarInstitucion());
    $('#txt-user').on('blur', (e) => consultarUsuario());
    
    $('#frmRegister').on('submit', (e) => { e.preventDefault(); registrarUsuario();});
}

var consultarInstitucion = () => {
    let rucInstitucion = $('#txt-ruc').val().trim();

    if (rucInstitucion == "") return;

    let urlObtenerInstitucionRuc = `/api/institucion/obtenerinstitucionporruc?ruc=${rucInstitucion}`;

    fetch(urlObtenerInstitucionRuc)
    .then(r => r.json())
    .then(j => cargarDatosInstitucion(j));
}

var cargarDatosInstitucion = (data) => {
    let existeInformacion = data != null;

    $('#txt-user').val('');
    limpiarDatosInstitucion();
    limpiarDatosUsuario();
    cambiarPropiedadLecturaInstitucion(!existeInformacion);
    cambiarPropiedadLecturaUsuario(!existeInformacion);

    if (!existeInformacion) {
        let ruc = $('#txt-ruc').val();
        let urlObtenerInstitucionMRVPorRuc = `/api/mrv/institucion/obtenerinstitucionporruc?ruc=${ruc}`;

        fetch(urlObtenerInstitucionMRVPorRuc)
        .then(r => r.json())
        .then(j => cargarDatosInstitucionMRV(j));
        return;
    }


    $('#frmRegister').data('idInstitucion', data.ID_INSTITUCION);
    $('#txt-institucion').val(data.RAZON_SOCIAL);
    $('#txt-direccion').val(data.DOMICILIO_LEGAL);
    $('#cbo-sector').val(data.ID_SECTOR);

    $('form > .row:nth(0)').alert({ type: 'success', title: 'BIEN HECHO', message: 'Hemos encontrado información relacionada a su número de RUC, por favor continue y complete sus datos en los campos restantes del siguiente formulario.' });
}

var cargarDatosInstitucionMRV = (data) => {
    let existeInformacion = data != null;

    //$('#txt-ruc').val('');
    limpiarDatosInstitucion();
    limpiarDatosUsuario();
    cambiarPropiedadLecturaInstitucion(!existeInformacion);
    cambiarPropiedadLecturaUsuario(!existeInformacion);

    if (!existeInformacion) {
        $('form > .row:nth(0)').alert({ type: 'warning', title: 'NÚMERO DE RUC NUEVO', message: 'Por favor continue y complete sus datos en todos los campos del siguiente formulario.' });
        return;
    }

    $('#frmRegister').data('idInstitucion', data.ID_INSTITUCION);
    $('#txt-institucion').val(data.NOMBRE_INSTITUCION);
    $('#txt-direccion').val(data.DIRECCION_INSTITUCION);
    $('#cbo-sector').val(data.ID_SECTOR_INSTITUCION);

    $('form > .row:nth(0)').alert({ type: 'success', title: 'BIEN HECHO', message: 'Hemos encontrado información relacionada a su número de RUC, por favor continue y complete sus datos en los campos restantes del siguiente formulario.' });
}

var cargarComboSector = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_SECTOR}">${x.NOMBRE}</option>`).join('');
    options = `<option>-Seleccione un sector-</option>${options}`;
    $(selector).html(options);
}

var consultarUsuario = () => {
    let idInstitucion = $('#frmRegister').data('idInstitucion');

    if(idInstitucion == null) return;

    let correo = $('#txt-user').val().trim();

    if(correo == '' || idInstitucion == '') return;

    let urlObtenerUsuarioPorinstitucionCorreo = `/api/usuario/obtenerusuarioporinstitucioncorreo?idInstitucion=${idInstitucion}&correo=${correo}`;

    fetch(urlObtenerUsuarioPorinstitucionCorreo)
    .then(r => r.json())
    .then(j => cargarDatosUsuario(j));
}

var cargarDatosUsuario = (data) => {
    //limpiarDatosUsuario();
    //cambiarPropiedadLecturaUsuario(!data.EXISTE);

    if(!data.EXISTE) {
        let correo = $('#txt-user').val().trim();

        let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

        fetch(urlVerificarCorreo)
        .then(r => r.json())
        .then(abrirModalLoginMRV)
        return;
    }

    $('form > .row:nth(3)').alert({ type: 'warning', title: 'ADVERTENCIA', message: 'El correo ya se encuentra registrado', close: { time: 3000 } });

    //$('#frmRegister').data('idUsuario', data.USUARIO.ID_USUARIO);
    //$('#txtNombresUsuario').val(data.USUARIO.NOMBRES);
    //$('#txtApellidosUsuario').val(data.USUARIO.APELLIDOS);
    //$('#txtTelefonoUsuario').val(data.USUARIO.TELEFONO);
    //$('#txtAnexoUsuario').val(data.USUARIO.ANEXO);
    //$('#txtCelularUsuario').val(data.USUARIO.CELULAR);
}

var abrirModalLoginMRV = (data) => {
    if(data == true){

        $('form > .row:nth(4)').alert({ type: 'warning', title: 'AUTENTICACIÓN DE CUENTA MRV', message: 'Encontramos que su Usuario - Correo electrónico se encuentra registrado en la Plataforma MRV del sector energía, por favor inicie sesión con sus credenciales de acceso para continuar.', html: '<hr class="mt-1 mb-3"><div class="row"><div class="col-sm-12"><div class="form-group"><label class="estilo-01" for="txt-user">Correo electrónico MRV</label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text bg-nama-azul text-white"><i class="fas fa-envelope"></i></span></div><input class="form-control estilo-01" type="email" id="txt-user-mrv" placeholder="Ingresar correo electrónico" maxlength="50" pattern="^[a-zA-Z0-9.!#$%&amp;’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:.[a-zA-Z0-9-]+)*$" title="Ingrese una dirección de correo válida." autocomplete="new-user" required></div></div></div><div class="col-sm-12"><div class="form-group"><label class="estilo-01" for="txt-pswd">Contraseña MRV</label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text bg-nama-azul text-white"><i class="fas fa-key"></i></span></div><input class="form-control estilo-01" type="password" id="txt-pswd-mrv" placeholder="Ingresar contraseña" maxlength="30" title="Ingrese su contraseña." autocomplete="new-password" required><div class="input-group-append"><label class="input-group-text cursor-pointer ver-clave estilo-01 bg-nama-azul text-white"><i class="fas fa-eye mr-1"></i>Mostrar</label></div></div></div></div></div><div class="row"><div class="col-sm-12 text-center"><button class="btn-lg btn-nama-azul w-100 mb-3" id="mrvBtn" type="button" data-toggle="tooltip" data-placement="bottom" title="Sistema de gestión y reporte de información de las medidas de mitigación de las Contribuciones Nacionalmente Determinadas (NDC).">Ingresa con tu cuenta MRV</button><a class="text-sres-verde estilo-01" href="http://sismrv.minem.gob.pe/Home/recuperar" target="_blank">¿Olvidó su contraseña?</a></div></div>' })
        $('#mrvBtn').on('click', (e) => validarUsuarioLogin());

        let correo = $('#txt-user').val().trim();

        $('#txt-user-mrv').val(correo);

        //$('#modalValidacionMrv').modal('show');
        //$('#txtCorreoMRV').val(correo);
        //$('#viewLoginMRV').show();
        //$('#viewContraseñaUsuario').hide();
        return;
    }
    limpiarDatosUsuario();
    cambiarPropiedadLecturaUsuario(!data.EXISTE);
}

var validarUsuarioLogin = () => {
    let correo = $('#txt-user-mrv').val().trim();
    let contraseña = $('#txt-pswd-mrv').val().trim();

    if (correo == '' || contraseña == '') return;

    let init = { method: 'POST' };
    let params = { correo, contraseña };
    let paramsString = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');
    let urlObtenerUsuarioRucCorreo = `/api/mrv/usuario/validarloginusuario?${paramsString}`;

    fetch(urlObtenerUsuarioRucCorreo, init)
    .then(r => r.json())
    .then(j => cargarDatosUsuarioMRV(j));
}

var cargarDatosUsuarioMRV = (data) => {
    limpiarDatosUsuario();
    cambiarPropiedadLecturaUsuario(!data.VALIDO);

    debugger;
    $('form > .row:nth(8)').hide();
    //$('#viewLoginMRV').hide();

    if(!data.VALIDO) {
        $('form > .row:nth(8)').show();
        //$('#viewContraseñaUsuario').show();
        let modal = $('form > .row:nth(4)').alert('get');
        if (modal != null) modal.find('.modal-content .modal-body .row:first').alert({ type: 'danger', title: 'ERROR', message: 'Usuario y/o contraseña inválidos', close: { time: 3000 } });
        return;
    }
    
    $('form > .row:nth(4)').alert('remove');

    //$('#modalValidacionMrv').modal('hide');

    let contraseña = $('#txt-pswd-mrv').val();

    $('#frmRegister').data('idUsuario', data.USUARIO.ID_USUARIO);
    $('#txt-nombre').val(data.USUARIO.NOMBRES_USUARIO);
    $('#txt-apellido').val(data.USUARIO.APELLIDOS_USUARIO);
    $('#txt-telefono').val(data.USUARIO.TELEFONO_USUARIO);
    $('#txt-anexo').val(data.USUARIO.ANEXO_USUARIO);
    $('#txt-celular').val(data.USUARIO.CELULAR_USUARIO);
    $('#txt-pswd').val(contraseña);
    $('#txt-re-pswd').val(contraseña);
}

var limpiarFormulario = () => {
    $('#txt-ruc').val('');
    limpiarDatosInstitucion();
    $('#txt-user').val('');
    limpiarDatosUsuario();
    $('form .alert').remove();
}

var limpiarDatosInstitucion = () => {
    $('#frmRegister').removeData('idInstitucion');
    $('#txt-institucion').val('');
    $('#txt-direccion').val('');
    $('#cbo-sector option:first').prop('checked', true);
    cambiarPropiedadLecturaInstitucion(false);
}

var limpiarDatosUsuario = () => {
    $('#frmRegister').val('idUsuario');
    $('#txt-nombre').val('');
    $('#txt-apellido').val('');
    $('#txt-telefono').val('');
    $('#txt-anexo').val('');
    $('#txt-celular').val('');
    $('#txt-pswd').val('');
    $('#txt-re-pswd').val('');
    cambiarPropiedadLecturaUsuario(false);
}

var cambiarPropiedadLecturaInstitucion = (valor) => {
    $('#txt-institucion').prop('readonly', !valor);
    $('#txt-direccion').prop('readonly', !valor);
    $('#cbo-sector').prop('disabled', !valor);
}

var cambiarPropiedadLecturaUsuario = (valor) => {
    $('#txt-nombre').prop('readonly', !valor);
    $('#txt-apellido').prop('readonly', !valor);
    $('#txt-telefono').prop('readonly', !valor);
    $('#txt-anexo').prop('readonly', !valor);
    $('#txt-celular').prop('readonly', !valor);
    $('#txt-pswd').prop('readonly', !valor);
    $('#txt-re-pswd').prop('readonly', !valor);
}

var registrarUsuario = () => {
    let aceptarTerminosYCondiciones = $('#chk-terminos-condiciones').prop('checked');

    if(!aceptarTerminosYCondiciones){
        $('form > .row:last').alert({ type: 'danger', title: 'Error', message: 'Debe aceptar los términos y condiciones' });
        return;
    }

    //$('#modalValidacionSres').modal('show');

    let idUsuario = $('#frmRegister').data('id_usuario');
    let nombres = $('#txt-nombre').val();
    let apellidos = $('#txt-apellido').val();
    let correo = $('#txt-user').val();
    let contraseña = $('#txt-pswd').val();
    let telefono = $('#txt-telefono').val();
    let anexo = $('#txt-anexo').val();
    let celular = $('#txt-celular').val();
    let idInstitucion = $('#frmRegister').data('id_institucion');
    let rucInstitucion = $('#txt-ruc').val();
    let razonSocialInstitucion = $('#txt-institucion').val();
    let domicilioLegalInstitucion = $('#txt-direccion').val();
    let idSectorInstitucion = $('#cbo-sector').val();

    let url = `/api/usuario/guardarusuario`;

    let data = { ID_USUARIO: idUsuario == null ? -1 : idUsuario, NOMBRES: nombres, APELLIDOS: apellidos, CORREO: correo, CONTRASENA: contraseña, TELEFONO: telefono, ANEXO: anexo, CELULAR: celular, ID_INSTITUCION: idInstitucion, INSTITUCION: idInstitucion != null ? null : { idInstitucion: idInstitucion == null ? -1 : idInstitucion, RUC: rucInstitucion, RAZON_SOCIAL: razonSocialInstitucion,  DOMICILIO_LEGAL: domicilioLegalInstitucion, ID_SECTOR: idSectorInstitucion, UPD_USUARIO: idUsuarioLogin }, ID_ROL: 3, UPD_USUARIO: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        console.log(j);
        if(j == true) {
            limpiarFormulario();
            $('form > .row:last').alert({ type: 'success', title: 'BIEN HECHO', message: '¡Se registró correctamente!', html: '<p id="redireccionarText" class="text-center estilo-01">Lo estamos redirigiendo en <strong id="txtSegundosRedirigir"></strong> segundos</p>' });
            //$('#modalValidacionSres .modal-content .modal-body .row > *:last > *:last').remove();
            //$('#redireccionarText').show();
            $('#txtSegundosRedirigir').counter({ start: 5, end: 0, time: 1000, callback: () => location.href = `${baseUrl}Login` });
            //alert('Se registró correctamente');
        }
    });
}
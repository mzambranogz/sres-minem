$(document).ready(() => {
    cargarInformacionInicial(inicializar());
});

var cargarInformacionInicial = (fn) => {
    let urlListarSectorPorEstado = `/api/sector/listarsectorporestado?flagEstado=1`;

    Promise.all([
        fetch(urlListarSectorPorEstado)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([listaSector]) => {
        cargarComboSector('#selIdSectorInstitucion', listaSector);
        fn;
    })
}

var inicializar = () => {
    $('#btnConsultarInstitucion').on('click', (e) => consultarInstitucion());
    $('#txtCorreoUsuario').on('blur', (e) => consultarUsuario());
    $('#btnValidarUsuarioMRV').on('click', (e) => validarUsuarioLogin());
    $('#btnRegistrarme').on('click', (e) => registrarUsuario());
}

var consultarInstitucion = () => {
    let rucInstitucion = $('#txtRucInstitucion').val().trim();

    if (rucInstitucion == "") return;

    let urlObtenerInstitucionRuc = `/api/institucion/obtenerinstitucionporruc?ruc=${rucInstitucion}`;

    fetch(urlObtenerInstitucionRuc)
    .then(r => r.json())
    .then(j => cargarDatosInstitucion(j));
}

var cargarDatosInstitucion = (data) => {
    let existeInformacion = data != null;

    $('#txtCorreoUsuario').val('');
    limpiarDatosInstitucion();
    limpiarDatosUsuario();
    cambiarPropiedadLecturaInstitucion(!existeInformacion);
    cambiarPropiedadLecturaUsuario(!existeInformacion);

    if (!existeInformacion) {
        let ruc = $('#txtRucInstitucion').val();
        let urlObtenerInstitucionMRVPorRuc = `/api/mrv/institucion/obtenerinstitucionporruc?ruc=${ruc}`;

        fetch(urlObtenerInstitucionMRVPorRuc)
        .then(r => r.json())
        .then(j => cargarDatosInstitucionMRV(j));
        return;
    }

    $('#frmRegistro').data('idInstitucion', data.ID_INSTITUCION);
    $('#txtRazonSocialInstitucion').val(data.RAZON_SOCIAL);
    $('#txtDomicilioLegalInstitucion').val(data.DOMICILIO_LEGAL);
    $('#selIdSectorInstitucion').val(data.ID_SECTOR);
}

var cargarDatosInstitucionMRV = (data) => {
    let existeInformacion = data != null;

    $('#txtCorreoUsuario').val('');
    limpiarDatosInstitucion();
    limpiarDatosUsuario();
    cambiarPropiedadLecturaInstitucion(!existeInformacion);
    cambiarPropiedadLecturaUsuario(!existeInformacion);

    if (!existeInformacion) return;

    $('#frmRegistro').data('idInstitucion', data.ID_INSTITUCION);
    $('#txtRazonSocialInstitucion').val(data.NOMBRE_INSTITUCION);
    $('#txtDomicilioLegalInstitucion').val(data.DIRECCION_INSTITUCION);
    $('#selIdSectorInstitucion').val(data.ID_SECTOR_INSTITUCION);
}

var cargarComboSector = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_SECTOR}">${x.NOMBRE}</option>`).join('');
    options = `<option>-Seleccione un sector-</option>${options}`;
    $(selector).html(options);
}

var consultarUsuario = () => {
    let idInstitucion = $('#frmRegistro').data('idInstitucion');

    if(idInstitucion == null) return;

    let correo = $('#txtCorreoUsuario').val().trim();

    if(correo == '' || idInstitucion == '') return;

    let urlObtenerUsuarioPorinstitucionCorreo = `/api/usuario/obtenerusuarioporinstitucioncorreo?idInstitucion=${idInstitucion}&correo=${correo}`;

    fetch(urlObtenerUsuarioPorinstitucionCorreo)
    .then(r => r.json())
    .then(j => cargarDatosUsuario(j));
}

var cargarDatosUsuario = (data) => {
    limpiarDatosUsuario();
    cambiarPropiedadLecturaUsuario(!data.EXISTE);

    if(!data.EXISTE) {
        let correo = $('#txtCorreoUsuario').val().trim();

        let urlVerificarCorreo = `/api/mrv/usuario/verificarcorreo?correo=${correo}`;

        fetch(urlVerificarCorreo)
        .then(r => r.json())
        .then(abrirModalLoginMRV)
        return;
    }

    $('#frmRegistro').data('idUsuario', data.USUARIO.ID_USUARIO);
    $('#txtNombresUsuario').val(data.USUARIO.NOMBRES);
    $('#txtApellidosUsuario').val(data.USUARIO.APELLIDOS);
    $('#txtTelefonoUsuario').val(data.USUARIO.TELEFONO);
    $('#txtAnexoUsuario').val(data.USUARIO.ANEXO);
    $('#txtCelularUsuario').val(data.USUARIO.CELULAR);
}

var abrirModalLoginMRV = (data) => {
    if(data == true){
        $('#txtCorreoMRV').val(correo);
        $('#viewLoginMRV').show();
        $('#viewContraseñaUsuario').hide();
    }
}

var validarUsuarioLogin = () => {
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

var cargarDatosUsuarioMRV = (data) => {
    limpiarDatosUsuario();
    cambiarPropiedadLecturaUsuario(!data.VALIDO);

    $('#viewLoginMRV').hide();

    if(!data.VALIDO) {
        $('#viewContraseñaUsuario').show();
        return;
    }

    let contraseña = $('#txtContraseñaMRV').val();

    $('#frmRegistro').data('idUsuario', data.USUARIO.ID_USUARIO);
    $('#txtNombresUsuario').val(data.USUARIO.NOMBRES_USUARIO);
    $('#txtApellidosUsuario').val(data.USUARIO.APELLIDOS_USUARIO);
    $('#txtTelefonoUsuario').val(data.USUARIO.TELEFONO_USUARIO);
    $('#txtAnexoUsuario').val(data.USUARIO.ANEXO_USUARIO);
    $('#txtCelularUsuario').val(data.USUARIO.CELULAR_USUARIO);
    $('#txtContraseñaUsuario').val(contraseña);
    $('#txtConfirmarContraseñaUsuario').val(contraseña);
}

var limpiarFormulario = () => {
    $('#txtRucInstitucion').val('');
    limpiarDatosInstitucion();
    $('#txtCorreoUsuario').val('');
    limpiarDatosUsuario();
}

var limpiarDatosInstitucion = () => {
    $('#frmRegistro').removeData('idInstitucion');
    $('#txtRazonSocialInstitucion').val('');
    $('#txtDomicilioLegalInstitucion').val('');
    $('#selIdSectorInstitucion option:first').prop('checked', true);
    cambiarPropiedadLecturaInstitucion(false);
}

var limpiarDatosUsuario = () => {
    $('#frmRegistro').val('idUsuario');
    $('#txtNombresUsuario').val('');
    $('#txtApellidosUsuario').val('');
    $('#txtTelefonoUsuario').val('');
    $('#txtAnexoUsuario').val('');
    $('#txtCelularUsuario').val('');
    $('#txtContraseñaUsuario').val('');
    $('#txtConfirmarContraseñaUsuario').val('');
    cambiarPropiedadLecturaUsuario(false);
}

var cambiarPropiedadLecturaInstitucion = (valor) => {
    $('#txtRazonSocialInstitucion').prop('readonly', !valor);
    $('#txtDomicilioLegalInstitucion').prop('readonly', !valor);
    $('#selIdSectorInstitucion').prop('disabled', !valor);
}

var cambiarPropiedadLecturaUsuario = (valor) => {
    $('#txtNombresUsuario').prop('readonly', !valor);
    $('#txtApellidosUsuario').prop('readonly', !valor);
    $('#txtTelefonoUsuario').prop('readonly', !valor);
    $('#txtAnexoUsuario').prop('readonly', !valor);
    $('#txtCelularUsuario').prop('readonly', !valor);
    $('#txtContraseñaUsuario').prop('readonly', !valor);
    $('#txtConfirmarContraseñaUsuario').prop('readonly', !valor);
}

var registrarUsuario = () => {
    let idUsuario = $('#frmRegistro').data('id_usuario');
    let nombres = $('#txtNombresUsuario').val();
    let apellidos = $('#txtApellidosUsuario').val();
    let correo = $('#txtCorreoUsuario').val();
    let contraseña = $('#txtContraseñaUsuario').val();
    let telefono = $('#txtTelefonoUsuario').val();
    let anexo = $('#txtAnexoUsuario').val();
    let celular = $('#txtCelularUsuario').val();
    let idInstitucion = $('#frmUsuario').data('id_institucion');
    let rucInstitucion = $('#txtRucInstitucion').val();
    let razonSocialInstitucion = $('#txtRazonSocialInstitucion').val();
    let domicilioLegalInstitucion = $('#txtDomicilioLegalInstitucion').val();
    let idSectorInstitucion = $('#selIdSectorInstitucion').val();

    let url = `/api/usuario/guardarusuario`;

    let data = { ID_USUARIO: idUsuario == null ? -1 : idUsuario, NOMBRES: nombres, APELLIDOS: apellidos, CORREO: correo, CONTRASENA: contraseña, TELEFONO: telefono, ANEXO: anexo, CELULAR: celular, ID_INSTITUCION: idInstitucion, INSTITUCION: idInstitucion != null ? null : { idInstitucion: idInstitucion == null ? -1 : idInstitucion, RUC: rucInstitucion, RAZON_SOCIAL: razonSocialInstitucion,  DOMICILIO_LEGAL: domicilioLegalInstitucion, ID_SECTOR: idSectorInstitucion, UPD_USUARIO: idUsuarioLogin }, UPD_USUARIO: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        console.log(j);
        if(j) {
            alert('Se registró correctamente');
            limpiarFormulario();
        }
    });
}
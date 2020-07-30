$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevoUsuario());
    $('#btnCerrar').on('click', (e) => cerrarFormularioUsuario());
    $('#btnGuardar').on('click', (e) => guardarUsuario());
    $('#txtRucInstitucion').on('blur', (e) => consultarInstitucion());
});

var consultar = () => {
    let busqueda = $('#textoBusqueda').val();
    let registros = 10;
    let pagina = 1;
    let columna = 'id_usuario';
    let orden = 'asc'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/usuario/buscarusuario?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblUsuario');
        let cantidadCeldasCabecera = tabla.find('thead tr th').length;
        let contenido = renderizar(j, cantidadCeldasCabecera, pagina, registros);
        tabla.find('tbody').html(contenido);
        tabla.find('.btnCambiarEstado').each(x => {
            let elementButton = tabla.find('.btnCambiarEstado')[x];
            $(elementButton).on('click', (e) => {
                e.preventDefault();
                cambiarEstado(e.currentTarget);
            });
        });

        tabla.find('.btnEditar').each(x => {
            let elementButton = tabla.find('.btnEditar')[x];
            $(elementButton).on('click', (e) => {
                e.preventDefault();
                consultarUsuario(e.currentTarget);
            });
        });
    });
};

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let colNro = `<td>${(pagina - 1) * registros + (i + 1)}</td>`;
            let colNombres = `<td>${x.NOMBRES}</td>`;
            let colApellidos = `<td>${x.APELLIDOS}</td>`;
            let colCorreo = `<td>${x.CORREO}</td>`;
            let colTelefono = `<td>${x.TELEFONO}</td>`;
            let colAnexo = `<td>${x.ANEXO}</td>`;
            let colCelular = `<td>${x.CELULAR}</td>`;
            let colRucInstitucion = `<td>${x.INSTITUCION == null ? '' : x.INSTITUCION.RUC}</td>`;
            let colRazonSocialInstitucion = `<td>${x.INSTITUCION == null ? '' : x.INSTITUCION.RAZON_SOCIAL}</td>`;
            let colNombreRol = `<td>${x.ROL == null ? '' : x.ROL.NOMBRE}</td>`;
            let btnCambiarEstado = `${!["0", "1"].includes(x.FLAG_ESTADO) ? "" : `<a href="#" data-id="${x.ID_USUARIO}" data-estado="${x.FLAG_ESTADO}" class="btnCambiarEstado">${x.FLAG_ESTADO == "1" ? "DESHABILITAR" : "HABILITAR"}</a> `}`;
            let btnEditar = `<a href="#" data-id="${x.ID_USUARIO}" class="btnEditar">EDITAR</a>`;
            let colOpciones = `<td>${btnCambiarEstado}${btnEditar}</td>`;
            let fila = `<tr>${colNro}${colNombres}${colApellidos}${colCorreo}${colTelefono}${colAnexo}${colCelular}${colRucInstitucion}${colRazonSocialInstitucion}${colNombreRol}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var cambiarEstado = (element) => {

    let idUsuario = $(element).attr('data-id');
    let flagEstado = $(element).attr('data-estado');
    flagEstado = flagEstado == "1" ? "0" : "1";

    if (!confirm(`¿Está seguro que desea ${flagEstado == '1' ? 'Deshabilitar' : 'Habilitar'}?`)) return;

    let data = { ID_USUARIO: idUsuario, FLAG_ESTADO: flagEstado, UPD_USUARIO: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    let url = '/api/usuario/cambiarestadousuario';

    fetch(url, init)
        .then(r => r.json())
        .then(j => { if (j) $('#btnConsultar')[0].click(); });
};

var nuevoUsuario = () => {
    $('#frmUsuario').show();
    limpiarFormularioUsuario();
    cargarFormularioUsuario();
}

var cerrarFormularioUsuario = () => {
    $('#frmUsuario').hide();
}

var limpiarFormularioUsuario = () => {
    $('#frmUsuario').removeData();
    $('#txtNombres').val('');
    $('#txtApellidos').val('');
    $('#txtCorreo').val('');
    $('#txtTelefono').val('');
    $('#txtAnexo').val('');
    $('#txtCelular').val('');
    $('#txtRucInstitucion').val('');
    $('#txtRazonSocialInstitucion').val('');
    $('#selIdRol').val(null);
}

var cargarFormularioUsuario = () => {
    let urlRolListarPorEstado = `/api/rol/listarrolporestado?flagEstado=1`;
    Promise.all([
        fetch(urlRolListarPorEstado)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([dataRol]) => {
        cargarComboRol("#selIdRol", dataRol);
    });
}

var cargarComboRol = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ROL}">${x.NOMBRE}</option>`).join('');
    $(selector).html(options);
}

var consultarInstitucion = () => {
    let ruc = $('#txtRucInstitucion').val();

    let url = `/api/institucion/obtenerinstitucionporruc?ruc=${ruc}`;

    fetch(url)
    .then(r => r.json())
    .then (j => cargarDatosInstitucion(j));
}

var cargarDatosInstitucion = (data) => {
    if(data != null) $('#frmUsuario').data('id_institucion', data.ID_INSTITUCION);
    else $('#frmUsuario').removeData('id_institucion');
    $('#txtRazonSocialInstitucion').val(data == null ? '' : data.RAZON_SOCIAL);
}

var consultarUsuario = (element) => {
    $('#frmUsuario').show();
    limpiarFormularioUsuario();
    let idUsuario = $(element).attr('data-id');

    let urlUsuario = `/api/usuario/obtenerusuario?idUsuario=${idUsuario}`;

    fetch(urlUsuario)
    .then(r => r.json())
    .then (j => {
        let urlRolListar = `/api/rol/listarrolporestado?flagEstado=1`;
        let urlInstitucionObtener = `/api/institucion/obtenerinstitucion?idInstitucion=${j.ID_INSTITUCION}`;
        Promise.all([
            fetch(urlInstitucionObtener),
            fetch(urlRolListar)
        ])
        .then(r => Promise.all(r.map(v => v.json())))
        .then(([jInstitucion, jRol]) => {
            cargarComboRol('#selIdRol', jRol);
            cargarDatosUsuario(j);
            $('#txtRucInstitucion').val(jInstitucion.RUC);
            $('#txtRazonSocialInstitucion').val(jInstitucion.RAZON_SOCIAL);
        });
    });
}

var cargarDatosUsuario = (data) => {
    $('#frmUsuario').data('id_usuario', data.ID_USUARIO);
    $('#txtNombres').val(data.NOMBRES);
    $('#txtApellidos').val(data.APELLIDOS);
    $('#txtCorreo').val(data.CORREO);
    $('#txtTelefono').val(data.TELEFONO);
    $('#txtAnexo').val(data.ANEXO);
    $('#txtCelular').val(data.CELULAR);
    $('#frmUsuario').data('id_institucion', data.ID_INSTITUCION);
    $('#selIdRol').val(data.ID_ROL);
}

var guardarUsuario = () => {
    let idUsuario = $('#frmUsuario').data('id_usuario');
    let nombres = $('#txtNombres').val();
    let apellidos = $('#txtApellidos').val();
    let correo = $('#txtCorreo').val();
    let telefono = $('#txtTelefono').val();
    let anexo = $('#txtAnexo').val();
    let celular = $('#txtCelular').val();
    let idInstitucion = $('#frmUsuario').data('id_institucion');
    let idRol = $('#selIdRol').val();
    let flagEstado = '0';

    let url = `/api/usuario/guardarusuario`;

    let data = { ID_USUARIO: idUsuario == null ? -1 : idUsuario, NOMBRES: nombres, APELLIDOS: apellidos, CORREO: correo, TELEFONO: telefono, ANEXO: anexo, CELULAR: celular, ID_INSTITUCION: idInstitucion, ID_ROL: idRol, FLAG_ESTADO: flagEstado, UPD_USUARIO: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if(j) {
            alert('Se registró correctamente');
            cerrarFormularioUsuario();
            $('#btnConsultar')[0].click();
        }
    });
}
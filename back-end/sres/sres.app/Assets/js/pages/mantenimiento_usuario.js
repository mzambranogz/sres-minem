$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevoUsuario());
    $('#btnGuardar').on('click', (e) => guardarUsuario());
    $('#txt-ruc').on('blur', (e) => consultarInstitucion());
    $('#txt-user-correo').on('blur', (e) => consultarUsuarioCorreo());
    cargarInformacionInicial();
});
var flag_ndc = '0';
var fn_avance_grilla = (boton) => {
    var total = 0, miPag = 0;
    miPag = Number($("#ir-pagina").val());
    total = Number($(".total-paginas").html());

    if (boton == 1) miPag = 1;
    if (boton == 2) if (miPag > 1) miPag--;
    if (boton == 3) if (miPag < total) miPag++;
    if (boton == 4) miPag = total;

    $("#ir-pagina").val(miPag);
    $('#btnConsultar')[0].click();
}

var cambiarPagina = () => {
    $('#btnConsultar')[0].click();
}

$(".columna-filtro").click(function (e) {
    debugger;
    var id = e.target.id;

    $(".columna-filtro").removeClass("fa-sort-up");
    $(".columna-filtro").removeClass("fa-sort-down");
    $(".columna-filtro").addClass("fa-sort");

    if ($("#columna").val() == id) {
        if ($("#orden").val() == "ASC") {
            $("#orden").val("DESC")
            $(`#${id}`).removeClass("fa-sort");
            $(`#${id}`).removeClass("fa-sort-up");
            $(`#${id}`).addClass("fa-sort-down");
        }
        else {
            $("#orden").val("ASC")
            $(`#${id}`).removeClass("fa-sort");
            $(`#${id}`).removeClass("fa-sort-down");
            $(`#${id}`).addClass("fa-sort-up");
        }
    }
    else {
        $("#columna").val(id);
        $("#orden").val("ASC")
        $(`#${id}`).removeClass("fa-sort");
        $(`#${id}`).removeClass("fa-sort");
        $(`#${id}`).addClass("fa-sort-up");
    }

    $('#btnConsultar')[0].click();
});

var consultar = () => {
    let busqueda = $('#txt-descripcion').val();;
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();
    let columna = $("#columna").val();
    let orden = $("#orden").val();
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/usuario/buscarusuario?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblUsuario');
        tabla.find('tbody').html('');
        $('#viewPagination').attr('style', 'display: none !important');
        if (j.length > 0) {
            if (j[0].CANTIDAD_REGISTROS == 0) { $('#viewPagination').hide(); $('#view-page-result').hide(); }
            else { $('#view-page-result').show(); $('#viewPagination').show(); }
            $('.inicio-registros').text(j[0].CANTIDAD_REGISTROS == 0 ? 'No se encontraron resultados' : (j[0].PAGINA - 1) * j[0].CANTIDAD_REGISTROS + 1);
            $('.fin-registros').text(j[0].TOTAL_REGISTROS < j[0].PAGINA * j[0].CANTIDAD_REGISTROS ? j[0].TOTAL_REGISTROS : j[0].PAGINA * j[0].CANTIDAD_REGISTROS);
            $('.total-registros').text(j[0].TOTAL_REGISTROS);
            $('.pagina').text(j[0].PAGINA);
            $('#ir-pagina').val(j[0].PAGINA);
            $('#ir-pagina').attr('max', j[0].TOTAL_PAGINAS);
            $('.total-paginas').text(j[0].TOTAL_PAGINAS);

            let cantidadCeldasCabecera = tabla.find('thead tr th').length;
            let contenido = renderizar(j, cantidadCeldasCabecera, pagina, registros);
            tabla.find('tbody').html(contenido);
            //tabla.find('.btnCambiarEstado').each(x => {
            //    let elementButton = tabla.find('.btnCambiarEstado')[x];
            //    $(elementButton).on('click', (e) => {
            //        e.preventDefault();
            //        cambiarEstado(e.currentTarget);
            //    });
            //});

            tabla.find('.btnEditar').each(x => {
                let elementButton = tabla.find('.btnEditar')[x];
                $(elementButton).on('click', (e) => {
                    e.preventDefault();
                    consultarUsuario(e.currentTarget);
                });
            });
        } else {
            $('#viewPagination').hide(); $('#view-page-result').hide();
            $('.inicio-registros').text('No se encontraron resultados');
        }        
    });
};

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let formatoCodigo = '00000000';
            let colNro = `<td class="text-center" data-encabezado="Número de orden" scope="row" data-count="0">${(pagina - 1) * registros + (i + 1)}</td>`;
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_USUARIO}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colNombres = `<td class="text-left" data-encabezado="Nombre">${x.NOMBRES} ${x.APELLIDOS}</td>`;
            let colCorreo = `<td class="text-left" data-encabezado="Correo">${x.CORREO}</td>`;
            let colRazonSocialInstitucion = `<td class="text-left" data-encabezado="Institución">${x.INSTITUCION == null ? '' : x.INSTITUCION.RAZON_SOCIAL}</td>`;
            let colTelefono = `<td class="text-center" data-encabezado="Teléfono/Celular">${x.TELEFONO == null ? '' : x.TELEFONO == '' ? '' : `${x.TELEFONO}&nbsp;/&nbsp;`}${x.CELULAR == null ? '' : x.CELULAR}</td>`;
            let colNombreRol = `<td class="text-center" data-encabezado="Perfil"><div class="badge badge-${x.ID_ROL == 1 ? 'success' : x.ID_ROL == 2 ? 'info' : 'primary'} p-1"><small class="estilo-01">${x.ROL.NOMBRE == null ? '' : x.ROL.NOMBRE}</div></td>`;
            let colEstado = `<td data-encabezado="Estado"><div class="badge badge-${x.FLAG_ESTADO == 0 ? 'info' : x.FLAG_ESTADO == 1 ? 'success' : 'danger'} p-1"><i class="fas fa-times-circle mr-1"></i><small class="estilo-01">${x.FLAG_ESTADO == 0 ? 'Por habilitar' : x.FLAG_ESTADO == 1 ? 'Habilitado' : 'Deshabilitado'}</small></div></td>`;
            //let colAnexo = `<td>${x.ANEXO || ''}</td>`;
            //let colCelular = `<td>${x.CELULAR}</td>`;
            //let colRucInstitucion = `<td>${x.INSTITUCION == null ? '' : x.INSTITUCION.RUC}</td>`;
            //let btnCambiarEstado = `${!["0", "1"].includes(x.FLAG_ESTADO) ? "" : `<a href="#" data-id="${x.ID_USUARIO}" data-estado="${x.FLAG_ESTADO}" class="btnCambiarEstado">${x.FLAG_ESTADO == "1" ? "DESHABILITAR" : "HABILITAR"}</a> `}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_USUARIO}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            //let colOpciones = `<td>${btnCambiarEstado}${btnEditar}</td>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnEditar}</div></div></td>`;
            //let fila = `<tr>${colNro}${colNombres}${colApellidos}${colCorreo}${colTelefono}${colAnexo}${colCelular}${colRucInstitucion}${colRazonSocialInstitucion}${colNombreRol}${colOpciones}</tr>`;
            let fila = `<tr>${colNro}${colCodigo}${colNombres}${colCorreo}${colRazonSocialInstitucion}${colTelefono}${colNombreRol}${colEstado}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

//var cambiarEstado = (element) => {

//    let idUsuario = $(element).attr('data-id');
//    let flagEstado = $(element).attr('data-estado');
//    flagEstado = flagEstado == "1" ? "0" : "1";

//    if (!confirm(`¿Está seguro que desea ${flagEstado == '1' ? 'Deshabilitar' : 'Habilitar'}?`)) return;

//    let data = { ID_USUARIO: idUsuario, FLAG_ESTADO: flagEstado, UPD_USUARIO: idUsuarioLogin };

//    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

//    let url = `${baseUrl}api/usuario/cambiarestadousuario`;

//    fetch(url, init)
//        .then(r => r.json())
//        .then(j => { if (j) $('#btnConsultar')[0].click(); });
//};

var nuevoUsuario = () => {
    $('#frmUsuario').removeData('idUsuario');
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#exampleModalLabel').html('REGISTRAR USUARIO');
    $('#txt-user-correo').val('');
    $('#txt-ruc').val('');
    limpiarDatosInstitucion();
    limpiarDatosUsuario();
    cambiarPropiedadLecturaInstitucion(false);
    cambiarPropiedadLecturaUsuario(false);
    $('#txt-user-correo').prop('readonly', false);
    $('#txt-ruc').prop('readonly', false);
    $('.editar-usuario-sres').removeClass('d-none');
    $('.admin-edit').removeClass('d-none');
    $('.spanNivelesColores').removeClass().addClass('spanNivelesColores');
    $('#nivelseguridad > span').html('');
    //$('#frmUsuario').show();
    //limpiarFormularioUsuario();
    //cargarFormularioUsuario();
}

//var cerrarFormularioUsuario = () => {
//    $('#frmUsuario').hide();
//}

//var limpiarFormularioUsuario = () => {
//    $('#frmUsuario').removeData();
//    $('#txtNombres').val('');
//    $('#txtApellidos').val('');
//    $('#txtCorreo').val('');
//    $('#txtTelefono').val('');
//    $('#txtAnexo').val('');
//    $('#txtCelular').val('');
//    $('#txtRucInstitucion').val('');
//    $('#txtRazonSocialInstitucion').val('');
//    $('#selIdRol').val(null);
//    $('#txtContraseña').val('');
//}

//var cargarFormularioUsuario = () => {
//    let urlRolListarPorEstado = `${baseUrl}api/rol/listarrolporestado?flagEstado=1`;
//    Promise.all([
//        fetch(urlRolListarPorEstado)
//    ])
//    .then(r => Promise.all(r.map(v => v.json())))
//    .then(([dataRol]) => {
//        cargarComboRol("#selIdRol", dataRol);
//    });
//}

//var cargarComboRol = (selector, data) => {
//    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ROL}">${x.NOMBRE}</option>`).join('');
//    $(selector).html(options);
//}

var consultarUsuarioCorreo = () => {
    if ($('#frmUsuario').data('idUsuario') != null) return;
    let correo = $('#txt-user-correo').val().trim();
    limpiarDatosUsuario();
    if(correo == '') { cambiarPropiedadLecturaUsuario(false); return;}
    if (!(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(correo))) { cambiarPropiedadLecturaUsuario(false); return;}

    let urlValidarUsuarioPorCorreo = `${baseUrl}api/usuario/validarusuarioporcorreo?correo=${correo}`;
    fetch(urlValidarUsuarioPorCorreo)
    .then(r => r.json())
    .then(j => cargarDatosUsuarioCorreo(j));
}

var cargarDatosUsuarioCorreo = (data) => {
    if(!data.EXISTE) cambiarPropiedadLecturaUsuario(true);
    else cambiarPropiedadLecturaUsuario(false);
}

var consultarInstitucion = () => {
    if ($('#frmUsuario').data('idUsuario') != null) return;
    let ruc = $('#txt-ruc').val().trim();
    if (ruc.length < 11) { return;}
    let url = `${baseUrl}api/institucion/obtenerinstitucionporruc?ruc=${ruc}`;

    fetch(url)
    .then(r => r.json())
    .then (j => cargarDatosInstitucion(j));
}

var cargarDatosInstitucion = (data) => {
    limpiarDatosInstitucion();
    if(data != null) $('#frmUsuario').data('idInstitucion', data.ID_INSTITUCION);
    else $('#frmUsuario').removeData('idInstitucion');
    cambiarPropiedadLecturaInstitucion(data == null ? true : false);
    $('#txt-institucion').val(data == null ? '' : data.RAZON_SOCIAL);
    $('#txt-direccion').val(data == null ? '' : data.DOMICILIO_LEGAL);
    $('#cbo-sector').val(data == null ? 0 : data.ID_SECTOR);
}

var limpiarDatosInstitucion = () => {
    $('#frmUsuario').removeData('idInstitucion');
    $('#txt-institucion').val('');
    $('#txt-direccion').val('');
    $('#cbo-sector').val(0);
    //cambiarPropiedadLecturaInstitucion(false);
}

var limpiarDatosUsuario = () => {
    $('#txt-nombre').val('');
    $('#txt-apellido').val('');
    $('#txt-telefono').val('');
    $('#txt-anexo').val('');
    $('#txt-celular').val('');
    $('#txt-pswd').val('');
    $('#txt-re-pswd').val('');
    $('#cbo-perfil').val(0);
    $('#rad-01').prop('checked', false);
    $('#rad-02').prop('checked', false);
    //cambiarPropiedadLecturaUsuario(false);
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
    $('#cbo-perfil').prop('disabled', !valor);
    $('#rad-01').prop('disabled', !valor);
    $('#rad-02').prop('disabled', !valor);
}

var consultarUsuario = (element) => {
    //$('#frmUsuario').show();
    //limpiarFormularioUsuario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#exampleModalLabel').html('ACTUALIZAR USUARIO');
    $('#txt-user-correo').val('');
    $('.editar-usuario-sres').addClass('d-none');
    limpiarDatosInstitucion();
    limpiarDatosUsuario();
    cambiarPropiedadLecturaInstitucion(false);
    cambiarPropiedadLecturaUsuario(false);    

    let idUsuario = $(element).attr('data-id');
    let urlUsuario = `${baseUrl}api/usuario/obtenerusuario?idUsuario=${idUsuario}`;

    fetch(urlUsuario)
    .then(r => r.json())
    .then (j => {
        //let urlRolListar = `${baseUrl}api/rol/listarrolporestado?flagEstado=1`;
        let urlInstitucionObtener = `${baseUrl}api/institucion/obtenerinstitucion?idInstitucion=${j.ID_INSTITUCION}`;
        Promise.all([
            fetch(urlInstitucionObtener)//,
            //fetch(urlRolListar)
        ])
        .then(r => Promise.all(r.map(v => v.json())))
        .then(([jInstitucion]) => {
            //cargarComboRol('#selIdRol', jRol);
            cargarDatosUsuario(j);
            $('#txt-ruc').val(jInstitucion.RUC);
            $('#txt-institucion').val(jInstitucion.RAZON_SOCIAL);
            $('#txt-direccion').val(jInstitucion.DOMICILIO_LEGAL);
            $('#cbo-sector').val(jInstitucion.ID_SECTOR);
            cambiarPropiedadLecturaUsuario(true);
            $('#txt-ruc').prop('readonly', true);
            flag_ndc = jInstitucion.FLAG_APORTENDC;
        });
    });
}

var cargarDatosUsuario = (data) => {
    $('#frmUsuario').data('idUsuario', data.ID_USUARIO);
    $('#txt-nombre').val(data.NOMBRES);
    $('#txt-apellido').val(data.APELLIDOS);
    $('#txt-user-correo').val(data.CORREO);
    $('#txt-telefono').val(data.TELEFONO);
    $('#txt-anexo').val(data.ANEXO);
    $('#txt-celular').val(data.CELULAR);
    $('#frmUsuario').data('idInstitucion', data.ID_INSTITUCION);
    $('#cbo-perfil').val(data.ID_ROL);
    $('#rad-01').prop('checked', data.FLAG_ESTADO == 1 ? true : false);
    $('#rad-02').prop('checked', data.FLAG_ESTADO == 2 ? true : false);
    $('#txt-user-correo').prop('readonly', true);
    if (data.ID_ROL == 1) $('.admin-edit').addClass('d-none');
    else $('.admin-edit').removeClass('d-none');
}

var guardarUsuario = () => {
    $('.alert-add').html('');
    let arr = [];
    if ($('#txt-nombre').val().trim() === "") arr.push("Debe ingresar el/los nombre(s)");
    if ($('#txt-apellido').val().trim() === "") arr.push("Debe ingresar el/los apellido(s)");
    if ($('#txt-celular').val().trim() === "") arr.push("Debe ingresar el celular");
    if ($("#txt-ruc").val().length < 11) arr.push("El ruc debe contener 11 caracteres");
    if ($("#txt-ruc").val().trim().length > 2) {ruc = $("#txt-ruc").val().substring(0, 2);
        if (ruc != '20' && ruc != '10') arr.push("El ruc debe iniciar con el número 20 o 10");}    
    if ($("#txt-institucion").val().trim() === "") arr.push("Debe ingresar el nombre o la razón social de la Institución");
    if ($("#cbo-sector").val() == 0) arr.push("Debe seleccionar un sector");
    if ($('#cbo-perfil').val() == 0) arr.push("Debe seleccionar un perfil");
    if ($("#frmUsuario").data("idUsuario") == null) {
        if ($("#txt-pswd").val().trim() === $("#txt-re-pswd").val().trim()) {
            if (!(/[a-zñ]/.test($("#txt-pswd").val().trim()) && /[A-ZÑ]/.test($("#txt-pswd").val().trim()) && /[0-9]/.test($("#txt-pswd").val().trim()) && /[!@#$&*]/.test($("#txt-pswd").val().trim()))) arr.push("La contraseña debe contener minúscula(s), mayúscula(s), número(s) y caracter(es) especial(es) [!@#$&*]");
            if ($("#txt-pswd").val().trim().length < 6) arr.push("La contraseña debe contener 6 o más caracteres por seguridad"); }
        else arr.push("Las contraseñas no coinciden"); }   
    if (!$('#rad-01').prop('checked') && !$('#rad-02').prop('checked')) arr.push('Debe seleccionar un estado');

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }
    let inst = {
        ID_INSTITUCION: $('#frmUsuario').data('idInstitucion'),
        RUC: $('#txt-ruc').val(),
        RAZON_SOCIAL: $('#txt-institucion').val(),
        DOMICILIO_LEGAL: $('#txt-direccion').val(),
        ID_SECTOR: $('#cbo-sector').val(),
        FLAG_APORTENDC: $('#frmUsuario').data('idUsuario') == null ? '0' : $('#cbo-perfil').val() == 3 ? flag_ndc : '0',
        UPD_USUARIO: idUsuarioLogin
    }

    let idUsuario = $('#frmUsuario').data('idUsuario');
    let nombres = $('#txt-nombre').val();
    let apellidos = $('#txt-apellido').val();
    let correo = $('#txt-user-correo').val();
    let telefono = $('#txt-telefono').val();
    let anexo = $('#txt-anexo').val();
    let celular = $('#txt-celular').val();
    let idInstitucion = $('#frmUsuario').data('idInstitucion');
    let idRol = $('#cbo-perfil').val();
    let contraseña = $('#txt-pswd').val().trim();
    let flagEstado = $('#rad-01').prop('checked') ? '1' : $('#rad-02').prop('checked') ? '2' : '0';

    let url = `${baseUrl}api/usuario/guardarusuario`;
    let data = { ID_USUARIO: idUsuario == null ? -1 : idUsuario, CONTRASENA: contraseña, NOMBRES: nombres, APELLIDOS: apellidos, CORREO: correo, TELEFONO: telefono, ANEXO: anexo, CELULAR: celular, ID_INSTITUCION: idInstitucion, ID_ROL: idRol, FLAG_ESTADO: flagEstado, INSTITUCION: inst, UPD_USUARIO: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-add').html('');
        if (j) { $('#btnGuardar').hide(); $('#btnGuardar').next().html('Cerrar'); }
        j ? $('.alert-add').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los datos fueron guardados correctamente.', close: { time: 1000 }, url: `` }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
        if (j) $('#btnConsultar')[0].click();
    });
}

var cargarInformacionInicial = () => {
    let urlListarSectorPorEstado = `${baseUrl}api/sector/listarsectorporestado?flagEstado=1`;
    let urlListaRol = `${baseUrl}api/rol/listarrolporestado?flagEstado=1`;
    Promise.all([
        fetch(urlListarSectorPorEstado),
        fetch(urlListaRol)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([listaSector, listaRol]) => {
        cargarComboSector('#cbo-sector', listaSector);
        cargarComboRol('#cbo-perfil', listaRol);
    })
}

var cargarComboSector = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_SECTOR}">${x.NOMBRE}</option>`).join('');
    options = `<option value="0">-Seleccione un sector-</option>${options}`;
    $(selector).html(options);
}

var cargarComboRol = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option class="badge badge-${x.ID_ROL == 1 ? 'success' : x.ID_ROL == 2 ? 'info' : 'primary'} font-weight-bold" value="${x.ID_ROL}">${x.NOMBRE}</option>`).join('');
    options = `<option value="0">-Seleccione un perfil-</option>${options}`;
    $(selector).html(options);
}

$(document).on("keydown", ".solo-numero", function (e) {
    var key = window.e ? e.which : e.keyCode;
    //var id = $("#" + e.target.id)[0].type;
    if ((key < 48 || key > 57) && (event.keyCode < 96 || event.keyCode > 105) && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46) return false;
});

$(document).on("keydown", ".sin-espacio", function (e) {
    var key = window.e ? e.which : e.keyCode;
    if (key == 32) return false;
});

$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnConfirmar').on('click', (e) => eliminar());
    $('#btnGuardar').on('click', (e) => guardar());
    $('input[type="file"][id="fle-imagen"]').on('change', fileChange);
});

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
    let busqueda = $('#txt-descripcion').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();
    let columna = $("#columna").val();
    let orden = $("#orden").val();
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/medidamitigacion/buscarmedidamitigacion?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblCriterio');
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
                    consultarCriterio(e.currentTarget);
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
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_MEDMIT}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colNombres = `<td class="text-left" data-encabezado="Nombre">${x.NOMBRE}</td>`;
            let colDescripcion = `<td data-encabezado="Descripción"><div class="text-limit-1">${x.DESCRIPCION == null ? '' : x.DESCRIPCION}</div></td>`;
            let colImagen = `<td class="text-center" data-encabezado="Imagen"><img src="${baseUrl}${$('#ruta').val().replace('{0}', x.ID_MEDMIT)}/${x.ARCHIVO_BASE == null ? '' : x.ARCHIVO_BASE}" width="50%" height="auto"></td>`;
            let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_MEDMIT}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_MEDMIT}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnCambiarEstado}${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colCodigo}${colNombres}${colDescripcion}${colImagen}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var cambiarEstado = (element) => {
    idEliminar = $(element).attr('data-id');
    $("#modal-confirmacion").modal('show');
};

var eliminar = () => {
    if (idEliminar == 0) return;
    let data = { ID_MEDMIT: idEliminar, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    let url = `${baseUrl}api/medidamitigacion/cambiarestadomedidamitigacion`;
    fetch(url, init)
        .then(r => r.json())
        .then(j => {
            if (j) { $('#btnConsultar')[0].click(); $("#modal-confirmacion").modal('hide'); }
        });
}

var nuevo = () => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    //$('#txt-codigo').parent().parent().removeClass('d-none');
    $('#txt-codigo').prop('readonly', false);
    $('#exampleModalLabel').html('REGISTRAR MEDIDA MITIGACIÓN');
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txt-codigo').val('');
    $('#txt-nombre').val('');
    $('#txa-descripcion').val('');
    $('#txt-imagen').val('');
    $('#fle-imagen').val('');
    $('#fle-imagen').removeData('file');
}

var consultarCriterio = (element) => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    //$('#txt-codigo').parent().parent().addClass('d-none');
    $('#txt-codigo').prop('readonly', true);
    $('#exampleModalLabel').html('ACTUALIZAR MEDIDA MITIGACIÓN');
    let id = $(element).attr('data-id');

    let url = `${baseUrl}api/medidamitigacion/obtenermedidamitigacion?idMedida=${id}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    $('#frm').data('id', data.ID_MEDMIT);
    $('#txt-codigo').val(data.ID_MEDMIT);
    $('#txt-nombre').val(data.NOMBRE);
    $('#txa-descripcion').val(data.DESCRIPCION);
    $('#txt-imagen').val(data.ARCHIVO_BASE);
    data.ARCHIVO_CONTENIDO == null ? '' : $(`#fle-imagen`).data('file', data.ARCHIVO_CONTENIDO);
}

var guardar = () => {
    $('.alert-add').html('');
    let verif = $('#fle-imagen').data('file') != null ? true : false;
    if (!verif) {
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Necesita ingresar una imagen' });
        return;
    }

    let arr = [];
    if ($('#txt-codigo').val().trim() === "" || $('#txt-codigo').val() == "0") arr.push("Ingrese un código para la medida de mitigación válido");
    if ($('#txt-nombre').val().trim() === "") arr.push("Ingrese el nombre de la medida de mitigación");
    if ($('#txa-descripcion').val().trim() === "") arr.push("Ingrese la descripción de la medida mitigación");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let id = $('#frm').data('id');
    let codigo = $('#txt-codigo').val();
    let nombre = $('#txt-nombre').val();
    let descripcion = $('#txa-descripcion').val();
    let nombrefile = $(`#txt-imagen`).val();
    let archivo = $('#fle-imagen').data('file');

    let url = `${baseUrl}api/medidamitigacion/guardarmedidamitigacion`;
    let data = { ID_MEDMIT: codigo, VAL: id == null ? -1 : id, NOMBRE: nombre, DESCRIPCION: descripcion, ARCHIVO_BASE: nombrefile, ARCHIVO_CONTENIDO: archivo, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        //$('.alert-add').html('');
        //if (j) { $('#btnGuardar').hide(); $('#btnGuardar').next().html('Cerrar'); }
        //j ? $('.alert-add').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los datos fueron guardados correctamente.', close: { time: 1000 }, url: `` }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
        //if (j) $('#btnConsultar')[0].click();
        $('.alert-add').html('');
        if (j.OK) {
            $('#btnGuardar').hide(); $('#btnGuardar').next().html('Cerrar');
            $('.alert-add').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los datos fueron guardados correctamente.', close: { time: 1000 }, url: `` });
            $('#btnConsultar')[0].click();
        } else
            $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: j.VAL == 1 ? 'Inténtelo nuevamente por favor.' : 'El código ingresado ha sido usado.' });
    });
}

var fileChange = (e) => {
    $('.alert-add').html('');
    let elFile = $(e.currentTarget);
    var fileContent = e.currentTarget.files[0];

    switch (fileContent.name.substring(fileContent.name.lastIndexOf('.') + 1).toLowerCase()) {
        case 'jpg': case 'jpeg': case 'png': case 'svg': break;
        default: $('.alert-add').alertWarning({ type: 'warning', title: 'ADVERTENCIA', message: `El archivo tiene una extensión no permitida` }); $('#txt-imagen').val(''); return false; break;
    }

    if (fileContent.size > maxBytes) { $('.alert-add').alertWarning({ type: 'warning', title: 'ADVERTENCIA', message: `La imagen debe tener un peso máximo de 4MB` }); $('#txt-imagen').val(''); return false; }
    else
        $('.alert-add').html('');

    if (e.currentTarget.files.length == 0) {
        $(e.currentTarget).removeData('file');
        $(e.currentTarget).removeData('fileContent');
        $(e.currentTarget).removeData('type');
        return;
    }

    $(`#txt-imagen`).val(fileContent.name);
    let reader = new FileReader();
    reader.onload = function (e) {
        debugger;
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
        elFile.data('fileContent', e.currentTarget.result);
        elFile.data('type', fileContent.type);
    }
    reader.readAsDataURL(e.currentTarget.files[0]);
}

$(document).on("keydown", ".solo-numero", function (e) {
    var key = window.e ? e.which : e.keyCode;
    //var id = $("#" + e.target.id)[0].type;
    if ((key < 48 || key > 57) && (event.keyCode < 96 || event.keyCode > 105) && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46) return false;
});
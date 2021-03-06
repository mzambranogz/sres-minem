﻿$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnConfirmar').on('click', (e) => eliminar());
    $('#btnGuardar').on('click', (e) => guardar());
    $('input[type="file"][id="fle-imagen"]').on('change', fileChange);
    cargarInformacionInicial();
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
    //let busqueda = $('#textoBusqueda').val();
    //let registros = 10;
    //let pagina = 1;
    //let columna = 'ID_CRITERIO';
    //let orden = 'ASC'
    //let params = { busqueda, registros, pagina, columna, orden };
    let busqueda = $('#txt-descripcion').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();
    let columna = $("#columna").val();
    let orden = $("#orden").val();
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/criterio/buscarcriterio?${queryParams}`;

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

            //let tabla = $('#tblCriterio');
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
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_CRITERIO}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colNombres = `<td class="text-left" data-encabezado="Nombre">${x.NOMBRE}</td>`;
            let colCategoria = `<td class="text-center" data-encabezado="Categoría">${x.CATEGORIA}</td>`;
            let colDescripcion = `<td data-encabezado="Descripción">${x.DESCRIPCION == null ? '' : x.DESCRIPCION}</td>`;
            let colDescripcionCorta = `<td data-encabezado="Descripción corta">${x.DESCRIPCION_CORTA == null ? '' : x.DESCRIPCION_CORTA}</td>`;
            let colImagen = `<td class="text-center" data-encabezado="Imagen"><img src="${baseUrl}${$('#ruta').val().replace('{0}', x.ID_CRITERIO)}/${x.ARCHIVO_BASE == null ? '' : x.ARCHIVO_BASE}" width="50%" height="auto"></td>`;
            let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_CRITERIO}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_CRITERIO}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnCambiarEstado}${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colCodigo}${colNombres}${colCategoria}${colDescripcion}${colDescripcionCorta}${colImagen}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var cambiarEstado = (element) => {
    idEliminar = $(element).attr('data-id');
    $("#modal-confirmacion").modal('show');
    //let id = $(element).attr('data-id');
    //if (!confirm(`¿Está seguro que desea eliminar este registro?`)) return;
    //let data = { ID_CRITERIO: id, USUARIO_GUARDAR: idUsuarioLogin };
    //let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    //let url = `${baseUrl}api/criterio/cambiarestadocriterio`;
    //fetch(url, init)
    //    .then(r => r.json())
    //    .then(j => { if (j) $('#btnConsultar')[0].click(); });
};

var eliminar = () => {
    if (idEliminar == 0) return;
    let data = { ID_CRITERIO: idEliminar, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    let url = `${baseUrl}api/criterio/cambiarestadocriterio`;
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
    $('#exampleModalLabel').html('REGISTRAR CRITERIO');
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txt-nombre').val('');
    $('#txa-descripcion').val('');
    $('#txa-descripcion-corta').val('');
    $('#txt-descripcion-valor').val('');
    $('#txt-imagen').val('');
    $('#fle-imagen').val('');
    $('#fle-imagen').removeData('file');
    $('#cbo-categoria').val(0);
}

var consultarCriterio = (element) => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#exampleModalLabel').html('ACTUALIZAR CRITERIO');
    let id = $(element).attr('data-id');

    let url = `${baseUrl}api/criterio/obtenercriterio?idCriterio=${id}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    $('#frm').data('id', data.ID_CRITERIO);
    $('#txt-nombre').val(data.NOMBRE);
    $('#txa-descripcion').val(data.DESCRIPCION);
    $('#txa-descripcion-corta').val(data.DESCRIPCION_CORTA == null ? '' : data.DESCRIPCION_CORTA);
    $('#txt-descripcion-valor').val(data.DESCRIPCION_VALOR == null ? '' : data.DESCRIPCION_VALOR);
    $('#cbo-categoria').val(data.ID_CATEGORIA);
    $('#txt-imagen').val(data.ARCHIVO_BASE);
    data.ARCHIVO_CONTENIDO == null ? '' : $(`#fle-imagen`).data('file', data.ARCHIVO_CONTENIDO);
}

var guardar = () => {
    $('.alert-add').html('');
    let arr = [];
    let verif = $('#fle-imagen').data('file') != null ? true : false;
    if (!verif) {
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Necesita ingresar una imagen' });
        return;
    }

    if ($('#txt-nombre').val().trim() === "") arr.push("Debe ingresar el nombre del criterio");
    if ($("#cbo-categoria").val() == 0) arr.push("Debe seleccionar una categoría");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let idCriterio = $('#frm').data('id');
    let nombre = $('#txt-nombre').val();
    let descripcion = $('#txa-descripcion').val();
    let descripcion_corta = $('#txa-descripcion-corta').val();
    let descripcion_valor = $('#txt-descripcion-valor').val();
    let nombrefile = $(`#txt-imagen`).val();
    let archivo = $('#fle-imagen').data('file');
    let categoria = $('#cbo-categoria').val();
    
    let url = `${baseUrl}api/criterio/guardarcriterio`;
    let data = { ID_CRITERIO: idCriterio == null ? -1 : idCriterio, NOMBRE: nombre, DESCRIPCION: descripcion, ARCHIVO_BASE: nombrefile, ARCHIVO_CONTENIDO: archivo, DESCRIPCION_CORTA: descripcion_corta, DESCRIPCION_VALOR: descripcion_valor, ID_CATEGORIA: categoria, USUARIO_GUARDAR: idUsuarioLogin };
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

var cargarInformacionInicial = () => {
    let urlListar = `${baseUrl}api/criterio/obtenerallcategoria`;
    Promise.all([
        fetch(urlListar),
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([lista]) => {
        cargarCombo('#cbo-categoria', lista);
    })
}

var cargarCombo = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_CATEGORIA}">${x.NOMBRE}</option>`).join('');
    options = `<option value="0">-Seleccione una categoría-</option>${options}`;
    $(selector).html(options);
}
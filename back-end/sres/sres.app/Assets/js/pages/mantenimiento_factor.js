$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    //$('#btnConfirmar').on('click', (e) => eliminar());
    $('#btnGuardar').on('click', (e) => guardar());
    $('#add-lista-param').on('click', (e) => agregarParametro());
    consultarListas();
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

    let url = `${baseUrl}api/factor/buscarfactor?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblPrincipal');
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
                    consultarDatos(e.currentTarget);
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
            let detalle = '';
            let colNro = `<td class="text-center" data-encabezado="Número de orden" scope="row" data-count="0">${(pagina - 1) * registros + (i + 1)}</td>`;
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_FACTOR}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colNombres = `<td class="text-left" data-encabezado="Nombre">${x.NOMBRE}</td>`;
            debugger;
            if (x.LISTA_PARAM_FACTOR != null) {
                if (x.LISTA_PARAM_FACTOR.length > 0) {
                    detalle = x.LISTA_PARAM_FACTOR.map((x, y) => {
                        return `<div class="p-1 text-center border-right"><div class="h6 span badge badge-info w-100">${x.NOMBRE_DETALLE}</div></div>`;
                    }).join('');
                    detalle = `<div class="form-control"><div class="list-group sortable-list m-0">${detalle}</div></div>`;
                }
            }
            let colValores = `<td class="text-center" data-encabezado="Valores">${detalle}</td>`
            //let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_CRITERIO}-${x.ID_CASO}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_FACTOR}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colCodigo}${colNombres}${colValores}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var consultarDatos = (element) => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#exampleModalLabel').html('ACTUALIZAR FACTOR');

    let id = $(element).attr('data-id');
    let url = `${baseUrl}api/factor/obtenerfactor?idfactor=${id}`;
    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    if (data == null) return false;
    $('#frm').data('id', data.ID_FACTOR);
    $('#txt-nombre').val(data.NOMBRE);
    $('#txt-etiqueta-factor').val(data.SOBRE_NOMBRE == null ? '' : data.SOBRE_NOMBRE);
    if (data.LISTA_PARAM_FACTOR != null) {
        if (data.LISTA_PARAM_FACTOR.length > 0) {
            let param = data.LISTA_PARAM_FACTOR.map((x, y) => {
                let small = `<small><i class="fas fa-list"></i>${x.NOMBRE_DETALLE}</small>`;
                let input = `<input type="hidden" class="hidden-control field-ctrol" value="cbo">`;
                let icono = `<i class="fas fa-minus-circle cursor-pointer m-2 delete-columna-detalle" data-toggle="tooltip" data-placement="top" title="" data-original-title="Eliminar parámetro"></i>`;
                let content = `<div class="btn btn-secondary btn-sm w-100 d-flex flex-row align-items-center justify-content-between my-2" id="param-${x.ID_PARAMETRO}" data-name="${x.NOMBRE_DETALLE}" data-detalle="${x.ID_DETALLE}">${small}${input}${icono}</div>`;
                return content;
            }).join('');
            $('#filas-parametro').html(param);
        }
    }
}

var guardar = () => {
    $('.alert-add').html('');

    let arr = [];
    if ($('#txt-nombre').val().trim() === "") arr.push("Ingrese el nombre del factor");
    if ($('#txt-etiqueta-factor').val().trim() === "") arr.push("Ingrese la etiqueta del factor");
    if ($('[id^=param-]').length == 0) arr.push("Ingrese los Parámetros asociados al factor, verifique antes de continuar");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let id = $('#frm').data('id');
    let nombre = $('#txt-nombre').val();
    let etiqueta = $(`#txt-etiqueta-factor`).val();
    let tipoControl = $('#cbo-tipo-control').val();
    let parametro = [], i = 1;
    $("[id^=param-]").each((x, y) => {
        debugger;
        var f = {
            ID_PARAMETRO: $(y).attr('id').split('-')[1],
            ID_DETALLE: $(y).data('detalle') == "" ? 0 : $(y).data('detalle'),
            NOMBRE_DETALLE: $(y).data('name'),
            ORDEN: i++,
            USUARIO_GUARDAR: idUsuarioLogin
        }
        parametro.push(f);
    });

    let url = `${baseUrl}api/factor/guardarfactor`;
    let data = {
        ID_FACTOR: id == null ? -1 : id,
        NOMBRE: nombre,
        SOBRE_NOMBRE: etiqueta,
        LISTA_PARAM_FACTOR: parametro,
        USUARIO_GUARDAR: idUsuarioLogin
    };
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

var agregarParametro = () => {
    if ($('#cbo-parametro').val() == 0) return;
    let verif = true;
    $("[id^=param-]").each((x, y) => {
        if ($(y).attr('id').split('-')[1] == $('#cbo-parametro').val()) { verif = false; }
    });
    if (!verif) { $('#cbo-parametro').val(0); return; }
    let small = `<small><i class="fas fa-list"></i>${$("#cbo-parametro option:selected").html()}</small>`;
    let input = `<input type="hidden" class="hidden-control field-ctrol" value="cbo">`;
    let icono = `<i class="fas fa-minus-circle cursor-pointer m-2 delete-columna-detalle" data-toggle="tooltip" data-placement="top" title="" data-original-title="Eliminar parámetro"></i>`;
    let content = `<div class="btn btn-secondary btn-sm w-100 d-flex flex-row align-items-center justify-content-between my-2" id="param-${$('#cbo-parametro').val()}" data-name="${$("#cbo-parametro option:selected").html()}" data-detalle="">${small}${input}${icono}</div>`;
    $('#filas-parametro').append(content);
    $('#cbo-parametro').val(0);
}

var consultarListas = () => {
    let url = `${baseUrl}api/parametro/obtenerallparametrolista`;
    fetch(url).then(r => r.json()).then(j => {
        let contenido = ``;
        if (j.length > 0) {
            contenido = j.map((x, y) => {
                return `<option value="${x.ID_PARAMETRO}">${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-parametro').html(`<option value="0">-Seleccione-</option>${contenido}`)
    });
}

var nuevo = () => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#exampleModalLabel').html('REGISTRAR FACTOR');
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txt-nombre').val('');
    $('#txt-etiqueta-factor').val('');
    $('#cbo-parametro').val(0);
    $("#filas-parametro").html('');
}
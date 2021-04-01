$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    //$('#btnConfirmar').on('click', (e) => eliminar());
    $('#btnGuardar').on('click', (e) => guardar());
    $('#cbo-tipo-control').on('change', (e) => changeTipoControl());
    $('#cbo-tipo-dato').on('change', (e) => changeTipoDato());
    $('.no-estatico').on('click', (e) => validarNoEstatico(e));
    $('#rad-estatico').on('click', (e) => validarEstatico());
    $('.unico').on('click', (e) => validarUnico(e));
    $('#add-lista').on('click', (e) => validarParametro());
    $('#edit-lista').on('click', (e) => actualizarParametro());
    $('#add-lista-param').on('click', (e) => agregarParametroFiltro());
    $('.tipo-lista-alterna').addClass('d-none');
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

    let url = `${baseUrl}api/parametro/buscarparametro?${queryParams}`;

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
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_PARAMETRO}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colNombres = `<td class="text-left" data-encabezado="Nombre">${x.NOMBRE}</td>`;
            let colTipoControl = `<td class="text-left" data-encabezado="Tipo Control">${x.TIPO_CONTROL}</td>`;            
            if (x.ID_TIPO_CONTROL == 1) {
                if (x.LISTA_DET.length > 0) {
                    detalle = x.LISTA_DET.map((x,y) => {
                        return `<div class="p-1 text-center border-right"><div class="h6 span badge badge-info w-100">${x.NOMBRE}</div></div>`;
                    }).join('');
                    detalle = `<div class="form-control"><div class="list-group sortable-list m-0">${detalle}</div></div>`;
                }
            }
            let colValores = `<td class="text-center" data-encabezado="Valores">${detalle}</td>`
            //let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_CRITERIO}-${x.ID_CASO}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_PARAMETRO}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colCodigo}${colNombres}${colTipoControl}${colValores}${colOpciones}</tr>`;
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
    $('#exampleModalLabel').html('ACTUALIZAR PARÁMETRO');

    let id = $(element).attr('data-id');
    let url = `${baseUrl}api/parametro/obtenerparametro?idparametro=${id}`;
    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    if (data == null) return false;
    $('#frm').data('id', data.ID_PARAMETRO);
    $('#txt-nombre').val(data.NOMBRE);
    $('#txt-etiqueta-parametro').val(data.ETIQUETA == null ? '' : data.ETIQUETA);
    $('#cbo-tipo-control').parent().parent().hide();
    $('#cbo-tipo-control').val(data.ID_TIPO_CONTROL);
    idTipoDato = data.ID_TIPO_DATO == null ? 0 : data.ID_TIPO_DATO;
    changeTipoControl();
    if (data.ID_TIPO_CONTROL == 1) {
        if (data.LISTA_DET.length > 0) {
            let detalle = data.LISTA_DET.map((x, y) => {
                let icono = `<i class="fas fa-list"></i>`;
                let nombre = `<small class="badge badge-info mostrar-valor" style="width: calc(100% - 42px); overflow: hidden;margin-left: 10px;">${x.NOMBRE}</small>`;
                let ctrlNombre = `<input class="hidden-control field-ctrol nombre" type="hidden" value="${x.NOMBRE}">`;
                let ctrlValor = `<input class="hidden-control field-ctrol valor" data-delete="1" type="hidden" value="${x.ID_DETALLE}">`;
                let delet = `<i class="fas fa-minus-circle cursor-pointer m-2 delete-columna-detalle esconder" data-parent="2" data-toggle="tooltip"  onclick="eliminarParametro(${x.ID_DETALLE});"  data-placement="top" title="" data-original-title="Eliminar"></i>`;
                let edit = `<i class="fas fa-edit cursor-pointer m-2 esconder" data-toggle="tooltip" data-placement="top" onclick="editarParametro(${x.ID_DETALLE},this);" title="" data-original-title="Editar"></i>`;
                let opciones = `<div class="opciones">${delet}${edit}</div>`;
                let contentDellate = `<div class="btn btn-secondary btn-sw w-100 d-flex flex-row align-items-center justify-content-between my-2 factor-div">${icono}${nombre}${ctrlNombre}${ctrlValor}${opciones}</div>`;
                return contentDellate;
            }).join('');
            $("#filas-valor").html(detalle);
        }

        if (data.LISTA_PARAM != null) {
            if (data.LISTA_PARAM.length > 0) {
                let param = data.LISTA_PARAM.map((x, y) => {
                    let small = `<small><i class="fas fa-list"></i>${x.NOMBRE}</small>`;
                    let input = `<input type="hidden" class="hidden-control field-ctrol" value="cbo">`;
                    let icono = `<i class="fas fa-minus-circle cursor-pointer m-2 delete-columna-detalle" data-toggle="tooltip" data-placement="top" title="" data-original-title="Eliminar parámetro"></i>`;
                    let content = `<div class="btn btn-secondary btn-sm w-100 d-flex flex-row align-items-center justify-content-between my-2" id="param-${x.ID_PARAMETRO}">${small}${input}${icono}</div>`;
                    return content;
                }).join('');
                $('#filas-parametro').html(param);
            }
        }        
    }
    debugger;
    $("#rad-estatico").prop("checked", data.ESTATICO == '1' ? true : false);
    $("#rad-editable").prop("checked", data.EDITABLE == '1' ? true : false);
    $("#rad-verificable").prop("checked", data.VERIFICABLE == '1' ? true : false);
    //$("#rad-filtro").prop("checked", data.FILTRO == '1' ? true : false);
    $("#rad-decimal").prop("checked", data.DECIMAL_V == '1' ? true : false);
    $("#rad-resultado").prop("checked", data.RESULTADO == '1' ? true : false);
    $("#rad-emisiones").prop("checked", data.EMISIONES == '1' ? true : false);
    $("#rad-ahorro").prop("checked", data.AHORRO == '1' ? true : false);
    $("#rad-combustible").prop("checked", data.COMBUSTIBLE == '1' ? true : false);
    $('#txt-descripcion-parametro').val(data.DESCRIPCION == null ? '' : data.DESCRIPCION);
    $('#txt-combinacion-unidades').val(data.UNIDAD == null ? '' : data.UNIDAD);
    $('#txt-tamano').val(data.TAMANO);
}

var guardar = () => {
    $('.alert-add').html('');

    let arr = [];
    if ($('#txt-nombre').val().trim() === "") arr.push("Ingrese el nombre del parámetro");
    if ($('#txt-etiqueta-parametro').val().trim() === "") arr.push("Ingrese la etiqueta del parámetro");
    if ($('#cbo-tipo-control').val() > 1) if ($('#cbo-tipo-dato').val() == 0) arr.push("Seleccione el tipo de caja");
    if ($('#cbo-tipo-control').val() > 1) if ($('#txt-tamano').val().trim() === "" || $('#txt-tamano').val() == "0") arr.push("El número del tamaño ingresado no es válido");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let id = $('#frm').data('id');
    let nombre = $('#txt-nombre').val();
    let etiqueta = $(`#txt-etiqueta-parametro`).val();
    let tipoControl = $('#cbo-tipo-control').val();
    let tipoDato = $('#cbo-tipo-dato').val();
    let parametro = [];
    if (tipoControl == 1) {
        $("#filas-valor").find(".factor-div").each((x,y) => {
            let indx = {
                ID_PARAMETRO: id == null ? 0 : id,
                ID_DETALLE: $(y).find(".valor").val(),
                NOMBRE: $(y).find(".nombre").val(),
                USUARIO_GUARDAR: idUsuarioLogin
            };
            parametro.push(indx);
        });
    }
    let estatico = $("#rad-estatico").prop("checked") ? '1' : '0';
    let editable = $("#rad-editable").prop("checked") ? '1' : '0';
    let verificable = $("#rad-verificable").prop("checked") ? '1' : '0';
    //let filtro = $("#rad-filtro").prop("checked") ? '1' : '0';
    let decimal = $("#rad-decimal").prop("checked") ? '1' : '0';
    let resultado = $("#rad-resultado").prop("checked") ? '1' : '0';
    let emisiones = $("#rad-emisiones").prop("checked") ? '1' : '0';
    let ahorro = $("#rad-ahorro").prop("checked") ? '1' : '0';
    let combustible = $("#rad-combustible").prop("checked") ? '1' : '0';
    let descripcion = $('#txt-descripcion-parametro').val();
    let unidad = $('#txt-combinacion-unidades').val();
    let tamano = $('#txt-tamano').val();
    let id_delete = "";
    if ($("#parametros-id").data("eliminar") != "") { id_delete = $("#parametros-id").data("eliminar").substring(0, $("#parametros-id").data("eliminar").length - 1); }
    let filtro = $("[id^=param-]").length == 0 ? '0' : idFiltro();

    let url = `${baseUrl}api/parametro/guardarparametro`;
    let data = {
        ID_PARAMETRO: id == null ? -1 : id,
        NOMBRE: nombre,
        ETIQUETA: etiqueta,
        ID_TIPO_CONTROL: tipoControl,
        ID_TIPO_DATO: tipoDato,
        ESTATICO: estatico,
        EDITABLE: editable,
        VERIFICABLE: verificable,
        FILTRO: filtro,
        DECIMAL_V: decimal,
        RESULTADO: resultado,
        EMISIONES: emisiones,
        AHORRO: ahorro,
        COMBUSTIBLE: combustible,
        DESCRIPCION: descripcion,
        UNIDAD: unidad,
        TAMANO: tamano,
        ID_DELETE_DETALLE: id_delete,
        LISTA_DET: parametro,
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

var changeTipoControl = () => {
    limpiarCheck();
    if ($('#cbo-tipo-control').val() == 1) {
        $('#cbo-tipo-dato').val(0);
        $('#txt-tamano').val('0');
        $('.tipo-caja').addClass('d-none');
        $('.tipo-lista').removeClass('d-none');
        //$('#rad-filtro').parent().parent().removeClass('d-none');
        $('#rad-decimal').parent().parent().addClass('d-none');
        $('#rad-estatico').parent().parent().addClass('d-none');
        $('#rad-resultado').parent().parent().addClass('d-none');
        $('#rad-emisiones').parent().parent().addClass('d-none');
        $('#rad-ahorro').parent().parent().addClass('d-none');
        $('#rad-combustible').parent().parent().addClass('d-none'); 
        $('#txt-tamano').parent().parent().addClass('d-none');
    }
    else {
        $("#filas-valor").html('');
        $("#filas-parametro").html('');
        $('.tipo-caja').removeClass('d-none');
        $('.tipo-lista').addClass('d-none');
        //$('#rad-filtro').parent().parent().addClass('d-none');
        $('#rad-estatico').parent().parent().removeClass('d-none');
        $('#txt-tamano').parent().parent().removeClass('d-none');
        let opcion = $('#cbo-tipo-control').val() == 2 ? '<option value="0">-seleccione-</option><option value="1">Numérico</option><option value="2">Texto</option><option value="3">Fecha</option>' : '<option value="0">-seleccione-</option><option value="2">Texto</option>';
        $('#cbo-tipo-dato').html(opcion);
        if (idTipoDato > 0) { $('#cbo-tipo-dato').val(idTipoDato); idTipoDato = 0; changeTipoDato(); }
    }
}

var changeTipoDato = () => {
    limpiarCheck();
    if ($('#cbo-tipo-dato').val() == 0) return;
    if ($('#cbo-tipo-dato').val() == 1) {
        $('#rad-decimal').parent().parent().removeClass('d-none');
        $('#rad-resultado').parent().parent().removeClass('d-none');
        $('#rad-emisiones').parent().parent().removeClass('d-none');
        $('#rad-ahorro').parent().parent().removeClass('d-none');
        $('#rad-combustible').parent().parent().removeClass('d-none');
    }
    else {
        $('#rad-decimal').parent().parent().addClass('d-none');
        $('#rad-resultado').parent().parent().addClass('d-none');
        $('#rad-emisiones').parent().parent().addClass('d-none');
        $('#rad-ahorro').parent().parent().addClass('d-none');
        $('#rad-combustible').parent().parent().addClass('d-none');
    }
}

var nuevo = () => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#exampleModalLabel').html('REGISTRAR PARÁMETRO');
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txt-nombre').val('');
    $('#txt-etiqueta-parametro').val('');
    $('#txt-descripcion-parametro').val('');
    $('#cbo-tipo-control').parent().parent().show();
    limpiarCheck();
    $('#cbo-tipo-control').val(1);
    changeTipoControl();
    $('#cbo-tipo-dato').val(0);
    $("#filas-valor").html('');
    $("#filas-parametro").html('');
}

var limpiarCheck = () => {
    $('#rad-decimal').prop('checked', false);
    $('#rad-verificable').prop('checked', false);
    $('#rad-editable').prop('checked', false);
    $('#rad-estatico').prop('checked', false);
    //$('#rad-filtro').prop('checked', false);
    $('#rad-resultado').prop('checked', false);
    $('#rad-emisiones').prop('checked', false);
    $('#rad-ahorro').prop('checked', false);
    $('#rad-combustible').prop('checked', false);
}

var validarNoEstatico = (e) => {
    $(`#${$(e)[0].target.id}`).prop('checked') ? $('#rad-estatico').prop('checked', false) : ``;
}

var validarEstatico = () => {
    $(`#rad-estatico`).prop('checked') ? $('.no-estatico').prop('checked', false) : ``;
}

var validarUnico = (e) => {
    let verif = $(`#${$(e)[0].target.id}`).prop('checked');
    $('.unico').prop('checked', false);
    $(`#${$(e)[0].target.id}`).prop('checked', verif);
}

$(document).on("keydown", ".solo-numero", function (e) {
    var key = window.e ? e.which : e.keyCode;
    //var id = $("#" + e.target.id)[0].type;
    if ((key < 48 || key > 57) && (event.keyCode < 96 || event.keyCode > 105) && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46) return false;
});

//==================================================

var validarParametro = () => {
    let v = true;
    let param = $("#txt-etiqueta").val();
    if (param.trim() === "") { return; }
        let componentes = $("#filas-valor").find(".factor-div");
        componentes.each(function (index, value) {
            if ($(value).find(".nombre").val() == param) v = false;
        });
        v ? agregarParametro() : $("#txt-etiqueta").val("");
}

var agregarParametro = () => {    
    let valor = 0;
    let param = $("#txt-etiqueta").val();

    let icono = `<i class="fas fa-list"></i>`;
    let nombre = `<small class="badge badge-info mostrar-valor">${param}</small>`;
    let ctrlNombre = `<input class="hidden-control field-ctrol nombre" type="hidden" value="${param}">`;
    let ctrlValor = `<input class="hidden-control field-ctrol valor" data-delete="0" type="hidden" value="${valor}">`;
    let delet = `<i class="fas fa-minus-circle cursor-pointer m-2 delete-columna-detalle esconder" data-parent="2" data-toggle="tooltip"  onclick="eliminarParametro(${valor});"  data-placement="top" title="" data-original-title="Eliminar"></i>`;
    let edit = `<i class="fas fa-edit cursor-pointer m-2 esconder" data-toggle="tooltip" data-placement="top" onclick="editarParametro(${valor},this);" title="" data-original-title="Editar"></i>`;
    let opciones = `<div class="opciones">${delet}${edit}</div>`;
    let contentDellate = `<div class="btn btn-secondary btn-sw w-100 d-flex flex-row align-items-center justify-content-between my-2 factor-div">${icono}${nombre}${ctrlNombre}${ctrlValor}${opciones}</div>`;
    $("#filas-valor").append(contentDellate);
    $("#txt-etiqueta").val("");
}

var eliminarParametro = (id) => {
    if (id > 0) {
        let del = $("#parametros-id").data("eliminar") + id + ",";
        $("#parametros-id").data("eliminar", del);
    }
}

var editarParametro = (id, etiqueta, param) => {
    $("#add-lista").addClass("d-none");
    $("#edit-lista").removeClass("d-none");

    let e = $(etiqueta).parent().parent().find(".nombre").val();
    $("#txt-etiqueta").val(e);
    $("#edit-lista").data("evaluar", id);
    $("#edit-lista").data("detalle", e);
    $(".esconder").prop("hidden", true);
}

var actualizarParametro = () => {
    $(".esconder").prop("hidden", false);
    $("#filas-valor").find(".factor-div").each((x,y) => {
        if ($(y).find(".nombre").val() == $("#edit-lista").data("detalle")) {
            $(y).find(".nombre").val($("#txt-etiqueta").val());
            $(y).find(".mostrar-valor").html("").html($("#txt-etiqueta").val());
        }
    });
    $("#add-lista").removeClass("d-none");
    $("#edit-lista").addClass("d-none");
    $("#txt-etiqueta").val("");
}

var idFiltro = () => {
    let filtro = "";
    $("[id^=param-]").each((x, y) => {
        filtro += $(y).attr('id').split('-')[1] + '|';
    });
    return filtro.substring(0, filtro.length - 1);
}

var agregarParametroFiltro = () => {
    if ($('#cbo-parametro').val() == 0) return;
    let verif = true;
    $("[id^=param-]").each((x, y) => {
        if ($(y).attr('id').split('-')[1] == $('#cbo-parametro').val()) { verif = false;}
    });
    if (!verif) { $('#cbo-parametro').val(0); return; }
    let small = `<small><i class="fas fa-list"></i>${$("#cbo-parametro option:selected").html()}</small>`;
    let input = `<input type="hidden" class="hidden-control field-ctrol" value="cbo">`;
    let icono = `<i class="fas fa-minus-circle cursor-pointer m-2 delete-columna-detalle" data-toggle="tooltip" data-placement="top" title="" data-original-title="Eliminar parámetro"></i>`;
    let content = `<div class="btn btn-secondary btn-sm w-100 d-flex flex-row align-items-center justify-content-between my-2" id="param-${$('#cbo-parametro').val()}">${small}${input}${icono}</div>`;
    $('#filas-parametro').append(content);
    $('#cbo-parametro').val(0);
}

var consultarListas = () => {
    let url = `${baseUrl}api/parametro/obtenerallparametrolista?idControl=1`;
    fetch(url).then(r => r.json()).then(j => {
        let contenido = ``;
        if (j.length > 0) {
            contenido = j.map((x, y) => {
                return `<option value="${x.ID_PARAMETRO}">[P${x.ID_PARAMETRO}] ${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-parametro').html(`<option value="0">-Seleccione-</option>${contenido}`)
    });
}


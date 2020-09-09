$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    //$('#btnNuevo').on('click', (e) => nuevo());
    //$('#btnConfirmar').on('click', (e) => eliminar());
    $('#btnGuardar').on('click', (e) => guardar());
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
    let busqueda = $('#txt-descripcion').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();
    let columna = $("#columna").val();
    let orden = $("#orden").val();
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/institucion/buscarinstitucion?${queryParams}`;

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
            let colNro = `<td class="text-center" data-encabezado="Número de orden" scope="row" data-count="0">${(pagina - 1) * registros + (i + 1)}</td>`;
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_INSTITUCION}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colRazonsocial = `<td class="text-left" data-encabezado="Razón social">${x.RAZON_SOCIAL}</td>`;
            let colRuc = `<td class="text-left" data-encabezado="Razón social">${x.RUC}</td>`;
            let colDomicilio = `<td class="text-left" data-encabezado="Razón social">${x.DOMICILIO_LEGAL}</td>`;
            let colSector = `<td class="text-left" data-encabezado="Razón social">${x.NOMBRE_SECTOR}</td>`;
            //let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_SECTOR}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_INSTITUCION}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colCodigo}${colRazonsocial}${colRuc}${colDomicilio}${colSector}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txt-nombre').val('');
    $('#txt-ruc').val('');
    $('#txt-domicilio').val('');
    $('#cbo-sector').val(0);
}

var consultarDatos = (element) => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');
    $('#cbo-sector').prop('disabled', true);
    $('#exampleModalLabel').html('ACTUALIZAR INSTITUCIÓN');
    let id = $(element).attr('data-id');

    let url = `${baseUrl}api/institucion/obtenerinstitucion?idInstitucion=${id}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    $('#frm').data('id', data.ID_INSTITUCION);
    $('#txt-nombre').val(data.RAZON_SOCIAL);
    $('#txt-ruc').val(data.RUC);
    $('#txt-domicilio').val(data.DOMICILIO_LEGAL);
    $('#cbo-sector').val(data.ID_SECTOR);
}

var guardar = () => {
    $('.alert-add').html('');
    let arr = []; let r = "";
    if ($('#txt-nombre').val().trim() === "") arr.push("Ingrese la razón social");
    if ($('#txt-ruc').val().trim() === "") arr.push("Ingrese el ruc");
    if ($("#txt-ruc").val().length < 11) arr.push("El ruc debe contener 11 caracteres");
    if ($("#txt-ruc").val().trim().length > 2) {r = $("#txt-ruc").val().substring(0, 2);
        if (r != '20' && r != '10') arr.push("El ruc debe iniciar con el número 20 o 10");}  
    if ($('#txt-domicilio').val().trim() === "") arr.push("Ingrese el domicilio legal");
    if ($('#cbo-sector').val() == 0) arr.push("Seleccione el sector");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let id = $('#frm').data('id');
    let razonsocial = $('#txt-nombre').val();
    let ruc = $('#txt-ruc').val();
    let domicilio = $('#txt-domicilio').val();
    let sector = $('#cbo-sector').val();
    let url = `${baseUrl}api/institucion/actualizarinstitucion`;
    let data = { ID_INSTITUCION: id, RAZON_SOCIAL: razonsocial, RUC: ruc, DOMICILIO_LEGAL: domicilio, ID_SECTOR: sector, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-add').html('');
        if (j.OK){
            $('#btnGuardar').hide(); $('#btnGuardar').next().html('Cerrar'); 
            $('.alert-add').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los datos fueron guardados correctamente.', close: { time: 1000 }, url: `` });
            $('#btnConsultar')[0].click();
        } else
            $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: j.VAL == 1 ? 'El ruc pertenece a otra institución.' : 'Inténtelo nuevamente por favor.' });
    });
}

var cargarInformacionInicial = () => {
    let urlListarSectorPorEstado = `${baseUrl}api/sector/listarsectorporestado?flagEstado=1`;
    Promise.all([
        fetch(urlListarSectorPorEstado)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([listaSector]) => {
        cargarComboSector('#cbo-sector', listaSector);
    });
}

var cargarComboSector = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_SECTOR}">${x.NOMBRE}</option>`).join('');
    options = `<option value="0">-Seleccione un sector-</option>${options}`;
    $(selector).html(options);
}

$(document).on("keydown", ".solo-numero", function (e) {
    var key = window.e ? e.which : e.keyCode;
    //console.log(key);
    if ((key < 48 || key > 57) && (event.keyCode < 96 || event.keyCode > 105) && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46) return false;
});
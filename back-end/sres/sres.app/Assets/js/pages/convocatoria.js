$(document).ready((e) => {
    $('#fle-logo').on('change', logoChange);
    $('#btnGuardarLogo').on('click', btnGuardarLogoClick);
    $('#btnMostrarDatosInstitucion').on('click', btnMostrarDatosInstitucionClick);
    $('#btnActualizarDatosInstitucion').on('click', btnActualizarDatosInstitucionClick);
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnFirstPagination').on('click', btnFirstPaginationClick);
    $('#btnPreviousPagination').on('click', btnPreviousPaginationClick);
    $('#btnNextPagination').on('click', btnNextPaginationClick);
    $('#btnLastPagination').on('click', btnLastPaginationClick);
});

var consultar = () => {
    let nroInforme = $('#txt-expediente').val();
    let nombre = $('#txt-descripcion').val();
    let fechaDesde = $('#dat-desde').val();
    let fechaHasta = $('#dat-hasta').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();;
    let columna = 'id_convocatoria';
    let orden = 'asc'
    let params = { nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/convocatoria/buscarconvocatoria?${queryParams}`;

    fetch(url).then(r => r.json()).then(cargarDataBusqueda);
};

var cargarDataBusqueda = (data) => {
    let tabla = $('#tblConvocatoria');
    let cantidadCeldasCabecera = tabla.find('thead tr th').length;
    //debugger;
    $('#viewPagination').attr('style', 'display: none !important');
    console.log(data.TOTAL_REGISTROS, data.CANTIDAD_REGISTROS);
    if (data.TOTAL_REGISTROS > data.CANTIDAD_REGISTROS) $('#viewPagination').show();
    if (data.CANTIDAD_REGISTROS == 0) $('#view-page-result').hide();
    else $('#view-page-result').show();
    $('.inicio-registros').text(data.CANTIDAD_REGISTROS == 0 ? 'No se encontraron resultados' : (data.PAGINA - 1) * data.CANTIDAD_REGISTROS + 1);
    $('.fin-registros').text(data.TOTAL_REGISTROS < data.PAGINA * data.CANTIDAD_REGISTROS ? data.TOTAL_REGISTROS : data.PAGINA * data.CANTIDAD_REGISTROS);
    $('.total-registros').text(data.TOTAL_REGISTROS);
    $('.pagina').text(data.PAGINA);
    $('#ir-pagina').val(data.PAGINA);
    $('#ir-pagina').attr('max', data.TOTAL_PAGINAS);
    $('.total-paginas').text(data.TOTAL_PAGINAS);

    let contenido = renderizar(data, cantidadCeldasCabecera);
    tabla.find('tbody').html(contenido);
    tabla.find('.btnParticipar').each(x => {
        let elementButton = tabla.find('.btnParticipar')[x];
        $(elementButton).on('click', btnParticiparClick);
    });
    $('html, body').animate({ scrollTop: $('#sectionSearch').offset().top }, 'slow');
}

var renderizar = (data, cantidadCeldas) => {    
    let deboRenderizar = data.CANTIDAD_REGISTROS > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.DATA.map((x, i) => {
            let fechaActual = new Date();
            let fechaInicio = new Date(x.FECHA_INICIO);
            let fechaFin = new Date(x.FECHA_FIN);
            let diasPlazo = Math.floor((fechaFin - fechaInicio) / (1000 * 60 * 60 * 24));
            let diasTranscurridos = Math.floor((fechaActual - fechaInicio) / (1000 * 60 * 60 * 24));
            let porcentajeAvance = Math.floor(fechaInicio > fechaActual ? 0.00 : fechaActual > fechaFin ? 100 : (diasTranscurridos / diasPlazo * 100))
            let formatoCodigo = '00000000';

            let colNro = `<td class="text-center text-sm-left" data-encabezado="Número" scope="row">${x.ROWNUMBER}</td>`
            let colNroInforme = `<td class="text-center text-sm-left" data-encabezado="Número expediente" scope="row">${(`${formatoCodigo}${x.ID_CONVOCATORIA}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</td>`;
            let colPeriodo = `<td class="text-center" data-encabezado="Período">${fechaInicio.getFullYear()}</td>`;
            let colNombre = `<td data-encabezado="Progreso"><div class="text-limi-1">${x.NOMBRE}</div></td>`;
            let colFechaInicio = `<td class="text-center" data-encabezado="Fecha Inicio">${fechaInicio.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colFechaFin = `<td class="text-center" data-encabezado="Fecha Fin">${fechaFin.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colVencimiento = `<td class="text-center" data-encabezado="Vencimiento"><div class="progress" style="height: 21px;" data-toggle="tooltip" data-placement="top" title="Porcentaje de avance"><div class="progress-bar ${porcentajeAvance > 0 ? "vigente" : "preparado"} estilo-01" role="progressbar" style="width: ${porcentajeAvance}%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">${porcentajeAvance}%</div></div></td>`;
            let colEstado = `<td data-encabezado="Estado"><b class="text-sres-verde">${x.ETAPA.NOMBRE}</b></td>`;
            let btnDetalles = `<a class="btn btn-sm btn-success w-100" href="javascript:void(0)">Detalles</a>`;
            let btnIngresar = `<a class="btn btn-sm btn-success w-100" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme">Ingresar</a>`;
            let btnGestionar = `<a class="btn btn-sm bg-success text-white dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestión</a>`;
            let btnEditarRequisitos = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme"><i class="fas fa-edit mr-1"></i>Editar requisitos</a>`
            let btnEditarCriterios = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Criterios"><i class="fas fa-edit mr-1"></i>${x.ID_ETAPA == 3 ? `Ingresar` : `Editar`} criterios</a>`
            let btnSeguimiento = `<a class="dropdown-item estilo-01" href="#"><i class="fas fa-history mr-1"></i>Seguimiento</a>`;
            let btnVerReconocimiento = `<a class="dropdown-item estilo-01" href="#"><i class="fas fa-medal mr-1"></i>Ver reconocimiento</a>`;
            let btnEvaluarCriterios = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/EvaluacionCriterios"><i class="fas fa-edit mr-1"></i>Evaluar criterios</a>`;

            let OpcionesEtapa3 = `<div class="dropdown-menu">${btnEditarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcionesEvaluacion = `<div class="dropdown-menu">${btnEvaluarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcionesEtapa7 = `<div class="dropdown-menu">${btnEditarRequisitos}${btnEditarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión">${x.ID_ETAPA == 1 ? btnDetalles : ''}${x.ID_ETAPA == 2 ? btnIngresar : ''}${x.ID_ETAPA == 3 ? `${btnGestionar}${OpcionesEtapa3}` : ''}${x.ID_ETAPA == 5 ? `${btnGestionar}${OpcionesEvaluacion}` : ``}${x.ID_ETAPA == 7 ? `${btnGestionar}${OpcionesEtapa7}` : ''}</td>`;
            //let colOpciones = `<td class="text-center" data-encabezado="Gestión">${x.ID_ETAPA == 1 ? btnDetalles : ''}${x.ID_ETAPA == 2 ? btnIngresar : ''}${x.ID_ETAPA == 3 ? btnGestionar : ''}<div class="dropdown-menu">${btnEditarRequisitos}${btnEditarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div></div></td>`;
            //let colOpciones = `<td class="text-center" data-encabezado="Gestión">${x.FLAG_ESTADO == '1' ? btnIngresar : btnGestionar}<div class="dropdown-menu">${btnEditarRequisitos}${btnEditarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div></div></td>`;
            let fila = `<tr>${colNro}${colNroInforme}${colPeriodo}${colNombre}${colFechaInicio}${colFechaFin}${colVencimiento}${colEstado}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var btnFirstPaginationClick = (e) => {
    let valor = $('#ir-pagina').attr('min');
    $('#ir-pagina').val(valor);
    consultar();
}

var btnPreviousPaginationClick = (e) => {
    let valor = Number($('#ir-pagina').val());
    $('#ir-pagina').val(valor - 1);
    consultar();
}

var btnNextPaginationClick = (e) => {
    let valor = Number($('#ir-pagina').val());
    $('#ir-pagina').val(valor + 1);
    consultar();
}

var btnLastPaginationClick = (e) => {
    let valor = $('#ir-pagina').attr('max');
    $('#ir-pagina').val(valor);
    consultar();
}

var btnGuardarLogoClick = (e) => {
    e.preventDefault();
    if ($('#fle-logo').data('fileContent') == null) {
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `Debe seleccionar una imagen` });
        return;
    }

    let data = { ID_INSTITUCION: idInstitucionLogin, LOGO_CONTENIDO: $('#fle-logo').data('file'), LOGO: $('#fle-logo').data('fileName'), LOGO_TIPO: $('#fle-logo').data('type'), UPD_USUARIO: idUsuarioLogin };

    let url = `/api/institucion/modificarlogoinstitucion`;
    let init = { method: 'put', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) }

    fetch(url, init)
    .then(r => r.json())
    .then(responseGuardarLogo)
}

var responseGuardarLogo = (data) => {
    if (data == true) {
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'success', title: '¡BIEN HECHO!', message: `Se actualizó el logo correctamente`, close: { time: 3000 } });
        $('#btnGuardarLogo').hide();
        
        let url = `${baseUrl}Login/RefrescarDatosSession`;
        fetch(url).then(r => r.json()).then(j => console.log(j));

    } else {
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `No se pudo actualizar el logo` });
        let srcDefault = $('#fle-logo').parent().parent().find('.img-fluid').attr('data-src-default');
        $('#fle-logo').parent().parent().find('.img-fluid').attr('src', srcDefault);
    }
}

var logoChange = (e) => {
    let elFile = $(e.currentTarget);

    if (e.currentTarget.files.length == 0) {
        let srcDefault = $(e.currentTarget).parent().parent().find('.img-fluid').attr('data-src-default');
        $(e.currentTarget).parent().parent().find('.img-fluid').attr('src', srcDefault);
        $('#btnGuardarLogo').hide();
        $(e.currentTarget).removeData('file');
        $(e.currentTarget).removeData('fileName');
        $(e.currentTarget).removeData('fileContent');
        $(e.currentTarget).removeData('type');
        return;
    }

    var fileContent = e.currentTarget.files[0];

    if (fileContent.size > maxBytes) $(elFile).parent().parent().parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `El archivo debe tener un peso máximo de 4MB` });
    else $(elFile).parent().parent().parent().parent().parent().parent().alert('remove');


    var idElement = $(e.currentTarget).attr("data-id");

    let reader = new FileReader();
    reader.onload = function (e) {
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
        elFile.data('fileContent', e.currentTarget.result);
        elFile.data('fileName', fileContent.name);
        elFile.data('type', fileContent.type);
    }
    reader.readAsDataURL(e.currentTarget.files[0]);
}

var btnMostrarDatosInstitucionClick = (e) => {
    e.preventDefault();

    let url = `/api/institucion/obtenerinstitucion?idInstitucion=${idInstitucionLogin}`;

    fetch(url)
    .then(r => r.json())
    .then(responseMostrarDatosInstitucion);
}

var responseMostrarDatosInstitucion = (data) => {
    $('#txt-nombre-corto').val(data.NOMBRE_COMERCIAL);
    $('#txa-descripcion').val(data.DESCRIPCION);
    $('#modal-edit-descripcion').modal('show');
}

var btnActualizarDatosInstitucionClick = (e) => {
    e.preventDefault();

    let nombreComercial = $('#txt-nombre-corto').val();
    let descripcion = $('#txa-descripcion').val();

    let data = { ID_INSTITUCION: idInstitucionLogin, NOMBRE_COMERCIAL: nombreComercial, DESCRIPCION: descripcion, UPD_USUARIO: idUsuarioLogin };

    let url = `/api/institucion/modificardatosinstitucion`;
    let init = { method: 'put', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(responseActualizarDatosInstitucion);
}

var responseActualizarDatosInstitucion = (data) => {
    if (data == true) {
        $('#lblDescripcionInstitucion').text($('#txa-descripcion').val());
        $('#txt-nombre-corto').val('');
        $('#txa-descripcion').val('');
        $('#modal-edit-descripcion').modal('hide');
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'success', title: '¡BIEN HECHO!', message: `Se actualizó los datos de la institución correctamente`, close: { time: 3000 } });
        let url = `${baseUrl}Login/RefrescarDatosSession`;
        fetch(url).then(r => r.json()).then(j => console.log(j));
    } else {
        $('#modal-edit-descripcion .modal-body >*:last').alert({ type: 'danger', title: 'ERROR', message: `No se pudo actualizar los datos de la institución` });
    }
}
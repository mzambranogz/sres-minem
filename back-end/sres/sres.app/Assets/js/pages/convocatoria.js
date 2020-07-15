$(document).ready((e) => {
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
    debugger;
    $('#viewPagination').attr('style', 'display: none !important');
    console.log(data.TOTAL_REGISTROS, data.CANTIDAD_REGISTROS);
    if (data.TOTAL_REGISTROS > data.CANTIDAD_REGISTROS) $('#viewPagination').show();
    $('.inicio-registros').text((data.PAGINA - 1) * data.CANTIDAD_REGISTROS + 1);
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
            let porcentajeAvance = (fechaInicio > fechaActual ? 0.00 : fechaActual > fechaFin ? 100 : (diasTranscurridos / diasPlazo * 100)).toFixed(2);

            let colNroInforme = `<td class="text-center text-sm-left" data-encabezado="Número expediente" scope="row">${x.NRO_INFORME == null ? '' : `<a href="#"><i class="fas fa-eye mr-1"></i><span>${x.NRO_INFORME}</span></a>`}</td>`;
            let colPeriodo = `<td class="text-center" data-encabezado="Período">${fechaInicio.getFullYear() == fechaFin.getFullYear() ? fechaInicio.getFullYear() : fechaInicio.getFullYear() + "-" + fechaFin.getFullYear()}</td>`;
            let colNombre = `<td data-encabezado="Progreso"><div class="text-limi-1">${x.NOMBRE}</div></td>`;
            let colFechaInicio = `<td class="text-center" data-encabezado="Fecha Inicio">${fechaInicio.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colFechaFin = `<td class="text-center" data-encabezado="Fecha Fin">${fechaFin.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colVencimiento = `<td class="text-center" data-encabezado="Vencimiento"><div class="progress" style="height: 21px;" data-toggle="tooltip" data-placement="top" title="Porcentaje de avance"><div class="progress-bar ${porcentajeAvance > 0 ? "vigente" : "preparado"} estilo-01" role="progressbar" style="width: ${porcentajeAvance}%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">${porcentajeAvance}%</div></div></td>`;
            let colEstado = `<td data-encabezado="Estado"><b class="text-sres-verde">${obtenerEstadoConvocatoria(x.FLAG_ESTADO)}</b></td>`;
            let btnGestionar = `<a class="btn btn-sm bg-success text-white dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestión</a>`;
            let btnIngresar = `<a class="dropdown-item" target="_blank" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme"><i class="fas fa-edit mr-1"></i>Ingresar</a>`;
            let btnSeguimiento = `<a class="dropdown-item" href="#"><i class="fas fa-history mr-1"></i>Seguimiento</a>`;
            let btnVerReconocimiento = `<a class="dropdown-item" href="#"><i class="fas fa-medal mr-1"></i>Ver reconocimiento</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión">${btnGestionar}<div class="dropdown-menu">${btnIngresar}${btnSeguimiento}${btnVerReconocimiento}</div></div></td>`;
            let fila = `<tr>${colNroInforme}${colPeriodo}${colNombre}${colFechaInicio}${colFechaFin}${colVencimiento}${colEstado}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var obtenerEstadoConvocatoria = (flagEstado) => {
    let estado = '';

    switch(flagEstado){
        case 'A' : estado = 'ABIERTO'; break;
        case 'E' : estado = 'EN PROCESO'; break;
        case 'R' : estado = 'EN REVISIÓN'; break;
        case 'P' : estado = 'PENDIENTE'; break;
        case 'C' : estado = 'CERRADO'; break;
        case 'V' : estado = 'VENCIDO'; break;
    }

    return estado;
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
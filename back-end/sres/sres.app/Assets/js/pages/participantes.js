$(document).ready((e) => {
    $('#txt-buscar').on('blur', (e) => consultar());
    $('#txt-buscar')[0].blur();
    $('.btnFirstPagination').on('click', btnFirstPaginationClick);
    $('.btnPreviousPagination').on('click', btnPreviousPaginationClick);
    $('.btnNextPagination').on('click', btnNextPaginationClick);
    $('.btnLastPagination').on('click', btnLastPaginationClick);
})

var consultar = () => {
    let busqueda = $('#txt-buscar').val();
    let registros = 10;
    //let registros = $('#catidad-rgistros').val();
    let pagina = $($('.ir-pagina')[0]).val();
    let columna = 'id_reconocimiento';
    let orden = 'asc'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/institucion/buscarparticipantes?${queryParams}`;

    fetch(url).then(r => r.json()).then(cargarDataBusqueda);
};

var cargarDataBusqueda = (data) => {
    let tabla = $('#tblParticipantes');
    let cantidadCeldasCabecera = tabla.find('thead tr th').length;
    //debugger;
    $('.viewPagination').attr('style', 'display: none !important');
    console.log(data.TOTAL_REGISTROS, data.CANTIDAD_REGISTROS);
    if (data.TOTAL_REGISTROS > data.CANTIDAD_REGISTROS) $('.viewPagination').show();
    $('.inicio-registros').text(data.CANTIDAD_REGISTROS == 0 ? 'No se encontraron resultados' : (data.PAGINA - 1) * data.CANTIDAD_REGISTROS + 1);
    $('.fin-registros').text(data.TOTAL_REGISTROS < data.PAGINA * data.CANTIDAD_REGISTROS ? data.TOTAL_REGISTROS : data.PAGINA * data.CANTIDAD_REGISTROS);
    $('.total-registros').text(data.TOTAL_REGISTROS);
    $('.pagina').text(data.PAGINA);
    $('.ir-pagina').val(data.PAGINA);
    $('.ir-pagina').attr('max', data.TOTAL_PAGINAS);
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
            //let fechaActual = new Date();
            //let fechaInicio = new Date(x.FECHA_INICIO);
            //let fechaFin = new Date(x.FECHA_FIN);
            //let diasPlazo = Math.floor((fechaFin - fechaInicio) / (1000 * 60 * 60 * 24));
            //let diasTranscurridos = Math.floor((fechaActual - fechaInicio) / (1000 * 60 * 60 * 24));
            //let porcentajeAvance = (fechaInicio > fechaActual ? 0.00 : fechaActual > fechaFin ? 100 : (diasTranscurridos / diasPlazo * 100)).toFixed(2);

            let colLogo = `<td class="text-center text-sm-left" data-encabezado="Logo" scope="row" data-count="0"><img class="img-fluid" src="${x.LOGO == null ? '' : `${baseUrl}${x.LOGO}`}" alt=""></td>`;
            let colReconocimiento = `<td class="text-center" data-encabezado="Reconocimiento"><img class="img-fluid medal-sres" src="./images/dos_estrellas.png" alt="" data-toggle="tooltip" data-placement="top" title="Reconocimiento de oro con 3 estrellas"></td>`;
            let colRazonSocial = `<td data-encabezado="Empresa participante"><div class="text-limi-1">${x.RAZON_SOCIAL}</div></td>`;
            let colMedida = `<td class="text-center" data-encabezado="Medida NDC"><b class="text-sres-azul" data-toggle="tooltip" data-placement="top" title="Nombre de la medida de mitigación">MNO</b></td>`;
            let btnVerFicha = `<a class="btn btn-sm btn-success w-100" href="./ficha.html">Ver</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Ficha">${btnVerFicha}</td>`;
            let fila = `<tr>${colLogo}${colReconocimiento}${colRazonSocial}${colMedida}${colOpciones}</tr>`;
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
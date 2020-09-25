$(document).ready((e) => {
    cargarCombos();
    $('#txt-buscar').on('blur', (e) => consultar());
    $('#txt-buscar')[0].blur();
    $('.btnFirstPagination').on('click', btnFirstPaginationClick);
    $('.btnPreviousPagination').on('click', btnPreviousPaginationClick);
    $('.btnNextPagination').on('click', btnNextPaginationClick);
    $('.btnLastPagination').on('click', btnLastPaginationClick);
    cambioNav();

    $("#btn-buscar").click(consultar);
    $('#cbo-categoria, #cbo-criterio, #cbo-medmit, #cbo-periodo, #cbo-insignia, #cbo-estrellas, #cbo-sector, #cbo-reconocimiento-especial').change(consultar);
    $("#btn-buscar")[0].click();
})

var cargarCombos = () => {
    //let urlListarComboTipoEmpresa = `${baseUrl}api/subsectortipoempresa/listasubsetortipoempresa?idSector=${idSectorLogin}`;
    //let urlListarComboCriterio = `${baseUrl}api/criterio/obtenerallcriterio`;
    //let urlListarComboMedMit = `${baseUrl}api/medidamitigacion/obtenerallmedidamitigacion`;
    //let urlListarComboPeriodo = `${baseUrl}api/anno/obtenerallanno`;
    let urlListarComboInsignia = `${baseUrl}api/insignia/obtenerallinsignia`;
    let urlListarComboEstrella = `${baseUrl}api/estrella/obtenerallestrella`;
    let urlListarSectorPorEstado = `${baseUrl}api/sector/listarsectorporestado?flagEstado=1`;

    Promise.all([
            //fetch(urlListarComboTipoEmpresa),
            //fetch(urlListarComboCriterio),
            //fetch(urlListarComboMedMit),
            //fetch(urlListarComboPeriodo),
            fetch(urlListarComboInsignia),
            fetch(urlListarComboEstrella),
            fetch(urlListarSectorPorEstado)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    //.then(([listaTipoEmpresa, listaCriterio, listaMedMit, listaPeriodo, listaInsignia, listaEstrella]) => {
    .then(([listaInsignia, listaEstrella, listaSector]) => {
        //cargarComboTipoEmpresa('#cbo-categoria', listaTipoEmpresa);
        //cargarComboCriterio('#cbo-criterio', listaCriterio);
        //cargarComboMedidaMitigacion('#cbo-medmit', listaMedMit);
        //cargarComboPeriodo('#cbo-periodo', listaPeriodo);
        cargarComboInsignia('#cbo-insignia', listaInsignia);
        cargarComboEstrella('#cbo-estrellas', listaEstrella);
        cargarComboSector('#cbo-sector', listaSector);
    });
};

var cargarComboSector = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_SECTOR}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var cargarComboTipoEmpresa = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_SUBSECTOR_TIPOEMPRESA}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var cargarComboCriterio = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_CRITERIO}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var cargarComboMedidaMitigacion = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_MEDMIT}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var cargarComboPeriodo = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.NOMBRE}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var cargarComboInsignia = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_INSIGNIA}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var cargarComboEstrella = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ESTRELLA}">${x.NOMBRE}</option>`).join('');
    options = `<option>Todos</option>${options}`;
    $(selector).html(options);
}

var consultar = () => {
    let razonSocialInstitucion = $('#txt-buscar').val();
    let idTipoEmpresa = $('#cbo-categoria').val();
    let idCriterio = $('#cbo-criterio').val();
    let idMedMit = $('#cbo-medmit').val();
    let añoInicioConvocatoria = $('#cbo-periodo').val();
    let idInsignia = $('#cbo-insignia').val();
    let idEstrella = $('#cbo-estrellas').val();
    let idSector = $('#cbo-sector').val();
    let idEspecial = $('#cbo-reconocimiento-especial').val();
    let registros = 10;
    //let registros = $('#catidad-rgistros').val();
    let pagina = $($('.ir-pagina')[0]).val();
    let columna = 'id_reconocimiento';
    let orden = 'asc'
    let params = { razonSocialInstitucion, idTipoEmpresa, idCriterio, idMedMit, añoInicioConvocatoria, idInsignia, idEstrella, idSector, idEspecial, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/reconocimiento/buscarparticipantes?${queryParams}`;

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
    $('html, body').animate({ scrollTop: $('#tblParticipantes').offset().top }, 'slow');
    $("[data-toggle='tooltip']").tooltip();
}

var renderizar = (data, cantidadCeldas) => {
    let deboRenderizar = data.CANTIDAD_REGISTROS > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.DATA.map((x, i) => {
            debugger;
            //let colLogo = `<td class="text-center text-sm-left" data-encabezado="Logo" scope="row" data-count="0"><img class="img-fluid" src="${x.INSCRIPCION.INSTITUCION.LOGO == null ? `${baseUrl}Assets/images/sin-foto.png` : `${baseUrl}${x.INSCRIPCION.INSTITUCION.LOGO}`}" alt=""></td>`;
            let colLogo = `<td class="text-center text-sm-left" data-encabezado="Logo" scope="row" data-count="0"><img class="img-fluid" src="${x.INSCRIPCION.INSTITUCION.LOGO == null ? `${baseUrl}Assets/images/sin-foto.png` : `${baseUrl}${$('#ruta').val().replace('{0}', x.INSCRIPCION.INSTITUCION.ID_INSTITUCION)}/${x.INSCRIPCION.INSTITUCION.LOGO}`}" alt=""></td>`;
            //let colSello = `<td class="text-center" data-encabezado="Reconocimiento"><img class="img-fluid medal-sres" src="${(x.INSIGNIA == null ? `sres_0.png` : `${baseUrl}Assets/images/${x.INSIGNIA.ARCHIVO_BASE}`)}" alt="" data-toggle="tooltip" data-placement="top" title="Reconocimiento de oro con ${x.ESTRELLA}"></td>`;
            let colSello = `<td class="text-center" data-encabezado="Reconocimiento"><img class="img-fluid medal-sres" src="${baseUrl}${$('#ruta_rec').val().replace('{0}', x.ID_PREMIACION)}/${x.ARCHIVO_BASE}" alt="" data-toggle="tooltip" data-placement="top" title="Reconocimiento oro ${x.ESTRELLA == null ? 'sin estrellas' : x.ESTRELLA == 0 ? 'sin estrellas' : `con ${x.ESTRELLA}`}">&nbsp;${x.FLAG_MEJORACONTINUA == '1' || x.FLAG_EMISIONESMAX == '1' || PREMIO_MEDMIT > 0 ? `<img class="img-fluid medal-sres" src="${baseUrl}Assets/images/gei.png" alt="" data-toggle="tooltip" data-placement="top" title="Premiación especial">` : ``}</td>`;
            let colRazonSocial = `<td data-encabezado="Empresa participante"><div class="text-limi-1">${x.INSCRIPCION.INSTITUCION.RAZON_SOCIAL}</div></td>`;
            let colPuntaje = `<td data-encabezado="Puntaje"><div class="text-center">${x.PUNTAJE}</div></td>`;
            let colEnergia = `<td data-encabezado="Ahorro energético de electricidad"><div class="text-right">${formatoMiles(x.ENERGIA)}</div></td>`;
            let colCombustible = `<td data-encabezado="Ahorro de combustible"><div class="text-right">${formatoMiles(x.COMBUSTIBLE)}</div></td>`;
            let colEmisiones = `<td data-encabezado="Reducción de emisiones"><div class="text-right">${formatoMiles(x.EMISIONES)}</div></td>`;            
            let colEstrella = `<td class="text-center" data-encabezado="Medida NDC"><i class="fas fa-medal fa-2x"></i></td>`;
            let btnVerFicha = `<a class="btn btn-sm btn-success w-100" href="${baseUrl}Participantes/${x.INSCRIPCION.INSTITUCION.ID_INSTITUCION}/Reconocimiento">Ver</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Ficha">${btnVerFicha}</td>`;
            //let fila = `<tr>${colLogo}${colSello}${colRazonSocial}${colPuntaje}${colEmisiones}${colEstrella}${colOpciones}</tr>`;
            let fila = `<tr>${colLogo}${colSello}${colRazonSocial}${colPuntaje}${colEnergia}${colCombustible}${colEmisiones}${colOpciones}</tr>`;
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

var cambioNav = () => {
    $('.barra').removeClass('activo');
    $('.barra a').removeClass('nav-active');
    $('.barra-participante').addClass('activo');
    $('.barra-participante a').addClass('nav-active');
}

function formatoMiles(n) { //add20
    return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}
$(document).ready((e) => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnFirstPagination').on('click', btnFirstPaginationClick);
    $('#btnPreviousPagination').on('click', btnPreviousPaginationClick);
    $('#btnNextPagination').on('click', btnNextPaginationClick);
    $('#btnLastPagination').on('click', btnLastPaginationClick);
});

var consultar = () => {
    let idInscripcion = $('#txt-codigo').val();
    idInscripcion = isNaN(idInscripcion) == true ? -1 : (idInscripcion.trim() == "" ? null : parseInt(idInscripcion));
    let razonSocialInstitucion = $('#txt-empresa').val();
    let nombresCompletosUsuario = $('#txt-responsable').val();
    let idUsuario = idUsuarioLogin;
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();;
    let columna = 'id_inscripcion';
    let orden = 'asc'
    let params = { idConvocatoria, idInscripcion, razonSocialInstitucion, nombresCompletosUsuario, idUsuario, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/inscripcion/buscarinscripcion?${queryParams}`;

    fetch(url).then(r => r.json()).then(cargarDataBusqueda);
};

var cargarDataBusqueda = (data) => {
    let tabla = $('#tblBandejaParticipantes');
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
            let formatoCodigo = '00000000';

            let colNro = `<td class="text-center" data-encabezado="Número" scope="row" data-count="0">${x.ROWNUMBER}</td>`
            let colCodigo = `<td class="text-center" data-encabezado="Número expediente" scope="row">${(`${formatoCodigo}${x.ID_INSCRIPCION}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</td>`;
            let colEmpresaParticipante = `<td data-encabezado="Empresa participante"><div class="text-limit-1">${x.INSTITUCION.RAZON_SOCIAL}</div><span class="text-sres-gris estilo-06">RUC: ${x.INSTITUCION.RUC}</span></td>`;
            let colResponsable = `<td data-encabezado="Responsable"><div class="text-limit-1">${x.USUARIO.NOMBRES} ${x.USUARIO.APELLIDOS}</div><span class="text-sres-gris estilo-06">${x.USUARIO.CORREO}</span></td>`;
            let colCriterios = `<td class="text-center" data-encabezado="Criterios">${x.CANTIDADCRITERIOSINGRESADOS} ${(x.CANTIDADCRITERIOSINGRESADOS == 1 ? "criterio" : "criterios")}</td>`;
            let colPuntuacion = `<td class="text-center" data-encabezado="Puntuación">${x.PUNTOSACUMULADOS} ${(x.PUNTOSACUMULADOS == 1 ? "punto" : "puntos")}</td>`;
            let colAspiraciones = x.ASPIRACIONES == null ? "<td></td>" : `<td class="text-center" data-encabezado="Fecha Fin">${x.ASPIRACIONES.map(y => `<img class="img-fluid medal-sres" src="./images/${y.NOMBRE}.png" alt="" data-toggle="tooltip" data-placement="top" title="Reconocimiento de oro con 3 estrellas">`)}</td>`;
            let btnDetalles = `<a class="btn btn-sm btn-success w-100" href="javascript:void(0)">Detalles</a>`;
            let btnIngresar = `<a class="btn btn-sm btn-success w-100" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme">Ingresar</a>`;
            let btnGestionar = `<a class="btn btn-sm bg-success text-white dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestión</a>`;
            let btnEvaluarRequisitos = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${idConvocatoria}/Inscripcion/${x.ID_INSCRIPCION}/Evaluar"><i class="fas fa-edit mr-1"></i>Evaluar requisitos</a>`;
            let btnEvaluarCriterios = `<a class="dropdown-item estilo-01" href="./evaluar-criterios.html"><i class="fas fa-edit mr-1"></i>Evaluar criterios</a>`;
            let btnVerPerfil = `<a class="dropdown-item" href="#"><i class="fas fa-id-card mr-1"></i>Ver perfil</a>`;
            let btnSeguimiento = `<a class="dropdown-item" href="#"><i class="fas fa-history mr-1"></i>Seguimiento</a>`;
            let btnVerReconocimiento = `<a class="dropdown-item" href="#"><i class="fas fa-medal mr-1"></i>Ver reconocimiento</a>`;

            let OpcionesEtapa5y8 = `<div class="dropdown-menu">${btnEvaluarRequisitos}${btnEvaluarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión">${x.CONVOCATORIA.ID_ETAPA == 5 || x.CONVOCATORIA.ID_ETAPA == 8 ? `${btnGestionar}${OpcionesEtapa5y8}` : btnDetalles}</td>`;
            let fila = `<tr>${colNro}${colCodigo}${colEmpresaParticipante}${colResponsable}${colCriterios}${colPuntuacion}${colAspiraciones}${colOpciones}</tr>`;
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
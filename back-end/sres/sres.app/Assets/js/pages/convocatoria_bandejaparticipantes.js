$(document).ready((e) => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnFirstPagination').on('click', btnFirstPaginationClick);
    $('#btnPreviousPagination').on('click', btnPreviousPaginationClick);
    $('#btnNextPagination').on('click', btnNextPaginationClick);
    $('#btnLastPagination').on('click', btnLastPaginationClick);
    $('#btn-informe').on('click', btnGenerarInforme);
    $('#btn-informefinal').on('click', btnGenerarInformeFinal);
    btnInformePreliminarvalidar();
    btnInformeFinalvalidar();
});

var consultar = () => {
    let idInscripcion = $('#txt-codigo').val();
    idInscripcion = isNaN(idInscripcion) == true ? -1 : (idInscripcion.trim() == "" ? null : parseInt(idInscripcion));
    let razonSocialInstitucion = $('#txt-empresa').val();
    let nombresCompletosUsuario = $('#txt-responsable').val();
    let idUsuario = (idEtapa < 14) ? idRolLogin == 1 ? null : idUsuarioLogin : null;
    //let idUsuario = idUsuarioLogin;
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();;
    let columna = 'id_inscripcion';
    let orden = 'asc'
    let params = { idConvocatoria, idInscripcion, razonSocialInstitucion, nombresCompletosUsuario, idUsuario, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/inscripcion/buscarinscripcion?${queryParams}`;

    fetch(url).then(r => r.json()).then(cargarDataBusqueda);
};

var cargarDataBusqueda = (data) => {
    let tabla = $('#tblBandejaParticipantes');
    let cantidadCeldasCabecera = tabla.find('thead tr th').length;
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

            //let btnDetalles = `<a class="btn btn-sm btn-success w-100" href="javascript:void(0)">Detalles</a>`;
            let btnGestionar = `<a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestión</a>`;
            let btnEvaluarRequisitos = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${idConvocatoria}/Institucion/${x.ID_INSTITUCION}/Evaluar"><i class="fas fa-edit mr-1"></i>Evaluar requisitos</a>`;
            let btnEvaluarCriterios = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${idConvocatoria}/Inscripcion/${x.ID_INSCRIPCION}/EvaluacionCriterios"><i class="fas fa-edit mr-1"></i>Evaluar criterios</a>`;
            let btnVerPerfil = `<a class="dropdown-item" href="#"><i class="fas fa-id-card mr-1"></i>Ver perfil</a>`;
            let btnSeguimiento = `<a class="dropdown-item" href="#"><i class="fas fa-history mr-1"></i>Seguimiento</a>`;
            let btnVerReconocimiento = `<a class="dropdown-item" href="#"><i class="fas fa-medal mr-1"></i>Ver reconocimiento</a>`;
            debugger;
            let OpcioneEta1 = `<div class="dropdown-menu">${btnVerPerfil}</div>`;
            let OpcioneEta2 = `<div class="dropdown-menu">${btnVerPerfil}</div>`;
            let OpcioneEta3 = `<div class="dropdown-menu">${idRolLogin == 2 ? btnEvaluarRequisitos : ''}${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta4 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta5 = `<div class="dropdown-menu">${idRolLogin == 2 ? btnEvaluarRequisitos : ''}${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta6 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta7 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta8 = `<div class="dropdown-menu">${idRolLogin == 2 ? btnEvaluarCriterios : ''}${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta9 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta10 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}<div>`;
            let OpcioneEta11 = `<div class="dropdown-menu">${idRolLogin == 2 ? btnEvaluarCriterios : ''}${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta12 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}</div>`;
            let OpcioneEta13 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcioneEta14 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcioneEta15 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcioneEta16 = `<div class="dropdown-menu">${btnVerPerfil}${btnSeguimiento}${btnVerReconocimiento}</div>`;

            //let OpcionesEtapa5y8 = `<div class="dropdown-menu">${btnEvaluarRequisitos}${btnEvaluarCriterios}${btnSeguimiento}${btnVerReconocimiento}</div>`;
            //let colOpciones = `<td class="text-center" data-encabezado="Gestión">${x.CONVOCATORIA.ID_ETAPA == 5 || x.CONVOCATORIA.ID_ETAPA == 8 ? `${btnGestionar}${OpcionesEtapa5y8}` : btnDetalles}</td>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100">${btnGestionar}${x.CONVOCATORIA.ID_ETAPA == 1 ? OpcioneEta1 : ''}${x.CONVOCATORIA.ID_ETAPA == 2 ? OpcioneEta2 : ''}${x.CONVOCATORIA.ID_ETAPA == 3 ? OpcioneEta3 : ''}${x.CONVOCATORIA.ID_ETAPA == 4 ? OpcioneEta4 : ''}${x.CONVOCATORIA.ID_ETAPA == 5 ? OpcioneEta5 : ''}${x.CONVOCATORIA.ID_ETAPA == 6 ? OpcioneEta6 : ''}${x.CONVOCATORIA.ID_ETAPA == 7 ? OpcioneEta7 : ''}${x.CONVOCATORIA.ID_ETAPA == 8 ? OpcioneEta8 : ''}${x.CONVOCATORIA.ID_ETAPA == 9 ? OpcioneEta9 : ''}${x.CONVOCATORIA.ID_ETAPA == 10 ? OpcioneEta10 : ''}${x.CONVOCATORIA.ID_ETAPA == 11 ? OpcioneEta11 : ''}${x.CONVOCATORIA.ID_ETAPA == 12 ? OpcioneEta12 : ''}${x.CONVOCATORIA.ID_ETAPA == 13 ? OpcioneEta13 : ''}${x.CONVOCATORIA.ID_ETAPA == 14 ? OpcioneEta14 : ''}${x.CONVOCATORIA.ID_ETAPA == 15 ? OpcioneEta15 : ''}${x.CONVOCATORIA.ID_ETAPA == 16 ? OpcioneEta16 : ''}</div></td>`;
            let colAnulado = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100">${btnGestionar}${OpcioneEta12}</div></td>`;
            let fila = `<tr ${x.FLAG_ANULAR == 1 ? 'style="background-color: #FED0C6";' : ''}>${colNro}${colCodigo}${colEmpresaParticipante}${colResponsable}${colCriterios}${colPuntuacion}${colAspiraciones}${x.FLAG_ANULAR == 1 ? colAnulado : colOpciones}</tr>`;
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

var btnInformePreliminarvalidar = () => {
    let idetapa = $('#convocatoria_ID_ETAPA').val();
    idetapa == 9 ? $('#btn-informe').removeClass('d-none') : $('#btn-informe').addClass('d-none');
}

var btnInformeFinalvalidar = () => {
    let idetapa = $('#convocatoria_ID_ETAPA').val();
    idetapa == 12 ? $('#btn-informefinal').removeClass('d-none') : $('#btn-informefinal').addClass('d-none');
}

var btnGenerarInforme = () => {
    debugger;
    let data = { ID_CONVOCATORIA: idConvocatoria, ID_USUARIO: idUsuarioLogin, USUARIO_GUARDAR: idUsuarioLogin };

    let url = `${baseUrl}api/informepreliminar/generarinformepreliminar`;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(alert('Informe generado'));
}

var btnGenerarInformeFinal = () => {
    debugger;
    let data = { ID_CONVOCATORIA: idConvocatoria, ID_USUARIO: idUsuarioLogin, USUARIO_GUARDAR: idUsuarioLogin };

    let url = `${baseUrl}api/informepreliminar/generarinformefinal`;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(alert('Informe generado'));
}
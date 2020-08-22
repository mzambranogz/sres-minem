var pageLoad = () => {
    cargarListaInscripcionRequerimiento();
    $('#btnEvaluar').on('click', btnEvaluarClick);    
}

var cargarListaInscripcionRequerimiento = () => {
    let url = `/api/inscripcionrequerimiento/listarinscripcionrequerimientoporconvocatoriainscripcion/${idConvocatoria}/${idInscripcion}`;

    fetch(url)
    .then(r => r.json())
    .then(mostrarListaInscripcionRequerimiento)
}

var mostrarListaInscripcionRequerimiento = (data) => {
    if (data.length > 0) {
        let tituloCriterios = '<div class="col-lg-6 col-md-12 col-sm-12"><h3 class="estilo-02 text-sres-azul mb-5">REQUISITOS GENERALES</h3></div>';
        let tituloArchivosAdjuntos = '<div class="col-lg-6 col-md-12 col-sm-12 d-none d-lg-block"><h3 class="estilo-02 text-sres-azul mb-5">EVALUACIÓN DE REQUISITOS</h3></div>';
        let cabecera = `<div class="row">${tituloCriterios}${tituloArchivosAdjuntos}</div>`;

        let contenido = data.map(x => {
            let existeFileRequerimientoSubido = x.ARCHIVO_BASE || '' != '';

            let tituloFileRequerimiento = `<label class="estilo-01 text-limit-1">${x.REQUERIMIENTO.NOMBRE}</label>`;
            let nombreFileRequerimiento = `<input class="form-control form-control-sm cursor-pointer txt-file-control" type="text" id="txt-requisito-${x.ID_REQUERIMIENTO}" value="${x.ARCHIVO_BASE}" readonly>`;
            let btnDescargaFileRequerimiento = `<div class="input-group-append"><a class="input-group-text cursor-pointer estilo-01" href="${baseUrl}api/inscripcionrequerimiento/obtenerarchivo/${idConvocatoria}/${idInscripcion}/${idInstitucionLogin}/${x.ID_REQUERIMIENTO}" download><i class="fas fa-download mr-1"></i>Bajar archivo</a></div>`;
            let fileRequerimiento = `<div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-file"></i></span></div>${nombreFileRequerimiento}${btnDescargaFileRequerimiento}</div>`;
            let colLeft = `<div class="col-lg-6 col-md-12 col-sm-12">${tituloFileRequerimiento}${fileRequerimiento}</div>`;

            let checkAprobado = `<div class="form-check form-check-inline"><input class="form-check-input" type="radio" data-id="${x.ID_REQUERIMIENTO}" name="rad-evaluacion-05-${x.ID_REQUERIMIENTO}" id="rad-eva-09-${x.ID_REQUERIMIENTO}" value="1" ${(x.VALIDO == true ? "checked" : "")} ><label class="form-check-label" for="rad-eva-09-${x.ID_REQUERIMIENTO}">Aprobado</label></div>`;
            let checkDesaprobado = `<div class="form-check form-check-inline"><input class="form-check-input" type="radio" data-id="${x.ID_REQUERIMIENTO}" name="rad-evaluacion-05-${x.ID_REQUERIMIENTO}" id="rad-eva-10-${x.ID_REQUERIMIENTO}" value="0" ${(x.VALIDO == false ? "checked" : "")} ><label class="form-check-label" for="rad-eva-10-${x.ID_REQUERIMIENTO}">Desaprobado</label></div>`;
            let checkOptions = `<label class="estilo-01">&nbsp;${checkAprobado}${checkDesaprobado}</label>`;

            let mensajeEvaluacionFileRequerimiento = `<div id="msg-${x.ID_REQUERIMIENTO}" data-id-req="${x.ID_REQUERIMIENTO}" class="alert alert-secondary p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha evaluado el documento</span></div></div>`;
            if (x.VALIDO == true) {
                mensajeEvaluacionFileRequerimiento = `<div id="msg-${x.ID_REQUERIMIENTO}" data-id-req="${x.ID_REQUERIMIENTO}" class="alert alert-success p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">El documento es correcto</span></div></div>`;
            } else if (x.VALIDO == false) {
                mensajeEvaluacionFileRequerimiento = `<div id="msg-${x.ID_REQUERIMIENTO}" data-id-req="${x.ID_REQUERIMIENTO}" class="alert alert-danger p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-times-circle px-2 py-1"></i><span class="estilo-01">El documento es incorrecto</span></div></div>`;
            }
            let colRight = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="form-group">${checkOptions}${mensajeEvaluacionFileRequerimiento}</div></div>`;
            let contenidoFinal = `<div class="row">${colLeft}${colRight}</div>`;
            return contenidoFinal;
        }).join('')
        $('#lstInscripcionRequerimiento').html(`${cabecera}${contenido}`);
        data.forEach(x => {
            $(`input[type="file"][id*="fle-requisito-${x.ID_REQUERIMIENTO}"]`).data('file', x.ARCHIVO_CONTENIDO);
            $(`input[type="file"][id*="fle-requisito-${x.ID_REQUERIMIENTO}"]`).data('type', x.ARCHIVO_TIPO);
        })
        
        $('input[id*=rad-eva-09].form-check-input').on('change', chkAprobadoChange);
        $('input[id*=rad-eva-10].form-check-input').on('change', chkDesaprobadoChange);
        //$('input[type="file"].fil-file-control').on('change', fileRequerimientoChange);
        //$('input[type="file"][id*="fle-requisito-"]').on('change', fileRequerimientoChange);
        //$(`[id*="viewContentFile-"] .btnEliminarFile`).on('click', btnEliminarFileClick);
        if (idEtapa == 5) validarAnular();
    }
}

var chkAprobadoChange = (e) => {
    let id = $(e.currentTarget).attr('data-id');
    $(`#msg-${id}`).removeClass('alert-secondary').removeClass('alert-success').removeClass('alert-danger').addClass('alert-success');
    $(`#msg-${id}`).html(`<div class="mr-lg-auto"><i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">El documento es correcto</span></div>`);
    if (idEtapa == 5) validarAnular();
}

var chkDesaprobadoChange = (e) => {
    let id = $(e.currentTarget).attr('data-id');
    $(`#msg-${id}`).removeClass('alert-secondary').removeClass('alert-success').removeClass('alert-danger').addClass('alert-danger');
    $(`#msg-${id}`).html(`<div class="mr-lg-auto"><i class="fas fa-times-circle px-2 py-1"></i><span class="estilo-01">El documento es incorrecto</span></div>`);
    if (idEtapa == 5) validarAnular();
}

var btnEvaluarClick = (e) => {
    e.preventDefault();
    evaluarInscripcion();
}

var evaluarInscripcion = () => {
    let norma1 = $('#chk-norma-01').prop('checked');
    let norma2 = $('#chk-norma-02').prop('checked');
    let norma3 = $('#chk-norma-02').prop('checked');

    let observacion = $('#txa-observaciones').val().trim();

    let mensaje = [];

    if (!norma1) mensaje.push('norma 1');
    if (!norma2) mensaje.push('norma 2');
    if (!norma3) mensaje.push('norma 3');

    if (mensaje.length > 0) {
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'ERROR', message: `Debe marcar ${mensaje.join(' y ')}` });
        return;
    }

    if (observacion == '' && $('.alert-secondary').length == 0) {
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'ERROR', message: `Debes ingresar observación` });
        return;
    }

    let listaInputFile = $('[data-id-req]');
    //debugger;
    let listaInscripcionRequerimiento = Array.from(listaInputFile).filter(x => !$(x).hasClass('alert-secondary'));

    if (listaInscripcionRequerimiento.length == 0) {
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'ERROR', message: `No se ha evaluado ningún requisito` });
        return;
    }

    //if (listaInscripcionRequerimiento.length < listaInputFile.length) {
    //    $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'ERROR', message: 'Necesita completas todos los requerimientos' });
    //    return;
    //}

    listaInscripcionRequerimiento = listaInscripcionRequerimiento.map((x, i) => {
        let idRequerimiento = $(x).attr('data-id-req');
        return {
            ID_CONVOCATORIA: idConvocatoria,
            ID_INSCRIPCION: idInscripcion,
            ID_REQUERIMIENTO: idRequerimiento,
            VALIDO: $(x).hasClass('alert-secondary') ? null : $(x).hasClass('alert-success'),
            OBSERVACION: $(x).hasClass('alert-secondary') ? null : $(x).find('.estilo-01').text(),
            UPD_USUARIO: idUsuarioLogin
        }
    });

    let data = {
        ID_CONVOCATORIA: idConvocatoria,
        ID_INSCRIPCION: idInscripcion,
        ID_ETAPA: idEtapa,
        LISTA_INSCRIPCION_REQUERIMIENTO: listaInscripcionRequerimiento,
        ID_TIPO_EVALUACION: $('.alert-secondary').length > 0 ? null : $('.alert-danger').length == 0 ? 1 : 2,
        OBSERVACION: observacion,
        UPD_USUARIO: idUsuarioLogin,
        USUARIO_GUARDAR: idUsuarioLogin
    }

    //console.log(formData);
    //let url = `/api/inscripcion/evaluarinscripcion`;
    let url = ``;

    if ($('.alert-danger').length == 0 || idEtapa != 5) url = `/api/inscripcion/evaluarinscripcion`;
    else url = `/api/inscripcion/anularinscripcion`;

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    //let init = { method: 'POST', body: formData };

    fetch(url, init)
    .then(r => r.json())
    .then(mostrarMensaje)
};

var mostrarMensaje = (data) => {
    if (data == true) {
        $('#btnEvaluar').parent().parent().hide();
        if ($('.alert-danger').length == 0 || idEtapa != 5) $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'success', title: 'BIEN HECHO', message: `¡Se guardó correctamente!`, close: { time: 5000 } });
        else $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'success', title: 'CORRECTO', message: `¡Se realizó correctamente el proceso de anulación y se notificó al usuario responsable!`, close: { time: 5000 } });
        setTimeout(() => { location.href = `${baseUrl}Convocatoria/${idConvocatoria}/BandejaParticipantes/`; }, 5000);
        //cargarListaInscripcionRequerimiento();
    } else {
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'OCURRIÓ UN ERROR INESPERADO', message: `Inténtelo nuevamente por favor.`, close: { time: 5000 } });
    }
}

var validarAnular = () => {
    if ($('.alert-danger').length == 0) {
        $('#btnEvaluar').html('Evaluar');
        $('#btnEvaluar').removeClass('btn-danger').addClass('btn-primary');
    } else if ($('.alert-danger').length > 0) {
        $('#btnEvaluar').html('Anular');
        $('#btnEvaluar').removeClass('btn-primary').addClass('btn-danger');
    }
}

$(document).ready(pageLoad);
﻿$(document).ready(() => {
    consultar();
    consultarDoc();
    cargarEvaluacion();
    $('#btnGuardar').on('click', (e) => guardar());
});

var cargarEvaluacion = () => {
    let params = { idCriterio, idInscripcion, idConvocatoria };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/criterio/obtenerconvcriteriopuntajeinscripcion?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        if (j != null) {
            $(`#rad-eva-cri-0${j.ID_TIPO_EVALUACION}`).prop('checked', true);
            $(`#cbo-puntaje`).val(j.ID_DETALLE);
            $(`#txa-observaciones`).val(j.OBSERVACION);
        }
    });
}

var consultar = () => {
    let id_criterio = idCriterio;
    let id_caso = $(`#cbo-caso`).val();
    let id_inscripcion = idInscripcion;
    let params = { id_criterio, id_inscripcion, id_caso };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/criterio/buscarcriteriocaso?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = j.map((x, i) => {
            let head = armarHead(x.LIST_INDICADOR_HEAD, x.INCREMENTABLE, "'" + x.ID_CRITERIO + '-' + x.ID_CASO + '-' + x.ID_COMPONENTE + "'", x.ID_COMPONENTE);
            let body = armarBody(x.LIST_INDICADOR_BODY, x.INCREMENTABLE);
            return `<div class="table-responsive tabla-principal"><table class="table table-sm table-hover m-0 get" id="${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}" data-comp="${x.ID_COMPONENTE}" data-eliminar="">${head}${body}</table></div>`;
        }).join('');
        $("#table-add").html(`${contenido}`);

        j.map((x, i) => {
            if (x.INCREMENTABLE == '0')
                armarBodyEstatico(x.LIST_INDICADOR_BODY);
        });
        $("[data-toggle='tooltip']").tooltip();
    });
};

var consultarDoc = () => {
    let id_criterio = idCriterio;
    let id_caso = $(`#cbo-caso`).val();
    let id_convocatoria = idConvocatoria;
    let id_inscripcion = idInscripcion;
    let params = { id_criterio, id_caso, id_convocatoria, id_inscripcion };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/criterio/buscarcriteriocasodocumento?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        mostrarDocumentos(j);
    });
};

var mostrarDocumentos = (data) => {
    if (data.length > 0) {
        let tituloDoc = '<div class="col-lg-6 col-md-12 col-sm-12"><h3 class="estilo-02 text-sres-azul mb-5 text-left">DOCUMENTOS</h3></div>';
        let tituloArchivosAdjuntos = '<div class="col-lg-6 col-md-12 col-sm-12 d-none d-lg-block"><h3 class="estilo-02 text-sres-azul mb-5 text-left"></h3></div>';
        let cabecera = `<div class="row">${tituloDoc}${tituloArchivosAdjuntos}</div>`;

        let contenido = data.map(x => {
            let fileDoc = `<div class="form-group"><label class="estilo-01 text-limit-1 text-left" for="fle-requisito-${x.ID_DOCUMENTO}">${x.NOMBRE}<span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span></label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-file"></i></span></div><input class="form-control form-control-sm cursor-pointer txt-file-control" type="text" id="txt-requisito-${x.ID_DOCUMENTO}" value="${x.OBJ_INSCDOC == null ? `` : x.OBJ_INSCDOC.ARCHIVO_BASE}" disabled><div class="input-group-append"><a class="input-group-text cursor-pointer estilo-01" href="${baseUrl}api/criterio/obtenerdocumento/${idConvocatoria}/${idCriterio}/${$(`#cbo-caso`).val()}/${idInscripcion}/${x.ID_DOCUMENTO}" download><i class="fas fa-download mr-1"></i>Bajar archivo</a></div></div></div>`
            let colLeft = `<div class="col-lg-6 col-md-12 col-sm-12">${fileDoc}</div>`;
            let contenidoFileDoc = ``;
            //let contenidoFileDoc = `<div class="alert alert-secondary p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha subido el documento requerido</span></div></div>`;
            //if (x.OBJ_INSCDOC != null) {
            //    let nombreFileDoc = `<i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${x.OBJ_INSCDOC.ARCHIVO_BASE}</span>`;
            //    let btnDescargaFileDoc = `<a class="text-sres-verde" href="${baseUrl}api/inscripcionrequerimiento/obtenerarchivo/${idConvocatoria}/${idCriterio}/${$(`#cbo-caso`).val()}/${x.ID_DOCUMENTO}"><i class="fas fa-download px-2 py-1"></i></a>`;
            //    let btnEliminarFileDoc = `<a class="text-sres-verde btnEliminarFile" href="#" data-id="${x.ID_DOCUMENTO}"><i class="fas fa-trash px-2 py-1"></i></a>`;
            //    contenidoFileDoc = `<div class="alert alert-success p-1 d-flex"><div class="mr-auto">${nombreFileDoc}</div><div class="ml-auto">${btnDescargaFileDoc}${btnEliminarFileDoc}</div></div>`;
            //}


            let colRight = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="form-group" id="viewContentFile-${x.ID_DOCUMENTO}"><label class="estilo-01">&nbsp;</label>${contenidoFileDoc}</div></div>`;
            let contenidoFinal = `<div class="row">${colLeft}${colRight}</div>`;
            return contenidoFinal;
        }).join('')
        $('#doc-add').html(`${cabecera}${contenido}`);
        //data.forEach(x => {
        //    x.OBJ_INSCDOC == null ? '' : $(`input[type="file"][id*="fle-requisito-${x.ID_DOCUMENTO}"]`).data('file', x.OBJ_INSCDOC.ARCHIVO_CONTENIDO);
        //})
    }
}

var armarHead = (lista, incremental, id, componente) => {
    let cont = ``;
    for (var i = 0; i < lista.length; i++) {
        cont += `<th scope="col"><div class="d-flex flex-column justify-content-start align-items-center"><span>${lista[i]["OBJ_PARAMETRO"].NOMBRE}</span>${lista[i]["OBJ_PARAMETRO"].UNIDAD == null ? `` : lista[i]["OBJ_PARAMETRO"].UNIDAD == '' ? `` : `<small>(${lista[i]["OBJ_PARAMETRO"].UNIDAD})</small>`}${lista[i]["OBJ_PARAMETRO"].DESCRIPCION == null ? `<i class="mt-2"></i>` : `<i class="fas fa-question-circle mt-2" data-toggle="tooltip" data-placement="bottom" title="${lista[i]["OBJ_PARAMETRO"].DESCRIPCION}"></i>`}</div></th>`;
    }
    return `<thead class="estilo-06"><tr>${cont}</tr></thead>`;
};

var armarBody = (lista, incremental) => {
    let body = ``;
    for (var i = 0; i < lista.length; i++) {
        body += armarFila(lista[i]["FLAG_NUEVO"] == 0 ? lista[i]["LIST_INDICADORFORM"] : lista[i]["LIST_INDICADORDATA"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"], lista[i]["FLAG_NUEVO"], incremental, lista[i]["FLAG_NUEVO"] == 0 ? lista[i]["ID_INDICADOR"] : (i + 1));
    }
    return `<tbody class="estilo-01">${body}</tbody>`;
};

var armarFila = (lista, id_criterio, id_caso, id_componente, id_indicador, flag_nuevo, incremental, row) => {
    let filas = ``;
    filas += `<tr id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? incremental == '1' ? row : id_indicador : row}" data-ind="${flag_nuevo == 0 ? 0 : id_indicador}">`;
    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["ID_TIPO_CONTROL"] == 2) {
            if (lista[i]["ESTATICO"] == '1')
                filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="text-center estilo-01">${validarNull(lista[i]["VALOR"])}</div><input class="get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${validarNull(lista[i]["VALOR"])}" data-param="${lista[i]["ID_PARAMETRO"]}" type="hidden" /></td>`;
            else
                filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><input class="form-control-plaintext form-control-sm estilo-01 text-sres-gris ${lista[i]["ID_TIPO_DATO"] == '1' ? 'solo-numero' : ''} ${lista[i]["DECIMAL_V"] == null ? '' : lista[i]["DECIMAL_V"] == '1' ? 'formato-decimal' : ''} ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} get-valor" type="${lista[i]["ID_TIPO_DATO"] == '1' ? 'text' : lista[i]["ID_TIPO_DATO"] == '3' ? 'date' : 'text'}" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${validarNull(lista[i]["VALOR"])}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["RESULTADO"] == '1' ? `data-resultado="1"` : `` : ``} ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["OBTENIBLE"] == '1' ? `data-obtenible="1"` : `` : ``} maxlength="${lista[i]["TAMANO"]}" ${lista[i]["VERIFICABLE"] == '1' ? `onBlur="verificarValor(this)"` : ``}  ${lista[i]["EDITABLE"] == '0' ? `readonly` : ``} readonly /></div></div></td>`;
        } else {
            let v = 0;
            //filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><select class="form-control form-control-sm multi-opciones ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["FILTRO"] == null ? `` : lista[i]["FILTRO"] == '' ? `` : `data-filtro="${lista[i]["FILTRO"]}" onchange="filtrar(this)"`}  ${lista[i]["VERIFICABLE"] == '1' ? `onchange="verificarValor(this)"` : ``}><option value="0">Seleccione</option>`;
            for (var j = 0; j < lista[i]["LIST_PARAMDET"].length; j++) {
                //filas += `<option value="${lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"]}" ${validarNull(lista[i]["VALOR"]) == lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"] ? `selected` : ``}>${lista[i]["LIST_PARAMDET"][j]["NOMBRE"]}</option>`;
                if (validarNull(lista[i]["VALOR"]) == lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"]) {
                    //debugger;
                    filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><input class="form-control-plaintext form-control-sm estilo-01 text-sres-gris" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${lista[i]["LIST_PARAMDET"][j]["NOMBRE"]}" readonly /></div></div></td>`;
                    v++;
                }
            }
            if (v == 0) filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><input class="form-control-plaintext form-control-sm estilo-01 text-sres-gris" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="" readonly /></div></div></td>`;
            //filas += `</select></div></td>`;
            //$(`#${id_criterio}-${id_caso}-${id_componente}-${incremental == '1' ? row : id_indicador}-${lista[i]["ID_PARAMETRO"]}`).val(validarNull(lista[i]["VALOR"]));
        }

    }
    //filas += incremental == '1' ? `<td><div class="btn btn-info btn-sm estilo-01" type="button" onclick="eliminarFila(this);"><i class="fas fa-minus-circle mr-1"></i>Quitar</div></td>` : ``;
    return `${filas}</tr>`;
}

var armarBodyEstatico = (lista) => {
    for (var i = 0; i < lista.length; i++) {
        armarFilaEstatico(lista[i]["LIST_INDICADORDATA"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"]);
    }
};

var armarFilaEstatico = (lista, id_criterio, id_caso, id_componente, id_indicador) => {
    $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador).data('ind', id_indicador)

    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["ID_TIPO_CONTROL"] == 1) {
            for (var j = 0; j < lista[i]["LIST_PARAMDET"].length; j++) {
                if (validarNull(lista[i]["VALOR"]) == lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"]) {
                    $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).val(validarNull(lista[i]["LIST_PARAMDET"][j]["NOMBRE"]));
                }
            }
        } else {
            lista[i]["ESTATICO"] == '1' ? '' : $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).val(validarNull(lista[i]["VALOR"]));
        }

    }
}

var validarNull = (valor) => {
    if (valor == null) valor = '';
    return valor;
}

var guardar = () => {
    var arr = [];
    let idEvaluacion = 0;
    $('input[type="radio"][id*="rad-eva-cri-0"]').each((x, y) => {
        if ($(y).prop('checked')) {
            idEvaluacion = $(y).attr('id').replace('rad-eva-cri-0', '');
        }
    });

    if (idEvaluacion == 0) arr.push('Seleccione un tipo de evaluación (aprobado o desaprobado)');
    if ($(`#cbo-puntaje`).val() == 0) arr.push('Seleccione un puntaje');
    if ($(`#txa-observaciones`).val().trim() === "") arr.push('Ingrese la descripción de la observación');

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let puntaje = $(`#cbo-puntaje`).val();
    let observacion = $(`#txa-observaciones`).val();
    let url = `/api/criterio/guardarevaluacioncriterio`;
    let data = { ID_CONVOCATORIA: idConvocatoria, ID_CRITERIO: idCriterio, ID_DETALLE: puntaje, ID_INSCRIPCION: idInscripcion, ID_TIPO_EVALUACION: idEvaluacion, OBSERVACION: observacion, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        j ? $('#btnGuardar').parent().parent().hide() : '';
        j ? $('.alert-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'La evaluación de este criterio fue guardado correctamente.', close: { time: 4000 }, url: `${baseUrl}Convocatoria/${idConvocatoria}/EvaluacionCriterios` }) : $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}
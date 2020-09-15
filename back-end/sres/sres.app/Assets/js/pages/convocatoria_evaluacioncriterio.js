$(document).ready(() => {
    consultar();
    consultarDoc();
    cargarEvaluacion();
    $('#btnGuardar').on('click', (e) => guardar());
});

var cargarEvaluacion = () => {
    let params = { idCriterio, idInscripcion, idConvocatoria };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/criterio/obtenerconvcriteriopuntajeinscripcion?${queryParams}`;
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

    let url = `${baseUrl}api/criterio/buscarcriteriocaso?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = j.map((x, i) => {
            let head = armarHead(x.LIST_INDICADOR_HEAD, x.INCREMENTABLE, "'" + x.ID_CRITERIO + '-' + x.ID_CASO + '-' + x.ID_COMPONENTE + "'", x.ID_COMPONENTE);
            let body = armarBody(x.LIST_INDICADOR_BODY, x.INCREMENTABLE);
            return `<h3 class="estilo-02 text-sres-azul mt-5 text-left">${x.ETIQUETA == null ? '' : x.ETIQUETA}</h3><div class="table-responsive tabla-principal"><table class="table table-sm table-hover m-0 get" id="${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}" data-comp="${x.ID_COMPONENTE}" data-eliminar="">${head}${body}</table></div>`;
        }).join('');
        $("#table-add").html(`${contenido}`);

        j.map((x, i) => {
            if (x.INCREMENTABLE == '0')
                armarBodyEstatico(x.LIST_INDICADOR_BODY);
        });
        $("[data-toggle='tooltip']").tooltip();
        let emisiones = 0.0
        $(document).find('.get-emisiones').each((x, y) => {
            //emisiones += $(y).val() == '' ? 0.0 : parseFloat($(y).val());
            emisiones += $(y).html() == '' ? 0.0 : parseFloat($(y).html());
        });
        //$(`#txt-emisiones`).val(formatoMiles(emisiones / 1000));
        $(`#txt-emisiones`).val(formatoMiles(emisiones));

        let energia = 0.0
        $(document).find('.get-ahorro').each((x, y) => {
            energia += $(y).html() == '' ? 0.0 : parseFloat($(y).html());
        });
        $(`#txt-ahorro`).val(formatoMiles(energia));

        let combustible = 0.0
        $(document).find('.get-combustible').each((x, y) => {
            combustible += $(y).html() == '' ? 0.0 : parseFloat($(y).html());
        });
        $(`#txt-combustible`).val(formatoMiles(combustible));
    });
};

var consultarDoc = () => {
    let id_criterio = idCriterio;
    let id_caso = $(`#cbo-caso`).val();
    let id_convocatoria = idConvocatoria;
    let id_inscripcion = idInscripcion;
    let params = { id_criterio, id_caso, id_convocatoria, id_inscripcion };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/criterio/buscarcriteriocasodocumento?${queryParams}`;
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
            let fileDoc = `<div class="form-group text-left"><label class="estilo-01" for="fle-requisito-${x.ID_DOCUMENTO}">${x.NOMBRE}</label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-file"></i></span></div><input class="form-control form-control-sm cursor-pointer txt-file-control" type="text" id="txt-requisito-${x.ID_DOCUMENTO}" value="${x.OBJ_INSCDOC == null ? `` : x.OBJ_INSCDOC.ARCHIVO_BASE}" readonly><div class="input-group-append"><a class="input-group-text cursor-pointer estilo-01" href="${baseUrl}api/criterio/obtenerdocumento/${idConvocatoria}/${idCriterio}/${$(`#cbo-caso`).val()}/${idInscripcion}/${x.ID_DOCUMENTO}" download><i class="fas fa-download mr-1"></i>Bajar archivo</a></div></div></div>`
            let colLeft = `<div class="col-lg-6 col-md-12 col-sm-12">${fileDoc}</div>`;
            let radioAprobado = `<div class="form-check form-check-inline"><input class="form-check-input get-evaluacion" type="radio" name="rad-evaluacion-${x.ID_DOCUMENTO}" id="rad-eva-${x.ID_DOCUMENTO}-1" onchange="verificarEvaluacion(this)" value="1" ${x.OBJ_INSCDOC == null ? `` : x.OBJ_INSCDOC.ID_TIPO_EVALUACION == null ? `` : x.OBJ_INSCDOC.ID_TIPO_EVALUACION == 1 ? `checked` : ``}><label class="form-check-label" for="rad-eva-${x.ID_DOCUMENTO}-1">Aprobado</label></div>`;
            let radioDesaprobado = `<div class="form-check form-check-inline"><input class="form-check-input get-evaluacion" type="radio" name="rad-evaluacion-${x.ID_DOCUMENTO}" id="rad-eva-${x.ID_DOCUMENTO}-2" onchange="verificarEvaluacion(this)" value="0" ${x.OBJ_INSCDOC == null ? `` : x.OBJ_INSCDOC.ID_TIPO_EVALUACION == null ? `` : x.OBJ_INSCDOC.ID_TIPO_EVALUACION == 2 ? `checked` : ``}><label class="form-check-label" for="rad-eva-${x.ID_DOCUMENTO}-2">Desaprobado</label></div>`;
            let contenidoFileDoc = `<div class="alert alert-secondary p-1 d-flex w-100"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha evaluado el documento</span></div></div>`;
            if (x.OBJ_INSCDOC != null) {
                if (x.OBJ_INSCDOC.ID_TIPO_EVALUACION > 0) {
                    let labelAprobado = x.OBJ_INSCDOC.ID_TIPO_EVALUACION == 1 ? `<div class="alert alert-success p-1 d-flex w-100"><div class="mr-lg-auto"><i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">El documento es correcto</span></div></div>` : ``;
                    let labelDesaprobado = x.OBJ_INSCDOC.ID_TIPO_EVALUACION == 2 ? `<div class="alert alert-danger p-1 d-flex w-100"><div class="mr-lg-auto"><i class="fas fa-times-circle px-2 py-1"></i><span class="estilo-01">El documento es incorrecto</span></div></div>` : ``;
                    contenidoFileDoc = `${labelAprobado}${labelDesaprobado}`;
                }
            }
            //let colRight = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="form-group" id="viewContentFile-${x.ID_DOCUMENTO}"><label class="estilo-01 text-left w-100">${radioAprobado}${radioDesaprobado}</label><div id="evaluacion-${x.ID_DOCUMENTO}">${contenidoFileDoc}</div></div></div>`;
            let colRight = `<div class="col-lg-6 col-md-12 col-sm-12 d-flex align-items-end"><div class="w-100 text-left" id="viewContentFile-${x.ID_DOCUMENTO}"><label class="estilo-01">${radioAprobado}${radioDesaprobado}</label><div id="evaluacion-${x.ID_DOCUMENTO}">${contenidoFileDoc}</div></div></div>`;
            let contenidoFinal = `<div class="row">${colLeft}${colRight}</div>`;
            return contenidoFinal;
        }).join('')
        $('#doc-add').html(`${cabecera}${contenido}`);
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
                //filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><input class="form-control-plaintext form-control-sm estilo-01 text-sres-gris ${lista[i]["ID_TIPO_DATO"] == '1' ? 'solo-numero' : ''} ${lista[i]["DECIMAL_V"] == null ? '' : lista[i]["DECIMAL_V"] == '1' ? 'formato-decimal' : ''} ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} ${lista[i]["EMISIONES"] == null ? `` : lista[i]["EMISIONES"] == '1' ? `get-emisiones` : ``} ${lista[i]["AHORRO"] == null ? `` : lista[i]["AHORRO"] == '1' ? `get-ahorro` : ``}  get-valor" type="${lista[i]["ID_TIPO_DATO"] == '1' ? 'text' : lista[i]["ID_TIPO_DATO"] == '3' ? 'date' : 'text'}" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${validarNull(lista[i]["VALOR"])}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["RESULTADO"] == '1' ? `data-resultado="1"` : `` : ``} ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["OBTENIBLE"] == '1' ? `data-obtenible="1"` : `` : ``} maxlength="${lista[i]["TAMANO"]}" ${lista[i]["VERIFICABLE"] == '1' ? `onBlur="verificarValor(this)"` : ``}  ${lista[i]["EDITABLE"] == '0' ? `readonly` : ``} readonly /></div></div></td>`;
                filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><span class="form-control-plaintext form-control-sm estilo-01 text-sres-gris ${lista[i]["ID_TIPO_DATO"] == '1' ? 'solo-numero text-right' : ''} ${lista[i]["DECIMAL_V"] == null ? '' : lista[i]["DECIMAL_V"] == '1' ? 'formato-decimal text-right' : ''} ${lista[i]["EMISIONES"] == null ? `` : lista[i]["EMISIONES"] == '1' ? `get-emisiones` : ``} ${lista[i]["AHORRO"] == null ? `` : lista[i]["AHORRO"] == '1' ? `get-ahorro` : ``} ${lista[i]["COMBUSTIBLE"] == null ? `` : lista[i]["COMBUSTIBLE"] == '1' ? `get-combustible` : ``} get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}">${validarNull(lista[i]["VALOR"])}</span></div></div></td>`;
        } else if (lista[i]["ID_TIPO_CONTROL"] == 1) {
            let v = 0;
            for (var j = 0; j < lista[i]["LIST_PARAMDET"].length; j++) {
                if (validarNull(lista[i]["VALOR"]) == lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"]) {
                    //filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><input class="form-control-plaintext form-control-sm estilo-01 text-sres-gris" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${lista[i]["LIST_PARAMDET"][j]["NOMBRE"]}" readonly /></div></div></td>`;
                    filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><span class="form-control-plaintext form-control-sm estilo-01 text-sres-gris" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}">${lista[i]["LIST_PARAMDET"][j]["NOMBRE"]}</span></div></div></td>`;
                    v++;
                }
            }
            //if (v == 0) filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><input class="form-control-plaintext form-control-sm estilo-01 text-sres-gris" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="" readonly /></div></div></td>`;
            if (v == 0) filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><span class="form-control-plaintext form-control-sm estilo-01 text-sres-gris" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}"><span></div></div></td>`;
        } else if (lista[i]["ID_TIPO_CONTROL"] == 3) {
            filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><div class="input-group"><span class="form-control-plaintext form-control-sm estilo-01 text-sres-gris get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}">${validarNull(lista[i]["VALOR"])}</span></div></div></td>`;
        }
    }
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
                    //$('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).val(validarNull(lista[i]["LIST_PARAMDET"][j]["NOMBRE"]));
                    $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).html(validarNull(lista[i]["LIST_PARAMDET"][j]["NOMBRE"]));
                }
            }
        } else {
            //lista[i]["ESTATICO"] == '1' ? '' : $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).val(validarNull(lista[i]["VALOR"]));
            lista[i]["ESTATICO"] == '1' ? '' : $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).html(validarNull(lista[i]["VALOR"]));
        }

    }
}

var validarNull = (valor) => {
    if (valor == null) valor = '';
    return valor;
}

var verificarEvaluacion = (e) => {
    if ($(e).is(':checked')) {
        let arr = $(e).attr('id').replace('rad-eva-', '').split('-');
        let evaluacion = arr[1] == 1 ? `<div class="alert alert-success p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">El documento es correcto</span></div></div>` : `<div class="alert alert-danger p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-times-circle px-2 py-1"></i><span class="estilo-01">El documento es incorrecto</span></div></div>`;
        $(`#evaluacion-${arr[0]}`).html(evaluacion);
    }
}

var guardar = () => {
    var arr = [];
    let listaEvaluacion = [];
    let idEvaluacion = 0;
    let emisiones = $(`#txt-emisiones`).val();
    let energia = $(`#txt-ahorro`).val();
    let combustible = $(`#txt-combustible`).val();
    $('input[type="radio"][id*="rad-eva-cri-0"]').each((x, y) => {
        if ($(y).prop('checked')) {
            idEvaluacion = $(y).attr('id').replace('rad-eva-cri-0', '');
        }
    });

    if (idEvaluacion == 0) arr.push('Seleccione un tipo de evaluación (aprobado o desaprobado)');
    if ($(`#cbo-puntaje`).val() == 0) arr.push('Seleccione un puntaje');
    if ($(`#txa-observaciones`).val().trim() === "") arr.push('Ingrese la descripción de la observación');

    $('[id*="viewContentFile-"]').each((x, y) => {
        let validar = false;
        let idDocumento = 0;
        let idTipoEvaluacion = 0;
        $(y).find('.get-evaluacion').each((w, z) => {
            if ($(z).is(':checked')) {
                validar = true;
                idDocumento = $(z).attr('id').replace('rad-eva-', '').split('-')[0];
                idTipoEvaluacion = $(z).attr('id').replace('rad-eva-', '').split('-')[1];
            }
        });
        if (!validar) { arr.push('Falta(n) evaluar documento(s)'); return false; }
        let r = {
            ID_CONVOCATORIA: idConvocatoria,
            ID_CRITERIO: idCriterio,
            ID_CASO: $(`#cbo-caso`).val(),
            ID_DOCUMENTO: idDocumento,
            ID_INSCRIPCION: idInscripcion,
            ID_TIPO_EVALUACION: idTipoEvaluacion
        }
        listaEvaluacion.push(r)
    });

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let puntaje = $(`#cbo-puntaje`).val();
    let observacion = $(`#txa-observaciones`).val();
    let url = `${baseUrl}api/criterio/guardarevaluacioncriterio`;
    let data = { ID_CONVOCATORIA: idConvocatoria, ID_CRITERIO: idCriterio, ID_DETALLE: puntaje, ID_INSCRIPCION: idInscripcion, ID_TIPO_EVALUACION: idEvaluacion, EMISIONES_REDUCIDAS: emisiones, ENERGIA: energia, COMBUSTIBLE: combustible, OBSERVACION: observacion, NOMBRE_CRI: $(`.nom-cri`).val(), LIST_INSCDOC: listaEvaluacion, ID_ETAPA: idEtapa, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-add').html('');
        j ? $('#btnGuardar').parent().parent().hide() : '';
        j ? $('.alert-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'La evaluación de este criterio fue guardado correctamente.', close: { time: 4000 }, url: `${baseUrl}Convocatoria/${idConvocatoria}/Inscripcion/${idInscripcion}/EvaluacionCriterios` }) : $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}

var formatoMiles = (n) => {
    var m = n * 1;
    return m.toFixed(3).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}
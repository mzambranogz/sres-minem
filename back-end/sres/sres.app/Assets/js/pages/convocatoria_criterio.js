var electricidadHead = 0, combustibleHead = 0, energiaelectrica = 0, energiatermica = 0, cambiomatriz = 0, emisiones = 0;

$(document).ready(() => {
    //$('#btnConsultar').on('click', (e) => consultar());
    //$('#btnConsultar')[0].click();
    //$('#btnNuevo').on('click', (e) => nuevo());
    //$('#btnCerrar').on('click', (e) => cerrarFormulario());
    consultar();
    consultarDoc();
    $('#btnGuardar').on('click', (e) => guardar());
});

$(document).on("change", "#cbo-caso", () => {
    consultar();
    consultarDoc();
});

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
            //return `<h3 class="estilo-02 text-sres-azul mt-5 text-left">${x.ETIQUETA == null ? '' : x.ETIQUETA}</h3><div class="table-responsive tabla-principal"><table class="table table-sm table-hover m-0 get" id="${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}" data-comp="${x.ID_COMPONENTE}" data-eliminar="">${head}${body}</table></div>${x.INCREMENTABLE == '1' ? `<div class="btn btn-warning btn-sm estilo-01 addBtnGnralLeft" type="button" onclick="agregarFila('${`${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}`}',${x.ID_COMPONENTE});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>` : ''}`;
            return `<h3 class="estilo-02 text-sres-azul mt-5 text-left">${x.ETIQUETA == null ? '' : x.ETIQUETA}</h3><div class="table-responsive tabla-principal"><table class="table table-sm table-hover m-0 get" id="${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}" data-comp="${x.ID_COMPONENTE}" data-eliminar="">${head}${body}</table></div><div class="btn-agregar"></div>`;
        }).join('');
        $("#table-add").html(`${contenido}`);

        j.map((x, i) => {
            if (x.INCREMENTABLE == '0')
                armarBodyEstatico(x.LIST_INDICADOR_BODY);
        });
        $("[data-toggle='tooltip']").tooltip();
        scrollButtons();
        scrollButtonsAgregar(id_criterio, id_caso);
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
        let tituloDoc = '<div class="col-lg-6 col-md-12 col-sm-12"><h3 class="estilo-02 text-sres-azul mb-5 text-left">DOCUMENTOS DE SUSTENTO</h3></div>';
        let tituloArchivosAdjuntos = '<div class="col-lg-6 col-md-12 col-sm-12 d-none d-lg-block"><h3 class="estilo-02 text-sres-azul mb-5 text-left">ARCHIVOS ADJUNTOS</h3></div>';
        let cabecera = `<div class="row">${tituloDoc}${tituloArchivosAdjuntos}</div>`;

        let contenido = data.map(x => {
            let fileDoc = `<div class="form-group text-left"><label class="estilo-01" for="fle-requisito-${x.ID_DOCUMENTO}">${x.NOMBRE}<span class="text-danger font-weight-bold">&nbsp;${x.OBLIGATORIO == '1' ? '(*)&nbsp;' : ''}<i class="fas fa-question-circle ayuda-tooltip" data-toggle="tooltip" data-placement="top" title="Seleccione un archivo para adjuntarlo en el registro de requisitos, se recomienda un archivo del tipo (PDF, DOC, JPG, PNG)"></i></span></label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-file"></i></span></div><input class="form-control form-control-sm cursor-pointer txt-file-control" type="text" id="txt-requisito-${x.ID_DOCUMENTO}" placeholder="Subir documentos" value="${x.OBJ_INSCDOC == null ? `` : x.OBJ_INSCDOC.ARCHIVO_BASE}" required><input class="d-none fil-file-control ${x.OBLIGATORIO == '1' ? 'obligatorio' : ''}" type="file" id="fle-requisito-${x.ID_DOCUMENTO}" data-id="${x.ID_DOCUMENTO}" accept="application/msword, application/vnd.ms-excel, text/plain, application/pdf, image/*"><div class="input-group-append"><label class="input-group-text cursor-pointer estilo-01" for="fle-requisito-${x.ID_DOCUMENTO}"><i class="fas fa-upload mr-1"></i>Subir archivo</label></div></div></div>`
            let colLeft = `<div class="col-lg-6 col-md-12 col-sm-12">${fileDoc}</div>`;
            let contenidoFileDoc = `<div class="alert alert-secondary p-1 d-flex w-100"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha subido el documento requerido</span></div></div>`;
            if (x.OBJ_INSCDOC != null) {
                let nombreFileDoc = `<i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${x.OBJ_INSCDOC.ARCHIVO_BASE}</span>`;
                let btnDescargaFileDoc = `<a class="text-sres-verde" href="${baseUrl}api/criterio/obtenerdocumento/${idConvocatoria}/${idCriterio}/${$(`#cbo-caso`).val()}/${idInscripcion}/${x.ID_DOCUMENTO}"><i class="fas fa-download px-2 py-1"></i></a>`;
                let btnEliminarFileDoc = `<a class="text-sres-verde btnEliminarFile" href="#" data-id="${x.ID_DOCUMENTO}"><i class="fas fa-trash px-2 py-1"></i></a>`;
                contenidoFileDoc = `<div class="alert alert-success p-1 d-flex w-100"><div class="mr-auto">${nombreFileDoc}</div><div class="ml-auto">${btnDescargaFileDoc}${btnEliminarFileDoc}</div></div>`;
            }
            //let colRight = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="form-group" id="viewContentFile-${x.ID_DOCUMENTO}"><label class="estilo-01">&nbsp;</label>${contenidoFileDoc}</div></div>`;
            let colRight = `<div class="col-lg-6 col-md-12 col-sm-12 d-flex align-items-end" id="viewContentFile-${x.ID_DOCUMENTO}"><label class="estilo-01">&nbsp;</label>${contenidoFileDoc}</div>`;
            let contenidoFinal = `<div class="row">${colLeft}${colRight}</div>`;
            return contenidoFinal;
        }).join('')
        $('#doc-add').html(`${cabecera}${contenido}`);
        data.forEach(x => {
            x.OBJ_INSCDOC == null ? '' : $(`input[type="file"][id*="fle-requisito-${x.ID_DOCUMENTO}"]`).data('file', x.OBJ_INSCDOC.ARCHIVO_CONTENIDO);
            //$(`input[type="file"][id*="fle-requisito-${x.ID_DOCUMENTO}"]`).data('type', x.OBJ_INSCDOC == null ? 'file' : x.OBJ_INSCDOC.ARCHIVO_TIPO);
        })
        $('input[type="file"][id*="fle-requisito-"]').on('change', fileDocChange);
        $(`[id*="viewContentFile-"] .btnEliminarFile`).on('click', btnEliminarFileClick);
    }
}

var fileDocChange = (e) => {
    let elFile = $(e.currentTarget);
    var fileContent = e.currentTarget.files[0];
    let verificar;

    switch (fileContent.name.substring(fileContent.name.lastIndexOf('.') + 1).toLowerCase()) {
        case 'pdf': case 'jpg': case 'jpeg': case 'png': case 'doc': case 'docx': case 'xls': case 'xlsx': case 'xlsm': break;
        default: $(elFile).parent().parent().parent().parent().alertWarning({ type: 'warning', title: 'ADVERTENCIA', message: `El archivo tiene una extensión no permitida` }); return false; break;
    }

    if (fileContent.size > maxBytes) { $(elFile).parent().parent().parent().parent().alertWarning({ type: 'warning', title: 'ADVERTENCIA', message: `El archivo debe tener un peso máximo de 4MB` }); return false; }
    else
        $(elFile).parent().parent().parent().parent().alert('remove');

    //if (!verificar) return false;

    if (e.currentTarget.files.length == 0) {
        $(e.currentTarget).removeData('file');
        $(e.currentTarget).removeData('fileContent');
        $(e.currentTarget).removeData('type');
        return;
    }

    //var fileContent = e.currentTarget.files[0];

    //switch (f.name.substring(fileContent.name.lastIndexOf('.') + 1).toLowerCase()) {
    //    case 'pdf': case 'jpg': case 'jpeg': case 'png': case 'doc': case 'docx': case 'xls': case 'xlsx': case 'xlsm': break;
    //    default: $(elFile).parent().parent().parent().parent().alert({ type: 'warning', title: 'ADVERTENCIA', message: `El archivo debe tener las extensiones indicadas` }); return false; break;
    //}

    //if (fileContent.size > maxBytes) { $(elFile).parent().parent().parent().parent().alert({ type: 'warning', title: 'ADVERTENCIA', message: `El archivo debe tener un peso máximo de 4MB` }); return;}
    //else
    //    $(elFile).parent().parent().parent().parent().alert('remove');


    var idElement = $(e.currentTarget).attr("data-id");
    $(`#txt-requisito-${idElement}`).val(fileContent.name);

    let reader = new FileReader();
    reader.onload = function (e) {
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
        elFile.data('fileContent', e.currentTarget.result);
        elFile.data('type', fileContent.type);
        let content = `<label class="estilo-01">&nbsp;</label><div class ="alert alert-success p-1 d-flex w-100"><div class ="mr-auto"><i class ="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${fileContent.name}</span></div><div class ="ml-auto"><a class ="text-sres-verde" href="${e.currentTarget.result}" download="${fileContent.name}"><i class ="fas fa-download px-2 py-1"></i></a><a class ="text-sres-verde btnEliminarFile" data-id="${idElement}" href="#"><i class ="fas fa-trash px-2 py-1"></i></a></div></div>`
        $(`#viewContentFile-${idElement}`).html(content);
        $(`#viewContentFile-${idElement} .btnEliminarFile`).on('click', btnEliminarFileClick);
    }
    reader.readAsDataURL(e.currentTarget.files[0]);
}

var btnEliminarFileClick = (e) => {
    e.preventDefault();
    let id = $(e.currentTarget).attr('data-id');
    $(`#txt-requisito-${id}, #fle-requisito-${id}`).each((i, x) => {
        $(x).val('');
        $(x).removeData('file');
        $(x).removeData('fileContent');
        $(x).removeData('type');
        console.log(x);
    });
    $(e.currentTarget).closest('.align-items-end').html(`<label class="estilo-01">&nbsp;</label><div class="alert alert-secondary p-1 d-flex w-100"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha subido el documento requerido</span></div></div>`);
}

var armarHead = (lista, incremental, id, componente) => {
    let cont = ``, contbau = 0, contini = 0, contresul = 0, indicador = 0;
    for (var i = 0; i < lista.length; i++) {
        if (indicador == 0) {
            if (lista[i]["OBJ_PARAMETRO"].VISIBLE == 1) contbau++;
            if (lista[i]["OBJ_PARAMETRO"].ID_PARAMETRO == 124) indicador = 124;
        } else if (indicador == 124) {
            if (lista[i]["OBJ_PARAMETRO"].VISIBLE == 1) contini++;
            if (lista[i]["OBJ_PARAMETRO"].ID_PARAMETRO == 133) indicador = 133;
        } else if (indicador == 133) {
            contresul++;
        }
        //cont += `<th scope="col"><div class="d-flex flex-column justify-content-start align-items-center"><span>${lista[i]["OBJ_PARAMETRO"].NOMBRE}</span>${lista[i]["OBJ_PARAMETRO"].UNIDAD == null ? `` : lista[i]["OBJ_PARAMETRO"].UNIDAD == '' ? `` : `<small>(${lista[i]["OBJ_PARAMETRO"].UNIDAD})</small>`}${lista[i]["OBJ_PARAMETRO"].DESCRIPCION == null ? `<i class="mt-2"></i>` : `<i class="fas fa-question-circle mt-2" data-toggle="tooltip" data-placement="bottom" title="${lista[i]["OBJ_PARAMETRO"].DESCRIPCION}"></i>`}</div></th>`;
        cont += `<th scope="col" ${lista[i]["OBJ_PARAMETRO"].VISIBLE == '0' ? `class="d-none"` : ''}><div class="d-flex flex-column justify-content-start align-items-center"><span>${lista[i]["OBJ_PARAMETRO"].NOMBRE}</span>${lista[i]["OBJ_PARAMETRO"].UNIDAD == null ? `` : lista[i]["OBJ_PARAMETRO"].UNIDAD == '' ? `` : `<small>(${lista[i]["OBJ_PARAMETRO"].UNIDAD})</small>`}<i class="fas fa-info-circle mt-2" data-toggle="tooltip" data-placement="right" title="${lista[i]["OBJ_PARAMETRO"].DESCRIPCION == null ? '' : lista[i]["OBJ_PARAMETRO"].DESCRIPCION}"></i></div></th>`;
    }
    let columnabau = `<th colspan="${contbau}"><div class="d-flex flex-column justify-content-center align-items-center"><span>LÍNEA BASE</span><small></small><i class="fas fa-info-circle mt-2" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Línea base"></i></div></th>`;
    let columnaini = `<th colspan="${contini}"><div class="d-flex flex-column justify-content-center align-items-center"><span>MEJORA</span><small></small><i class="fas fa-info-circle mt-2" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Después de la implementación de mejora"></i></div></th>`;
    let columnares = `<th colspan="${contresul}"><div class="d-flex flex-column justify-content-center align-items-center"><span>RESULTADOS</span><small></small><i class="fas fa-info-circle mt-2" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Resultados"></i></div></th>`;
    //cont += incremental == '1' ? `<th scope="col"><div class="d-flex flex-column justify-content-center align-items-center"><div class="btn btn-warning btn-sm estilo-01 addBtnGeneral" type="button" onclick="agregarFila(${id},${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div></div></th>` : ``;
    cont += incremental == '1' ? `<th scope="col"><div class="d-flex flex-column justify-content-center align-items-center addBtnGeneral"></div></th>` : ``;
    return `<thead class="estilo-06">${idCriterio == 1 && componente == 2 ? `<tr>${columnabau}${columnaini}${columnares}</tr>` : ''}<tr>${cont}</tr></thead>`;
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
                filas += `<td data-encabezado="${lista[i]["NOMBRE"]}" ${lista[i]["VISIBLE"] == '0' ? `class="d-none"` : ''}><div class="text-center estilo-01">${validarNull(lista[i]["VALOR"])}</div><input class="get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${validarNull(lista[i]["VALOR"])}" data-param="${lista[i]["ID_PARAMETRO"]}" type="hidden" /></td>`;
            else
                filas += `<td data-encabezado="${lista[i]["NOMBRE"]}" ${lista[i]["VISIBLE"] == '0' ? `class="d-none"` : ''}><div class="form-group m-0"><input class="form-control form-control-sm ${lista[i]["ID_TIPO_DATO"] == '3' ? 'estilo-06' : 'estilo-01'} ${lista[i]["ID_TIPO_DATO"] == '1' ? 'solo-numero text-right' : lista[i]["ID_TIPO_DATO"] == '3' ? 'text-right' : ''} ${lista[i]["DECIMAL_V"] == null ? '' : lista[i]["DECIMAL_V"] == '1' ? 'formato-decimal text-right' : 'formato-numero'} ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} ${lista[i]["RESULTADO"] == null ? `alert-warning` : lista[i]["RESULTADO"] == '0' ? `alert-warning` : ``} ${lista[i]["EMISIONES"] == null ? `` : lista[i]["EMISIONES"] == '1' ? `get-emisiones` : ``} ${lista[i]["AHORRO"] == null ? `` : lista[i]["AHORRO"] == '1' ? `get-ahorro` : ``} ${lista[i]["COMBUSTIBLE"] == null ? `` : lista[i]["COMBUSTIBLE"] == '1' ? `get-combustible` : ``} ${lista[i]["CAMBIO_MATRIZ"] == null ? `` : lista[i]["CAMBIO_MATRIZ"] == '1' ? `get-cambio-matriz` : ``} get-valor" type="${lista[i]["ID_TIPO_DATO"] == '1' ? 'text' : lista[i]["ID_TIPO_DATO"] == '3' ? 'date' : 'text'}" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${lista[i]["DECIMAL_V"] == null ? validarNull(lista[i]["VALOR"]) : lista[i]["DECIMAL_V"] == '1' ? formatoMiles(validarNull(lista[i]["VALOR"])) : validarNull(lista[i]["VALOR"])}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["RESULTADO"] == '1' ? `data-resultado="1"` : `` : ``} ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["OBTENIBLE"] == '1' ? `data-obtenible="1"` : `` : ``} maxlength="${lista[i]["TAMANO"]}" ${lista[i]["VERIFICABLE"] == '1' ? `onBlur="verificarValor(this)"` : ``}  ${lista[i]["EDITABLE"] == '0' ? `readonly` : ``} /></div></td>`;
        } else if (lista[i]["ID_TIPO_CONTROL"] == 1) {
            filas += `<td data-encabezado="${lista[i]["NOMBRE"]}" ${lista[i]["VISIBLE"] == '0' ? `class="d-none"` : ''}><div class="form-group m-0"><select class="form-control form-control-sm multi-opciones alert-warning ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["FILTRO"] == null ? `` : lista[i]["FILTRO"] == '' ? `` : `data-filtro="${lista[i]["FILTRO"]}" onchange="filtrar(this)"`}  ${lista[i]["VERIFICABLE"] == '1' ? `onchange="verificarValor(this)"` : ``}><option value="0">Seleccione</option>`;
            for (var j = 0; j < lista[i]["LIST_PARAMDET"].length; j++)
                filas += `<option value="${lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"]}" ${validarNull(lista[i]["VALOR"]) == lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"] ? `selected` : ``}>${lista[i]["LIST_PARAMDET"][j]["NOMBRE"]}</option>`;
            filas += `</select></div></td>`;
            //$(`#${id_criterio}-${id_caso}-${id_componente}-${incremental == '1' ? row : id_indicador}-${lista[i]["ID_PARAMETRO"]}`).val(validarNull(lista[i]["VALOR"]));
        } else if (lista[i]["ID_TIPO_CONTROL"] == 3) {
            filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><textarea class="form-control form-control-sm estilo-01 alert-warning get-valor" cols="30" rows="5" placeholder="Ingrese la descripción" maxlength="${lista[i]["TAMANO"]}" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" data-param="${lista[i]["ID_PARAMETRO"]}">${validarNull(lista[i]["VALOR"])}</textarea></div></td>`;
        }

    }
    filas += incremental == '1' ? `<td><div class="btn btn-info btn-sm estilo-01" type="button" onclick="eliminarFila(this);"><i class="fas fa-minus-circle mr-1"></i>&nbsp;Quitar&nbsp;&nbsp;</div></td>` : ``;
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
        lista[i]["ESTATICO"] == '1' ? '' : $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).val(lista[i]["DECIMAL_V"] == '1' ? formatoMiles(validarNull(lista[i]["VALOR"])) : validarNull(lista[i]["VALOR"]));
    }
}

var validarNull = (valor) => {
    if (valor == null) valor = '';
    return valor;
}

var eliminarFila = (e) => {
    let id = "#" + $(e).parent().parent().attr("id");
    let idTabla = "#" + $(e).parent().parent().parent().parent().attr("id");
    let indicador = $(e).parent().parent().data("ind");
    let validar = $(idTabla).find('tbody').find('tr').length > 1 ? $(id).remove() : 0;
    validar == 0 ? '' : indicador > 0 ? $(idTabla).data("eliminar", $(idTabla).data("eliminar") + indicador.toString() + ",") : '';
    validar == 0 ? '' : ordenarTabla(idTabla);
}

var ordenarTabla = (idTabla) => {
    $(idTabla).find('tbody').find('tr').each((x, y) => {
        let idT = $(y).attr('id').substring(0, $(y).attr('id').length - 1);
        $(idTabla).find('tbody').find('tr').eq(x).find('td').find('input').each((w, z) => {
            let idC = $(z).attr('id').split('-');
            $(idTabla).find('tbody').find('tr').eq(x).find("td").find('input').eq(w).removeAttr('id').attr({ 'id': idT + (x + 1) + '-' + idC[4] });
        });
        $(idTabla).find('tbody').find('tr').eq(x).find('td').find('select').each((w, z) => {
            let idC = $(z).attr('id').split('-');
            $(idTabla).find('tbody').find('tr').eq(x).find("td").find('select').eq(w).removeAttr('id').attr({ 'id': idT + (x + 1) + '-' + idC[4] });
        });
        $(idTabla).find('tbody').find('tr').eq(x).removeAttr('id').attr({ 'id': idT + (x + 1) });
    });
}

var agregarFila = (id, componente) => {
    let id_criterio = idCriterio;
    let id_caso = $(`#cbo-caso`).val();
    let id_componente = componente;
    let params = { id_criterio, id_caso, id_componente };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/criterio/filacriteriocaso?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = armarNuevaFila(j.LIST_INDICADOR_BODY, $("#" + id).find('tbody').find('tr').length + 1);
        $("#" + id).append(contenido);
    });

}

var armarNuevaFila = (lista, row) => {
     
    let fila = '';
    for (var i = 0; i < lista.length; i++) {
        fila += armarFila(lista[i]["LIST_INDICADORFORM"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], row, 0, '1', row);
    }
    return fila;
};

//=== GUARDAR

var guardar = () => {
    let idCriterio_ = idCriterio;
    let idCaso = $(`#cbo-caso`).val();
    let idInscripcion_ = idInscripcion;
    componente_ind = [];

    let url = `${baseUrl}api/criterio/guardarcriteriocaso`;

    $(".get").each((x, y) => {
        indicador_ind = [];
        let componente = $(y).data('comp');
        $(y).find('tbody').find('tr').each((x, y) => {
            indicador_data = [];
            let indicador = $(y).data('ind');
            $(y).find('.get-valor').each((x, y) => {
                var r = {
                    ID_CRITERIO: idCriterio_,
                    ID_CASO: idCaso,
                    ID_COMPONENTE: componente,
                    ID_PARAMETRO: $(y).data('param'),
                    ID_INSCRIPCION: idInscripcion_,
                    VALOR: $(y)[0].className.indexOf("multi-opciones") != -1 ? $(y).val() : $(y)[0].className.indexOf("solo-numero") != -1 && $(y)[0].className.indexOf("formato-decimal") != -1 ? $(y).val().replace(/,/gi, '') : $(y).val()
                };
                indicador_data.push(r);
            });
            let ind_r = { LIST_INDICADORDATA: indicador_data, ID_INDICADOR: indicador };
            indicador_ind.push(ind_r);
        });
        let ind = { LIST_INDICADOR: indicador_ind, ID_CRITERIO: idCriterio_, ID_CASO: idCaso, ID_COMPONENTE: componente, ID_INSCRIPCION: idInscripcion_, ELIMINAR_INDICADOR: $("#" + idCriterio_ + "-" + idCaso + "-" + componente).data("eliminar").substring(0, $("#" + idCriterio_ + "-" + idCaso + "-" + componente).data('eliminar').length - 1) };
        componente_ind.push(ind);
    });

    let listaInputFile = $('input[type="file"][id*="fle-requisito-"][class*="obligatorio"]');
    let listaDoc = Array.from(listaInputFile).filter(x => $(x).data('file') != null)

    if (listaDoc.length < listaInputFile.length) {
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Necesita completar todos los documentos obligatorios (*)' });
        return;
    }

    listaInputFile = $('input[type="file"][id*="fle-requisito-"]');
    listaDoc = Array.from(listaInputFile).filter(x => $(x).data('file') != null)

    listaDoc = listaDoc.map((x, i) => {
        let idDoc = $(x).attr('data-id');
        let name = $(`#txt-requisito-${idDoc}`).val();
        return {
            ID_CONVOCATORIA: idConvocatoria,
            ID_CRITERIO: idCriterio_,
            ID_CASO: idCaso,
            ID_DOCUMENTO: idDoc,
            ID_INSCRIPCION: idInscripcion_,
            ARCHIVO_BASE: name,
            ARCHIVO_TIPO: $(x).data('type'),
            ARCHIVO_CONTENIDO: $(x).data('file'),
            USUARIO_GUARDAR: idUsuarioLogin
        }
    });

    let emisiones = 0.0
    $(document).find('.get-emisiones').each((x, y) => {
        emisiones += $(y).val() == '' ? 0.0 : parseFloat($(y).val().replace(/,/gi, '')) < 0 ? 0.0 : parseFloat($(y).val().replace(/,/gi, ''));
    });

    let energia = 0.0
    $(document).find('.get-ahorro').each((x, y) => {
        energia += $(y).val() == '' ? 0.0 : parseFloat($(y).val().replace(/,/gi, '')) < 0 ? 0.0 : parseFloat($(y).val().replace(/,/gi, ''));
    });

    let combustible = 0.0;
    $(document).find('.get-combustible').each((x, y) => {
        combustible += $(y).val() == '' ? 0.0 : parseFloat($(y).val().replace(/,/gi, '')) < 0 ? 0.0 : parseFloat($(y).val().replace(/,/gi, ''));
    });

    let cambio_matriz = 0.0;
    $(document).find('.get-cambio-matriz').each((x, y) => {
        cambio_matriz += $(y).val() == '' ? 0.0 : parseFloat($(y).val().replace(/,/gi, '')) < 0 ? 0.0 : parseFloat($(y).val().replace(/,/gi, ''));
    });

    if (idCriterio == 1) {
        energia = energia / 1000;
        combustible = combustible / 1000;
        cambio_matriz = cambio_matriz / 1000;
    }

    combustible += cambio_matriz; //cambio de matriz es ahorro de combustible

    let data = { LIST_COMPONENTE: componente_ind, LIST_DOCUMENTO: listaDoc, ID_CONVOCATORIA: idConvocatoria, ID_CRITERIO: idCriterio_, ID_CASO: idCaso, ID_INSCRIPCION: idInscripcion_, NOMBRE_CRI: $('.nom-cri').val(), EMISIONES: emisiones, ENERGIA: energia, COMBUSTIBLE: combustible, CAMBIO_MATRIZ: cambio_matriz, ID_ETAPA: idEtapa, USUARIO_GUARDAR: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        j ? $('#btnGuardar').parent().parent().hide() : '';
        j ? $('.alert-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Felicidades por completar este criterio, ya puedes continuar con el siguiente.', close: { time: 4000 }, url: `${baseUrl}Convocatoria/${idConvocatoria}/Criterios` }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}

$(document).on("keydown", ".solo-numero", function (e) {
    var key = window.e ? e.which : e.keyCode;
    if ((key < 48 || key > 57) && (event.keyCode < 96 || event.keyCode > 105) && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46) return false;
});

$(document).on("keyup", ".formato-decimal", function (e) {
    $(e.target).val(function (index, value) {
        return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{2})$/, '$1.$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",");
    });
});

var verificarValor = (e) => {
    let verificar = 0;
    let obj = $(e).parent().parent().parent();
    $(e).parent().parent().parent().find('.verificar').each((x, y) => {
        verificar += $(y).val() == "" || $(y).val() == "0" ? 1 : 0;
    });
    verificar == 0 ? calcular(obj, $(e).parent().parent().parent().attr('id').split('-')[2], $(e).parent().parent().parent().attr('id').split('-')[3]) : '';
}

var calcular = (obj, id_componente, fila) => {
    let valores = [];
    obj.find('[data-param]').each((x, y) => {
        var v = {
            ID_CRITERIO: idCriterio,
            ID_CASO: $(`#cbo-caso`).val(),
            ID_COMPONENTE: id_componente,
            ID_PARAMETRO: $(y).attr('data-param'),
            RESULTADO: $(y).attr('data-resultado') == undefined ? 0 : 1,
            OBTENIBLE: $(y).attr('data-obtenible') == undefined ? 0 : 1,
            VALOR: $(y)[0].className.indexOf("multi-opciones") != -1 ? $(y).val() : $(y)[0].className.indexOf("solo-numero") != -1 && $(y)[0].className.indexOf("formato-decimal") != -1 ? $(y).val().replace(/,/gi, '') : $(y).val()
        };
        valores.push(v);
    });
    enviarValores(valores, fila);
}

var enviarValores = (lista, fila) => {
     
    let url = `${baseUrl}api/indicadordata/calcular`;
    let data = { LIST_INDICADORDATA: lista, USUARIO_GUARDAR: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
         
        j.map((x, i) => {
             
            y = $(`#${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}-${fila}-${x.ID_PARAMETRO}`);
            $(y)[0].className.indexOf("multi-opciones") != -1 ? $(y).val(x.VALOR) : $(y)[0].className.indexOf("solo-numero") != -1 && $(y)[0].className.indexOf("formato-decimal") != -1 ? $(y).val(formatoMiles(x.VALOR)) : $(y).val(x.VALOR);            
        });
        contabilizar();
        contabilizar2();
    });
}

var formatoMiles = (n) => {
    var m = n * 1;
    return m.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}

var valorInicial = (idSelect, arr) => {
    let filtro = [];
    let verificar = idSelect.attr('data-filtro') == undefined ? false : true;
    if (!verificar) {
        idSelect.val(0);
        return false;
    } else {
        filtro = idSelect.attr('data-filtro').split('|');
    }

    if (idSelect.val() == 0) {
        for (var i = 0; i < filtro.length; i++) {
            $(`#${arr}-${filtro[i]}`).val(0);
            valorInicial($(`#${arr}-${filtro[i]}`), arr);
        }
        return false;
    }
    return true;
}

var filtrar = (e) => {
    // 
    let arr = $(e).parent().parent().parent().attr('id'); //[0] ID_CRITERIO / [1] ID_cASO / [2] ID_COMPONENTE / [3] ID_INDICADOR
    let parametro = $(e).attr('data-param');

    if (!valorInicial($(e), arr)) return false;
    let filtro = $(e).attr('data-filtro').split('|');

    for (var i = 0; i < filtro.length; i++) {
        if ($(e).parent().parent().parent().find('td').find(`[data-param=${filtro[i]}]`).length > 0) {
            let arrFiltro = verificarFiltro(filtro[i], $(e).parent().parent().parent());
            if (arrFiltro.length > 0) {
                var lista = {
                    ID_PARAMETRO: parseInt(filtro[i]),
                    PARAMETROS: arrFiltro[0],
                    DETALLES: arrFiltro[1]
                }
                let url = `${baseUrl}api/parametrodetallerelacion/filtrar`;
                let data = { PARAMDETREL: lista, USUARIO_GUARDAR: idUsuarioLogin };

                let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
                let contenido = ``;
                fetch(url, init)
                .then(r => r.json())
                .then(j => {
                    if (j.length > 0) {
                        contenido = j.map((x, m) => {
                            let opciones = `<option value="${x.ID_DETALLE}" ${(j.length == 1 ? "selected" : "")}>${x.NOMBRE}</option>`;
                            //let opciones = `<option value="${x.ID_DETALLE}">${x.NOMBRE}</option>`;
                            return opciones;
                        }).join('');
                        $(`#${arr}-${j[0].ID_PARAMETRO}`).html(`<option value="0">Seleccione</option>${contenido}`);
                        let idparam = $(`#${$(e).attr('id')}`)[0].className.indexOf("verificar")
                        if (idparam != -1) verificarValor(e);
                    }
                });
            }
        }
    }
}

var verificarFiltro = (filtro, obj) => {
    let verificar = true;
    let parametros = "";
    let detalles = "";
    let arrFiltros = [];
    obj.find('td').find('[data-filtro]').each((x, y) => {
        let arr = $(y).attr('data-filtro').split('|');
        for (let i = 0; i < arr.length; i++)
            if (filtro == arr[i]) {
                if ($(y).val() > 0) {
                    parametros += $(y).attr('data-param') + '|';
                    detalles += $(y).val() + '|';
                    verificar = true;
                } else
                    verificar = false;
            }
        if (!verificar) return false;
    });
    if (verificar) {
        arrFiltros.push(parametros.substring(0, parametros.length - 1));
        arrFiltros.push(detalles.substring(0, detalles.length - 1));
    }
    return arrFiltros;
}

var contabilizar = () => {    
    if (idCriterio != 1) return;
    electricidadHead = 0; combustibleHead = 0, energiaelectrica = 0, energiatermica = 0, cambiomatriz = 0, emisiones = 0;
    $('#1-1-1').find('tbody').find('tr').each((x, y) => {        
        let v = $(y).find('[data-param=109]').val();
        if (v != null && v > 0) {
            if (v == "1") electricidadHead += $(y).find('[data-param=114]').val() == null ? 0 : $(y).find('[data-param=114]').val() == "" ? 0 : parseFloat($(y).find('[data-param=114]').val().replace(/,/gi, ''));
            else combustibleHead += $(y).find('[data-param=114]').val() == null ? 0 : $(y).find('[data-param=114]').val() == "" ? 0 : parseFloat($(y).find('[data-param=114]').val().replace(/,/gi, ''));
        }            
    });

    $('#1-1-2').find('tbody').find('tr').each((x, y) => {
        let m = $(y).find('[data-param=135]').val().replace(/,/gi, '');
        let n = $(y).find('[data-param=134]').val().replace(/,/gi, '');
        if (m < 0) $(y).find('[data-param=135]').val('0.00');
        if (n < 0) $(y).find('[data-param=134]').val('0.00');
        let v1 = $(y).find('[data-param=118]').val();
        let v2 = $(y).find('[data-param=127]').val();
        if (v1 != null && v1 > 0 && v2 != null && v2 > 0) {
            let v = evaluarEnergia(v1,v2);
            if (v == 1) energiaelectrica += $(y).find('[data-param=134]').val() == null ? 0 : $(y).find('[data-param=134]').val() == "" ? 0 : parseFloat($(y).find('[data-param=134]').val().replace(/,/gi, '')) < 0 ? 0 : parseFloat($(y).find('[data-param=134]').val().replace(/,/gi, ''));
            else if (v == 2) energiatermica += $(y).find('[data-param=134]').val() == null ? 0 : $(y).find('[data-param=134]').val() == "" ? 0 : parseFloat($(y).find('[data-param=134]').val().replace(/,/gi, '')) < 0 ? 0 : parseFloat($(y).find('[data-param=134]').val().replace(/,/gi, ''));
            else if (v == 3) cambiomatriz += $(y).find('[data-param=134]').val() == null ? 0 : $(y).find('[data-param=134]').val() == "" ? 0 : parseFloat($(y).find('[data-param=134]').val().replace(/,/gi, '')) < 0 ? 0 : parseFloat($(y).find('[data-param=134]').val().replace(/,/gi, ''));
            emisiones += $(y).find('[data-param=135]').val() == null ? 0 : $(y).find('[data-param=135]').val() == "" ? 0 : parseFloat($(y).find('[data-param=135]').val().replace(/,/gi, ''));
        }
    });
    energiaelectrica = energiaelectrica / 1000;
    energiatermica = energiatermica / 1000;
    cambiomatriz = cambiomatriz / 1000;
    console.log(`electrica: ${energiaelectrica}, termica: ${energiatermica}, matriz: ${cambiomatriz}, emisiones: ${emisiones}`);
}

var contabilizar2 = () => {
    if (idCriterio != 2) return;

    $('.tabla-principal').each((x, y) => {
        $(y).find('tbody').find('tr').each((m, n) => {
            let v1 = $(n).find('[data-param=51]').val().replace(/,/gi, '');
            let v2 = $(n).find('[data-param=54]').val().replace(/,/gi, '');
            let v3 = $(n).find('[data-param=55]').val().replace(/,/gi, '');
            if (v1 < 0) $(n).find('[data-param=51]').val('0.00');
            if (v2 < 0) $(n).find('[data-param=54]').val('0.00');
            if (v3 < 0) $(n).find('[data-param=55]').val('0.00');
        });        
    });
    //console.log(`electrica: ${energiaelectrica}, termica: ${energiatermica}, matriz: ${cambiomatriz}, emisiones: ${emisiones}`);
}

var evaluarEnergia = (e1, e2) => {
    let v = 0;
    if (e1 == "1" && e2 == "1") v = 1;
    else if (e1 > 1 && e2 > 1) v = 2;
    else if ((e1 == "1" && e2 > 1) || (e1 > 1 && e2 == "1")) v = 3;
    return v;
}

function scrollButtonsAgregar(criterio, caso) {
    var $wrapAll = $(".tabla-principal");

    $wrapAll.each(function () {
        $wrap = $(this);
        var $btn = $wrap.find(".addBtnGeneral");
        let componente = $wrap.find("table").data('comp');
        if ($btn.length > 0) {
            console.log("si");
            let elemento = $wrap.parent();
            if (elemento[0].className == "wrap-principal") {
                //$wrap.parent().append(`<div class="btn btn-warning btn-sm estilo-01 addBtnGnralLeft" type="button" onclick="agregarFila('${`${criterio}-${caso}-${componente}`}',${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>`);
                $wrap.parent().append(`<div class="btn btn-warning btn-sm estilo-01 addBtnGnralRight mr-1" type="button" onclick="agregarFila('${`${criterio}-${caso}-${componente}`}',${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>`);
            } else {
                $wrap.wrap("<div class='wrap-principal'></div>");
                //$wrap.parent().append(`<div class="btn btn-warning btn-sm estilo-01 addBtnGnralLeft" type="button" onclick="agregarFila('${`${criterio}-${caso}-${componente}`}',${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>`);
                $wrap.parent().append(`<div class="btn btn-warning btn-sm estilo-01 addBtnGnralRight mr-1" type="button" onclick="agregarFila('${`${criterio}-${caso}-${componente}`}',${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>`);
                //$wrap.next().append(`<div class="btn btn-warning btn-sm estilo-01" style="left: 0; position: absolute;" type="button" onclick="agregarFila('${`${criterio}-${caso}-${componente}`}',${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>`);
                //$wrap.next().append(`<div class="btn btn-warning btn-sm estilo-01" style="right: 0; position: absolute;" type="button" onclick="agregarFila('${`${criterio}-${caso}-${componente}`}',${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div>`);
            }
            
        } else {
            console.log("no");
        }
    });
}

$(document).on("keyup", ".formato-numero", function (event) {
    $(event.target).val(function (index, value) {
        return value.replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",");
    });
});

function formatoMilesSoloNumero(n) {
    var m = n * 1;
    return m.toFixed(0).replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",");
}
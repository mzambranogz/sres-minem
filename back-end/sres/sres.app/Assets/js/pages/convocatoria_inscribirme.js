var pageLoad = () => {
    cargarListaInscripcionRequerimiento();
    $('#btnInscribirme').on('click', btnInscribirmeClick);
}

var cargarListaInscripcionRequerimiento = () => {
    let url = `/api/inscripcionrequerimiento/listarinscripcionrequerimientoporconvocatoriainscripcion/${idConvocatoria}${idInscripcion == null ? `` : `/${idInscripcion}`}`

    fetch(url)
    .then(r => r.json())
    .then(mostrarListaInscripcionRequerimiento)
}

var mostrarListaInscripcionRequerimiento = (data) => {
    if (data.length > 0) {
        let tituloCriterios = '<div class="col-lg-6 col-md-12 col-sm-12"><h3 class="estilo-02 text-sres-azul mb-5">REQUISITOS GENERALES</h3></div>';
        let tituloArchivosAdjuntos = '<div class="col-lg-6 col-md-12 col-sm-12 d-none d-lg-block"><h3 class="estilo-02 text-sres-azul mb-5">ARCHIVOS ADJUNTOS</h3></div>';
        let cabecera = `<div class="row">${tituloCriterios}${tituloArchivosAdjuntos}</div>`;

        let contenido = data.map(x => {
            let fileRequerimiento = `<div class="form-group"><label class="estilo-01 text-limit-1" for="fle-requisito-${x.ID_REQUERIMIENTO}">${x.REQUERIMIENTO.NOMBRE}<span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;<i class="fas fa-question-circle ayuda-tooltip" data-toggle="tooltip" data-placement="top" title="Seleccione un archivo para adjuntarlo en el registro de requisitose, se recomienda un archivo del tipo (PDF, DOC, JPG, PNG)"></i></span></label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-file"></i></span></div><input class="form-control form-control-sm cursor-pointer txt-file-control" type="text" id="txt-requisito-${x.ID_REQUERIMIENTO}" placeholder="Subir documentos" value="${x.ARCHIVO_BASE || ''}" required><input class="d-none fil-file-control" type="file" id="fle-requisito-${x.ID_REQUERIMIENTO}" data-id="${x.ID_REQUERIMIENTO}" accept="application/msword, application/vnd.ms-excel, application/vnd.ms-powerpoint, text/plain, application/pdf, image/*"><div class="input-group-append"><label class="input-group-text cursor-pointer estilo-01" for="fle-requisito-${x.ID_REQUERIMIENTO}"><i class="fas fa-upload mr-1"></i>Subir archivo</label></div></div></div>`
            let colLeft = `<div class="col-lg-6 col-md-12 col-sm-12">${fileRequerimiento}</div>`;
            let existeFileRequerimientoSubido = x.ARCHIVO_BASE || '' != '';
            let contenidoFileRequerimiento = `<div class="alert alert-secondary p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha subido el documento requerido</span></div></div>`;
            if (existeFileRequerimientoSubido) {
                let nombreFileRequerimiento = `<i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${x.ARCHIVO_BASE}</span>`;
                let btnDescargaFileRequerimiento = `<a class="text-sres-verde" href="${baseUrl}api/inscripcionrequerimiento/obtenerarchivo/${idConvocatoria}/${idInscripcion}/${idInstitucionLogin}/${x.ID_REQUERIMIENTO}"><i class="fas fa-download px-2 py-1"></i></a>`;
                let btnEliminarFileRequerimiento = `<a class="text-sres-verde btnEliminarFile" href="#" data-id="${x.ID_REQUERIMIENTO}"><i class="fas fa-trash px-2 py-1"></i></a>`;
                contenidoFileRequerimiento = `<div class="alert alert-success p-1 d-flex"><div class="mr-auto">${nombreFileRequerimiento}</div><div class="ml-auto">${btnDescargaFileRequerimiento}${btnEliminarFileRequerimiento}</div></div>`;
            }
            let colRight = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="form-group" id="viewContentFile-${x.ID_REQUERIMIENTO}"><label class="estilo-01">&nbsp;</label>${contenidoFileRequerimiento}</div></div>`;
            let contenidoFinal = `<div class="row">${colLeft}${colRight}</div>`;
            return contenidoFinal;
        }).join('')
        $('#lstInscripcionRequerimiento').html(`${cabecera}${contenido}`);
        data.forEach(x => {
            $(`input[type="file"][id*="fle-requisito-${x.ID_REQUERIMIENTO}"]`).data('file', x.ARCHIVO_CONTENIDO);
            $(`input[type="file"][id*="fle-requisito-${x.ID_REQUERIMIENTO}"]`).data('type', x.ARCHIVO_TIPO);

        })
        //$('input[type="file"].fil-file-control').on('change', fileRequerimientoChange);
        $('input[type="file"][id*="fle-requisito-"]').on('change', fileRequerimientoChange);
        $(`[id*="viewContentFile-"] .btnEliminarFile`).on('click', btnEliminarFileClick);
    }
}

var btnInscribirmeClick = (e) => {
    e.preventDefault();
    enviarInscripcion();
}

var limpiarFiles = () => {
    let listaInputFile = $('input[type="file"][id*="fle-requisito-"], input[type="text"][id*="txt-requisito-"]');
    //let listaInputFile = $('input[type="file"]');
    listaInputFile.each(x => $(x).val(''));
}

var enviarInscripcion = () => {
    let norma1 = $('#chk-norma-01').prop('checked');
    let norma2 = $('#chk-norma-02').prop('checked');

    let mensaje = [];

    if (!norma1) mensaje.push('norma 1');
    if (!norma2) mensaje.push('norma 2');

    if (mensaje.length > 0) {
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'ERROR', message: `Debe marcar ${mensaje.join(' y ')}` });
        return;
    }

    let listaInputFile = $('input[type="file"][id*="fle-requisito-"]');
    //debugger;
    let listaInscripcionRequerimiento = Array.from(listaInputFile).filter(x => $(x).data('file') != null)

    if (listaInscripcionRequerimiento.length < listaInputFile.length) {
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'danger', title: 'ERROR', message: 'Necesita completas todos los requerimientos' });
        return;
    }

    listaInscripcionRequerimiento = listaInscripcionRequerimiento.map((x, i) => {
        let idRequerimiento = $(x).attr('data-id');
        let name = $(`#txt-requisito-${idRequerimiento}`).val();
        return {
            ID_CONVOCATORIA:  idConvocatoria,
            ID_INSCRIPCION: idInscripcion == null ? -1 : idInscripcion,
            ID_REQUERIMIENTO: idRequerimiento,
            ARCHIVO_BASE: name,
            //ARCHIVO_BASE: x.files[0] == null ? null : x.files[0].name,
            ARCHIVO_TIPO: $(x).data('type'),
            //ARCHIVO_TIPO: x.files[0] == null ? null : x.files[0].type,
            ARCHIVO_CONTENIDO: $(x).data('file'),
            UPD_USUARIO: idUsuarioLogin
        }
    });

    let data = {
        ID_INSCRIPCION: idInscripcion == null ? -1 : idInscripcion,
        ID_CONVOCATORIA: idConvocatoria,
        ID_INSTITUCION: idInstitucionLogin,
        LISTA_INSCRIPCION_REQUERIMIENTO: listaInscripcionRequerimiento,
        UPD_USUARIO: idUsuarioLogin
    }

    //console.log(formData);
    
    let url = `/api/inscripcion/guardarinscripcion`;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    //let init = { method: 'POST', body: formData };

    fetch(url, init)
    .then(r => r.json())
    .then(mostrarMensaje)
};

var mostrarMensaje = (data) => {
    if (data.success == true) {
        idInscripcion = data.id;
        $('#viewInscripcionRequerimiento > .row:last').alert({ type: 'success', title: 'BIEN HECHO', message: `¡Se guardó correctamente!`, close: { time: 3000 } });

        //alert('¡Se guardó correctamente!');
        limpiarFiles();
        cargarListaInscripcionRequerimiento();
    }
}

var fileRequerimientoChange = (e) => {
    let elFile = $(e.currentTarget);

    if (e.currentTarget.files.length == 0) {
        $(e.currentTarget).removeData('file');
        $(e.currentTarget).removeData('fileContent');
        $(e.currentTarget).removeData('type');
        return;
    }

    var fileContent = e.currentTarget.files[0];

    if (fileContent.size > maxBytes) $(elFile).parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `El archivo debe tener un peso máximo de 4MB` });
    else
        $(elFile).parent().parent().parent().parent().alert('remove');


    var idElement = $(e.currentTarget).attr("data-id");
    $(`#txt-requisito-${idElement}`).val(fileContent.name);

    let reader = new FileReader();
    reader.onload = function (e) {
        //console.log(e.currentTarget.result)
        //debugger;
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
        elFile.data('fileContent', e.currentTarget.result);
        elFile.data('type', fileContent.type);
        //console.log(fileContent);
        let content = `<label class="estilo-01">&nbsp;</label><div class ="alert alert-success p-1 d-flex"><div class ="mr-auto"><i class ="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${fileContent.name}</span></div><div class ="ml-auto"><a class ="text-sres-verde" href="${e.currentTarget.result}" download="${fileContent.name}"><i class ="fas fa-download px-2 py-1"></i></a><a class ="text-sres-verde btnEliminarFile" data-id="${idElement}" href="#"><i class ="fas fa-trash px-2 py-1"></i></a></div></div>`
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
    $(e.currentTarget).closest('.form-group').html(`<label class="estilo-01">&nbsp;</label><div class="alert alert-secondary p-1 d-flex"><div class="mr-lg-auto"><i class="fas fa-exclamation-circle px-2 py-1"></i><span class="estilo-01">Aún no ha subido el documento requerido</span></div></div>`);
}

$(document).ready(pageLoad);
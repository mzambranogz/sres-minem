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
        let tituloCriterios = '<div class="col-lg-6 col-md-12 col-sm-12"><h3 class="estilo-02 text-sres-azul mb-5">CRITERIOS GENERALES</h3></div>';
        let tituloArchivosAdjuntos = '<div class="col-lg-6 col-md-12 col-sm-12 d-none d-lg-block"><h3 class="estilo-02 text-sres-azul mb-5">ARCHIVOS ADJUNTOS</h3></div>';
        let cabecera = `<div class="row">${tituloCriterios}${tituloArchivosAdjuntos}</div>`;

        let contenido = data.map(x => {
            let fileRequerimiento = `<div class="form-group"><label class="estilo-01 text-limit-1" for="fle-requisito-${x.ID_REQUERIMIENTO}">${x.REQUERIMIENTO.NOMBRE}<span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;<i class="fas fa-question-circle ayuda-tooltip" data-toggle="tooltip" data-placement="top" title="Seleccione un archivo para adjuntarlo en el registro de requisitose, se recomienda un archivo del tipo (PDF, DOC, JPG, PNG)"></i></span></label><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-file"></i></span></div><input class="form-control form-control-sm cursor-pointer txt-file-control" type="text" id="txt-requisito-${x.ID_REQUERIMIENTO}" placeholder="Subir documentos" required><input class="d-none fil-file-control" type="file" id="fle-requisito-${x.ID_REQUERIMIENTO}" accept="application/msword, application/vnd.ms-excel, application/vnd.ms-powerpoint, text/plain, application/pdf, image/*"><div class="input-group-append"><label class="input-group-text cursor-pointer estilo-01" for="fle-requisito-${x.ID_REQUERIMIENTO}"><i class="fas fa-upload mr-1"></i>Subir archivo</label></div></div></div>`
            let colLeft = `<div class="col-lg-6 col-md-12 col-sm-12">${fileRequerimiento}</div>`;
            let existeFileRequerimientoSubido = x.ARCHIVO_BASE || '' != '';
            let contenidoFileRequerimiento = ``
            if (existeFileRequerimientoSubido) {
                let nombreFileRequerimiento = `<i class="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${x.ARCHIVO_BASE}</span>`;
                let btnDescargaFileRequerimiento = `<a class="text-sres-verde" href="${baseUrl}api/inscripcionrequerimiento/obtenerarchivo/${idConvocatoria}/${idInscripcion}/${idInstitucionLogin}/${x.ID_REQUERIMIENTO}"><i class="fas fa-download px-2 py-1"></i></a>`;
                let btnEliminarFileRequerimiento = `<a class="text-sres-verde" href="#"><i class="fas fa-trash px-2 py-1"></i></a>`;
                contenidoFileRequerimiento = `<div class="form-group"><label class="estilo-01">&nbsp;</label><div class="alert alert-success p-1 d-flex"><div class="mr-auto">${nombreFileRequerimiento}</div><div class="ml-auto">${btnDescargaFileRequerimiento}${btnEliminarFileRequerimiento}</div></div></div>`;
            }
            let colRight = `<div class="col-lg-6 col-md-12 col-sm-12">${contenidoFileRequerimiento}</div>`;
            let contenidoFinal = `<div class="row">${colLeft}${colRight}</div>`;
            return contenidoFinal;
        }).join('')
        $('#lstInscripcionRequerimiento').html(`${cabecera}${contenido}`);
        $('input[type="file"][id*="txt-requisito-"]').on('change', fileRequerimientoChange);
    }
}

var btnInscribirmeClick = (e) => {
    e.preventDefault();
    enviarInscripcion();
}

var limpiarFiles = () => {
    let listaInputFile = $('input[type="file"][id*="txt-requisito-"]');
    listaInputFile.each(x => $(x).val(''));
}

var enviarInscripcion = () => {
    let listaInputFile = $('input[type="file"][id*="txt-requisito-"]');
    let listaInscripcionRequerimiento = Array.from(listaInputFile).filter(x => x.files[0] != null)

    if (listaInscripcionRequerimiento.length < listaInputFile.length) return;

    //let formData = new FormData();
    //formData.append('ID_INSCRIPCION', idInscripcion == null ? -1 : idInscripcion);
    //formData.append('ID_CONVOCATORIA', idConvocatoria);
    //formData.append('ID_INSTITUCION', idInstitucionLogin);
    //formData.append('UPD_USUARIO', idUsuarioLogin);


    //listaInscripcionRequerimiento.forEach((x, i) => {
    //    formData.append(`LISTA_INSCRIPCION_REQUERIMIENTO[${i}].ID_CONVOCATORIA`, idConvocatoria);
    //    formData.append(`LISTA_INSCRIPCION_REQUERIMIENTO[${i}].ID_INSCRIPCION`, idInscripcion == null ? -1 : idInscripcion);
    //    formData.append(`LISTA_INSCRIPCION_REQUERIMIENTO[${i}].ID_REQUERIMIENTO`, $(x).attr('data-id'));
    //    formData.append(`LISTA_INSCRIPCION_REQUERIMIENTO[${i}].ARCHIVO_BASE`, x.files[0] == null ? null : x.files[0].name);
    //    formData.append(`LISTA_INSCRIPCION_REQUERIMIENTO[${i}].FILE`, x.files[0] == null ? null : x.files[0], x.files[0] == null ? null : x.files[0].name);
    //    formData.append(`LISTA_INSCRIPCION_REQUERIMIENTO[${i}].UPD_USUARIO`, idUsuarioLogin);
    //});

    listaInscripcionRequerimiento = listaInscripcionRequerimiento.map((x, i) => {
        return {
            ID_CONVOCATORIA:  idConvocatoria,
            ID_INSCRIPCION: idInscripcion == null ? -1 : idInscripcion,
            ID_REQUERIMIENTO: $(x).attr('data-id'),
            ARCHIVO_BASE: x.files[0] == null ? null : x.files[0].name,
            ARCHIVO_TIPO: x.files[0] == null ? null : x.files[0].type,
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
    let init = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    //let init = { method: 'POST', body: formData };

    fetch(url, init)
    .then(r => r.json())
    .then(mostrarMensaje)
};

var mostrarMensaje = (data) => {
    if (data == true) {
        alert('¡Se guardó correctamente!');
        limpiarFiles();
        cargarListaInscripcionRequerimiento();
    }
}

var fileRequerimientoChange = (e) => {
    let elFile = $(e.currentTarget);
    if (e.currentTarget.files.length == 0) {
        $(e.currentTarget).removeData('file');
        return;
    }

    let reader = new FileReader();
    reader.onload = function (e) {
        //console.log(e.currentTarget.result)
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
    }
    reader.readAsDataURL(e.currentTarget.files[0]);
}

$(document).ready(pageLoad);
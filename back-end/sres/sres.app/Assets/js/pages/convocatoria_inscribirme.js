var pageLoad = () => {
    $('#btnInscribirme').on('click', btnInscribirmeClick);
    $('input[type="file"][id*="fileRequerimiento_"]').on('change', fileRequerimientoChange);
}

var btnInscribirmeClick = (e) => {
    e.preventDefault();
    enviarInscripcion();
}

var enviarInscripcion = () => {
    let listaInputFile = $('input[type="file"][id*="fileRequerimiento_"]');
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
    debugger;
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
        location.reload();
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
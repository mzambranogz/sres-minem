$(document).ready((e) => {
    $('#fle-logo').on('change', logoChange);
    $('#btnGuardarLogo').on('click', btnGuardarLogoClick);
    $('#btnMostrarDatosInstitucion').on('click', btnMostrarDatosInstitucionClick);
    $('#btnActualizarDatosInstitucion').on('click', btnActualizarDatosInstitucionClick);
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnFirstPagination').on('click', btnFirstPaginationClick);
    $('#btnPreviousPagination').on('click', btnPreviousPaginationClick);
    $('#btnNextPagination').on('click', btnNextPaginationClick);
    $('#btnLastPagination').on('click', btnLastPaginationClick);
    $('#cbo-subsector-tipoemp').on('change', subsectortipoempresaChange);
    $(`#cbo-trabajador-cama`).on('change', trabajadorcamaChange);
    $(`#txt-numero`).on('blur', cantidadChange);
    $('#cbo-departamento').on('change', departamentoChange);
    $('#cbo-provincia').on('change', provinciaChange);
    listaSubsector();
    listaDepartamento();
    cambiarPrimerInicio();    
});

var consultar = () => {
    let nroInforme = $('#txt-expediente').val();
    let nombre = $('#txt-descripcion').val();
    let fechaDesde = $('#dat-desde').val();
    let fechaHasta = $('#dat-hasta').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();;
    let columna = 'id_convocatoria';    
    let orden = 'asc';    
    let idInstitucion = idRolLogin == 3 ? idInstitucionLogin : 0;
    let idUsuario = idRolLogin == 2 ? idUsuarioLogin : 0;
    let params = { nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden, idInstitucion, idUsuario };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/convocatoria/buscarconvocatoria?${queryParams}`;

    fetch(url).then(r => r.json()).then(cargarDataBusqueda);
};

var cargarDataBusqueda = (data) => {
    let tabla = $('#tblConvocatoria');
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
            let fechaActual = new Date();
            let fechaInicio = new Date(x.FECHA_INICIO);
            let fechaFin = new Date(x.FECHA_FIN);
            let diasPlazo = Math.floor((fechaFin - fechaInicio) / (1000 * 60 * 60 * 24));
            let diasTranscurridos = Math.floor((fechaActual - fechaInicio) / (1000 * 60 * 60 * 24));
            //let porcentajeAvance = Math.floor(fechaInicio > fechaActual ? 0.00 : fechaActual > fechaFin ? 100 : (diasTranscurridos / diasPlazo * 100))
            let porcentajeAvance = x.ID_ETAPA > 14 ? 100 : Math.round((x.ID_ETAPA - 1) / 13 * 100);
            let formatoCodigo = '00000000';

            let colNro = `<td class="text-center" data-encabezado="Número" scope="row">${x.ROWNUMBER}</td>`
            let colNroInforme = `<td class="text-center" data-encabezado="Número expediente" scope="row">${(`${formatoCodigo}${x.ID_CONVOCATORIA}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</td>`;
            let colPeriodo = `<td class="text-center" data-encabezado="Período">${fechaInicio.getFullYear()}</td>`;
            let colNombre = `<td data-encabezado="Progreso"><div class="text-limi-1">${x.NOMBRE}</div></td>`;
            let colFechaInicio = `<td class="text-center" data-encabezado="Fecha Inicio">${fechaInicio.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colFechaFin = `<td class="text-center" data-encabezado="Fecha Fin">${fechaFin.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colVencimiento = `<td class="text-center" data-encabezado="Vencimiento"><div class="progress" style="height: 21px; ${porcentajeAvance > 0 ? "background-color: #E2DBDA;" : ""}" data-toggle="tooltip" data-placement="top" title="Porcentaje de avance"><div class="progress-bar ${porcentajeAvance > 0 ? "vigente" : "preparado"} estilo-01" role="progressbar" style="width: ${porcentajeAvance}%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">${porcentajeAvance}%</div></div></td>`;
            let colEstado = `<td data-encabezado="Estado"><b class="text-sres-verde">${x.ETAPA.NOMBRE}</b></td>`;
            let btnDetalles = `<a class="btn btn-sm btn-success w-100" href="javascript:void(0)">Detalles</a>`;
            let btnPostulacion = `<a class="btn btn-sm btn-success w-100" href="javascript:void(0)" onclick="verificarDatosInternos(${x.ID_CONVOCATORIA})" >Ingresar</a>`;
            //let btnPostulacion = `<a class="btn btn-sm btn-success w-100" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme">Ingresar</a>`;
            //EVALUADOR-ADMIN
            let btnVerEvaluar = `<a class="btn btn-sm btn-success w-100" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/BandejaParticipantes">${x.ID_ETAPA >= 14 ? `Ver` : idRolLogin == 1 ? `Ver` : `Evaluar`}</a>`;
            //POSTULANTE
            let btnGestionar = `<a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a>`;
            let btnEditarReq = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme"><i class="fas fa-edit mr-1"></i>Editar requisitos</a>`;
            let btnIngresarEditarCriterios = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Criterios"><i class="fas fa-edit mr-1"></i>${x.ID_ETAPA == 6 ? `Ingresar` : `Editar`} criterios</a>`

            let btnSeguimiento = `<a class="dropdown-item estilo-01" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Seguimiento/${0}"><i class="fas fa-history mr-1"></i>Seguimiento</a>`;
            let btnVerReconocimiento = `<a class="dropdown-item estilo-01" href="#"><i class="fas fa-medal mr-1"></i>Ver reconocimiento</a>`;

            let OpcioneEta1 = `${btnDetalles}`;
            let OpcioneEta2 = `${btnPostulacion}`;
            let OpcioneEta3 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta4 = `<div class="dropdown-menu">${btnEditarReq}${btnSeguimiento}</div>`;
            let OpcioneEta5 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta6 = `<div class="dropdown-menu">${btnIngresarEditarCriterios}${btnSeguimiento}</div>`;
            let OpcioneEta7 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta8 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta9 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta10 = `<div class="dropdown-menu">${btnIngresarEditarCriterios}${btnSeguimiento}<div>`;
            let OpcioneEta11 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta12 = `<div class="dropdown-menu">${btnSeguimiento}</div>`;
            let OpcioneEta13 = `<div class="dropdown-menu">${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcioneEta14 = `<div class="dropdown-menu">${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcioneEta15 = `<div class="dropdown-menu">${btnSeguimiento}${btnVerReconocimiento}</div>`;
            let OpcioneEta16 = `<div class="dropdown-menu">${btnSeguimiento}${btnVerReconocimiento}</div>`;
            
            let colOpcionesEvaAdmin = `<td class="text-center" data-encabezado="Gestión">${x.ID_ETAPA == 1 || x.ID_ETAPA == 2 ? OpcioneEta1 : x.ID_ETAPA < 14 ? idRolLogin == 2 ? x.VALIDAR_EVALUADOR == 0 ? OpcioneEta1 : btnVerEvaluar : btnVerEvaluar : btnVerEvaluar}</td>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión">${x.ID_ETAPA == 1 ? OpcioneEta1 : ''}${x.ID_ETAPA == 2 ? OpcioneEta2 : ''}<div class="btn-group w-100 ${x.ID_ETAPA > 2 ? '' : 'd-none'}">${btnGestionar}${x.ID_ETAPA == 3 ? OpcioneEta3 : ''}${x.ID_ETAPA == 4 ? OpcioneEta4 : ''}${x.ID_ETAPA == 5 ? OpcioneEta5 : ''}${x.ID_ETAPA == 6 ? OpcioneEta6 : ''}${x.ID_ETAPA == 7 ? OpcioneEta7 : ''}${x.ID_ETAPA == 8 ? OpcioneEta8 : ''}${x.ID_ETAPA == 9 ? OpcioneEta9 : ''}${x.ID_ETAPA == 10 ? OpcioneEta10 : ''}${x.ID_ETAPA == 11 ? OpcioneEta11 : ''}${x.ID_ETAPA == 12 ? OpcioneEta12 : ''}${x.ID_ETAPA == 13 ? OpcioneEta13 : ''}${x.ID_ETAPA == 14 ? OpcioneEta14 : ''}${x.ID_ETAPA == 15 ? OpcioneEta15 : ''}${x.ID_ETAPA == 16 ? OpcioneEta16 : ''}</div></td>`;
            let colAnulado = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100">${btnGestionar}${OpcioneEta12}</div></td>`
            let fila = `<tr ${x.FLAG_ANULAR == 1 ? 'style="background-color: #FED0C6";' : ''}>${colNro}${colNroInforme}${colPeriodo}${colNombre}${colFechaInicio}${colFechaFin}${colVencimiento}${colEstado}${idRol == 3 ? x.FLAG_ANULAR == 1 ? colAnulado : colOpciones : colOpcionesEvaAdmin}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var verificarDatosInternos = (idConvocatoria) => {
    if (idSubsectortipoemp > 0)
        location.href = `${baseUrl}Convocatoria/${idConvocatoria}/Inscribirme`;
    else
        $("#modal-edit-descripcion").modal("show");
}

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

var btnGuardarLogoClick = (e) => {
    e.preventDefault();
    if ($('#fle-logo').data('fileContent') == null) {
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `Debe seleccionar una imagen` });
        return;
    }

    let data = { ID_INSTITUCION: idInstitucionLogin, LOGO_CONTENIDO: $('#fle-logo').data('file'), LOGO: $('#fle-logo').data('fileName'), LOGO_TIPO: $('#fle-logo').data('type'), UPD_USUARIO: idUsuarioLogin };

    let url = `${baseUrl}api/institucion/modificarlogoinstitucion`;
    let init = { method: 'put', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) }

    fetch(url, init)
    .then(r => r.json())
    .then(responseGuardarLogo)
}

var responseGuardarLogo = (data) => {
    if (data == true) {
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'success', title: '¡BIEN HECHO!', message: `Se actualizó el logo correctamente`, close: { time: 3000 } });
        $('#btnGuardarLogo').hide();
        
        let url = `${baseUrl}Login/RefrescarDatosSession`;
        fetch(url).then(r => r.json()).then(j => console.log(j));

    } else {
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `No se pudo actualizar el logo` });
        let srcDefault = $('#fle-logo').parent().parent().find('.img-fluid').attr('data-src-default');
        $('#fle-logo').parent().parent().find('.img-fluid').attr('src', srcDefault);
    }
}

var logoChange = (e) => {
    let elFile = $(e.currentTarget);

    if (e.currentTarget.files.length == 0) {
        let srcDefault = $(e.currentTarget).parent().parent().find('.img-fluid').attr('data-src-default');
        $(e.currentTarget).parent().parent().find('.img-fluid').attr('src', srcDefault);
        $('#btnGuardarLogo').hide();
        $(e.currentTarget).removeData('file');
        $(e.currentTarget).removeData('fileName');
        $(e.currentTarget).removeData('fileContent');
        $(e.currentTarget).removeData('type');
        return;
    }

    var fileContent = e.currentTarget.files[0];

    if (fileContent.size > maxBytes) $(elFile).parent().parent().parent().parent().parent().parent().alert({ type: 'danger', title: 'ERROR', message: `El archivo debe tener un peso máximo de 4MB` });
    else $(elFile).parent().parent().parent().parent().parent().parent().alert('remove');


    var idElement = $(e.currentTarget).attr("data-id");

    let reader = new FileReader();
    reader.onload = function (e) {
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
        elFile.data('fileContent', e.currentTarget.result);
        elFile.data('fileName', fileContent.name);
        elFile.data('type', fileContent.type);
    }
    reader.readAsDataURL(e.currentTarget.files[0]);
}

var btnMostrarDatosInstitucionClick = (e) => {
    e.preventDefault();

    let url = `${baseUrl}api/institucion/obtenerinstitucion?idInstitucion=${idInstitucionLogin}`;

    fetch(url)
    .then(r => r.json())
    .then(responseMostrarDatosInstitucion);
}

var responseMostrarDatosInstitucion = (data) => {
    $('#txt-nombre-corto').val(data.NOMBRE_COMERCIAL);
    $('#txa-descripcion').val(data.DESCRIPCION);
    vidProvincia = idProvincia;
    vidDistrito = idDistrito;
    $('#txt-tipo-contribuyente').val(data.CONTRIBUYENTE);
    $('#cbo-ciiu').val(data.ID_ACTIVIDAD);
    vidTrabajadorCama = idTrabajadorCama;
    vcantidad = cantidad;
    $('#txt-total-mujeres').val(data.CANTIDAD_MUJERES);
    listaSubsector();
    listaDepartamento();
    debugger;
    if (data.LISTA_CONTACTO.length > 0) { let i = 0;
        data.LISTA_CONTACTO.map(x => { i++;
            $(`#txt-nombre-0${i}`).val(x.NOMBRE);
            $(`#txt-cargo-0${i}`).val(x.CARGO);
            $(`#txt-telefono-0${i}`).val(x.TELEFONO);
            $(`#txt-email-0${i}`).val(x.CORREO);
        });
    }
    $('#modal-edit-descripcion').modal('show');
}

var btnActualizarDatosInstitucionClick = (e) => {
    e.preventDefault();
    let contacto = [];
    let arr = [];   

    if ($('#txt-nombre-corto').val().trim() === "") arr.push("Ingrese el nombre corto");
    if ($('#txa-descripcion').val().trim() === "") arr.push("Ingrese la descripción");
    if ($('#cbo-departamento').val() == 0) arr.push("Seleccione un departamento");
    if ($('#cbo-provincia').val() == 0) arr.push("Seleccione una provincia");
    if ($('#cbo-distrito').val() == 0) arr.push("Seleccione un distrito");
    if ($('#txt-tipo-contribuyente').val().trim() === "") arr.push("Ingrese el tipo de contribuyente");
    if ($('#cbo-ciiu').val() == 0) arr.push("Seleccione la actividad económica");
    if ($(`#cbo-subsector-tipoemp`).val() == 0) arr.push(`${idSector == 1 ? "Seleccione el subsector" : "Seleccione el tipo de empresa"}`);
    if ($(`#cbo-trabajador-cama`).val() == 0) arr.push(`${idSector == 1 ? "Seleccione el número de empleados/camas:" : "Seleccione el número de empleados:"}`);
    if ($(`#txt-numero`).val() == "") arr.push("Ingrese la cantidad de empleados/camas");
    if ($(`#txt-total-mujeres`).val() == "") arr.push("Ingrese la cantidad de mujeres");
    if ($(`#txt-nombre-01`).val() == "" || $(`#txt-cargo-01`).val() == "" || $(`#txt-telefono-01`).val() == "" || $(`#txt-email-01`).val() == "") arr.push("Ingrese los datos del directivo o representante legal");
    if ($(`#txt-nombre-02`).val() == "" || $(`#txt-cargo-02`).val() == "" || $(`#txt-telefono-02`).val() == "" || $(`#txt-email-02`).val() == "") arr.push("Ingrese los datos del responsable técnico");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let departamento = $('#cbo-departamento').val();
    let provincia = $('#cbo-provincia').val();
    let distrito = $('#cbo-distrito').val();
    let contribuyente = $('#txt-tipo-contribuyente').val();
    let ciiu = $('#cbo-ciiu').val();
    let nombreComercial = $('#txt-nombre-corto').val();
    let descripcion = $('#txa-descripcion').val();
    let subsectortipoempresa = $(`#cbo-subsector-tipoemp`).val();
    let trabajadorcama = $(`#cbo-trabajador-cama`).val();
    let cantidad = $(`#txt-numero`).val();
    let cantidadmujeres = $(`#txt-total-mujeres`).val();

    for (var i = 0; i < 4 ; i++) {
        var r = {
            ID_INSTITUCION: idInstitucionLogin,
            ID_CONTACTO: (i+1),
            NOMBRE: $(`#txt-nombre-0${i+1}`).val(),
            CARGO: $(`#txt-cargo-0${i+1}`).val(),
            TELEFONO: $(`#txt-telefono-0${i+1}`).val(),
            CORREO: $(`#txt-email-0${i + 1}`).val(),
            USUARIO_GUARDAR: idUsuarioLogin
        }
        contacto.push(r);
    }

    let data = { ID_INSTITUCION: idInstitucionLogin, NOMBRE_COMERCIAL: nombreComercial, DESCRIPCION: descripcion, ID_DEPARTAMENTO: departamento, ID_PROVINCIA: provincia, ID_DISTRITO: distrito, CONTRIBUYENTE:  contribuyente, ID_ACTIVIDAD: ciiu, ID_SUBSECTOR_TIPOEMPRESA: subsectortipoempresa, ID_TRABAJADORES_CAMA: trabajadorcama, CANTIDAD: cantidad, CANTIDAD_MUJERES: cantidadmujeres, LISTA_CONTACTO: contacto, UPD_USUARIO: idUsuarioLogin };

    let url = `${baseUrl}api/institucion/modificardatosinstitucion`;
    let init = { method: 'put', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(responseActualizarDatosInstitucion);
}

var responseActualizarDatosInstitucion = (data) => {
    $('.alert-add').html('');
    if (data == true) {
        idDepartamento = $(`#cbo-departamento`).val().toString();
        idProvincia = $(`#cbo-provincia`).val().toString();
        idDistrito = $(`#cbo-distrito`).val().toString();
        idSubsectortipoemp = $(`#cbo-subsector-tipoemp`).val();
        idTrabajadorCama = $(`#cbo-trabajador-cama`).val();
        cantidad = $(`#txt-numero`).val();
        $('#lblDescripcionInstitucion').text($('#txa-descripcion').val());
        $('#txt-nombre-corto').val('');
        $('#txa-descripcion').val('');
        $('#modal-edit-descripcion').modal('hide');
        $('#fle-logo').parent().parent().parent().parent().parent().parent().alert({ type: 'success', title: '¡BIEN HECHO!', message: `Se actualizó los datos de la institución correctamente`, close: { time: 3000 } });
        let url = `${baseUrl}Login/RefrescarDatosSession`;
        fetch(url).then(r => r.json()).then(j => console.log(j));
    } else {
        if ($('#modal-edit-descripcion .modal-body >*:last').next().hasClass('alert')) $('#modal-edit-descripcion .modal-body >*:last').next().remove();
        $('#modal-edit-descripcion .modal-body >*:last').alert({ type: 'danger', title: 'ERROR', message: `No se pudo actualizar los datos de la institución` });
    }
}

var listaSubsector = () => {
    if (idRol != 3) return false;
    let url = `${baseUrl}api/subsectortipoempresa/listasubsetortipoempresa?idSector=${idSector}`;
    fetch(url)
    .then(r => r.json())
    .then(armarCombosubsectortipoempresa);
}

var armarCombosubsectortipoempresa = (data) => {
    let combo = data.map((x, y) => {
        return `<option value="${x.ID_SUBSECTOR_TIPOEMPRESA}">${x.NOMBRE}</option>`
    }).join('');
    $(`#cbo-subsector-tipoemp`).html(`<option value="0">${idSector == 1 ? "-seleccione subsector-" : "-seleccione tipo empresa-"}</option>${combo}`);
    if (idSubsectortipoemp > 0) { $(`#cbo-subsector-tipoemp`).val(idSubsectortipoemp); subsectortipoempresaChange(); }
}

var subsectortipoempresaChange = () => {
    $(`#txt-numero`).val('');
    if ($(`#cbo-subsector-tipoemp`).val() == 0) { $(`#cbo-trabajador-cama`).html(`<option value="0">-Seleccione-</option>`); return };
    let url = `${baseUrl}api/trabajadorcama/listatrabajadorcama?idSubsectorTipoempresa=${$(`#cbo-subsector-tipoemp`).val()}`;
    fetch(url)
    .then(r => r.json())
    .then(armarCombotrabajadorcama);
}

var armarCombotrabajadorcama = (data) => {
    let combo = data.map((x, y) => {
        return `<option value="${x.ID_TRABAJADORES_CAMA}" ${x.MAYOR_SIGNO == '1' ? `data-max="${x.MAYOR_VALOR}"` : ``} ${x.MENOR_SIGNO == '1' ? `data-min="${x.MENOR_VALOR}"` : ``}>${x.NOMBRE}</option>`
    }).join('');
    $(`#cbo-trabajador-cama`).html(data.length == 1 ? combo : `<option value="0">-Seleccione-</option>${combo}`);
    if (vidTrabajadorCama > 0) { $(`#cbo-trabajador-cama`).val(vidTrabajadorCama); vidTrabajadorCama = 0; trabajadorcamaChange(); vcantidad == 0 ? $(`#txt-numero`).val('') : $(`#txt-numero`).val(vcantidad); vcantidad = 0; }
    else { trabajadorcamaChange(); }
}

var trabajadorcamaChange = () => {
    if ($(`#cbo-trabajador-cama`).val() == 0) { $(`#txt-numero`).val(''); return };
    let option = $(`#cbo-trabajador-cama`).find(`option[value='${$(`#cbo-trabajador-cama`).val()}']`);
    let mayor = option.data('max');
    let menor = option.data('min');
    mayor != undefined ? $(`#txt-numero`).removeAttr('max').attr('max', mayor) : $(`#txt-numero`).removeAttr('max');
    menor != undefined ? $(`#txt-numero`).removeAttr('min').attr('min', menor) : $(`#txt-numero`).removeAttr('min');
}

var cantidadChange = () => {
    debugger;
    let mayor = parseFloat($(`#txt-numero`).attr('max'));
    let menor = parseFloat($(`#txt-numero`).attr('min'));
    let num = parseFloat($(`#txt-numero`).val());
    if (mayor != undefined && menor != undefined) { if (num < menor || num > mayor) $(`#txt-numero`).val(menor); }
    else if (mayor != undefined){ if (num > mayor) $(`#txt-numero`).val(mayor);}
    else if (menor != undefined) { if (num < menor) $(`#txt-numero`).val(menor); }
}

var cambiarPrimerInicio = () => {
    debugger;
    if (idRol != 3) return;
    if (primerinicio != 0) return;
    let url = `${baseUrl}api/institucion/cambiarprimerinicio?idInstitucion=${idInstitucionLogin}`;
    fetch(url)
    .then(r => r.json())
    .then(x => {
        debugger;
        if (x) {
            console.log('cambiado');
            let url = `${baseUrl}Login/RefrescarDatosSession`;
            fetch(url).then(r => r.json()).then(j => console.log(j));
        }
    });
}

var listaDepartamento = () => {
    if (idRol != 3) return false;
    let url = `${baseUrl}api/institucion/listadepartamento`;
    fetch(url)
    .then(r => r.json())
    .then(armarDepartamento);
}

var armarDepartamento = (data) => {
    let combo = data.map((x, y) => {
        return `<option value="${x.ID_DEPARTAMENTO}">${x.NOMBRE}</option>`
    }).join('');
    $(`#cbo-departamento`).html(`<option value="0">-Seleccionar-</option>${combo}`);
    if (idDepartamento > 0) { $(`#cbo-departamento`).val(idDepartamento); departamentoChange(); }
}

var departamentoChange = () => {
    if ($(`#cbo-departamento`).val() == 0) { $(`#cbo-provincia`).html(`<option value="0">-Seleccione-</option>`); return };
    let url = `${baseUrl}api/institucion/listaprovincia?idDepartamento=${$(`#cbo-departamento`).val()}`;
    fetch(url)
    .then(r => r.json())
    .then(armarProvincia);
}

var armarProvincia = (data) => {
    let combo = data.map((x, y) => {
        return `<option value="${x.ID_PROVINCIA}">${x.NOMBRE}</option>`
    }).join('');
    $(`#cbo-provincia`).html(`<option value="0">-Seleccionar-</option>${combo}`);
    if (vidProvincia > 0) { $(`#cbo-provincia`).val(vidProvincia); provinciaChange(); vidProvincia = 0;}
}

var provinciaChange = () => {
    if ($(`#cbo-provincia`).val() == 0) { $(`#cbo-distrito`).html(`<option value="0">-Seleccione-</option>`); return };
    let url = `${baseUrl}api/institucion/listadistrito?idProvincia=${$(`#cbo-provincia`).val()}`;
    fetch(url)
    .then(r => r.json())
    .then(armarDistrito);
}

var armarDistrito = (data) => {
    let combo = data.map((x, y) => {
        return `<option value="${x.ID_DISTRITO}">${x.NOMBRE}</option>`
    }).join('');
    $(`#cbo-distrito`).html(`<option value="0">-Seleccionar-</option>${combo}`);
    if (vidDistrito > 0) { $(`#cbo-distrito`).val(vidDistrito); vidDistrito = 0;}
}
$(document).ready((e) => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
});

var consultar = () => {
    let nroInforme = $('#txtNroInformeBusqueda').val();
    let nombre = $('#txtNombreBusqueda').val();
    let fechaDesde = $('#txtFechaDesdeBusqueda').val();
    let fechaHasta = $('#txtFechaHastaBusqueda').val();
    let registros = 10;
    let pagina = 1;
    let columna = 'id_convocatoria';
    let orden = 'asc'
    let params = { nroInforme, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/convocatoria/buscarconvocatoria?${queryParams}`;

    fetch(url).then(r => r.json()).then(cargarDataBusqueda);
};

var cargarDataBusqueda = (data) => {
    let tabla = $('#tblConvocatoria');
    let registros = 10;
    let pagina = 1;
    let cantidadCeldasCabecera = tabla.find('thead tr th').length;
    let contenido = renderizar(data, cantidadCeldasCabecera, pagina, registros);
    tabla.find('tbody').html(contenido);
    tabla.find('.btnParticipar').each(x => {
        let elementButton = tabla.find('.btnParticipar')[x];
        $(elementButton).on('click', btnParticiparClick);
    });
}

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let fechaActual = new Date();
            let fechaInicio = new Date(x.FECHA_INICIO);
            let fechaFin = new Date(x.FECHA_FIN);
            let diasPlazo = Math.floor((fechaFin - fechaInicio) / (1000 * 60 * 60 * 24));
            let diasTranscurridos = Math.floor((fechaActual - fechaInicio) / (1000 * 60 * 60 * 24));
            let porcentajeAvance = (fechaInicio > fechaActual ? 0.00 : (diasTranscurridos / diasPlazo * 100)).toFixed(2);

            let colNroInforme = `<td>${x.NRO_INFORME || ''}</td>`;
            let colPeriodo = `<td>${fechaInicio.getFullYear() == fechaFin.getFullYear() ? fechaInicio.getFullYear() : fechaInicio.getFullYear() + "-" + fechaFin.getFullYear()}</td>`;
            let colNombre = `<td>${x.NOMBRE}</td>`;
            let colFechaInicio = `<td>${fechaInicio.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colFechaFin = `<td>${fechaFin.toLocaleDateString("es-PE", { day: '2-digit', month: '2-digit', year: 'numeric' })}</td>`;
            let colVencimiento = `<td>${porcentajeAvance}%</td>`;
            let colEstado = `<td>${obtenerEstadoConvocatoria(x.FLAG_ESTADO)}</td>`;
            let btnIngresar = `<a target="_blank" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Inscribirme" class="btn btn-xs">INGRESAR</a>`;
            let colOpciones = `<td>${btnIngresar}</td>`;
            let fila = `<tr>${colNroInforme}${colPeriodo}${colNombre}${colFechaInicio}${colFechaFin}${colVencimiento}${colEstado}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var obtenerEstadoConvocatoria = (flagEstado) => {
    let estado = '';

    switch(flagEstado){
        case 'A' : estado = 'ABIERTO'; break;
        case 'E' : estado = 'EN PROCESO'; break;
        case 'R' : estado = 'EN REVISIÓN'; break;
        case 'P' : estado = 'PENDIENTE'; break;
        case 'C' : estado = 'CERRADO'; break;
        case 'V' : estado = 'VENCIDO'; break;
    }

    return estado;
};
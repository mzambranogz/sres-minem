$(document).ready(() => {
    consultarReconocimiento();
});

var consultarReconocimiento = () => {
    if (idInstitucion == 0) return;
    let url = `${baseUrl}api/premiacion/listareconocimiento?idInstitucion=${idInstitucion}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarreconocimiento(j);
    });
}

var cargarreconocimiento = (data) => {
    if (data == null) return;
    if (data.length > 0) {
        let contenido = data.map((x, y) => {
            let formatoCodigo = '00';
            let reconocimiento_5 = reconocimientomejora(x);
            let reconocimiento_4 = reconocimientoemisiones(x);
            let reconocimiento_3 = reconocimientomedida(x);
            let reconocimiento_1_2 = reconocimientocategoriaestrella(x);
            let contenidorecono = `<div class="bg-sres-claro px-2 py-3 mb-5 text-sres-claro collapse show" id="reconocimiento-${(`${formatoCodigo}${y + 1}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}"><p class="text-justify estilo-01 text-sres-gris">${x.DESCRIPCION}</p>${reconocimiento_1_2}${reconocimiento_3}${reconocimiento_4}${reconocimiento_5}</div>`;
            let colTitulo = `<div class="col-12 mb-3"><h2 class="p-2 estilo-04 text-white bg-degradado wow fadeIn wow slideInUp cursor-pointer" data-toggle="collapse" href="#reconocimiento-${(`${formatoCodigo}${y + 1}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}" aria-expanded="true" aria-controls="reconocimiento-01"><i class="fas fa-medal mr-1"></i>CONVOCATORIA PROGRAMADA PARA ${x.MES_CONVOCATORIA} ${x.ANIO_CONVOCATORIA}</h2>${contenidorecono}</div>`;
            let rowinicial = `<div class="row">${colTitulo}</div>`;
            return rowinicial;
        });
        $('#reconocimientos-institucion').html(contenido);
        $("[data-toggle='tooltip']").tooltip();
    }
}

var reconocimientocategoriaestrella = (obj) => {    
    let body = `<tbody class="estilo-01"><tr><td data-encabezado="Puntaje"><div class="text-center">${obj.PUNTAJE} ptos.</div></td><td data-encabezado="Reducción"><div class="text-right">${formatoMiles(obj.EMISIONES)}</div></td></tr></tbody>`;
    let columnaPuntaje = `<div class="d-flex flex-column justify-content-between align-items-center"><div class="d-flex justify-content-between align-items-center"><div class="pl-1 text-center w-100">PUNTAJE&nbsp;</div></div><div class="d-flex justify-content-center align-items-center"><i class="fas fa-info-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Puntuación obtenida"></i></div></div>`;
    let columnaReduccion = `<div class="d-flex flex-column justify-content-between align-items-center"><div class="d-flex justify-content-between align-items-center"><div class="pl-1 text-center w-100">REDUCCIÓN&nbsp;tCO<sub>2</sub></div></div><div class="d-flex justify-content-center align-items-center"><i class="fas fa-info-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Reducción lograda"></i></div></div>`;
    let head = `<thead class="estilo-06 free-with"><tr><th scope="col" width="50%">${columnaPuntaje}</th><th scope="col" width="50%">${columnaReduccion}</th></tr></thead>`;
    let contentseccion = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="table-responsive tabla-principal"><table class="table table-sm table-hover mb-0">${head}${body}</table></div></div>`;
    let imagen = `<div class="offset-lg-2 col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center"><img class="img-fluid" src="${baseUrl}${$('#ruta').val().replace('{0}', obj.ID_PREMIACION)}/${obj.ARCHIVO_BASE == null ? '' : obj.ARCHIVO_BASE}" alt="" data-toggle="tooltip" data-placement="top" title="${obj.INSIGNIA.ID_INSIGNIA == 1 ? 'Sin categoría' : obj.ESTRELLA_E.ID_ESTRELLA == 1 ? `Reconocimiento ${obj.INSIGNIA.NOMBRE.toLowerCase()} sin estrellas` : `Reconocimiento ${obj.INSIGNIA.NOMBRE.toLowerCase()} con ${obj.ESTRELLA_E.NOMBRE}`}"></div>`;
    let contenttabla = `<div class="row">${imagen}${contentseccion}</div>`;
    let titulo2 = `<h3 class="mb-4 estilo-04 text-sres-azul d-inline">NIVELES DE RECONOCIMIENTO POR REDUCCIÓN DE EMISIONES GEI: ${obj.ESTRELLA_E.NOMBRE}<a class="p-2 estilo-01 text-sres-verde text-right" href="${baseUrl}Participantes/Categoria#categoria-02" target="_blank">Leer más...</a></h3>`;
    let titulo1 = `<h3 class="mb-4 estilo-04 text-sres-azul d-inline">CATEGORÍA DE RECONOCIMIENTO DE ENERGÍA EFECIENTE Y SOSTENIBLE: ${obj.INSIGNIA.NOMBRE}<a class="p-2 estilo-01 text-sres-verde text-right scroll-to-section" href="${baseUrl}Participantes/Categoria#categoria-01" target="_blank">Leer más...</a></h3>`;
    let div1 = `<div class="offset-lg-2 col-lg-8 col-md-12 col-sm-12">${titulo1}<div class="rayado wow fadeIn mb-3"></div>${titulo2}<div class="rayado wow fadeIn mb-3"></div>${contenttabla}</div>`;
    let content = `<div class="row my-5">${div1}</div>`;
    return content;
}

var reconocimientomedida = (obj) => {
    let contenido = "";
    if (obj.LISTA_REC_MEDMIT != null) {
        if (obj.LISTA_REC_MEDMIT.length > 0) {
            let tablas = obj.LISTA_REC_MEDMIT.map((x, y) => {
                let body = `<tbody class="estilo-01"><tr><td data-encabezado="Reducción"><div class="text-right">${formatoMiles(x.REDUCIDO)}</div></td></tr></tbody>`;
                let columnaReduccion = `<div class="d-flex flex-column justify-content-between align-items-center"><div class="d-flex justify-content-between align-items-center"><div class="pl-1 text-center w-100">REDUCCIÓN&nbsp;tCO<sub>2</sub></div></div><div class="d-flex justify-content-center align-items-center"><i class="fas fa-info-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Reducción lograda"></i></div></div>`;
                let head = `<thead class="estilo-06 free-with"><tr><th scope="col" width="50%">${columnaReduccion}</th></tr></thead>`;
                let contentseccion = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="table-responsive tabla-principal"><table class="table table-sm table-hover mb-0">${head}${body}</table></div></div>`;
                let imagen = `<div class="offset-lg-2 col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center"><img class="img-fluid" src="${baseUrl}${$('#ruta_m').val().replace('{0}', x.ID_MEDMIT)}/${x.ARCHIVO_BASE == null ? '' : x.ARCHIVO_BASE}" alt="" data-toggle="tooltip" data-placement="top" title="${x.NOMBRE_MEDMIT}"></div>`;
                let contenttabla = `<div class="row mb-3">${imagen}${contentseccion}</div>`;
                return contenttabla;
            }).join('');

            let titulo1 = `<h3 class="mb-4 estilo-04 text-sres-azul d-inline">PREMIACIÓN POR CADA MEDIDA QUE APORTA A LAS CONTRIBUCIONES NACIONALES DETERMINADAS (NDC)<a class="p-2 estilo-01 text-sres-verde text-right scroll-to-section" href="${baseUrl}Participantes/Reconocimientos-y-Criterio#categoria-03" target="_blank">Leer más...</a></h3>`;
            let div1 = `<div class="offset-lg-2 col-lg-8 col-md-12 col-sm-12">${titulo1}<div class="rayado wow fadeIn mb-3"></div>${tablas}</div>`;
            let content = `<div class="row my-5">${div1}</div>`;
            contenido = content;
        }
    }
    return contenido;
}

var reconocimientoemisiones = (obj) => {
    if (obj.FLAG_EMISIONESMAX == null) return '';
    let contenido = "";
    if (obj.FLAG_EMISIONESMAX == '1') {
        let body = `<tbody class="estilo-01"><tr></td><td data-encabezado="Reducción"><div class="text-right">${formatoMiles(obj.EMISIONES)}</div></td></tr></tbody>`;
        let columnaReduccion = `<div class="d-flex flex-column justify-content-between align-items-center"><div class="d-flex justify-content-between align-items-center"><div class="pl-1 text-center w-100">REDUCCIÓN&nbsp;tCO<sub>2</sub></div></div><div class="d-flex justify-content-center align-items-center"><i class="fas fa-info-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Reducción lograda"></i></div></div>`;
        let head = `<thead class="estilo-06 free-with"><tr><th scope="col" width="50%">${columnaReduccion}</th></tr></thead>`;
        let contentseccion = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="table-responsive tabla-principal"><table class="table table-sm table-hover mb-0">${head}${body}</table></div></div>`;
        //let imagen = `<div class="offset-lg-2 col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center"><img class="img-fluid" src="${baseUrl}${$('#ruta').val().replace('{0}', obj.ID_PREMIACION)}/${obj.ARCHIVO_BASE == null ? '' : obj.ARCHIVO_BASE}" alt="" data-toggle="tooltip" data-placement="top" title="${obj.INSIGNIA.ID_INSIGNIA == 1 ? 'Sin categoría' : obj.ESTRELLA_E.ID_ESTRELLA == 1 ? `Reconocimiento de ${obj.INSIGNIA.NOMBRE.toLowerCase()} sin estrellas` : `Reconocimiento de ${obj.INSIGNIA.NOMBRE.toLowerCase()} con ${obj.ESTRELLA_E.NOMBRE}`}"></div>`;
        let imagen = `<div class="offset-lg-2 col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center"><img class="img-fluid" src="${baseUrl}Assets/images/gei.png" alt="" data-toggle="tooltip" data-placement="top" title="Premiación especial"></div>`;
        let contenttabla = `<div class="row">${imagen}${contentseccion}</div>`;
        let titulo1 = `<h3 class="mb-4 estilo-04 text-sres-azul d-inline">PREMIACIÓN DESTACADA POR REDUCCIÓN DE EMISIONES GEI (TCO2)<a class="p-2 estilo-01 text-sres-verde text-right scroll-to-section" href="${baseUrl}Participantes/Reconocimientos-y-Criterio#categoria-04" target="_blank">Leer más...</a></h3>`;
        let div1 = `<div class="offset-lg-2 col-lg-8 col-md-12 col-sm-12">${titulo1}<div class="rayado wow fadeIn mb-3"></div>${contenttabla}</div>`;
        let content = `<div class="row my-5">${div1}</div>`;
        contenido = content;
    }
    return contenido;
}

var reconocimientomejora = (obj) => {
    if (obj.FLAG_MEJORACONTINUA == null) return '';
    let contenido = "";
    if (obj.FLAG_MEJORACONTINUA == '1') {
        let body = `<tbody class="estilo-01"><tr><td data-encabezado="Puntaje"><div class="text-center">${obj.PUNTAJE} ptos.</div></td><td data-encabezado="Reducción"><div class="text-right">${formatoMiles(obj.EMISIONES)}</div></td></tr></tbody>`;
        let columnaReduccion = `<div class="d-flex flex-column justify-content-between align-items-center"><div class="d-flex justify-content-between align-items-center"><div class="pl-1 text-center w-100">REDUCCIÓN&nbsp;tCO<sub>2</sub></div></div><div class="d-flex justify-content-center align-items-center"><i class="fas fa-info-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Reducción lograda"></i></div></div>`;
        let head = `<thead class="estilo-06 free-with"><tr><th scope="col" width="50%">${columnaReduccion}</th></tr></thead>`;
        let contentseccion = `<div class="col-lg-6 col-md-12 col-sm-12"><div class="table-responsive tabla-principal"><table class="table table-sm table-hover mb-0">${head}${body}</table></div></div>`;
        //let imagen = `<div class="offset-lg-2 col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center"><img class="img-fluid" src="${baseUrl}${$('#ruta').val().replace('{0}', obj.ID_PREMIACION)}/${obj.ARCHIVO_BASE == null ? '' : obj.ARCHIVO_BASE}" alt="" data-toggle="tooltip" data-placement="top" title="${obj.INSIGNIA.ID_INSIGNIA == 1 ? 'Sin categoría' : obj.ESTRELLA_E.ID_ESTRELLA == 1 ? `Reconocimiento de ${obj.INSIGNIA.NOMBRE.toLowerCase()} sin estrellas` : `Reconocimiento de ${obj.INSIGNIA.NOMBRE.toLowerCase()} con ${obj.ESTRELLA_E.NOMBRE}`}"></div>`;
        let imagen = `<div class="offset-lg-2 col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center"><img class="img-fluid" src="${baseUrl}Assets/images/gei.png" alt="" data-toggle="tooltip" data-placement="top" title="Premiación especial"></div>`;
        let contenttabla = `<div class="row">${imagen}${contentseccion}</div>`;
        let titulo1 = `<h3 class="mb-4 estilo-04 text-sres-azul d-inline">PREMIACIÓN DE MEJORA CONTINUA DE ENERGÍA EFICIENTE Y SOSTENIBLE<a class="p-2 estilo-01 text-sres-verde text-right scroll-to-section" href="${baseUrl}Participantes/Reconocimientos-y-Criterio#categoria-05" target="_blank">Leer más...</a></h3>`;
        let div1 = `<div class="offset-lg-2 col-lg-8 col-md-12 col-sm-12">${titulo1}<div class="rayado wow fadeIn mb-3"></div>${contenttabla}</div>`;
        let content = `<div class="row my-5">${div1}</div>`;
        contenido = content;
    }
    return contenido;
}

function formatoMiles(n) { //add20
    return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}
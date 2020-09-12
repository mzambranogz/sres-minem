$(document).ready(() => {
    consultarListas();
});

var consultarListas = () => {

    let urlConvocatoriaCri = `${baseUrl}api/convocatoria/listarconvocatoriacridet?id=${id}`;
    let urlConvocatoriaEva = `${baseUrl}api/convocatoria/listarconvocatoriaeva?id=${id}`;
    let urlConvocatoriaEta = `${baseUrl}api/convocatoria/listarconvocatoriaeta?id=${id}`;
    let urlConvocatoriaInsig = `${baseUrl}api/convocatoria/listarconvocatoriainsig?id=${id}`;
    Promise.all([
            fetch(urlConvocatoriaCri),
            fetch(urlConvocatoriaEva),
            fetch(urlConvocatoriaEta),
            fetch(urlConvocatoriaInsig),
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([jCri, jEva, jEta, jInsig]) => {
        //armarEvaluadores(jEva, '#tbl-evaluador');
        armarEtapas(jEta, '#tbl-etapa');
        armarCriterios(jCri, '#criterios-det');
        armarInsignias(jInsig, '#tbl-insignia');
        $("[data-toggle='tooltip']").tooltip();
    });
}

var armarEvaluadores = (data, selector) => {
    let items = data.length == 0 ? '' : data.map((x,y) => `<tr><td class="text-center" data-encabezado="Número ${y+1}">${y+1}</td><td data-encabezado="Nombres y Apellidos">${x.NOMBRE}</td><td data-encabezado="Cargo">${x.ROL}</td></tr>`).join('');
    $(selector).find('tbody').html(items);
}

var armarEtapas = (data, selector) => {
    let items = data.length == 0 ? '' : data.map((x,y) => x.ID_ETAPA > 14 ? '' : `<tr><td class="text-center" data-encabezado="Orden ${y+1}">${y+1}</td><td data-encabezado="Etapa">${x.NOMBRE_ETAPA}</td><td data-encabezado="Proceso">${x.PROCESO}</td><td class="text-center" data-encabezado="Fecha"><i class="fas fa-calendar mr-1"></i>${x.FECHA_ETAPA_DET}</td></tr>`).join('');
    $(selector).find('tbody').html(items);
}

var armarCriterios = (data, selector) => {
    let contenido = "";
    if (data.length > 0){
        contenido = data.map((x,y) => {
            let contentpuntaje = armarPuntaje(x.LISTA_CONVCRIPUNT);
            let fila = `<tr><td data-encabezado="Descripción">${x.DESCRIPCION == null ? '' : x.DESCRIPCION}</td><td class="text-center" data-encabezado="Puntaje"><table class="table table-sm table-hover mb-0"><tbody class="estilo-01">${contentpuntaje}</tbody></table></td></tr>`;
            let body = `<tbody class="estilo-01">${fila}</tbody>`;
            let head = `<thead class="estilo-06 free-with"><tr><th scope="col" width="60%"><div class="d-flex justify-content-between align-items-center"><span class="d-flex justify-content-between align-items-center"><i class="fas fa-question-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Descripción del criterio"></i><span>DESCRIPCIÓN&nbsp;</span></span></div></th><th scope="col" width="40%"><div class="d-flex justify-content-between align-items-center"><span class="d-flex justify-content-between align-items-center"><i class="fas fa-question-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Puntajes del criterio"></i><span>PUNTAJE&nbsp;</span></span></div></th></tr></thead>`;
            let contentCriterio = `<div class="col-lg-10 col-md-12 col-sm-12"><div class="table-responsive tabla-principal"><table class="table table-sm table-hover mb-0">${head}${body}</table></div></div>`;
            let imagen = `<div class="col-lg-2 col-md-12 col-sm-12 d-flex justify-content-center align-items-center" data-count="count-0"><img class="img-fluid" src="${baseUrl}${$('#ruta').val().replace('{0}', x.ID_CRITERIO)}/${x.ARCHIVO_BASE == null ? '' : x.ARCHIVO_BASE}" alt="" data-toggle="tooltip" data-placement="top" title="Criterio 01"></div>`;
            let content = `</div><div class="row mb-1">${imagen}${contentCriterio}</div>`;
            return content;
        }).join('');
        $(selector).html(`<h3 class="mb-4 estilo-04 text-sres-azul d-inline">CARACTERÍSTICAS DE LOS CRITERIOS</h3><div class="rayado wow fadeIn mb-3">${contenido}`);
    }
}

var armarPuntaje = (data, selector) => {
    let items = data.length == 0 ? '' : data.map((x,y) => `<tr><td class="text-left">${x.DESCRIPCION}</td><td class="text-center">${x.PUNTAJE}</td></tr>`).join('');
    return items;
}

var armarInsignias = (data, selector) => {
    let head = data.length == 0 ? '' : data.map((x,y) => `<th scope="col" width="${100/data.length}%"><div class="d-flex justify-content-between align-items-center"><span class="d-flex justify-content-between align-items-center"><i class="fas fa-question-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Categoría"></i><span>${x.ID_INSIGNIA == 1 ? 'SIN CATEGORÍA' : x.NOMBRE_INSIGNIA.toUpperCase()}&nbsp;</span></span></div></th>`).join('');
    let body = data.length == 0 ? '' : data.map((x,y) => `<td data-encabezado="${x.ID_INSIGNIA == 1 ? 'sin categoría' : x.NOMBRE_INSIGNIA.toLowerCase()}"><div class="text-center w-100"><img class="img-fluid" src="${baseUrl}${$('#ruta_insig').val().replace('{0}', x.ID_INSIGNIA)}/${x.ARCHIVO_BASE == null ? '' : x.ARCHIVO_BASE}" alt="" width="50%"></div><div class="text-center my-3" data-encabezado="Oro">${x.PUNTAJE_MIN} a ${y == 0 ? 'mas' : (data[y-1].PUNTAJE_MIN - 1)} puntos</div></td>`).join('');
    $(selector).find('thead').html(`<tr>${head}</tr>`);
    $(selector).find('tbody').html(`<tr>${body}</tr>`);
}
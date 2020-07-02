$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
    consultarRequerimiento('#list-req');
    consultarCriterio('#list-criterio');
    consultarEvaluador('#list-evaluador');
    consultarEtapa('#tbl-etapa');
});


var consultar = () => {
    let busqueda = $('#textoBusqueda').val();
    let registros = 10;
    let pagina = 1;
    let columna = 'ID_CONVOCATORIA';
    let orden = 'ASC'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/convocatoria/buscarconvocatoria?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblPrincipal');
        let cantidadCeldasCabecera = tabla.find('thead tr th').length;
        let contenido = renderizar(j, cantidadCeldasCabecera, pagina, registros);
        tabla.find('tbody').html(contenido);
        tabla.find('.btnCambiarEstado').each(x => {
            let elementButton = tabla.find('.btnCambiarEstado')[x];
            $(elementButton).on('click', (e) => {
                e.preventDefault();
                cambiarEstado(e.currentTarget);
            });
        });

        tabla.find('.btnEditar').each(x => {
            let elementButton = tabla.find('.btnEditar')[x];
            $(elementButton).on('click', (e) => {
                e.preventDefault();
                consultarConvocatoria(e.currentTarget);
            });
        });
    });
};

var consultarConvocatoria = (element) => {
    $('#frm').show();
    limpiarFormulario();
    let id = $(element).attr('data-id');

    let url = `/api/convocatoria/obtenerconvocatoria?id=${id}`;

    fetch(url)
    .then(r => r.json())
    .then (j => {
        let urlConvocatoriaReq = `/api/convocatoria/listarconvocatoriareq?id=${id}`;
        let urlConvocatoriaCri = `/api/convocatoria/listarconvocatoriacri?id=${id}`;
        let urlConvocatoriaEva = `/api/convocatoria/listarconvocatoriaeva?id=${id}`;
        let urlConvocatoriaEta = `/api/convocatoria/listarconvocatoriaeta?id=${id}`;
        Promise.all([
            fetch(urlConvocatoriaReq),
            fetch(urlConvocatoriaCri),
            fetch(urlConvocatoriaEva),
            fetch(urlConvocatoriaEta)
        ])
        .then(r => Promise.all(r.map(v => v.json())))
        .then(([jReq, jCri, jEva, jEta]) => {
            cargarDatos(j);
            jReq.length == 0 ? '' : jReq.map(x => $('#chk-r-'+x.ID_REQUERIMIENTO).prop('checked', true));
            jCri.length == 0 ? '' : jCri.map(x => $('#chk-c-'+x.ID_CRITERIO).prop('checked', true));
            jEva.length == 0 ? '' : jEva.map(x => $('#chk-e-'+x.ID_USUARIO).prop('checked', true));
            jEta.length == 0 ? '' : jEta.map(x => $('#txt-e-'+x.ID_ETAPA).val(x.DIAS));
        });
    });
}

var cargarDatos = (data) => {
    //debugger;
    $('#frm').data('id', data.ID_CONVOCATORIA);
    $('#txtConvocatoria').val(data.NOMBRE);
    data.TXT_FECHA_INICIO == '0001-01-01' ? null : $('#fchFechaInicio').val(data.TXT_FECHA_INICIO);
    data.TXT_FECHA_FIN == '0001-01-01' ? null : $('#fchFechaFin').val(data.TXT_FECHA_FIN);
    $('#txtLimite').val(data.LIMITE_POSTULANTE);
}

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let colNro = `<td>${(pagina - 1) * registros + (i + 1)}</td>`;
            let colNombre = `<td>${x.NOMBRE}</td>`;
            let colFechaInicio = `<td>${x.TXT_FECHA_INICIO == '01/01/0001' ? '' : x.TXT_FECHA_INICIO}</td>`;
            let colFechaFin = `<td>${x.TXT_FECHA_FIN == '01/01/0001' ? '' : x.TXT_FECHA_FIN}</td>`;
            let colLimite = `<td>${x.LIMITE_POSTULANTE}</td>`;
            let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a href="#" data-id="${x.ID_CONVOCATORIA}" data-estado="${x.FLAG_ESTADO}" class="btnCambiarEstado">ELIMINAR</a> `}`;
            let btnEditar = `<a href="#" data-id="${x.ID_CONVOCATORIA}" class="btnEditar">EDITAR</a>`;
            let colOpciones = `<td>${btnCambiarEstado}${btnEditar}</td>`;
            let fila = `<tr>${colNro}${colNombre}${colFechaInicio}${colFechaFin}${colLimite}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var cambiarEstado = (element) => {

    let id = $(element).attr('data-id');

    if (!confirm(`¿Está seguro que desea eliminar este registro?`)) return;

    let data = { ID_CONVOCATORIA: id, USUARIO_GUARDAR: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    let url = '/api/convocatoria/cambiarestadoconvocatoria';

    fetch(url, init)
        .then(r => r.json())
        .then(j => { if (j) $('#btnConsultar')[0].click(); });
};

var consultarRequerimiento = (selector) => {

    let url = `/api/requerimiento/obtenerallrequerimiento`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckRequerimiento(selector, j));
}

var consultarCriterio = (selector) => {

    let url = `/api/criterio/obtenerallcriterio`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckCriterio(selector, j));
}

var consultarEvaluador = (selector) => {

    let url = `/api/usuario/obtenerallevaluador`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckEvaluador(selector, j));
}

var consultarEtapa = (selector) => {

    let url = `/api/etapa/obteneralletapa`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckEtapa(selector, j));
}

var cargarCheckRequerimiento = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="requerimiento" id="chk-r-${x.ID_REQUERIMIENTO}"><label for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckCriterio = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="criterio" id="chk-c-${x.ID_CRITERIO}"><label for="chk-c-${x.ID_CRITERIO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckEvaluador = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="evaluador" id="chk-e-${x.ID_USUARIO}"><label for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckEtapa = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<tr><td>${x.ETAPA}</td><td>${x.PROCESO}</td><td><input class="etapa" type="text" id="txt-e-${x.ID_ETAPA}" /></tr></td>`).join('');
    $(selector).find('tbody').html(items);
}

var nuevo = () => {
    $('#frm').show();
    limpiarFormulario();
    cargarFormulario();
}

var cerrarFormulario = () => {
    $('#frm').hide();
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txtConvocatoria').val('');
    $('#fchFechaInicio').val('');
    $('#fchFechaFin').val('');
    $('#txtLimite').val('');
    $('#list-req').find('.requerimiento').each((x, y) => { $(y).prop('checked', false); });
    $('#list-criterio').find('.criterio').each((x, y) => { $(y).prop('checked', false); });
    $('#list-evaluador').find('.evaluador').each((x, y) => { $(y).prop('checked', false); });
    $('#tbl-etapa').find('.etapa').each((x, y) => { $(y).val('') });
}

var guardar = () => {
    let id = $('#frm').data('id');
    let nombre = $('#txtConvocatoria').val();
    let fechaInicio = $('#fchFechaInicio').val();
    let fechaFin = $('#fchFechaFin').val();
    let limite = $('#txtLimite').val();
    requerimiento = [];
    criterio = [];
    evaluador = [];
    etapa = [];

    $('#list-req').find('.requerimiento').each((x, y) => {
        var r = {
            ID_REQUERIMIENTO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        requerimiento.push(r);
    });

    $('#list-criterio').find('.criterio').each((x, y) => {
        var r = {
            ID_CRITERIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        criterio.push(r);
    });

    $('#list-evaluador').find('.evaluador').each((x, y) => {
        var r = {
            ID_USUARIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        evaluador.push(r);
    });

    $('#tbl-etapa').find('.etapa').each((x, y) => {
        //debugger;
        var r = {
            ID_ETAPA: $(y).attr("id").substring(6, $(y).attr("id").length),
            DIAS: $(y).val()
        }
        etapa.push(r);
    });

    let url = `/api/convocatoria/guardarconvocatoria`;

    let data = { ID_CONVOCATORIA: id == null ? -1 : id, NOMBRE: nombre, FECHA_INICIO: fechaInicio, FECHA_FIN: fechaFin, LIMITE_POSTULANTE: limite, LISTA_REQ: requerimiento, LISTA_CRI: criterio, LISTA_EVA: evaluador, LISTA_ETA: etapa, USUARIO_GUARDAR: idUsuarioLogin };
    //debugger;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if (j) {
            alert('Se registró correctamente');
            cerrarFormulario();
            $('#btnConsultar')[0].click();
        }
    });
}
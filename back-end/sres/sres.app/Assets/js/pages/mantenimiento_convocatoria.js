﻿$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
    consultarListas();
    //consultarRequerimiento('#list-req');
    //consultarCriterio('#list-criterio');
    //consultarEvaluador('#list-evaluador');
    //consultarEtapa('#tbl-etapa');
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
    $('#etapa-convocatoria').show();
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
            //jCri.length == 0 ? '' : jReq.map(x => $('#chk-r-'+x.ID_REQUERIMIENTO).prop('checked', true));
            if (jCri.length > 0){
                jCri.map((x,y) => {
                    $('#chk-c-'+x.ID_CRITERIO).prop('checked', true);
                    x.LISTA_CASO.map((w,z) => {
                        $('#chk-c-'+w.ID_CRITERIO+'-s-'+w.ID_CASO).prop('checked', true);
                        w.LIST_DOC.map((a,b) => {
                            $('#chk-c-'+a.ID_CRITERIO+'-s-'+a.ID_CASO+'-d-'+a.ID_DOCUMENTO).prop('checked', true);
                        });
                    });
                    
                });
            }

            jReq.length == 0 ? '' : jReq.map(x => $('#chk-r-'+x.ID_REQUERIMIENTO).prop('checked', true));
            //jCri.length == 0 ? '' : jCri.map(x => $('#chk-c-'+x.ID_CRITERIO).prop('checked', true));
            jEva.length == 0 ? '' : jEva.map(x => $('#chk-e-'+x.ID_USUARIO).prop('checked', true));
            jEta.length == 0 ? '' : jEta.map(x => $('#txt-e-'+x.ID_ETAPA).val(x.DIAS));
        });
    });
}

var cargarDatos = (data) => {
    //debugger;
    $('#cbo-etapa').val(data.ID_ETAPA);
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

var consultarListas = () => {
    let urlConsultarListaCriterio = `/api/criterio/obtenerallcriterio`;
    let urlConsultarListaRequerimiento = `/api/requerimiento/obtenerallrequerimiento`;
    let urlConsultarListaEvaluador = `/api/usuario/obtenerallevaluador`;
    let urlConsultarListaEtapa = `/api/etapa/obteneralletapa`;
    Promise.all([
        fetch(urlConsultarListaCriterio),
        fetch(urlConsultarListaRequerimiento),
        fetch(urlConsultarListaEvaluador),
        fetch(urlConsultarListaEtapa)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(cargarCheckListas);
}

var cargarCheckListas = ([listaCriterio, listaRequerimiento, listaEvaluador, listaEtapa]) => {
    cargarCheckCriterio('#list-criterio', listaCriterio);
    cargarCheckRequerimiento('#list-req', listaRequerimiento);
    cargarCheckEvaluador('#list-evaluador', listaEvaluador);
    cargarCheckEtapa('#tbl-etapa', listaEtapa);
    cargarComboEtapa('#cbo-etapa', listaEtapa);
}

//var consultarRequerimiento = (selector) => {

//    let url = `/api/requerimiento/obtenerallrequerimiento`;

//    fetch(url)
//    .then(r => r.json())
//    .then(j => cargarCheckRequerimiento(selector, j));
//}

//var consultarCriterio = (selector) => {

//    let url = `/api/criterio/obtenerallcriterio`;

//    fetch(url)
//    .then(r => r.json())
//    .then(j => cargarCheckCriterio(selector, j));
//}

//var consultarEvaluador = (selector) => {

//    let url = `/api/usuario/obtenerallevaluador`;

//    fetch(url)
//    .then(r => r.json())
//    .then(j => cargarCheckEvaluador(selector, j));
//}

//var consultarEtapa = (selector) => {

//    let url = `/api/etapa/obteneralletapa`;

//    fetch(url)
//    .then(r => r.json())
//    .then(j => cargarCheckEtapa(selector, j));
//}

var cargarCheckRequerimiento = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="requerimiento" id="chk-r-${x.ID_REQUERIMIENTO}"><label for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckCriterio = (selector, data) => {
    //let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="criterio" id="chk-c-${x.ID_CRITERIO}"><label for="chk-c-${x.ID_CRITERIO}">${x.NOMBRE}&nbsp;</label><div id="listaRequerimientoCriterio_${x.ID_CRITERIO}"></div></li></ul></div></div>`).join('');
    //$(selector).html(items);
    //cargarCheckRequerimiento('div[id*="listaRequerimientoCriterio_"]', dataRequerimiento);

    let contenido = data.map((x, i) => {
        let caso = armarcaso(x.LISTA_CASO, x.LISTA_DOCUMENTO);
        let criterio = `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="criterio" id="chk-c-${x.ID_CRITERIO}"><label for="chk-c-${x.ID_CRITERIO}">${x.NOMBRE}&nbsp;</label>${caso}</li></ul></div></div>`;
        return criterio;
    }).join('');
    $(selector).html(contenido);
}

var armarcaso = (datacaso, datadoc) => {
    let caso = ``;
    datacaso.map((x, i) => {
        let documentos = armarDocumento(datadoc,x.ID_CASO);
        caso += `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="caso" id="chk-c-${x.ID_CRITERIO}-s-${x.ID_CASO}"><label for="chk-c-${x.ID_CRITERIO}-s-${x.ID_CASO}">${x.NOMBRE}&nbsp;</label>${documentos}</li></ul></div></div>`;    
    });
    return caso;
}

var armarDocumento = (datadoc,idcaso) => {
    let doc = ``;
    datadoc.map((x, i) => {
        doc += `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="documento" id="chk-c-${x.ID_CRITERIO}-s-${idcaso}-d-${x.ID_DOCUMENTO}"><label for="chk-c-${x.ID_CRITERIO}-s-${idcaso}-d-${x.ID_DOCUMENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`;    
    });
    return doc
}

var cargarCheckEvaluador = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="evaluador" id="chk-e-${x.ID_USUARIO}"><label for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckEtapa = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<tr><td>${x.ETAPA}</td><td>${x.PROCESO}</td><td><input class="etapa" type="text" id="txt-e-${x.ID_ETAPA}" /></tr></td>`).join('');
    $(selector).find('tbody').html(items);
}

var cargarComboEtapa = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ETAPA}">${x.ETAPA}</option>`).join('');
    $(selector).html(items);
}

var nuevo = () => {
    limpiarFormulario();
    $('#frm').show();
    $('#etapa-convocatoria').hide();
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
    criterioRequerimiento = [];

    $('#list-req').find('.requerimiento').each((x, y) => {
        var r = {
            ID_REQUERIMIENTO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        requerimiento.push(r);
    });

    $('#list-criterio').find('.criterio').each((x, y) => {
        let idcriterio = $(y).attr("id").substring(6, $(y).attr("id").length);
        let arr_caso = [];
        
        $(y).parent().find('.caso').each((w,z) => {
            let idcaso = $(z).attr('id').substring($(z).attr('id').indexOf("s")+2,$(z).attr('id').length);
            let arr_doc = [];
            $(z).parent().find('.documento').each((a,b) => {
                var d = {
                    ID_CRITERIO: idcriterio,
                    ID_CASO: idcaso,
                    ID_DOCUMENTO: $(b).attr('id').substring($(b).attr('id').indexOf("d")+2,$(b).attr('id').length),
                    FLAG_ESTADO: $(b).prop('checked') ? '1' : '0',
                    USUARIO_GUARDAR: idUsuarioLogin
                }
                arr_doc.push(d);
            });

            var c = {
                ID_CRITERIO: idcriterio,
                ID_CASO: idcaso,
                LIST_DOC: arr_doc,
                FLAG_ESTADO: $(z).prop('checked') ? '1' : '0',
                USUARIO_GUARDAR: idUsuarioLogin
            }
            arr_caso.push(c);
        });

        var r = {
            ID_CRITERIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            LISTA_CASO: arr_caso,
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0',
            USUARIO_GUARDAR: idUsuarioLogin
        }
        criterio.push(r);
    });
    //debugger;
    $('#list-evaluador').find('.evaluador').each((x, y) => {
        var r = {
            ID_USUARIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        evaluador.push(r);
    });

    $('#tbl-etapa').find('.etapa').each((x, y) => {
        var r = {
            ID_ETAPA: $(y).attr("id").substring(6, $(y).attr("id").length),
            DIAS: $(y).val()
        }
        etapa.push(r);
    });

    Array.from($('div[id*="listaRequerimientoCriterio"]')).forEach(x => {
        let idCriterio = $(x).parent().find('input[type="checkbox"]').attr('id').replace('chk-c-', '');
        Array.from($(x).find('.requerimiento')).forEach(y => {
            let idRequerimiento = $(y).attr('id').replace('chk-r-', '');
            let obligatorio = $(y).prop('checked').toString()[0].toLocaleUpperCase();
            criterioRequerimiento.push({ID_CRITERIO: idCriterio, ID_REQUERIMIENTO: idRequerimiento, OBLIGATORIO: obligatorio, UPD_USUARIO: idUsuarioLogin});
        });
    });

    let url = `/api/convocatoria/guardarconvocatoria`;

    let data = { ID_CONVOCATORIA: id == null ? -1 : id, ID_ETAPA: $('#cbo-etapa').val(), NOMBRE: nombre, FECHA_INICIO: fechaInicio, FECHA_FIN: fechaFin, LIMITE_POSTULANTE: limite, LISTA_REQ: requerimiento, LISTA_CRI: criterio, LISTA_EVA: evaluador, LISTA_ETA: etapa, LISTA_CONVOCATORIA_CRITERIO_REQUERIMIENTO: criterioRequerimiento, USUARIO_GUARDAR: idUsuarioLogin };
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
﻿$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
    $('.mostrar-relacion').on('click', (e) => relacionar());
    $('#btnCerrarRelacion').on('click', (e) => cerrarRelacion());
    $('#btnGuardarRelacion').on('click', (e) => guardarRelacion());
    $('#btnGuardarNoRelacion').on('click', (e) => guardarNoRelacion());
    $('#tab-head-01').on('click', (e) => limpiarSeccion());
    $('#tab-head-02').on('click', (e) => limpiarSeccionPos());
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#cbo-proceso').on('click', (e) => cambiarProceso());
    consultarListas();
    //consultarRequerimiento('#list-req');
    //consultarCriterio('#list-criterio');
    //consultarEvaluador('#list-evaluador');
    //consultarEtapa('#tbl-etapa');
});

var utc = new Date().toJSON().slice(0, 10).replace(/-/g, '-');

var consultar = () => {
    //let busqueda = $('#textoBusqueda').val();
    //let registros = 10;
    //let pagina = 1;
    //let columna = 'ID_CONVOCATORIA';
    //let orden = 'ASC'
    //let params = { busqueda, registros, pagina, columna, orden };
    let codigo = $('#txt-codigo').val();
    let nombre = $('#txt-descripcion').val();
    let fechaDesde = $('#dat-desde').val();
    let fechaHasta = $('#dat-hasta').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();
    let columna = $("#columna").val();
    let orden = $("#orden").val();
    let params = { codigo, nombre, fechaDesde, fechaHasta, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/convocatoria/buscarconvocatoria?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblPrincipal');
        tabla.find('tbody').html('');
        $('#viewPagination').attr('style', 'display: none !important');
        if (j.length > 0){            
            //console.log(data.TOTAL_REGISTROS, data.CANTIDAD_REGISTROS);
            //if (j[0].TOTAL_REGISTROS > j[0].CANTIDAD_REGISTROS) $('#viewPagination').show();
            if (j[0].CANTIDAD_REGISTROS == 0) {$('#viewPagination').hide(); $('#view-page-result').hide();} 
            else {$('#view-page-result').show(); $('#viewPagination').show();} 
            $('.inicio-registros').text(j[0].CANTIDAD_REGISTROS == 0 ? 'No se encontraron resultados' : (j[0].PAGINA - 1) * j[0].CANTIDAD_REGISTROS + 1);
            $('.fin-registros').text(j[0].TOTAL_REGISTROS < j[0].PAGINA * j[0].CANTIDAD_REGISTROS ? j[0].TOTAL_REGISTROS : j[0].PAGINA * j[0].CANTIDAD_REGISTROS);
            $('.total-registros').text(j[0].TOTAL_REGISTROS);
            $('.pagina').text(j[0].PAGINA);
            $('#ir-pagina').val(j[0].PAGINA);
            $('#ir-pagina').attr('max', j[0].TOTAL_PAGINAS);
            $('.total-paginas').text(j[0].TOTAL_PAGINAS);
            
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
        }else{
            $('#viewPagination').hide(); $('#view-page-result').hide();
            $('.inicio-registros').text('No se encontraron resultados');
        }        
    });
};

var consultarConvocatoria = (element) => {
    $('#frm').show();
    $('.alert-add').html('');
    $('#btnGuardar').show(); 
    $('#btnGuardar').next().html('Cancelar');
    $('.relacion-evaluador').removeClass('d-none');
    $('#etapa-convocatoria').show();
    $('#proceso-convocatoria').show();
    limpiarFormulario();
    let id = $(element).attr('data-id');

    let url = `${baseUrl}api/convocatoria/obtenerconvocatoria?id=${id}`;

    fetch(url)
    .then(r => r.json())
    .then (j => {
        let urlConvocatoriaReq = `${baseUrl}api/convocatoria/listarconvocatoriareq?id=${id}`;
        let urlConvocatoriaCri = `${baseUrl}api/convocatoria/listarconvocatoriacri?id=${id}`;
        let urlConvocatoriaEva = `${baseUrl}api/convocatoria/listarconvocatoriaeva?id=${id}`;
        let urlConvocatoriaEta = `${baseUrl}api/convocatoria/listarconvocatoriaeta?id=${id}`;
        let urlConvocatoriaPos = `${baseUrl}api/convocatoria/listarconvocatoriapos?id=${id}`;
        let urlConvocatoriaInsig = `${baseUrl}api/convocatoria/listarconvocatoriainsig?id=${id}`;
        let urlConvocatoriaEstTrab = `${baseUrl}api/convocatoria/listarconvocatoriaesttrab?id=${id}`;
        Promise.all([
            fetch(urlConvocatoriaReq),
            fetch(urlConvocatoriaCri),
            fetch(urlConvocatoriaEva),
            fetch(urlConvocatoriaEta),
            fetch(urlConvocatoriaPos),
            fetch(urlConvocatoriaInsig),
            fetch(urlConvocatoriaEstTrab)
        ])
        .then(r => Promise.all(r.map(v => v.json())))
        .then(([jReq, jCri, jEva, jEta, jPos, jInsig, jEsttrab]) => {
            $('[id*=tab-head-cri-] i').removeClass('text-sres-verde').addClass('text-primary')
            cargarDatos(j);
            //jCri.length == 0 ? '' : jReq.map(x => $('#chk-r-'+x.ID_REQUERIMIENTO).prop('checked', true));

            //if (jPos.length > 0){
            //    let postulante = jPos.map((x,y) => {
            //        let evaluadores = armarEvaluadores(jEva, x.ID_INSTITUCION);
            //        return `<div class="get-valor"><label class="get-institucion mr-3" id="${x.ID_INSTITUCION}">${x.RAZON_SOCIAL}</label>${evaluadores}</div>`;
            //    }).join('');    
            //    $('.postulante-evaluador').html(postulante);
            //    asignarEvaluador(jPos);
            //}

            if (jPos.length > 0){
                let postulante = jPos.map((x,y) => {
                    //return `<div class="get-valor"><label class="get-institucion mr-3" id="${x.ID_INSTITUCION}">${x.RAZON_SOCIAL}</label>${evaluadores}</div>`;
                    return `<div class="col-auto my-1"><div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input get-institucion" type="checkbox" id="chk-postulante-${x.ID_INSTITUCION}"><label class="custom-control-label estilo-01" for="chk-postulante-${x.ID_INSTITUCION}">${x.RAZON_SOCIAL}</label></div></div>`;
                }).join('');    
                $('.postulante-evaluador').html(postulante);
                removerPostulante(jPos);
                //let a = `<div class="col-auto my-1"><div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input" type="checkbox" id="chk-postulante-add-1"><label class="custom-control-label estilo-01" for="chk-postulante-add-1">Postulante&nbsp;1</label></div></div>`;
                //asignarEvaluador(jPos);
            }

            if (jEva.length > 0){
                let evaluador = jEva.map((x,y) => {
                    return `<option value="${x.ID_USUARIO}">${x.NOMBRE}</option>`;
                }).join('');
                $('#cbo-evaluadores-01').html(`<option value="0">-Seleccionar-</option>${evaluador}`);
                $('#cbo-evaluadores-02').html(`<option value="0">-Seleccionar-</option>${evaluador}`);
            }

            if (jCri.length > 0){
                jCri.map((x,y) => {
                    $('#chk-c-'+x.ID_CRITERIO).prop('checked', true);
                    $(`#tab-head-cri-0${x.ID_CRITERIO} i`).removeClass('text-primary').addClass('text-sres-verde')
                    x.LISTA_CASO.map((w,z) => {
                        $('#chk-c-'+w.ID_CRITERIO+'-s-'+w.ID_CASO).prop('checked', true);
                        w.LIST_DOC.map((a,b) => {
                            $('#chk-c-'+a.ID_CRITERIO+'-s-'+a.ID_CASO+'-d-'+a.ID_DOCUMENTO).prop('checked', true);
                        });
                    });
                    x.LISTA_CONVCRIPUNT.map((w,z) => {
                        $('#puntaje-'+w.ID_CRITERIO+'-'+w.ID_DETALLE).val(w.PUNTAJE);
                    });
                });
            }

            jReq.length == 0 ? '' : jReq.map(x => $('#chk-r-'+x.ID_REQUERIMIENTO).prop('checked', true));
            //jCri.length == 0 ? '' : jCri.map(x => $('#chk-c-'+x.ID_CRITERIO).prop('checked', true));
            jEva.length == 0 ? '' : jEva.map(x => $('#chk-e-'+x.ID_USUARIO).prop('checked', true));
            //jEta.length == 0 ? '' : jEta.map(x => $('#txt-e-'+x.ID_ETAPA).val(x.DIAS));
            jEta.length == 0 ? '' : jEta.map(x => $('#txt-e-'+x.ID_ETAPA).val(x.FECHA_ETAPA_CONV));
            jInsig.length == 0 ? '' : jInsig.map(x => $('#txt-i-'+x.ID_INSIGNIA).val(x.PUNTAJE_MIN));
            jEsttrab.length == 0 ? '' : jEsttrab.map(x => $(`#estrella-${x.ID_ESTRELLA}-${x.ID_TRABAJADORES_CAMA}`).val(x.EMISIONES_MIN))
        });
    });
}

var armarEvaluadores = (data, idInstitucion) => {
    let select = ``;
    if (data.length > 0){
        select = data.map((x,y) => {
            return `<option value="${x.ID_USUARIO}">${x.NOMBRE}</option>`;
        }).join('');
        select = `<select class="get-evaluador" id="eva-${idInstitucion}"><option value="0">-Seleccione-</option>${select}</select>`
    }
    return select;
}

var asignarEvaluador = (data) => {
    data.map((x,y) => {
        if (x.CONV_EVA_POS != null){
            $(`#eva-${x.ID_INSTITUCION}`).val(x.CONV_EVA_POS.ID_USUARIO);
            $(`#eva-${x.ID_INSTITUCION}`).find(`option[value='0']`).remove();
        }
    });
}

var removerPostulante = (data) => {
    data.map((x,y) => {
        if (x.CONV_EVA_POS != null){
            if (x.CONV_EVA_POS.FLAG_ESTADO == '1'){
                $(`#chk-postulante-${x.ID_INSTITUCION}`).parent().parent().remove();
            }            
        }
    });
}

var cargarDatos = (data) => {
    idEtapa_actual = data.ID_ETAPA;
    $('#cbo-etapa').val(data.ID_ETAPA);
    validarEtapa(data.ID_ETAPA);
    cambiarProceso();
    $('#frm').data('id', data.ID_CONVOCATORIA);
    $('#txt-titulo').val(data.NOMBRE);
    $('#txa-descripcion').val(data.DESCRIPCION);
    data.TXT_FECHA_INICIO == '0001-01-01' ? null : $('#dat-inicio').val(data.TXT_FECHA_INICIO);
    data.TXT_FECHA_FIN == '0001-01-01' ? null : $('#dat-fin').val(data.TXT_FECHA_FIN);
    $('#txt-capacidad').val(data.LIMITE_POSTULANTE);
}

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let formatoCodigo = '00000000';
            let porcentajeAvance = x.ID_ETAPA > 14 ? 100 : Math.round((x.ID_ETAPA - 1) / 13 * 100);
            let colNro = `<td class="text-center" data-encabezado="Número de orden" scope="row" data-count="0">${(pagina - 1) * registros + (i + 1)}</td>`;
            let colExp = `<td class="text-center" data-encabezado="Número expediente" scope="row"><span>${(`${formatoCodigo}${x.ID_CONVOCATORIA}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colNombre = `<td class="text-center" data-encabezado="Título">${x.NOMBRE}</td>`;
            let colDescripcion = `<td data-encabezado="Descripción">${x.DESCRIPCION}</td>`;
            let colFechaInicio = `<td class="text-center" data-encabezado="Fecha Inicio">${x.TXT_FECHA_INICIO == '01/01/0001' ? '' : x.TXT_FECHA_INICIO}</td>`;
            let colFechaFin = `<td class="text-center" data-encabezado="Fecha Fin">${x.TXT_FECHA_FIN == '01/01/0001' ? '' : x.TXT_FECHA_FIN}</td>`;
            let colProgreso = `<td class="text-center" data-encabezado="Progreso"><div class="progress" style="height: 21px; ${porcentajeAvance > 0 ? "background-color: #E2DBDA;" : ""}" data-toggle="tooltip" data-placement="top" title="Porcentaje de avance"><div class="progress-bar ${porcentajeAvance > 0 ? "vigente" : "preparado"} estilo-01" role="progressbar" style="width: ${porcentajeAvance}%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">${porcentajeAvance}%</div></div></td>`;
            let colEstado = `<td data-encabezado="Estado"><b class="text-sres-verde">${x.NOMBRE_ETAPA}</b></td>`;
            let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_CONVOCATORIA}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_CONVOCATORIA}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnCambiarEstado}${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colExp}${colNombre}${colDescripcion}${colFechaInicio}${colFechaFin}${colProgreso}${colEstado}${colOpciones}</tr>`;
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

    let url = `${baseUrl}api/convocatoria/cambiarestadoconvocatoria`;

    fetch(url, init)
        .then(r => r.json())
        .then(j => { if (j) $('#btnConsultar')[0].click(); });
};

var consultarListas = () => {
    let urlConsultarListaCriterio = `${baseUrl}api/criterio/obtenerallcriterio`;
    let urlConsultarListaRequerimiento = `${baseUrl}api/requerimiento/obtenerallrequerimiento`;
    let urlConsultarListaEvaluador = `${baseUrl}api/usuario/obtenerallevaluador`;
    let urlConsultarListaEtapa = `${baseUrl}api/etapa/obteneralletapa`;
    let urlConsultarListaInsignia = `${baseUrl}api/insignia/obtenerallinsignia`;
    let urlConsultarListaEstrella = `${baseUrl}api/estrella/obtenerallestrella`;
    let urlConsultarListaSector = `${baseUrl}api/sector/obtenerallsector`;
    let urlConsultarListaProceso = `${baseUrl}api/proceso/obtenerallproceso`;
    Promise.all([
        fetch(urlConsultarListaCriterio),
        fetch(urlConsultarListaRequerimiento),
        fetch(urlConsultarListaEvaluador),
        fetch(urlConsultarListaEtapa),
        fetch(urlConsultarListaInsignia),
        fetch(urlConsultarListaEstrella),
        fetch(urlConsultarListaSector),
        fetch(urlConsultarListaProceso)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(cargarCheckListas);
}

var cargarCheckListas = ([listaCriterio, listaRequerimiento, listaEvaluador, listaEtapa, listaInsignia, listaEstrella, listaSector, listaProceso]) => {
    //cargarCheckCriterio('#list-criterio', listaCriterio);
    cargarCheckCriterio('.tab-criterio-content', listaCriterio);
    //cargarCheckRequerimiento('#list-req', listaRequerimiento);
    cargarCheckRequerimiento('.list-req', listaRequerimiento);
    //cargarCheckEvaluador('#list-evaluador', listaEvaluador);
    cargarCheckEvaluador('.list-evaluador', listaEvaluador);
    //cargarCheckEtapa('#tbl-etapa', listaEtapa);
    cargarCheckEtapa('.tbl-etapa', listaEtapa);
    cargarComboEtapa('#cbo-etapa', listaEtapa);
    //cargarTablaInsignia('#tbl-insignia', listaInsignia);
    cargarTablaInsignia('.tbl-insignia', listaInsignia);
    //cargarTablaEstrellaSector("#tbl-estrellas", listaEstrella, listaSector);
    cargarTablaEstrellaSector(".tbl-estrellas", listaEstrella, listaSector);
    cargarComboProceso('#cbo-proceso', listaProceso);
}

var cargarCheckRequerimiento = (selector, data) => {
    //let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="requerimiento" id="chk-r-${x.ID_REQUERIMIENTO}"><label for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    let items = data.length == 0 ? '' : data.map(x => `<div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input requerimiento" type="checkbox" id="chk-r-${x.ID_REQUERIMIENTO}"><label class="custom-control-label estilo-01" for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}</label></div>`).join('');
    $(selector).html(items);
    //let a = `<div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input requerimiento" type="checkbox" id="chk-r-${x.ID_REQUERIMIENTO}"><label class="custom-control-label estilo-01" for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}.</label></div>`;
}

var cargarCheckCriterio = (selector, data) => {
    let tabs = data.map((x,i) => {
        return `<li class="nav-item" data-toggle="tooltip" data-placement="top" title="${x.NOMBRE}"><a class="nav-link estilo-01" id="tab-head-cri-0${x.ID_CRITERIO}" data-toggle="tab" href="#tab-body-cri-0${x.ID_CRITERIO}" role="tab" aria-controls="tab-body-cri-0${x.ID_CRITERIO}" aria-selected="${i == 0 ? 'true':'false'}"><i class="fas fa-check-circle text-primary mr-1"></i>Criterio 0${x.ID_CRITERIO < 3 ? x.ID_CRITERIO : x.ID_CRITERIO - 1}</a></li>`;
    }).join('');
    $('.tab-criterio').html(tabs);

    let contenido = data.map((x, i) => {
        let caso = armarcaso(x.LISTA_CASO, x.LISTA_DOCUMENTO, x.ID_CRITERIO);
        let puntaje = armarpuntaje(x.LISTA_CONVCRIPUNT);
        //let criterio = `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="criterio" id="chk-c-${x.ID_CRITERIO}"><label for="chk-c-${x.ID_CRITERIO}">${x.NOMBRE}&nbsp;</label>${caso}</li></ul>${puntaje}</div></div>`;
        let criterio = `<div class="tab-pane fade ${i == 0 ? 'show active':''}" id="tab-body-cri-0${x.ID_CRITERIO}" role="tabpanel" aria-labelledby="tab-head-cri-0${x.ID_CRITERIO}"><div class="row">${caso}${puntaje}</div></div>`;
        return criterio;
    }).join('');
    $(selector).html(contenido);
}

var armarpuntaje = (datapuntaje) => {
    let puntaje = ``;
    datapuntaje.map((x, i) => {
        //puntaje += `<tr><td class="get-detalle" id="${x.ID_DETALLE}">${x.ID_DETALLE}</td><td>${x.DESCRIPCION}</td><td><input id="puntaje-${x.ID_CRITERIO}-${x.ID_DETALLE}" class="get-puntaje" type="text" value="${x.PUNTAJE}" /></td></tr>`;    
        puntaje += `<div class="form-group row mb-0"><label class="col-sm-8 col-form-label" for="puntaje-${x.ID_CRITERIO}-${x.ID_DETALLE}">${x.DESCRIPCION}</label><div class="col-sm-4"><input class="form-control estilo-01 get-puntaje solo-numero text-right" type="text" id="puntaje-${x.ID_CRITERIO}-${x.ID_DETALLE}" placeholder="" value="${x.PUNTAJE}" maxlength="3"></div></div>`;
    });
    //puntaje = `<div class="ml-5"><table id="puntaje-${datapuntaje[0].ID_CRITERIO}" class="get-tabla-puntaje"><thead><th>N°</th><th>Descripción</th><th>Puntaje</th></thead><tbody>${puntaje}</tbody></table></div>`;  
    //return puntaje;
    let input_group_content = `<div class="input-group"><div class="input-group-prepend"><span class="input-group-text" id="inputGroup9"><i class="far fa-list-alt"></i></span></div><div class="form-control" style="height:auto; overflow-y: auto;"><div class="col-auto my-1">${puntaje}</div></div></div>`;
    return `<div class="col-sm-12 col-md-12 col-lg-6"><div class="form-group"><label class="estilo-01" for="mls-puntuación">Puntuación<span class="text-danger font-weight-bold">&nbsp;(*)</span></label>${input_group_content}</div></div>`;
}

var armarcaso = (datacaso, datadoc, idCriterio) => {
    let caso = ``;
    datacaso.map((x, i) => {
        let documentos = armarDocumento(datadoc,x.ID_CASO);
        //caso += `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="caso" id="chk-c-${x.ID_CRITERIO}-s-${x.ID_CASO}"><label for="chk-c-${x.ID_CRITERIO}-s-${x.ID_CASO}">${x.NOMBRE}&nbsp;</label>${documentos}</li></ul></div></div>`;    
        caso += `<div class="form-group"><div class="custom-control custom-checkbox mb-2"><input class="custom-control-input caso" type="checkbox" id="chk-c-${x.ID_CRITERIO}-s-${x.ID_CASO}"><label class="custom-control-label estilo-01" for="chk-c-${x.ID_CRITERIO}-s-${x.ID_CASO}">${x.NOMBRE}<span class="text-danger font-weight-bold">&nbsp;(*)</span></label></div>${documentos}</div>`;
    });
    let form_group1 = `<div class="form-group"><div class="custom-control custom-checkbox"><input class="custom-control-input criterio" type="checkbox" id="chk-c-${idCriterio}" onclick="cambiarColorEstado(this)"><label class="custom-control-label estilo-01" for="chk-c-${idCriterio}">Activar criterio<span class="text-danger font-weight-bold">&nbsp;(*)</span></label></div></div>`;
    let form_group2 = `<div class="form-group"><label class="estilo-01">Seleccionar casos:</label></div>`;
    return `<div class="col-sm-12 col-md-12 col-lg-6">${form_group1}<div class="dropdown-divider"></div>${form_group2}${caso}</div>`;
}

var armarDocumento = (datadoc,idcaso) => {
    let doc = datadoc.map((x, i) => {
        //doc += `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="documento" id="chk-c-${x.ID_CRITERIO}-s-${idcaso}-d-${x.ID_DOCUMENTO}"><label for="chk-c-${x.ID_CRITERIO}-s-${idcaso}-d-${x.ID_DOCUMENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`;    
        return `<div class="custom-control custom-checkbox"><input class="custom-control-input documento" type="checkbox" id="chk-c-${x.ID_CRITERIO}-s-${idcaso}-d-${x.ID_DOCUMENTO}"><label class="custom-control-label estilo-01" for="chk-c-${x.ID_CRITERIO}-s-${idcaso}-d-${x.ID_DOCUMENTO}">${x.NOMBRE}</label></div>`;
    }).join('');
    let input_group_content = `<div class="input-group-prepend"><span class="input-group-text" id="inputGroup9"><i class="far fa-list-alt"></i></span></div><div class="form-control" style="height:auto; overflow-y: auto;"><div class="col-auto my-1">${doc}</div></div>`;
    return `<div class="input-group">${input_group_content}</div>`;
}

var cargarCheckEvaluador = (selector, data) => {
    //let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="evaluador" id="chk-e-${x.ID_USUARIO}"><label for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}&nbsp;</label></li></ul></div></div>`).join('');
    let items = data.length == 0 ? '' : data.map(x => `<div class="col-auto my-1"><div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input evaluador" type="checkbox" id="chk-e-${x.ID_USUARIO}"><label class="custom-control-label estilo-01" for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}</label></div></div>`).join('');
    $(selector).html(items);
    //let a = `<div class="col-auto my-1"><div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input evaluador" type="checkbox" id="chk-e-${x.ID_USUARIO}"><label class="custom-control-label estilo-01" for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}</label></div></div>`;
}

var cargarCheckEtapa = (selector, data) => {
    let col1 = `<div class="col-sm-12 col-md-12 col-lg-4"><div class="estilo-01">Actividad</div><div class="dropdown-divider"></div></div>`;
    let col2 = `<div class="col-sm-12 col-md-12 col-lg-4"><div class="estilo-01">Etapa</div><div class="dropdown-divider"></div></div>`;
    //let col3 = `<div class="col-sm-12 col-md-12 col-lg-2"><div class="estilo-01">Cantidad de Días</div><div class="dropdown-divider"></div></div>`;
    let col3 = `<div class="col-sm-12 col-md-12 col-lg-4"><div class="estilo-01">Fecha</div><div class="dropdown-divider"></div></div>`;
    let cabecera = `<div class="row">${col1}${col2}${col3}</div>`;
    //let items = data.length == 0 ? '' : data.map(x => `<tr><td>${x.ETAPA}</td><td>${x.PROCESO}</td><td><input class="etapa" type="text" id="txt-e-${x.ID_ETAPA}" /></tr></td>`).join('');
    let items = data.length == 0 ? '' : data.map(x => {
        let c1 = `<div class="col-sm-12 col-md-12 col-lg-4"><div class="form-group mb-1"><label class="estilo-01">${x.ETAPA}<span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span></label></div></div>`;
        let c2 = `<div class="col-sm-12 col-md-12 col-lg-4"><div class="form-group mb-1"><label class="estilo-01">${x.PROCESO}</label></div></div>`;
        //let c3 = `<div class="col-sm-12 col-md-12 col-lg-2"><div class="form-group mb-1"><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-calendar-day"></i></span></div><input class="form-control estilo-01 text-sres-gris etapa solo-numero" type="text" id="txt-e-${x.ID_ETAPA}" value="0" maxlength="3"></div></div></div>`;
        let c3 = `<div class="col-sm-12 col-md-12 col-lg-4"><div class="form-group mb-1"><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-calendar-day"></i></span></div><input class="form-control estilo-01 text-sres-gris etapa" type="date" id="txt-e-${x.ID_ETAPA}"></div></div></div>`;
        return `<div class="row">${c1}${c2}${c3}</div>`;
    }).join('');
    $(selector).html(`${cabecera}${items}`);
}

var cargarComboEtapa = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ETAPA}">${x.ETAPA}</option>`).join('');
    $(selector).html(items);
}

var cargarTablaInsignia = (selector, data) => {
    //let items = data.length == 0 ? '' : data.map(x => `<tr><td>${x.NOMBRE}</td><td><input class="insignia" type="text" id="txt-i-${x.ID_INSIGNIA}" value="${x.PUNTAJE_MIN}" /></tr></td>`).join('');
    let items = data.length == 0 ? '' : data.map(x => `<div class="form-group row mb-0"><label class="col-sm-8 col-form-label" for="txt-i-${x.ID_INSIGNIA}">${x.NOMBRE}</label><div class="col-sm-4"><input class="form-control estilo-01 insignia solo-numero text-right" type="text" id="txt-i-${x.ID_INSIGNIA}" placeholder="" value="${x.PUNTAJE_MIN}" maxlength="3"></div></div>`).join('');
    //$(selector).find('tbody').html(items);
    $(selector).html(items);
    //let a = `<div class="form-group row mb-0"><label class="col-sm-8 col-form-label" for="txt-i-${x.ID_INSIGNIA}">${x.NOMBRE}</label><div class="col-sm-4"><input class="form-control estilo-01" type="text" id="txt-i-${x.ID_INSIGNIA}" placeholder="" value="${x.PUNTAJE_MIN}></div></div>`;
}

var cargarTablaEstrellaSector = (selector, dataE, dataS) => {
    //let head = dataE.length == 0 ? '' : dataE.map(x => `<th>${x.NOMBRE} tCo2</th>`).join('');
    let head = dataE.length == 0 ? '' : dataE.map(x => `<div class="col-sm-12 col-md-12 col-lg-2"><div class="estilo-01">${x.NOMBRE} tCO<sub>2</sub></div><div class="dropdown-divider"></div></div>`).join('');
    //$(selector).find('thead').html(`<tr><th>Sector</th><th><Tipo empresa/Subsector</th><th>Trabajadores/Camas</th>${head}</tr>`);
    let col1 = `<div class="col-sm-12 col-md-12 col-lg-2"><div class="estilo-01">Sector - Sub Sector</div><div class="dropdown-divider"></div></div>`;
    let col2 = `<div class="col-sm-12 col-md-12 col-lg-2"><div class="estilo-01">Trabajadores/Camas</div><div class="dropdown-divider"></div></div>`;
    let col = `<div class="row">${col1}${col2}${head}</div>`;

    let items = dataS.length == 0 ? '' : dataS.map((x,y) => {
        return armarSubsector(x.NOMBRE, x.LISTA_SUBSEC_TIPOEMP, dataE);
    }).join('');
    //$(selector).find('tbody').html(items);
    $(selector).html(`${col}${items}`);
}

var armarSubsector = (sector, data, dataE) => {
    //let sub = data.length == 0 ? `<tr><td>${sector}</td></tr>` : data.map((x,y) => {
    let sub = data.length == 0 ? `` : data.map((x,y) => {
        return armarTrabajadorCama(sector, x.NOMBRE, x.LISTA_TRAB_CAMA, dataE);
    }).join('');
    return sub;
}

var armarTrabajadorCama = (sector, sub, data, dataE) => {
    //let tc = data.length == 0 ? `<tr><td>${sector}</td><td>${sub}</td></tr>` : data.map((x,y) => {
    let tc = data.length == 0 ? `` : data.map((x,y) => {
        //let contenido = dataE.length == 0 ? `` : dataE.map(z => `<td><input class="get-estrella" id="estrella-${z.ID_ESTRELLA}-${x.ID_TRABAJADORES_CAMA}" value="0" /></td>`).join('');
        let contenido = dataE.length == 0 ? `` : dataE.map(z => `<div class="col-sm-12 col-md-12 col-lg-2"><div class="form-group mb-1"><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-calendar-day"></i></span></div><input class="form-control estilo-01 text-sres-gris get-estrella formato-decimal text-right" type="text" id="estrella-${z.ID_ESTRELLA}-${x.ID_TRABAJADORES_CAMA}" value="0" maxlength="6"></div></div></div>`).join('');
        //return `<tr class="get-fila-estrella"><td>${sector}</td><td>${sub}</td><td>${x.NOMBRE}</td>${contenido}</tr>`;
        let c1 = `<div class="col-sm-12 col-md-12 col-lg-2"><div class="form-group mb-1"><label class="estilo-01">${sector} - ${sub}<span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span></label></div></div>`;
        let c2 = `<div class="col-sm-12 col-md-12 col-lg-2"><div class="form-group mb-1"><label class="estilo-01">${x.NOMBRE}</label></div></div>`;
        return `<div class="row">${c1}${c2}${contenido}</div>`;
    }).join('');
    return tc;
}

var cargarComboProceso = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_PROCESO}">${x.NOMBRE}</option>`).join('');
    $(selector).html(items);
}

var nuevo = () => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show(); 
    $('#btnGuardar').next().html('Cancelar');
    $('#etapa-convocatoria').hide();
    $('#proceso-convocatoria').hide();
    $('.relacion-evaluador').addClass('d-none');
    $('#cbo-etapa').val(1);
    $('#cbo-proceso').val(1);
    //$('.postulante-evaluador-btn').addClass('d-none');
    //$('.postulante-evaluador').addClass('d-none');
    $('[id*=chk-r-]').prop('checked', true)
    $('[id*=chk-c-]').prop('checked', true)
    $('[id*=tab-head-cri-] i').removeClass('text-primary').addClass('text-sres-verde')
}

var cerrarFormulario = () => {
    $('#frm').hide();
}

var cerrarRelacion = () => {
    $('.postulante-evaluador').addClass('d-none');
    $('.postulante-evaluador-btn').addClass('d-none');
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txt-titulo').val('');
    $('#txa-descripcion').val('')
    $('#dat-inicio').val('');
    $('#dat-fin').val('');
    $('#txt-capacidad').val('');
    $('.list-req').find('.requerimiento').each((x, y) => { $(y).prop('checked', false); });
    //$('.list-criterio').find('.criterio').each((x, y) => { $(y).prop('checked', false); });
    $('.tab-criterio-content').find('.criterio').each((x, y) => {
        $(y).prop('checked', false);
        $(y).parent().parent().parent().find('.caso').each((w,z) => {
            $(z).prop('checked', false);
            $(z).parent().parent().find('.documento').each((a,b) => {
                $(b).prop('checked', false);
            });
        });
    });
    $('.list-evaluador').find('.evaluador').each((x, y) => { $(y).prop('checked', false); });
    $('.tbl-etapa').find('.etapa').each((x, y) => { $(y).val('') });
    $('.tbl-estrellas').find(`.get-estrella`).each((z,w) => {
        $(w).val(0);
    });
    //$('#tbl-insignia').find('.').each((x, y) => { $(y).val('') });
    //$('.postulante-evaluador').html('');
}

var guardar = () => {
    let id = $('#frm').data('id');
    let ve = 0, vc = 0, vi = 0;
    let arr = []; 
    if ($('#txt-titulo').val().trim() === "") arr.push("Ingrese el título de la convocatoria");
    if ($('#txa-descripcion').val().trim() === "") arr.push("Ingrese la descripción de la convocatoria");
    if ($('#dat-inicio').val() == "") arr.push("Seleccione la fecha de inicio");
    if (id == null) if ($('#dat-inicio').val() != "") if ($('#dat-inicio').val() < utc) arr.push("La fecha de inicio no puede ser menor a la actual");
    if ($('#dat-fin').val() == "") arr.push("Seleccione la fecha de finalización");
    if (id == null) if ($('#dat-fin').val() != "") if ($('#dat-fin').val() < utc) arr.push("La fecha de fin no puede ser menor a la actual");
    if ($('#dat-inicio').val() != "" && $('#dat-fin').val() != "") if ($('#dat-inicio').val() >= $('#dat-fin').val()) arr.push("La fecha de inicio no puede ser mayor o igual a la fecha de fin");
    if ($('#txt-capacidad').val() == "") arr.push("Ingrese la capacidad de postulantes");
    if ($('[id^=chk-e-]').length > 0) { $('[id^=chk-e-]').each((x,y) => {if ($(y).prop('checked')) ve = 1;}); if (ve == 0) arr.push("Seleccione al menos un evaluador")};
    if ($('.criterio').length > 0) { $('.criterio').each((x,y) => {if ($(y).prop('checked')) vc = 1;}); if (vc == 0) arr.push("Seleccione al menos un criterio")};
    if ($('.insignia').length > 0) { $('.insignia').each((x,y) => {if ($(y).val() == "") vi = 1;}); if (vi == 1) arr.push("Debe ingresar el puntaje de cada categoría")}; 

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let nombre = $('#txt-titulo').val();
    let descripcion = $('#txa-descripcion').val();
    let fechaInicio = $('#dat-inicio').val();
    let fechaFin = $('#dat-fin').val();
    let limite = $('#txt-capacidad').val();
    requerimiento = [];
    criterio = [];
    evaluador = [];
    etapa = [];
    insignia = [];
    estrella = [];
    criterioRequerimiento = [];

    $('.list-req').find('.requerimiento').each((x, y) => {
        var r = {
            ID_REQUERIMIENTO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        requerimiento.push(r);
    });

    $('.tab-criterio-content').find('.criterio').each((x, y) => {
        let idcriterio = $(y).attr("id").substring(6, $(y).attr("id").length);
        let arr_caso = [];
        let arr_puntaje = [];
        $(y).parent().parent().parent().find('.caso').each((w,z) => {
            let idcaso = $(z).attr('id').substring($(z).attr('id').indexOf("s")+2,$(z).attr('id').length);
            let arr_doc = [];
            $(z).parent().parent().find('.documento').each((a,b) => {
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
        
        $(y).parent().parent().parent().parent().find('.get-puntaje').each((w,z) => {            
            var d = {
                ID_CRITERIO: idcriterio,
                ID_DETALLE: $(z).attr('id').replace('puntaje-','').split('-')[1],
                PUNTAJE: $(z).val(),
                USUARIO_GUARDAR: idUsuarioLogin
            }
            arr_puntaje.push(d);

        });

        var r = {
            ID_CRITERIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            LISTA_CASO: arr_caso,
            LISTA_CONVCRIPUNT: arr_puntaje,
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0',
            USUARIO_GUARDAR: idUsuarioLogin
        }
        criterio.push(r);
    });
    
    $('.list-evaluador').find('.evaluador').each((x, y) => {
        var r = {
            ID_USUARIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        evaluador.push(r);
    });

    $('.tbl-etapa').find('.etapa').each((x, y) => {
        var r = {
            ID_ETAPA: $(y).attr("id").substring(6, $(y).attr("id").length),
            //DIAS: $(y).val()
            FECHA_ETAPA: $(y).val()
        }
        etapa.push(r);
    });

    $('.tbl-insignia').find('.insignia').each((x, y) => {
        var r = {
            ID_INSIGNIA: $(y).attr("id").replace('txt-i-',''),
            PUNTAJE_MIN: $(y).val()
        }
        insignia.push(r);
    });

    $('.tbl-estrellas').find(`.get-estrella`).each((z,w) => {
        var r = {
            ID_ESTRELLA: $(w).attr("id").replace('estrella-','').split('-')[0],
            ID_TRABAJADORES_CAMA: $(w).attr("id").replace('estrella-','').split('-')[1],
            EMISIONES_MIN: $(w).val()
        }
        estrella.push(r);
    });

    //Array.from($('div[id*="listaRequerimientoCriterio"]')).forEach(x => {
    //    let idCriterio = $(x).parent().find('input[type="checkbox"]').attr('id').replace('chk-c-', '');
    //    Array.from($(x).find('.requerimiento')).forEach(y => {
    //        let idRequerimiento = $(y).attr('id').replace('chk-r-', '');
    //        let obligatorio = $(y).prop('checked').toString()[0].toLocaleUpperCase();
    //        criterioRequerimiento.push({ID_CRITERIO: idCriterio, ID_REQUERIMIENTO: idRequerimiento, OBLIGATORIO: obligatorio, UPD_USUARIO: idUsuarioLogin});
    //    });
    //});

    let url = `${baseUrl}api/convocatoria/guardarconvocatoria`;
    //let data = { ID_CONVOCATORIA: id == null ? -1 : id, ID_ETAPA: $('#cbo-etapa').val(), NOMBRE: nombre, DESCRIPCION: descripcion, FECHA_INICIO: fechaInicio, FECHA_FIN: fechaFin, LIMITE_POSTULANTE: limite, LISTA_REQ: requerimiento, LISTA_CRI: criterio, LISTA_EVA: evaluador, LISTA_ETA: etapa, LISTA_CONVOCATORIA_CRITERIO_REQUERIMIENTO: criterioRequerimiento, LISTA_INSIG: insignia, LISTA_ESTRELLA_TRAB: estrella, USUARIO_GUARDAR: idUsuarioLogin };
    let data = { ID_CONVOCATORIA: id == null ? -1 : id, ID_ETAPA: $('#cbo-etapa').val(), VALIDAR_ETAPA: id == null ? 1 : $('#cbo-etapa').val() == idEtapa_actual ? 0 : 1, NOMBRE: nombre, DESCRIPCION: descripcion, FECHA_INICIO: fechaInicio, FECHA_FIN: fechaFin, LIMITE_POSTULANTE: limite, LISTA_REQ: requerimiento, LISTA_CRI: criterio, LISTA_EVA: evaluador, LISTA_ETA: etapa, LISTA_INSIG: insignia, LISTA_ESTRELLA_TRAB: estrella, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-add').html('');
        if ($('#cbo-etapa').val() == 7 && j.VAL == 0 && j.OK)  $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Ocurrió un problema en el registro del seguimiento en el proceso de Coordinación.' });
        if ($('#cbo-etapa').val() == 14 && j.VAL == 0 && j.OK)  $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Ocurrió un problema en el registro del seguimiento en el proceso de cierre de la convocatoria.' });
        else{
            if (j.OK){ $('#btnGuardar').hide(); $('#btnGuardar').next().html('Cerrar'); }
            j.OK ? $('.alert-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Se guardaron correctamente los datos de la convocatoria.', close: { time: 4000 }, url: `` }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });            
            $('#btnConsultar')[0].click();
        }        
    });
}

var guardarRelacion = () => {
    if ($('.postulante-evaluador').find('.get-institucion').length == 0){
        alert('No hay participantes en esta convocatoria'); return;
    }

    if ($('#cbo-evaluadores-01').val() == 0){
        alert('Debe selecionar a un evaluador para asignar el/los participante(s)'); return;
    }

    let relacion = [];
    //$('.postulante-evaluador').find('.get-valor').each((x, y) => {
    //    var r = {
    //        ID_CONVOCATORIA: $('#frm').data('id'),
    //        ID_INSTITUCION: $(y).find('.get-institucion').attr('id'),
    //        ID_USUARIO: $(`#eva-${$(y).find('.get-institucion').attr('id')}`).val()
    //    }
    //    relacion.push(r);        
    //});

    $('.postulante-evaluador').find('.get-institucion').each((x, y) => {
        debugger;
        if ($(y).prop('checked')){
            var r = {
                ID_CONVOCATORIA: $('#frm').data('id'),
                ID_INSTITUCION: $(y).attr('id').replace('chk-postulante-',''),
                ID_USUARIO: $('#cbo-evaluadores-01').val(),
                USUARIO_GUARDAR: idUsuarioLogin
            }
            relacion.push(r); 
        }               
    });

    let url = `${baseUrl}api/convocatoria/guardarevaluadorpostulante`;
    let data = { LIST_INSTITUCION: relacion, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if (j) {
            $('#cbo-evaluadores-01').val(0);
            $('.postulante-evaluador').html('');
            alert('Se guardó correctamente la relación');
            //cerrarRelacion();
            mostrarNuevaListaPostulantes();
        }
    });
}

var relacionar = () => {
    $('.postulante-evaluador').removeClass('d-none');
    $('.postulante-evaluador-btn').removeClass('d-none');
}

var cambiarColorEstado = (e) => {
    $(e).prop('checked') == true ? $(`#tab-head-cri-0${$(e).attr('id').replace('chk-c-','')} i`).removeClass('text-primary').addClass('text-sres-verde') : $(`#tab-head-cri-0${$(e).attr('id').replace('chk-c-','')} i`).removeClass('text-sres-verde').addClass('text-primary');
}

var mostrarNuevaListaPostulantes = () => {
    let id = $('#frm').data('id');
    let urlConvocatoriaPos = `${baseUrl}api/convocatoria/listarconvocatoriapos?id=${id}`;
    fetch(urlConvocatoriaPos)
    .then(r => r.json())
    .then(r => {
        if (r.length > 0){
            let postulante = r.map((x,y) => {
                return `<div class="col-auto my-1"><div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input get-institucion" type="checkbox" id="chk-postulante-${x.ID_INSTITUCION}"><label class="custom-control-label estilo-01" for="chk-postulante-${x.ID_INSTITUCION}">${x.RAZON_SOCIAL}</label></div></div>`;
            }).join('');    
            $('.postulante-evaluador').html(postulante);
            removerPostulante(r);
        }
    });
}

$(document).on('change', '#cbo-evaluadores-02', function (){
    let idEva = $('#cbo-evaluadores-02').val();
    let id = $('#frm').data('id');
    if ($('#cbo-evaluadores-02').val() > 0){
        $('.postulante-evaluador-relacion').html('');
        let url = `${baseUrl}api/convocatoria/listarpostulanteevaluador?idConvocatoria=${id}&idEvaluador=${idEva}`;
        fetch(url)
        .then(r => r.json())
        .then(r => {
            if (r.length > 0){
                let postulante = r.map((x,y) => {
                    return `<div class="col-auto my-1"><div class="custom-control custom-checkbox mr-sm-2"><input class="custom-control-input get-institucion" type="checkbox" id="chk-postulante-${x.ID_INSTITUCION}"><label class="custom-control-label estilo-01" for="chk-postulante-${x.ID_INSTITUCION}">${x.RAZON_SOCIAL}</label></div></div>`;
                }).join('');
                $('.postulante-evaluador-relacion').html(postulante);
                removerPostulante(r);
            }
        });
    }
});

var limpiarSeccion = () => {
    $('#cbo-evaluadores-02').val(0);
    $('.postulante-evaluador-relacion').html('');
}

var limpiarSeccionPos = () => {
    //mostrarNuevaListaPostulantes();
}

var guardarNoRelacion = () => {
    if ($('.postulante-evaluador-relacion').find('.get-institucion').length == 0){
        alert('No hay participantes en esta convocatoria'); return;
    }

    if ($('#cbo-evaluadores-02').val() == 0){
        alert('Debe selecionar a un evaluador para desasignar el/los participante(s)'); return;
    }

    let relacion = [];
    $('.postulante-evaluador-relacion').find('.get-institucion').each((x, y) => {
        debugger;
        if ($(y).prop('checked')){
            var r = {
                ID_CONVOCATORIA: $('#frm').data('id'),
                ID_INSTITUCION: $(y).attr('id').replace('chk-postulante-',''),
                ID_USUARIO: $('#cbo-evaluadores-02').val(),
                USUARIO_GUARDAR: idUsuarioLogin
            }
            relacion.push(r); 
        }               
    });

    let url = `${baseUrl}api/convocatoria/deseleccionarpostulante`;
    let data = { LIST_INSTITUCION: relacion, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if (j) {
            $('#cbo-evaluadores-02').val(0);
            $('#cbo-evaluadores-01').val(0);
            $('.postulante-evaluador-relacion').html('');
            alert('Se guardó correctamente la relación');
            //cerrarRelacion();
            mostrarNuevaListaPostulantes();
        }
    });
}

$(document).on("keydown", ".solo-numero", function (e) {
    var key = window.e ? e.which : e.keyCode;
    //var id = $("#" + e.target.id)[0].type;
    if ((key < 48 || key > 57) && (event.keyCode < 96 || event.keyCode > 105) && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46) return false;
});

$(document).on("keyup", ".formato-decimal", function (e) {
    $(e.target).val(function (index, value) {
        return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{2})$/, '$1.$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",");
    });
});

function fn_avance_grilla(boton) {
    var total = 0;
    var miPag = 0;
    miPag = Number($("#ir-pagina").val());
    total = Number($(".total-paginas").html());

    if (boton == 1) {
        miPag = 1;
    }
    if (boton == 2) {
        if (miPag > 1) {
            miPag--;
        }
    }
    if (boton == 3) {
        if (miPag < total) {
            miPag++;
        }
    }
    if (boton == 4) {
        miPag = total;
    }
    $("#ir-pagina").val(miPag);
    $('#btnConsultar')[0].click();
}

var cambiarPagina = () => {
    $('#btnConsultar')[0].click();
}

$(".columna-filtro").click(function (e) {
    var id = e.target.id;

    $(".columna-filtro").removeClass("fa-sort-up");
    $(".columna-filtro").removeClass("fa-sort-down");
    $(".columna-filtro").addClass("fa-sort");

    if ($("#columna").val() == id) {
        if ($("#orden").val() == "ASC") {
            $("#orden").val("DESC")
            $(`#${id}`).removeClass("fa-sort");
            $(`#${id}`).removeClass("fa-sort-up");
            $(`#${id}`).addClass("fa-sort-down");
        }
        else {
            $("#orden").val("ASC")
            $(`#${id}`).removeClass("fa-sort");
            $(`#${id}`).removeClass("fa-sort-down");
            $(`#${id}`).addClass("fa-sort-up");
        }
    }
    else {
        $("#columna").val(id);
        $("#orden").val("ASC")
        $(`#${id}`).removeClass("fa-sort");
        $(`#${id}`).removeClass("fa-sort");
        $(`#${id}`).addClass("fa-sort-up");
    }

    $('#btnConsultar')[0].click();
});

var cambiarProceso = () => {    
    $('#cbo-etapa option').prop('hidden', true);
    if ($('#cbo-proceso').val() == 1){
        for(let i=0; i<5; i++) $(`#cbo-etapa option[value=${i+1}]`).prop('hidden', false);
    }else if ($('#cbo-proceso').val() == 2){
        for(let i=0; i<7; i++) $(`#cbo-etapa option[value=${i+6}]`).prop('hidden', false);
    }else if ($('#cbo-proceso').val() == 3){
        for(let i=0; i<2; i++) $(`#cbo-etapa option[value=${i+13}]`).prop('hidden', false);
    }else if ($('#cbo-proceso').val() == 4){
        for(let i=0; i<1; i++) $(`#cbo-etapa option[value=${i+15}]`).prop('hidden', false);
    }
}

var validarEtapa = (idetapa) => {
    if (idetapa < 6) $('#cbo-proceso').val(1);
    else if (idetapa < 13) $('#cbo-proceso').val(2);
    else if (idetapa < 15) $('#cbo-proceso').val(3);
    else $('#cbo-proceso').val(4);
}

$("#modal-mantenimiento").on("hidden.bs.modal", function () {    
    $('#tab-head-01').removeClass('active show');
    $('#tab-01').removeClass('active show');
    $('#tab-head-02').removeClass('active show');
    $('#tab-02').removeClass('active show');
    $('#tab-head-00').addClass('active show');
    $('#tab-00').addClass('active show');
});

//tab-head-00


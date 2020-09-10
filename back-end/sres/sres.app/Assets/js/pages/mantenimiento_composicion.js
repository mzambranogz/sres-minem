$(document).ready(() => {
    $('#ir-pagina').on('change', (e) => cambiarPagina());
    $('#catidad-rgistros').on('change', (e) => cambiarPagina());
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnGuardar').on('click', (e) => guardar());
    $('#cbo-criterio').on('change', (e) => changeCriterio());
    $('#cbo-caso').on('change', (e) => changeCaso());
    $('#cbo-tipo-control').on('change', (e) => changeControl());
    $('#add-columna-detalle-1').on('click', (e) => agregarColumna());
    $('#btnGuardarForm').on('click', (e) => guardarForm());
    consultarListas();
});

var fn_avance_grilla = (boton) => {
    var total = 0, miPag = 0;
    miPag = Number($("#ir-pagina").val());
    total = Number($(".total-paginas").html());

    if (boton == 1) miPag = 1;
    if (boton == 2) if (miPag > 1) miPag--;
    if (boton == 3) if (miPag < total) miPag++;
    if (boton == 4) miPag = total;

    $("#ir-pagina").val(miPag);
    $('#btnConsultar')[0].click();
}

var cambiarPagina = () => {
    $('#btnConsultar')[0].click();
}

$(".columna-filtro").click(function (e) {
    debugger;
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

var consultar = () => {
    let busqueda = $('#txt-descripcion').val();
    let registros = $('#catidad-rgistros').val();
    let pagina = $('#ir-pagina').val();
    let columna = $("#columna").val();
    let orden = $("#orden").val();
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/indicador/buscarindicador?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblPrincipal');
        tabla.find('tbody').html('');
        $('#viewPagination').attr('style', 'display: none !important');
        if (j.length > 0) {
            if (j[0].CANTIDAD_REGISTROS == 0) { $('#viewPagination').hide(); $('#view-page-result').hide(); }
            else { $('#view-page-result').show(); $('#viewPagination').show(); }
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
            $('[data-toggle="tooltip"]').tooltip();
            //tabla.find('.btnCambiarEstado').each(x => {
            //    let elementButton = tabla.find('.btnCambiarEstado')[x];
            //    $(elementButton).on('click', (e) => {
            //        e.preventDefault();
            //        cambiarEstado(e.currentTarget);
            //    });
            //});

            tabla.find('.btnEditar').each(x => {
                let elementButton = tabla.find('.btnEditar')[x];
                $(elementButton).on('click', (e) => {
                    e.preventDefault();
                    consultarDatos(e.currentTarget);
                });
            });
        } else {
            $('#viewPagination').hide(); $('#view-page-result').hide();
            $('.inicio-registros').text('No se encontraron resultados');
        }
    });    
};

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let formatoCodigo = '00000000';
            let detalle = '';
            let colNro = `<td class="text-center" data-encabezado="Número de orden" scope="row" data-count="0">${(pagina - 1) * registros + (i + 1)}</td>`;
            let colCodigo = `<td class="text-center" data-encabezado="Código" scope="row"><span>${(`${formatoCodigo}${x.ID_CRITERIO}${x.ID_CASO}${x.ID_COMPONENTE}`).split('').reverse().join('').substring(0, formatoCodigo.length).split('').reverse().join('')}</span></td>`;
            let colComp = `<td class="text-left" data-encabezado="Componente">${x.COMPONENTE}</td>`;
            let colCaso = `<td class="text-left" data-encabezado="Caso">${x.CASO}</td>`;
            let colCri = `<td class="text-left" data-encabezado="Criterio">${x.CRITERIO}</td>`;
            if (x.LISTA_PARAM != null) {
                if (x.LISTA_PARAM.length > 0) {
                    detalle = x.LISTA_PARAM.map((x, y) => {
                        return `<div class="grupo-columna-03 p-1 text-center"><small>${x.NOMBRE}${x.FORMULA == null ? '' : x.FORMULA == '' ? '' : `<i class="fas fa-square-root-alt cursor-pointer m-2 enfoque-columna-detalle" data-toggle="tooltip" data-placement="top" title="Fórmula: ${x.FORMULA}"></i>`}</small></div>`;
                    }).join('');
                    detalle = `<div class="form-control" ${x.INCREMENTABLE == "0" ? `style="background-color: #C3ECC3"`:``}><div class="list-group sortable-list m-0">${detalle}</div></div>`;
                }
            }
            let colValores = `<td class="text-center" data-encabezado="Valores" ${x.INCREMENTABLE == "0" ? `data-toggle="modal" data-target="#modal-mantenimiento-form" onclick="configurarForm(${x.ID_CRITERIO},${x.ID_CASO},${x.ID_COMPONENTE})"`:``}>${detalle}</td>`
            //let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a class="dropdown-item estilo-01 btnCambiarEstado" href="#" data-id="${x.ID_CRITERIO}-${x.ID_CASO}" data-estado="${x.FLAG_ESTADO}"><i class="fas fa-edit mr-1"></i>Eliminar</a>`}`;
            let btnEditar = `<a class="dropdown-item estilo-01 btnEditar" href="#" data-id="${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}" data-toggle="modal" data-target="#modal-mantenimiento"><i class="fas fa-edit mr-1"></i>Editar</a>`;
            let colOpciones = `<td class="text-center" data-encabezado="Gestión"><div class="btn-group w-100"><a class="btn btn-sm bg-success text-white w-100 dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" tabindex="0">Gestionar</a><div class="dropdown-menu">${btnEditar}</div></div></td>`;
            let fila = `<tr>${colNro}${colCodigo}${colComp}${colCaso}${colCri}${colValores}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var consultarDatos = (element) => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');  
    $('#exampleModalLabel').html('ACTUALIZAR COMPOSICIÓN');

    let criterio = $(element).attr('data-id').split('-')[0];
    let caso = $(element).attr('data-id').split('-')[1];
    let componente = $(element).attr('data-id').split('-')[2];
    let url = `${baseUrl}api/indicador/obtenerindicador?idCriterio=${criterio}&idCaso=${caso}&idcomponente=${componente}`;
    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j, element);
    });
}

var cargarDatos = (data, e) => {
    if (data == null) return false;
    $('#cbo-criterio').prop('disabled', true);
    $('#cbo-caso').prop('disabled', true);
    $('#cbo-componente').prop('disabled', true);
    $('#cbo-criterio').val($(e).attr('data-id').split('-')[0]);
    factores = data.ID_FACTORES == null ? [] : data.ID_FACTORES == "" ? [] : data.ID_FACTORES.split('|');
    idCaso = $(e).attr('data-id').split('-')[1];
    idComponente = $(e).attr('data-id').split('-')[2];
    changeCriterio();
    
    if (data.LISTA_PARAM != null) {
        if (data.LISTA_PARAM.length > 0) {
            data.LISTA_PARAM.map((x, y) => {

                let aLetras = new Array('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j');
                let cLetra = aLetras[Math.floor(Math.random() * aLetras.length)];
                let campo = x.ID_PARAMETRO;
                let nombre_campo = x.NOMBRE;
                let num = Math.round(Math.random() * 100);
                let formula = "";
                let valoresformula = "";
                let id_mrv = `mrv-${campo}${num}${cLetra}`;

                if (x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != ""){
                    $.each(x.FORMULA_ARMADO.split('|'), (x, y) => {
                        let color = y.indexOf('F') != -1 ? "info" : y.indexOf('P') != -1 ? "primary" : y.indexOf('C') != -1 ? "warning" : "secondary";
                        let icono = `<i class="fas fa-2x fa-arrows-alt"></i>`;
                        let small = `<small class="badge badge-${color}">${y}</small>`;
                        let delet = `<i class="fas fa-minus-circle cursor-pointer delete-columna-detalle" data-toggle="tooltip"  data-placement="top" title="" data-enfoque="1" data-original-title="Eliminar"></i>`;
                        let cnt = `<div class="list-group-item sortable-item" data-value="${y}">${icono}${small}${delet}</div>`;
                        formula += y;
                        valoresformula += cnt;
                    });
                }

                let icono = `<i class="fas fa-2x fa-arrows-alt"></i>`;
                let small = `<small>${nombre_campo}</small>`;
                let inputCom = `<input class="hidden-control column-componente" type="hidden" name="" data-cm="${campo}">`;
                let delet = `<i class="fas fa-minus-circle cursor-pointer mr-2 mt-2 delete-columna-detalle" data-toggle="tooltip"  data-placement="top" title="" data-original-title="Eliminar"></i>`;
                let add = `<i class="fas fa-square-root-alt cursor-pointer ml-2 mt-2 enfoque-columna-detalle ${x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != "" ? `text-indigo` : ``}" data-toggle="tooltip"  data-placement="top" title="" data-original-title="Añadir fórmula"></i>`;
                let content = `<div id="mrv-${campo}${num}${cLetra}" class="list-group-item sortable-item recorrer ${x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != "" ? `enfoque-add sortable-chosen` : ``} grupo-columna-03" data-enfoque="${x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != "" ? `1` : ``}" data-resultado="${x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != "" ? `${formula}` : ``}" ${x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != "" ? `data-resultadobd="${x.FORMULA_ARMADO}|"` : ``} draggable="${x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != "" ? `true` : `false`}">${icono}${small}${inputCom}${delet}${add}</div>`;
                $("#columnas-detalle").append(content);

                if (x.FORMULA_ARMADO != null && x.FORMULA_ARMADO != ""){
                    let t = id_mrv, n = valoresformula, m = formula;
                    let o = [n, m];
                    sessionStorage.setItem(t, o);
                }
            });
        }
    }
}

var guardar = () => {
    $('.alert-add').html('');

    let arr = [];
    if ($('#cbo-criterio').val() == 0) arr.push("Seleccione un criterio");
    if ($('#cbo-caso').val() == 0) arr.push("Seleccione un caso");
    if ($('#cbo-componente').val() == 0) arr.push("Seleccione un componente");
    if ($("#columnas-detalle").find(".recorrer").length == 0) arr.push("No ha selecciona ninguna columna en la composición, verifique antes de continuar");

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let id = $('#frm').data('id');
    let criterio = $('#cbo-criterio').val();
    let caso = $('#cbo-caso').val();
    let componente = $('#cbo-componente').val();
    //================================================================

    indicadores = [];
    let i = 1, id_activo = "";
    $("#columnas-detalle").find(".recorrer").each((x,y) => {
        let formula = "", formula_armado = "", ins = 0;
        if ($(y).attr("data-enfoque") == 1) {
            formula = $(y).attr("data-resultado");
            formula_armado = $(y).attr("data-resultadobd").substring(0, $(y).attr("data-resultadobd").length - 1);
            verificarFactor(formula);
            ins = 1;
        }
        
        let itx = {
            ID_CRITERIO: criterio,
            ID_CASO: caso,
            ID_COMPONENTE: componente,
            ID_PARAMETRO: $(y).find(".column-componente").attr("data-cm"),
            ORDEN: i++,
            FORMULA: formula,
            FORMULA_ARMADO: formula_armado,
            //COMPORTAMIENTO: '=',
            //VALOR_FORMULA: 0,
            INS: ins,
            USUARIO_GUARDAR: idUsuarioLogin
        }
        indicadores.push(itx);
        id_activo += $(y).find(".column-componente").attr("data-cm") + ",";
    });
    id_activo = id_activo.substring(0, id_activo.length-1);

    let data = {
        ID_CRITERIO: criterio,
        ID_CASO: caso,
        ID_COMPONENTE: componente,
        LISTA_IND: indicadores,        
        ID_ACTIVO: id_activo,
        ID_FACTORES: factores.length == 0 ? '' : ordenar(factores),
        USUARIO_GUARDAR: idUsuarioLogin
    };
    //==============================================================
    let url = `${baseUrl}api/indicador/guardarindicador`;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-add').html('');
        if (j) { $('#btnGuardar').hide(); $('#btnGuardar').next().html('Cerrar'); }
        j ? $('.alert-add').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los datos fueron guardados correctamente.', close: { time: 1000 }, url: `` }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
        if (j) $('#btnConsultar')[0].click();
    });
}

var nuevo = () => {
    limpiarFormulario();
    $('.alert-add').html('');
    $('#btnGuardar').show();
    $('#btnGuardar').next().html('Cancelar');    
    $('#cbo-criterio').prop('disabled', false);
    $('#cbo-caso').prop('disabled', false);
    $('#cbo-componente').prop('disabled', false);
    //$('#cbo-criterio').parent().parent().show();
    //$('#cbo-caso').parent().parent().show();
    $('#exampleModalLabel').html('REGISTRAR COMPOSICIÓN');
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $("#columnas-detalle").html('');
    factores = [];
    //$('#rad-incrementable').prop('checked', false);
    $('#cbo-criterio').val(0);
    $('#cbo-caso').val(0);
    $('#cbo-componente').val(0);
    $('#cbo-tipo-control').val(1);
    changeControl();
}

var consultarListas = () => {
    let urlCriterio = `${baseUrl}api/criterio/obtenerallcriterio`;
    let urlParametro = `${baseUrl}api/parametro/obtenerallparametro`;
    let urlFactor = `${baseUrl}api/factor/obtenerallfactor`;
    Promise.all([
        fetch(urlCriterio),
        fetch(urlParametro),
        fetch(urlFactor)
    ])
    .then(r => Promise.all(r.map(v => v.json())))
    .then(([jCri, jPar, jFar]) => {
        
        let criterio = ``;
        if (jCri.length > 0) {
            criterio = jCri.map((x, y) => {
                return `<option value="${x.ID_CRITERIO}">${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-criterio').html(`<option value="0">-seleccione-</option>${criterio}`);

        let parametro = ``;
        if (jPar.length > 0) {
            parametro = jPar.map((x, y) => {
                return `<option value="P${x.ID_PARAMETRO}">[P${x.ID_PARAMETRO}] ${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-parametros').html(`<option value="0">-seleccione-</option>${parametro}`);

        let factor = ``;
        if (jFar.length > 0) {
            factor = jFar.map((x, y) => {
                return `<option value="F${x.ID_FACTOR}">[F${x.ID_FACTOR}] ${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-factores').html(`<option value="0">-seleccione-</option>${factor}`);
    });
}

var changeCriterio = () => {
    if ($('#cbo-criterio').val() == 0) { $('#cbo-caso').html(`<option value="0">-Seleccione-</option>`); $('#cbo-componente').html(`<option value="0">-Seleccione-</option>`); return; }
    let url = `${baseUrl}api/caso/obtenercasocriterio?id=${$('#cbo-criterio').val()}`;
    fetch(url).then(r => r.json()).then(j => {
        let contenido = ``;
        if (j.length > 0) {
            contenido = j.map((x, y) => {
                return `<option value="${x.ID_CASO}">${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-caso').html(`<option value="0">-seleccione-</option>${contenido}`);
        if (idCaso > 0) { $('#cbo-caso').val(idCaso); idCaso = 0; changeCaso();}
    });
}

var changeCaso = () => {
    if ($('#cbo-caso').val() == 0) { $('#cbo-componente').html(`<option value="0">-Seleccione-</option>`); return; }
    let url = `${baseUrl}api/componente/obtenercomponentecasocriterio?idCaso=${$('#cbo-caso').val()}&idCriterio=${$('#cbo-criterio').val()}`;
    fetch(url).then(r => r.json()).then(j => {
        let contenido = ``;
        if (j.length > 0) {
            contenido = j.map((x, y) => {
                return `<option value="${x.ID_COMPONENTE}">${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-componente').html(`<option value="0">-seleccione-</option>${contenido}`);
        if (idComponente > 0) { $('#cbo-componente').val(idComponente); idComponente = 0; }
    });
}

var changeControl = () => {
    let url = `${baseUrl}api/parametro/obtenerallparametrolista?idControl=${$('#cbo-tipo-control').val()}`;
    fetch(url).then(r => r.json()).then(j => {
        let contenido = ``;
        if (j.length > 0) {
            contenido = j.map((x, y) => {
                return `<option value="${x.ID_PARAMETRO}">[P${x.ID_PARAMETRO}] ${x.NOMBRE}</option>`;
            }).join('');;
        }
        $('#cbo-campo').html(`<option value="0">-Seleccione-</option>${contenido}`)
    });
}

var agregarColumna = () => {
    if ($("#cbo-campo").val() == 0) return;
    let aLetras = new Array('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j');
    let cLetra = aLetras[Math.floor(Math.random() * aLetras.length)];
    let control = $("#cbo-tipo-control").val();
    let campo = $("#cbo-campo").val();
    let nombre_campo = $("#cbo-campo option:selected").text().split(']')[1];
    let num = Math.round(Math.random() * 100);

    let icono = `<i class="fas fa-2x fa-arrows-alt"></i>`;
    let small = `<small>${nombre_campo}</small>`;
    let inputCom = `<input class="hidden-control column-componente" type="hidden" name="" data-cm="${campo}">`;
    let delet = `<i class="fas fa-minus-circle cursor-pointer mr-2 mt-2 delete-columna-detalle" data-toggle="tooltip"  data-placement="top" title="" data-original-title="Eliminar"></i>`;
    let add = `<i class="fas fa-square-root-alt cursor-pointer ml-2 mt-2 enfoque-columna-detalle" data-toggle="tooltip"  data-placement="top" title="" data-original-title="Añadir fórmula"></i>`;
    let content = `<div id="mrv-${control}${campo}${num}${cLetra}" class="list-group-item sortable-item recorrer grupo-columna-03" data-enfoque="" data-resultado="" draggable="false">${icono}${small}${inputCom}${delet}${add}</div>`;
    $("#columnas-detalle").append(content);
}

var verificarFactor = (f) => {
    if (f == '') return;
    let arr = f.split('['), arrfactor = [];
    $.each(arr, (x,y) => {
        if (y.substring(0,1) == 'F') arrfactor.push(y.substring(1,y.indexOf(']')));
    });    
    if (factores.length == 0) factores = arrfactor;
    else {
        $.each(arrfactor, (x,y) => {
            factores.indexOf(y) == -1 ? factores.push(y) : '';
        });
    }
}

var ordenar = (factores) => {
    factores = factores.sort((x, y) => x - y );
    return factores.join('|');
}

//=================================================================

var configurarForm = (criterio, caso, componente) => {
    $('.alert-add-form').html('');
    $('#btnGuardarForm').show();
    $('#btnGuardarForm').next().html('Cancelar');  
    let url = `${baseUrl}api/indicador/obtenerindicadorform?idCriterio=${criterio}&idCaso=${caso}&idcomponente=${componente}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = j.map((x, i) => {
            idCriterioform = x.ID_CRITERIO;
            idCasoform = x.ID_CASO;
            let head = armarHead(x.LIST_INDICADOR_HEAD, x.INCREMENTABLE, "'" + x.ID_CRITERIO + '-' + x.ID_CASO + '-' + x.ID_COMPONENTE + "'", x.ID_COMPONENTE);
            let body = armarBody(x.LIST_INDICADOR_BODY, x.INCREMENTABLE);
            return `<h3 class="estilo-01 text-sres-azul text-left">${x.CRITERIO == null ? '' : x.CRITERIO.toUpperCase()} - ${x.CASO == null ? '' : x.CASO.toUpperCase()} - ${x.NOMBRE == null ? '' : x.NOMBRE.toUpperCase()}</h3><div class="table-responsive tabla-principal"><table class="table table-sm table-hover m-0 get" id="${x.ID_CRITERIO}-${x.ID_CASO}-${x.ID_COMPONENTE}" data-comp="${x.ID_COMPONENTE}" data-eliminar="">${head}${body}</table></div>`;
        }).join('');
        $("#table-add").html(`${contenido}`);
        $("[data-toggle='tooltip']").tooltip();
    });
}

var armarHead = (lista, incremental, id, componente) => {
    let cont = ``;
    for (var i = 0; i < lista.length; i++) {
        cont += `<th scope="col"><div class="d-flex flex-column justify-content-start align-items-center"><span>${lista[i]["OBJ_PARAMETRO"].NOMBRE}</span>${lista[i]["OBJ_PARAMETRO"].UNIDAD == null ? `` : lista[i]["OBJ_PARAMETRO"].UNIDAD == '' ? `` : `<small>(${lista[i]["OBJ_PARAMETRO"].UNIDAD})</small>`}<i class="fas fa-question-circle mt-2" data-toggle="tooltip" data-placement="bottom" title="${lista[i]["OBJ_PARAMETRO"].DESCRIPCION == null ? '' : lista[i]["OBJ_PARAMETRO"].DESCRIPCION}"></i></div></th>`;
    }
    cont += incremental == '1' ? `<th scope="col"><div class="d-flex flex-column justify-content-center align-items-center"><div class="btn btn-warning btn-sm estilo-01" type="button" onclick="agregarFila(${id},${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div></div></th>` : ``;
    return `<thead class="estilo-06"><tr>${cont}</tr></thead>`;
};

var armarBody = (lista, incremental) => {
    let body = ``;
    for (var i = 0; i < lista.length; i++) {
        body += armarFila(lista[i]["FLAG_NUEVO"] == 0 ? lista[i]["LIST_INDICADORFORM"] : lista[i]["LIST_INDICADORDATA"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"], lista[i]["FLAG_NUEVO"], incremental, lista[i]["FLAG_NUEVO"] == 0 ? lista[i]["ID_INDICADOR"] : (i + 1));
    }
    return `<tbody class="estilo-01">${body}</tbody>`;
};

var armarFila = (lista, id_criterio, id_caso, id_componente, id_indicador, flag_nuevo, incremental, row) => {
    let filas = ``;
    filas += `<tr id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? incremental == '1' ? row : id_indicador : row}" data-ind="${flag_nuevo == 0 ? 0 : id_indicador}">`;
    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["ID_TIPO_CONTROL"] == 2) {
            filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><input class="form-control form-control-sm estilo-01 ${lista[i]["ID_TIPO_DATO"] == '1' ? 'solo-numero text-right' : ''} ${lista[i]["DECIMAL_V"] == null ? '' : lista[i]["DECIMAL_V"] == '1' ? 'formato-decimal text-right' : ''} ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} ${lista[i]["RESULTADO"] == null ? `` : lista[i]["RESULTADO"] == '0' ? `` : ``} ${lista[i]["ESTATICO"] == '1' ?  `alert-warning` : ``} ${lista[i]["EMISIONES"] == null ? `` : lista[i]["EMISIONES"] == '1' ? `get-emisiones` : ``} get-valor" type="${lista[i]["ID_TIPO_DATO"] == '1' ? 'text' : lista[i]["ID_TIPO_DATO"] == '3' ? 'date' : 'text'}" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" value="${validarNull(lista[i]["VALOR"])}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["RESULTADO"] == '1' ? `data-resultado="1"` : `` : ``} ${lista[i]["ID_TIPO_DATO"] == '1' ? lista[i]["OBTENIBLE"] == '1' ? `data-obtenible="1"` : `` : ``} maxlength="${lista[i]["TAMANO"]}" ${lista[i]["VERIFICABLE"] == '1' ? `onBlur="verificarValor(this)"` : ``}  ${lista[i]["EDITABLE"] == '0' ? `readonly` : ``} /></div></td>`;
        } else if (lista[i]["ID_TIPO_CONTROL"] == 1) {
            filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><select class="form-control form-control-sm multi-opciones ${lista[i]["VERIFICABLE"] == '1' ? `verificar` : ``} get-valor" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" data-param="${lista[i]["ID_PARAMETRO"]}" ${lista[i]["FILTRO"] == null ? `` : lista[i]["FILTRO"] == '' ? `` : `data-filtro="${lista[i]["FILTRO"]}" onchange="filtrar(this)"`}  ${lista[i]["VERIFICABLE"] == '1' ? `onchange="verificarValor(this)"` : ``}><option value="0">Seleccione</option>`;
            for (var j = 0; j < lista[i]["LIST_PARAMDET"].length; j++)
                filas += `<option value="${lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"]}" ${validarNull(lista[i]["VALOR"]) == lista[i]["LIST_PARAMDET"][j]["ID_DETALLE"] ? `selected` : ``}>${lista[i]["LIST_PARAMDET"][j]["NOMBRE"]}</option>`;
            filas += `</select></div></td>`;
        } else if (lista[i]["ID_TIPO_CONTROL"] == 3) {
            filas += `<td data-encabezado="${lista[i]["NOMBRE"]}"><div class="form-group m-0"><textarea class="form-control form-control-sm estilo-01 get-valor" cols="30" rows="5" placeholder="Ingrese la descripción" maxlength="${lista[i]["TAMANO"]}" id="${id_criterio}-${id_caso}-${id_componente}-${flag_nuevo == 0 ? id_indicador : row}-${lista[i]["ID_PARAMETRO"]}" data-param="${lista[i]["ID_PARAMETRO"]}">${validarNull(lista[i]["VALOR"])}</textarea></div></td>`;
        }

    }
    filas += incremental == '1' ? `<td><div class="btn btn-info btn-sm estilo-01" type="button" onclick="eliminarFila(this);"><i class="fas fa-minus-circle mr-1"></i>Quitar</div></td>` : ``;
    return `${filas}</tr>`;
}

var validarNull = (valor) => {
    if (valor == null) valor = '';
    return valor;
}

var valorInicial = (idSelect, arr) => {
    let filtro = [];
    let verificar = idSelect.attr('data-filtro') == undefined ? false : true;
    if (!verificar) {
        idSelect.val(0);
        return false;
    } else {
        filtro = idSelect.attr('data-filtro').split('|');
    }

    if (idSelect.val() == 0) {
        for (var i = 0; i < filtro.length; i++) {
            $(`#${arr}-${filtro[i]}`).val(0);
            valorInicial($(`#${arr}-${filtro[i]}`), arr);
        }
        return false;
    }
    return true;
}

var filtrar = (e) => {
    debugger;
    let arr = $(e).parent().parent().parent().attr('id'); //[0] ID_CRITERIO / [1] ID_cASO / [2] ID_COMPONENTE / [3] ID_INDICADOR
    let parametro = $(e).attr('data-param');

    if (!valorInicial($(e), arr)) return false;
    let filtro = $(e).attr('data-filtro').split('|');

    for (var i = 0; i < filtro.length; i++) {
        if ($(e).parent().parent().parent().find('td').find(`[data-param=${filtro[i]}]`).length > 0) {
            let arrFiltro = verificarFiltro(filtro[i], $(e).parent().parent().parent());
            if (arrFiltro.length > 0) {
                var lista = {
                    ID_PARAMETRO: parseInt(filtro[i]),
                    PARAMETROS: arrFiltro[0],
                    DETALLES: arrFiltro[1]
                }
                let url = `${baseUrl}api/parametrodetallerelacion/filtrar`;
                let data = { PARAMDETREL: lista, USUARIO_GUARDAR: idUsuarioLogin };

                let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
                let contenido = ``;
                fetch(url, init)
                .then(r => r.json())
                .then(j => {
                    if (j.length > 0) {
                        contenido = j.map((x, m) => {
                            let opciones = `<option value="${x.ID_DETALLE}" ${(j.length == 1 ? "selected" : "")}>${x.NOMBRE}</option>`;
                            return opciones;
                        }).join('');
                        $(`#${arr}-${j[0].ID_PARAMETRO}`).html(`<option value="0">Seleccione</option>${contenido}`);
                    }
                });
            }
        }
    }
}

var verificarFiltro = (filtro, obj) => {
    let verificar = true;
    let parametros = "";
    let detalles = "";
    let arrFiltros = [];
    obj.find('td').find('[data-filtro]').each((x, y) => {
        let arr = $(y).attr('data-filtro').split('|');
        for (let i = 0; i < arr.length; i++)
            if (filtro == arr[i]) {
                if ($(y).val() > 0) {
                    parametros += $(y).attr('data-param') + '|';
                    detalles += $(y).val() + '|';
                    verificar = true;
                } else
                    verificar = false;
            }
        if (!verificar) return false;
    });
    if (verificar) {
        arrFiltros.push(parametros.substring(0, parametros.length - 1));
        arrFiltros.push(detalles.substring(0, detalles.length - 1));
    }
    return arrFiltros;
}

var formatoMiles = (n) => {
    var m = n * 1;
    return m.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
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

var guardarForm = () => {
    $('.alert-add-form').html('');
    let idCriterio_ = idCriterioform;
    let idCaso = idCasoform;
    componente_ind = [];

    let url = `${baseUrl}api/indicador/guardarindicadorform`;
    $(".get").each((x, y) => {
        indicador_ind = [];
        let componente = $(y).data('comp');
        $(y).find('tbody').find('tr').each((x, y) => {
            indicador_data = [];
            let indicador = $(y).data('ind');
            $(y).find('.get-valor').each((x, y) => {
                var r = {
                    ID_CRITERIO: idCriterio_,
                    ID_CASO: idCaso,
                    ID_COMPONENTE: componente,
                    ID_PARAMETRO: $(y).data('param'),
                    ID_INDICADOR: 1,
                    VALOR: $(y)[0].className.indexOf("multi-opciones") != -1 ? $(y).val() : $(y)[0].className.indexOf("solo-numero") != -1 && $(y)[0].className.indexOf("formato-decimal") != -1 ? $(y).val().replace(/,/gi, '') : $(y).val()
                };
                indicador_data.push(r);
            });
            let ind_r = { LIST_INDICADORFORM: indicador_data, ID_INDICADOR: indicador };
            indicador_ind.push(ind_r);
        });
        let ind = { LIST_INDICADOR: indicador_ind, ID_CRITERIO: idCriterio_, ID_CASO: idCaso, ID_COMPONENTE: componente };
        componente_ind.push(ind);
    });

    let data = { LIST_COMPONENTE: componente_ind, ID_CRITERIO: idCriterio_, ID_CASO: idCaso, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-add-form').html('');
        if (j) { $('#btnGuardarForm').hide(); $('#btnGuardarForm').next().html('Cerrar'); }
        j ? $('.alert-add-form').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los datos fueron guardados correctamente.', close: { time: 1000 }, url: `` }) : $('.alert-add-form').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
        if (j) $('#btnConsultar')[0].click();
    });
}
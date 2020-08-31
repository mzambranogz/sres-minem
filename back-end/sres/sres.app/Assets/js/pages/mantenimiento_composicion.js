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
                    detalle = `<div class="form-control"><div class="list-group sortable-list m-0">${detalle}</div></div>`;
                }
            }
            let colValores = `<td class="text-center" data-encabezado="Valores">${detalle}</td>`
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
    debugger;
    factores = factores.sort((x, y) => x - y );
    return factores.join('|');
}
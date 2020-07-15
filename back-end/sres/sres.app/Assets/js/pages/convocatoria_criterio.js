$(document).ready(() => {
    //$('#btnConsultar').on('click', (e) => consultar());
    //$('#btnConsultar')[0].click();
    //$('#btnNuevo').on('click', (e) => nuevo());
    //$('#btnCerrar').on('click', (e) => cerrarFormulario());
    consultar();
    $('#btnGuardar').on('click', (e) => guardar());    
});

var id_caso_g;//temp

var consultar = () => {
    let id_criterio = idCriterio;
    //let id_caso = 1;
    let id_inscripcion = idInscripcion;
    let params = { id_criterio, id_inscripcion };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/criterio/buscarcriteriocaso?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = j.map((x, i) => {
            id_caso_g = x.ID_CASO; //temp
            let head = armarHead(x.LIST_INDICADOR_HEAD, x.INCREMENTABLE, "'" + x.ID_CRITERIO + '-' + x.ID_CASO + '-' + x.ID_COMPONENTE + "'", x.ID_COMPONENTE);
            let body = armarBody(x.LIST_INDICADOR_BODY, x.INCREMENTABLE);
            return '<div class="table-responsive tabla-principal"><table class="table table-sm table-hover m-0 get" id="' + x.ID_CRITERIO + '-' + x.ID_CASO + '-' + x.ID_COMPONENTE + '" data-comp="' + x.ID_COMPONENTE + '" data-eliminar="">' + head + body + '</table></div>';
        }).join('');
        debugger;
        $("#table-add").html(contenido);

        j.map((x, i) => {
            if (x.INCREMENTABLE == '0')
                armarBodyEstatico(x.LIST_INDICADOR_BODY);
        });

    });


};

var armarHead = (lista, incremental, id, componente) => {
    let cont = `<thead class="estilo-06"><tr>`;
    for (var i = 0; i < lista.length; i++) {
        cont += `<th scope="col" width="20%"><div class="d-flex justify-content-start align-items-center"><i class="fas fa-question-circle mr-1" data-toggle="tooltip" data-placement="bottom" title="Texto de ayuda"></i><span>${lista[i]["OBJ_PARAMETRO"].NOMBRE}</span></div></th>`;
    }
    cont += incremental == '1' ? `<th scope="col" width="20%"><div class="d-flex justify-content-center align-items-center"><div class="btn btn-sres-azul text-white" type="button" onclick="agregarFila(${id},${componente});"><i class="fas fa-plus-circle mr-1"></i>Agregar</div></div></th>` : '';
    return cont + `</tr></thead>`;
};

var armarBody = (lista, incremental) => {
    let body = '<tbody class="estilo-01">';
    debugger;
    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["FLAG_NUEVO"] == 0)
            body += armarFila(lista[i]["LIST_INDICADORFORM"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"], lista[i]["FLAG_NUEVO"], incremental, 0);
        else
            body += armarFila(lista[i]["LIST_INDICADORDATA"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"], lista[i]["FLAG_NUEVO"], incremental, (i + 1));
    }
    body += '</tbody>';
    return body;
};

var armarFila = (lista, id_criterio, id_caso, id_componente, id_indicador, flag_nuevo, incremental, row) => {
    let filas = '';
    if (flag_nuevo == 0) {
        if (incremental == '1')
            filas = '<tr id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + row + '" data-ind="0">';
        else
            filas = '<tr id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '" data-ind="0">';
    } else
        filas = '<tr id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + row + '" data-ind="' + id_indicador + '">';

    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["ESTATICO"] == '1') {
            filas += '<td><span>' + validarNull(lista[i]["VALOR"]) + '</span><input class="get-valor" id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"] + '" value="' + validarNull(lista[i]["VALOR"]) + '" data-param="' + lista[i]["ID_PARAMETRO"] + '" type="hidden" /></td>';
        } else {
            if (incremental == '1')
                filas += '<td data-encabezado="' + lista[i]["NOMBRE"] + '"><div class="form-group m-0"><input class="form-control form-control-sm estilo-01 get-valor" id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + row + '-' + lista[i]["ID_PARAMETRO"] + '" value="' + validarNull(lista[i]["VALOR"]) + '" data-param="' + lista[i]["ID_PARAMETRO"] + '" /></div></td>';
            else
                filas += '<td data-encabezado="' + lista[i]["NOMBRE"] + '"><div class="form-group m-0"><input class="form-control form-control-sm estilo-01 get-valor" id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"] + '" value="' + validarNull(lista[i]["VALOR"]) + '" data-param="' + lista[i]["ID_PARAMETRO"] + '" /></div></td>';
        }
    }

    filas += incremental == '1' ? '<td><div class="btn btn-sres-azul text-white" type="button" onclick="eliminarFila(this);"><i class="fas fa-minus-circle mr-1"></i>Quitar</div></td>' : '';

    filas += '</tr>';
    return filas;
}

var armarBodyEstatico = (lista) => {
    for (var i = 0; i < lista.length; i++) {
        armarFilaEstatico(lista[i]["LIST_INDICADORDATA"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"]);
    }
};

var armarFilaEstatico = (lista, id_criterio, id_caso, id_componente, id_indicador) => {
    $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador).data('ind', id_indicador)

    for (var i = 0; i < lista.length; i++) {
        lista[i]["ESTATICO"] == '1' ? '' : $('#' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"]).val(validarNull(lista[i]["VALOR"]));
    }
}

var validarNull = (valor) => {
    if (valor == null) valor = '';
    return valor;
}

var eliminarFila = (e) => {
    debugger;
    let id = "#" + $(e).parent().parent().attr("id");
    let idTabla = "#" + $(e).parent().parent().parent().parent().attr("id");
    let indicador = $(e).parent().parent().data("ind");
    let validar = $(idTabla).find('tbody').find('tr').length > 1 ? $(id).remove() : 0;
    validar == 0 ? '' : indicador > 0 ? $(idTabla).data("eliminar", $(idTabla).data("eliminar") + indicador.toString() + ",") : '';
    validar == 0 ? '' : ordenarTabla(idTabla);
}

var ordenarTabla = (idTabla) => {
    $(idTabla).find('tbody').find('tr').each((x, y) => {
        let idT = $(y).attr('id').substring(0, $(y).attr('id').length - 1);
        $(idTabla).find('tbody').find('tr').eq(x).find('td').find('input').each((w, z) => {
            let idC = $(z).attr('id').split('-');
            $(idTabla).find('tbody').find('tr').eq(x).find("td").find('input').eq(w).removeAttr('id').attr({ 'id': idT + (x + 1) + '-' + idC[4] });
        });
        $(idTabla).find('tbody').find('tr').eq(x).removeAttr('id').attr({ 'id': idT + (x + 1) });
    });
}

var agregarFila = (id, componente) => {
    let id_criterio = idCriterio;
    let id_caso = id_caso_g;
    let id_componente = componente;
    let params = { id_criterio, id_caso, id_componente };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/criterio/filacriteriocaso?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = armarNuevaFila(j.LIST_INDICADOR_BODY, $("#" + id).find('tbody').find('tr').length + 1);
        $("#" + id).append(contenido);
    });

}

var armarNuevaFila = (lista, row) => {
    debugger;
    let fila = '';
    for (var i = 0; i < lista.length; i++) {
        fila += armarFila(lista[i]["LIST_INDICADORFORM"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], row, 0, '1', row);
    }
    return fila;
};

//=== GUARDAR

var guardar = () => {
    let idCriterio_ = idCriterio;
    let idCaso = id_caso_g;
    let idInscripcion_ = idInscripcion;
    componente_ind = [];

    let url = `/api/criterio/guardarcriteriocaso`;

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
                    ID_INSCRIPCION: idInscripcion_,
                    VALOR: $(y).val()
                };
                indicador_data.push(r);
            });
            let ind_r = { LIST_INDICADORDATA: indicador_data, ID_INDICADOR: indicador };
            indicador_ind.push(ind_r);
        });
        let ind = { LIST_INDICADOR: indicador_ind, ID_CRITERIO: idCriterio_, ID_CASO: idCaso, ID_COMPONENTE: componente, ID_INSCRIPCION: idInscripcion_, ELIMINAR_INDICADOR: $("#" + idCriterio_ + "-" + idCaso + "-" + componente).data("eliminar").substring(0, $("#" + idCriterio_ + "-" + idCaso + "-" + componente).data('eliminar').length - 1) };
        componente_ind.push(ind);
    });

    let data = { LIST_COMPONENTE: componente_ind, USUARIO_GUARDAR: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if (j) {
            j ? $('.alert-add').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los registro de este criterio fueron guardados correctamente.' }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
        }
    });
}
$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    //$('#btnNuevo').on('click', (e) => nuevo());
    //$('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
});

var consultar = () => {
    let id_criterio = 1;
    let id_caso = 1;
    let id_inscripcion = 1;
    let params = { id_criterio, id_caso, id_inscripcion };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/criterio/buscarcriteriocaso?${queryParams}`;
    let contenido = '';
    fetch(url).then(r => r.json()).then(j => {
        contenido = j.map((x, i) => {
            debugger;
            let head = armarHead(x.LIST_INDICADOR_HEAD);
            let body = armarBody(x.LIST_INDICADOR_BODY);
            return '<table class="get" id="' + x.ID_CRITERIO + '-' + x.ID_CASO + '-' + x.ID_COMPONENTE + '" data-comp="' + x.ID_COMPONENTE + '">' + head + body + '</table>';
        }).join('');
        debugger;
        $("#cabecera").html(contenido);
    });


};

var armarHead = (lista) => {
    let cont = '<thead><tr>';
    for (var i = 0; i < lista.length; i++) {
        cont += `<th>${lista[i]["OBJ_PARAMETRO"].NOMBRE}</th>`;
    }
    return cont + '</tr></thead>';
};

var armarBody = (lista) => {
    let body = '<tbody>';
    debugger;
    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["FLAG_NUEVO"] == 0)
            body += armarFila(lista[i]["LIST_INDICADORFORM"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"], lista[i]["FLAG_NUEVO"]);
        else
            body += armarFila(lista[i]["LIST_INDICADORDATA"], lista[i]["ID_CRITERIO"], lista[i]["ID_CASO"], lista[i]["ID_COMPONENTE"], lista[i]["ID_INDICADOR"], lista[i]["FLAG_NUEVO"]);
    }
    body += '</tbody>';
    return body;
};

var armarFila = (lista, id_criterio, id_caso, id_componente, id_indicador, flag_nuevo) => {
    let filas = '';
    if (flag_nuevo == 0)
        filas = '<tr id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '" data-ind="0">';
    else
        filas = '<tr id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '" data-ind="' + id_indicador + '">';

    for (var i = 0; i < lista.length; i++) {
        if (lista[i]["ESTATICO"] == '1') {
            filas += '<td><span>' + validarNull(lista[i]["VALOR"]) + '</span><input class="get-valor" id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"] + '" value="' + validarNull(lista[i]["VALOR"]) + '" data-param="' + lista[i]["ID_PARAMETRO"] + '" type="hidden" /></td>';
        } else {
            filas += '<td><input class="get-valor" id="' + id_criterio + '-' + id_caso + '-' + id_componente + '-' + id_indicador + '-' + lista[i]["ID_PARAMETRO"] + '" value="' + validarNull(lista[i]["VALOR"]) + '" data-param="' + lista[i]["ID_PARAMETRO"] + '" /></td>';
        }
    }
    filas += '</tr>';
    return filas;
}

var validarNull = (valor) => {
    if (valor == null) valor = '';
    return valor;
}

//=== GUARDAR

var guardar = () => {
    let idCriterio = 1;
    let idCaso = 1;
    let idInscripcion = 1;
    componente_ind = [];

    let url = `${baseUrl}api/criterio/guardarcriteriocaso`;

    $(".get").each((x, y) => {
        indicador_ind = [];
        let componente = $(y).data('comp');
        $(y).find('tbody').find('tr').each((x, y) => {
            indicador_data = [];
            let indicador = $(y).data('ind');
            $(y).find('.get-valor').each((x, y) => {
                var r = {
                    ID_CRITERIO: idCriterio,
                    ID_CASO: idCaso,
                    ID_COMPONENTE: componente,
                    ID_PARAMETRO: $(y).data('param'),
                    ID_INSCRIPCION: idInscripcion,
                    VALOR: $(y).val()
                };
                indicador_data.push(r);
            });
            let ind_r = { LIST_INDICADORDATA: indicador_data, ID_INDICADOR: indicador };
            indicador_ind.push(ind_r);
        });
        let ind = { LIST_INDICADOR: indicador_ind, ID_CRITERIO: idCaso, ID_CASO: idCaso, ID_COMPONENTE: componente, ID_INSCRIPCION: idInscripcion };
        componente_ind.push(ind);
    });

    let data = { LIST_COMPONENTE: componente_ind, USUARIO_GUARDAR: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if (j) {
            alert('Se registró correctamente');
            $('#frm').hide();
            $('#btnConsultar')[0].click();
        }
    });
}
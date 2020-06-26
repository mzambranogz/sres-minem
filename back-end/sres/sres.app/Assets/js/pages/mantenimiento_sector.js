﻿$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
});

var consultar = () => {
    let busqueda = $('#textoBusqueda').val();
    let registros = 15;
    let pagina = 1;
    let columna = 'ID_SECTOR';
    let orden = 'ASC'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/sector/buscarsector?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblSector');
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
                consultarCriterio(e.currentTarget);
            });
        });
    });
};

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => {
            let colNro = `<td>${(pagina - 1) * registros + (i + 1)}</td>`;
            let colNombres = `<td>${x.NOMBRE}</td>`;
            let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a href="#" data-id="${x.ID_SECTOR}" data-estado="${x.FLAG_ESTADO}" class="btnCambiarEstado">ELIMINAR</a> `}`;
            let btnEditar = `<a href="#" data-id="${x.ID_SECTOR}" class="btnEditar">EDITAR</a>`;
            let colOpciones = `<td>${btnCambiarEstado}${btnEditar}</td>`;
            let fila = `<tr>${colNro}${colNombres}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var cambiarEstado = (element) => {

    let id = $(element).attr('data-id');

    if (!confirm(`¿Está seguro que desea eliminar este registro?`)) return;

    let data = { ID_SECTOR: id, USUARIO_GUARDAR: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    let url = '/api/sector/cambiarestadosector';

    fetch(url, init)
        .then(r => r.json())
        .then(j => { if (j) $('#btnConsultar')[0].click(); });
};

var nuevo = () => {
    $('#frm').show();
    limpiarFormulario();
}

var cerrarFormulario = () => {
    $('#frm').hide();
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txtSector').val('');
}

var consultarCriterio = (element) => {
    $('#frm').show();
    limpiarFormulario();
    let id = $(element).attr('data-id');

    let url = `/api/sector/obtenersector?id=${id}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    $('#frm').data('id', data.ID_SECTOR);
    $('#txtSector').val(data.NOMBRE);
}

var guardar = () => {
    let id = $('#frm').data('id');
    let nombre = $('#txtSector').val();

    let url = `/api/sector/guardarsector`;

    let data = { ID_SECTOR: id == null ? -1 : id, NOMBRE: nombre, USUARIO_GUARDAR: idUsuarioLogin };

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
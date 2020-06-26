$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
});

var consultar = () => {
    let busqueda = $('#textoBusqueda').val();
    let registros = 10;
    let pagina = 1;
    let columna = 'ID_ROL';
    let orden = 'ASC'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/rol/buscarobjeto?${queryParams}`;

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
                consultarObjeto(e.currentTarget);
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
            let btnEditar = `<a href="#" data-id="${x.ID_ROL}" class="btnEditar">EDITAR</a>`;
            let colOpciones = `<td>${btnEditar}</td>`;
            let fila = `<tr>${colNro}${colNombres}${colOpciones}</tr>`;
            return fila;
        }).join('');
    };

    return contenido;
};

var cerrarFormulario = () => {
    $('#frm').hide();
}

var limpiarFormulario = () => {
    $('#frm').removeData();
    $('#txtRol').val('');
}

var consultarObjeto = (element) => {
    $('#frm').show();
    limpiarFormulario();
    let id = $(element).attr('data-id');

    let url = `/api/rol/obtenerobjeto?id=${id}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    $('#frm').data('id', data.ID_ROL);
    $('#txtRol').val(data.NOMBRE);
}

var guardar = () => {
    let id = $('#frm').data('id');
    let nombre = $('#txtRol').val();

    let url = `/api/rol/guardarobjeto`;

    let data = { ID_ROL: id == null ? -1 : id, NOMBRE: nombre, USUARIO_GUARDAR: idUsuarioLogin };

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
$(document).ready(() => {
    console.log('Hola Mundo');
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
});

var consultar = () => {
    let busqueda = $('#textoBusqueda').val();
    let registros = 10;
    let pagina = 1;
    let columna = 'id_usuario';
    let orden = 'asc'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/usuario/buscarusuario?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblUsuario');
        let cantidadCeldasCabecera = tabla.find('thead tr th').length;
        let contenido = renderizar(j, cantidadCeldasCabecera, pagina, registros);
        tabla.find('tbody').html(contenido);
        tabla.find('.btnCambiarEstado').each(x => {
            let elementButton = tabla.find('.btnCambiarEstado')[x];
            $(elementButton).on('click', (e) => {
                console.log(e);
                e.preventDefault();
                cambiarEstado(e.currentTarget);
            })
        });
    });
};

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => `<tr><td>${(pagina - 1) * registros + (i + 1)}</td><td>${x.NOMBRES}</td><td>${x.APELLIDOS}</td><td>${x.CORREO}</td><td>${x.TELEFONO}</td><td>${x.ANEXO}</td><td>${x.CELULAR}</td><td>${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a href="#" data-id="${x.ID_USUARIO}" data-estado="${x.FLAG_ESTADO}" class="btnCambiarEstado">${x.FLAG_ESTADO == "1" ? "DESHABILITAR" : "HABILITAR"}</a> `}<a href="#" data-id="${x.ID_USUARIO}" class="btnEditar">EDITAR</a></td></tr>`).join('');
    };

    return contenido;
};

var cambiarEstado = (element) => {

    let idUsuario = $(element).attr('data-id');
    let flagEstado = $(element).attr('data-estado');
    flagEstado = flagEstado == "1" ? "0" : "1";

    if (!confirm(`¿Está seguro que desea ${flagEstado == '1' ? 'Deshabilitar' : 'Habilitar'}?`)) return;

    let data = { ID_USUARIO: idUsuario, FLAG_ESTADO: flagEstado, UPD_USUARIO: idUsuarioLogin };

    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    let url = '/api/usuario/cambiarestadousuario';

    fetch(url, init)
        .then(r => r.json())
        .then(j => { if (j) $('#btnConsultar')[0].click(); });
};

var cargarFormulario = () => {

}
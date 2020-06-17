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

    let url = `http://localhost:56109/api/usuario/buscarusuario?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblUsuario');
        let cantidadCeldasCabecera = tabla.find('thead tr th').length;
        let contenido = renderizar(j, cantidadCeldasCabecera, pagina, registros);
        tabla.find('tbody').html(contenido);
    });
};

var renderizar = (data, cantidadCeldas, pagina, registros) => {
    let deboRenderizar = data.length > 0;
    let contenido = `<tr><th colspan='${cantidadCeldas}'>No existe información</th></tr>`;

    if (deboRenderizar) {
        contenido = data.map((x, i) => `<tr><td>${(pagina - 1) * registros + (i+1)}</td><td>${x.NOMBRES}</td><td>${x.APELLIDOS}</td><td>${x.CORREO}</td><td>${x.TELEFONO}</td><td>${x.ANEXO}</td><td>${x.CELULAR}</td><td><a href="#">ELIMINAR</a></td></tr>`).join('');
    };

    return contenido;
};
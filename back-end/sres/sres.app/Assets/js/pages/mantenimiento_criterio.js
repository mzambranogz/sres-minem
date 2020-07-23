﻿$(document).ready(() => {
    $('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
    $('input[type="file"][id="fle-criterio"]').on('change', fileChange);
});

var consultar = () => {
    let busqueda = $('#textoBusqueda').val();
    let registros = 10;
    let pagina = 1;
    let columna = 'ID_CRITERIO';
    let orden = 'ASC'
    let params = { busqueda, registros, pagina, columna, orden };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/criterio/buscarcriterio?${queryParams}`;

    fetch(url).then(r => r.json()).then(j => {
        let tabla = $('#tblCriterio');
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
            let btnCambiarEstado = `${[0, 1].includes(x.FLAG_ESTADO) ? "" : `<a href="#" data-id="${x.ID_CRITERIO}" data-estado="${x.FLAG_ESTADO}" class="btnCambiarEstado">ELIMINAR</a> `}`;
            let btnEditar = `<a href="#" data-id="${x.ID_CRITERIO}" class="btnEditar">EDITAR</a>`;
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
    let data = { ID_CRITERIO: id, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    let url = '/api/criterio/cambiarestadocriterio';
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
    $('#txtCriterio').val('');
    $('#txt-criterio').val('');
    $('#fle-criterio').removeData('file');
}

var consultarCriterio = (element) => {
    $('#frm').show();
    limpiarFormulario();
    let id = $(element).attr('data-id');

    let url = `/api/criterio/obtenercriterio?idCriterio=${id}`;

    fetch(url)
    .then(r => r.json())
    .then(j => {
        cargarDatos(j);
    });
}

var cargarDatos = (data) => {
    $('#frm').data('id_criterio', data.ID_CRITERIO);
    $('#txtCriterio').val(data.NOMBRE);
    $('#txt-criterio').val(data.ARCHIVO_BASE);
    data.ARCHIVO_CONTENIDO == null ? '' : $(`#`).data('file', data.ARCHIVO_CONTENIDO);
}

var guardar = () => {
    let verif = $('#fle-criterio').data('file') != null ? true : false;
    debugger;
    if (!verif) {
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Necesita ingresar una imagen' });
        return;
    }
    let idCriterio = $('#frm').data('id_criterio');
    let nombre = $('#txtCriterio').val();
    let nombrefile = $(`#txt-criterio`).val();
    let archivo = $('#fle-criterio').data('file');
    
    let url = `/api/criterio/guardarcriterio`;
    let data = { ID_CRITERIO: idCriterio == null ? -1 : idCriterio, NOMBRE: nombre, ARCHIVO_BASE: nombrefile, ARCHIVO_CONTENIDO: archivo, USUARIO_GUARDAR: idUsuarioLogin };
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

var fileChange = (e) => {
    let elFile = $(e.currentTarget);

    if (e.currentTarget.files.length == 0) {
        $(e.currentTarget).removeData('file');
        $(e.currentTarget).removeData('fileContent');
        $(e.currentTarget).removeData('type');
        return;
    }

    var fileContent = e.currentTarget.files[0];

    if (fileContent.size > maxBytes) $(elFile).parent().parent().parent().parent().alert({ type: 'danger', title: 'ADVERTENCIA', message: `La imagen debe tener un peso máximo de 4MB` });
    else
        $(elFile).parent().parent().parent().parent().alert('remove');


    //var idElement = $(e.currentTarget).attr("data-id");
    $(`#txt-criterio`).val(fileContent.name);

    let reader = new FileReader();
    reader.onload = function (e) {
        debugger;
        let base64 = e.currentTarget.result.split(',')[1];
        elFile.data('file', base64);
        elFile.data('fileContent', e.currentTarget.result);
        elFile.data('type', fileContent.type);
        //let content = `<label class="estilo-01">&nbsp;</label><div class ="alert alert-success p-1 d-flex"><div class ="mr-auto"><i class ="fas fa-check-circle px-2 py-1"></i><span class="estilo-01">${fileContent.name}</span></div><div class ="ml-auto"><a class ="text-sres-verde" href="${e.currentTarget.result}" download="${fileContent.name}"><i class ="fas fa-download px-2 py-1"></i></a><a class ="text-sres-verde btnEliminarFile" data-id="${idElement}" href="#"><i class ="fas fa-trash px-2 py-1"></i></a></div></div>`
        //$(`#viewContentFile-${idElement}`).html(content);
        //$(`#viewContentFile-${idElement} .btnEliminarFile`).on('click', btnEliminarFileClick);
    }
    reader.readAsDataURL(e.currentTarget.files[0]);
}
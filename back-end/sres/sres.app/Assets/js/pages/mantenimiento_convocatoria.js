$(document).ready(() => {
    //$('#btnConsultar').on('click', (e) => consultar());
    $('#btnConsultar')[0].click();
    $('#btnNuevo').on('click', (e) => nuevo());
    $('#btnCerrar').on('click', (e) => cerrarFormulario());
    $('#btnGuardar').on('click', (e) => guardar());
    consultarAnno('#selIdAnno');
    consultarRequerimiento('#list-req');
    consultarCriterio('#list-criterio');
    consultarEvaluador('#list-evaluador');
});

var consultarAnno = (selector) => {

    let url = `/api/anno/obtenerallanno`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarComboAnno(selector, j));
}

var consultarRequerimiento = (selector) => {

    let url = `/api/requerimiento/obtenerallrequerimiento`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckRequerimiento(selector, j));
}

var consultarCriterio = (selector) => {

    let url = `/api/criterio/obtenerallcriterio`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckCriterio(selector, j));
}

var consultarEvaluador = (selector) => {

    let url = `/api/usuario/obtenerallevaluador`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckEvaluador(selector, j));
}

var cargarComboAnno = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ANNO}">${x.NOMBRE}</option>`).join('');
    $(selector).html(options);
}

var cargarCheckRequerimiento = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="requerimiento" id="chk-r-${x.ID_REQUERIMIENTO}"><label for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckCriterio = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="criterio" id="chk-c-${x.ID_CRITERIO}"><label for="chk-c-${x.ID_CRITERIO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckEvaluador = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" class="evaluador" id="chk-e-${x.ID_USUARIO}"><label for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var nuevo = () => {
    $('#frm').show();
    limpiarFormulario();
    cargarFormulario();
}

var limpiarFormularioUsuario = () => {
    $('#frmUsuario').removeData();
    $('#txtNombres').val('');
    $('#txtApellidos').val('');
    $('#txtCorreo').val('');
    $('#txtTelefono').val('');
    $('#txtAnexo').val('');
    $('#txtCelular').val('');
    $('#txtRucInstitucion').val('');
    $('#txtRazonSocialInstitucion').val('');
    $('#selIdRol').val(null);
}

var cerrarFormulario = () => {
    $('#frm').hide();
}

var guardar = () => {
    let id = $('#frm').data('id');
    requerimiento = [];
    criterio = [];
    evaluador = [];

    $('#list-req').find('.requerimiento').each((x, y) => {
        var r = {
            ID_REQUERIMIENTO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        requerimiento.push(r);
    });

    $('#list-criterio').find('.criterio').each((x, y) => {
        var r = {
            ID_CRITERIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        criterio.push(r);
    });

    $('#list-evaluador').find('.evaluador').each((x, y) => {
        var r = {
            ID_USUARIO: $(y).attr("id").substring(6, $(y).attr("id").length),
            FLAG_ESTADO: $(y).prop('checked') ? '1' : '0'
        }
        evaluador.push(r);
    });
    

    //let nombres = $('#txtNombres').val();
    //let apellidos = $('#txtApellidos').val();
    //let correo = $('#txtCorreo').val();
    //let telefono = $('#txtTelefono').val();
    //let anexo = $('#txtAnexo').val();
    //let celular = $('#txtCelular').val();
    //let idInstitucion = $('#frmUsuario').data('id_institucion');
    //let idRol = $('#selIdRol').val();

    let url = `/api/convocatoria/guardarconvocatoria`;

    let data = { ID_CONVOCATORIA: id == null ? -1 : id, LISTA_REQ: requerimiento, LISTA_CRI: criterio, LISTA_EVA: evaluador, USUARIO_GUARDAR: idUsuarioLogin };
    debugger;
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        if (j) {
            alert('Se registró correctamente');
            cerrarFormulario();
            $('#btnConsultar')[0].click();
        }
    });
}
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

    let url = `/api/convocatoria/obteneranno`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarComboAnno(selector, j));
}

var consultarRequerimiento = (selector) => {

    let url = `/api/convocatoria/obtenerrequerimiento`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckRequerimiento(selector, j));
}

var consultarCriterio = (selector) => {

    let url = `/api/convocatoria/obtenercriterio`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckCriterio(selector, j));
}

var consultarEvaluador = (selector) => {

    let url = `/api/convocatoria/obtenerevaluador`;

    fetch(url)
    .then(r => r.json())
    .then(j => cargarCheckEvaluador(selector, j));
}

var cargarComboAnno = (selector, data) => {
    let options = data.length == 0 ? '' : data.map(x => `<option value="${x.ID_ANNO}">${x.NOMBRE}</option>`).join('');
    $(selector).html(options);
}

var cargarCheckRequerimiento = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" id="chk-r-${x.ID_REQUERIMIENTO}"><label for="chk-r-${x.ID_REQUERIMIENTO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckCriterio = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" id="chk-c-${x.ID_CRITERIO}"><label for="chk-c-${x.ID_CRITERIO}">${x.NOMBRE}&nbsp;</label></li></ul></div></div>`).join('');
    $(selector).html(items);
}

var cargarCheckEvaluador = (selector, data) => {
    let items = data.length == 0 ? '' : data.map(x => `<div><div><ul style="list-style: none;"><li><input type="checkbox" id="chk-e-${x.ID_USUARIO}"><label for="chk-e-${x.ID_USUARIO}">${x.NOMBRE_COMPLETO}&nbsp;</label></li></ul></div></div>`).join('');
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
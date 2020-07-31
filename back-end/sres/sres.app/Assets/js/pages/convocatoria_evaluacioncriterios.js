$(document).ready(() => {
    cargarEvaluacion();
    $('#btnGuardar').on('click', (e) => guardar());
});

var cargarEvaluacion = () => {
    $(`#rad-eva-cri-0${eva}`).prop('checked', true);
}

var guardar = () => {
    var arr = [];
    let idEvaluacion = 0;
    $('input[type="radio"][id*="rad-eva-cri-0"]').each((x, y) => {
        if ($(y).prop('checked')) {
            idEvaluacion = $(y).attr('id').replace('rad-eva-cri-0', '');
        }
    });

    if (idEvaluacion == 0) arr.push('Seleccione un tipo de evaluación (aprobado o desaprobado)');
    if ($(`#txa-observaciones`).val().trim() === "") arr.push('Ingrese la descripción de la observación');

    if (arr.length > 0) {
        let error = '';
        $.each(arr, function (ind, elem) { error += '<li><small class="mb-0">' + elem + '</li></small>'; });
        error = `<ul>${error}</ul>`;
        $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: error });
        return;
    }

    let observacion = $(`#txa-observaciones`).val();
    let url = `/api/criterio/guardarevaluacioncriterios`;
    let data = { ID_INSCRIPCION: idInscripcion, ID_TIPO_EVALUACION: idEvaluacion, OBSERVACION: observacion, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };

    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        j ? $('#btnGuardar').parent().parent().hide() : '';
        j ? $('.alert-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'La evaluación fue guardada correctamente.', close: { time: 4000 }, url: `${baseUrl}Convocatoria` }) : $('.alert-add').html('').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}
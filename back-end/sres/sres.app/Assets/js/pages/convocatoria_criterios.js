$(document).ready(() => {
    $('#btnGuardar').on('click', (e) => guardar());
});

var guardar = () => {
    let url = `/api/convocatoria/guardarconvocatoriaetapainscripcion`;
    let data = { ID_CONVOCATORIA: idConvocatoria, ID_ETAPA: idEtapa, ID_INSCRIPCION: idInscripcion, INGRESADOS: ingresadosCri, TOTAL: totalCri, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        j ? $('#btnGuardar').parent().parent().hide() : '';
        j ? $('.alert-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Los criterios fueron guardados correctamente.', close: { time: 4000 }, url: `${baseUrl}Convocatoria` }) : $('.alert-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}
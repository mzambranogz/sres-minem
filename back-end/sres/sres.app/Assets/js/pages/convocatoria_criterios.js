$(document).ready(() => {
    $('#btnGuardar').on('click', (e) => guardar());
    $('#migrar-emisiones').on('click', (e) => filtrarEmisiones());
    $('#btn-migrar').on('click', (e) => migrarEmisiones());
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

var filtrarEmisiones = () => {
    $('.alert-emisones-add').html('');
    $('#tabla-migrar').find('tbody').html('');
    $('#btn-migrar').parent().show();
    let params = { idIniciativas, rucLogin, idUsuarioLogin };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `/api/mrv/migraremisiones/obtenermigraremisiones?${queryParams}`;
    fetch(url).then(r => r.json()).then(j => {
            if (j.VALIDACION == 1) {

            } else if (j.VALIDACION == 2) {
                let filas = j.LISTA_MIGRAR.map(z => {
                    return `<tr class="get-fila-iniciativas"><td><div class="custom-control custom-checkbox-new custom-checkbox d-inline-block pl-3"><input class="custom-control-input get-chk-ini" type="checkbox" id="chk-send-im-${z.ID_INICIATIVA}-${z.ID_MEDMIT}"><label class="custom-control-label" for="chk-send-im-${z.ID_INICIATIVA}-${z.ID_MEDMIT}">&nbsp;</label></div></td><td><span>${z.ID_INICIATIVA}</span></td><td><span>${z.DESC_INICIATIVA}</span></td><td><span>${z.NOMBRE_MEDMIT}</span></td><td><span>${formatoMiles(z.REDUCIDO)}<input class="get-reducido" value="${z.REDUCIDO}" type="hidden" /></span></td></tr>`;
                });
                $('#tabla-migrar').find('tbody').html(filas);
                mostrarSeleccionado();
            }
    });
}

var mostrarSeleccionado = () => {
    let url = `/api/migraremisiones/mostrarseleccionados?id=${idInscripcion}`;
    fetch(url).then(r => r.json()).then(j => {
        if (j.length > 0) {
            j.map(x => {
                $(`#chk-send-im-${x.ID_INICIATIVA}-${x.ID_MEDMIT}`).prop('checked', true);
            });
        }
    });
}

var migrarEmisiones = () => {
    let emisiones = [];
    $('#tabla-migrar').find('tbody').find('.get-fila-iniciativas').each((x, y) => {
        debugger;
        if ($(y).find('.get-chk-ini').prop('checked')) {
            var r = {
                ID_INSCRIPCION: idInscripcion,
                ID_INICIATIVA: $(y).find('.get-chk-ini').attr('id').replace('chk-send-im-', '').split('-')[0],
                ID_MEDMIT: $(y).find('.get-chk-ini').attr('id').replace('chk-send-im-', '').split('-')[1],
                REDUCIDO: $(y).find('.get-reducido').val()
            }
            emisiones.push(r);
        }
    });

    let url = `/api/migraremisiones/grabarmigraremisiones`;
    let data = { ID_INSCRIPCION: idInscripcion, LISTA_MIGRAR: emisiones, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        j ? $('#btn-migrar').parent().hide() : '';
        j ? $('.alert-emisones-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Se realizó la migración de emisiones correctamente.', close: { time: 4000 }, url: '' }) : $('.alert-emisones-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}

function formatoMiles(n) { //add20
    return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}

$(document).ready(() => {
    $('#btnGuardar').on('click', (e) => guardar());
    $('#migrar-emisiones').on('click', (e) => filtrarEmisiones());
    $('#btn-migrar').on('click', (e) => migrarEmisiones());
});

var guardar = () => {
    let url = `${baseUrl}api/convocatoria/guardarconvocatoriaetapainscripcion`;
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
    $('#btn-migrar').show();
    $('#btn-migrar').next().html('Cancelar');
    $('#tabla-migrar').find('tbody').html('');
    $('#importar-medidas').removeClass('d-none');
    $('.importar-mensaje').removeClass('d-none');
    $('#registrar-mrv').removeClass('d-none').addClass('d-flex');
    $('#btn-migrar').parent().removeClass('d-none').addClass('d-flex');
    //$('#btn-migrar').parent().show();
    let params = { idIniciativas, rucLogin, idUsuarioLogin };
    let queryParams = Object.keys(params).map(x => params[x] == null ? x : `${x}=${params[x]}`).join('&');

    let url = `${baseUrl}api/mrv/migraremisiones/obtenermigraremisiones?${queryParams}`;
    fetch(url).then(r => r.json()).then(j => {
            if (j.VALIDACION == 1) {
                $('#importar-medidas').addClass('d-none');
                $('.importar-mensaje').addClass('d-none');
                $('#btn-migrar').parent().removeClass('d-flex').addClass('d-none');
            } else if (j.VALIDACION == 2) {
                $('#registrar-mrv').removeClass('d-flex').addClass('d-none');
                let filas = j.LISTA_MIGRAR.map((z,w) => {
                    let td1 = `<td class="text-center" data-encabezado="Seleecionar 1"><div class="custom-control custom-checkbox p-0 m-0"><input class ="custom-control-input get-chk-ini" type="checkbox" id="chk-send-im-${z.ID_INICIATIVA}-${z.ID_MEDMIT}"><label class ="custom-control-label" for="chk-send-im-${z.ID_INICIATIVA}-${z.ID_MEDMIT}"></label></div></td>`;
                    let td2 = `<td class="text-center" data-encabezado="Número" scope="row">${w+1}</td>`;
                    let td3 = `<td class="text-center" data-encabezado="Código">${z.ID_INICIATIVA}</td>`;
                    let td4 = `<td class="text-center" data-encabezado="Imagen"><img class="img-thumbnail border-0" src="${baseUrl}${$('#ruta').val().replace('{0}', z.ID_MEDMIT)}/${z.ARCHIVO_BASE == null ? '' : z.ARCHIVO_BASE}"></td>`;
                    let td5 = `<td data-encabezado="Medida de Mitigación">${z.NOMBRE_MEDMIT}</td>`;
                    let td6 = `<td data-encabezado="Emisiones">${formatoMiles(z.REDUCIDO)} tCO<sub>2</sub><input class="get-reducido" value="${z.REDUCIDO}" type="hidden" /></td>`;
                    return tr = `<tr id="detalles-tr-${w + 1}" class="get-fila-iniciativas">${td1}${td2}${td3}${td4}${td5}${td6}</tr>`;
                    //return `<tr class="get-fila-iniciativas"><td><div class="custom-control custom-checkbox-new custom-checkbox d-inline-block pl-3"><input class="custom-control-input get-chk-ini" type="checkbox" id="chk-send-im-${z.ID_INICIATIVA}-${z.ID_MEDMIT}"><label class="custom-control-label" for="chk-send-im-${z.ID_INICIATIVA}-${z.ID_MEDMIT}">&nbsp;</label></div></td><td><span>${z.ID_INICIATIVA}</span></td><td><span>${z.DESC_INICIATIVA}</span></td><td><span>${z.NOMBRE_MEDMIT}</span></td><td><span>${formatoMiles(z.REDUCIDO)}<input class="get-reducido" value="${z.REDUCIDO}" type="hidden" /></span></td></tr>`;
                });
                $('#tabla-migrar').find('tbody').html(filas);
                mostrarSeleccionado();
            }
    });
}

var mostrarSeleccionado = () => {
    let url = `${baseUrl}api/migraremisiones/mostrarseleccionados?id=${idInscripcion}`;
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

    let url = `${baseUrl}api/migraremisiones/grabarmigraremisiones`;
    let data = { ID_INSCRIPCION: idInscripcion, LISTA_MIGRAR: emisiones, USUARIO_GUARDAR: idUsuarioLogin };
    let init = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) };
    fetch(url, init)
    .then(r => r.json())
    .then(j => {
        $('.alert-emisones-add').html('');
        //j ? $('#btn-migrar').parent().hide() : '';
        if (j) { $('#btn-migrar').hide(); $('#btn-migrar').next().html('Cerrar'); }
        j ? $('.alert-emisones-add').html('').alertSuccess({ type: 'success', title: 'BIEN HECHO', message: 'Se realizó la importación de emisiones correctamente.', close: { time: 4000 }, url: '' }) : $('.alert-emisones-add').alertError({ type: 'danger', title: 'ERROR', message: 'Inténtelo nuevamente por favor.' });
    });
}

function formatoMiles(n) { //add20
    return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}

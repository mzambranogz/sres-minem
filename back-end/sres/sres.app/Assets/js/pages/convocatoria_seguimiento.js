$(document).ready(() => {
    consultaInicial();
});

var consultaInicial = () => {
    let url = `${baseUrl}api/inscripcion/inscripciontrazabilidad?idInscripcion=${idInscripcion}`;
    fetch(url)
    .then(r => r.json())
    .then(armarConsulta);
}

var armarConsulta = (data) => {
    if (data == null) return;
    $('.nombre-comercial').html(data.RAZON_SOCIAL == null ? 'Entidad&nbsp;' : `${data.RAZON_SOCIAL}&nbsp;`);

    if (data.LISTA_INSC_TRAZ == null) return;
    if (data.LISTA_INSC_TRAZ.length > 0) {
        let seguimiento = data.LISTA_INSC_TRAZ.map((x, y) => {
            let pusuario = `<p class="description"><span class="badge badge-${x.ID_ROL == 1 ? 'success' : x.ID_ROL == 2 ? 'info' : 'primary'}">${x.ROL}</span></p>`;
            let divcontent = `<div><span><strong>DETALLE</strong><br>${x.DESCRIPCION}<br></span><span><strong>CORREO ELECTRÓNICO</strong><br>${x.CORREO}<br></span></div>`;
            let h5title = `<h5 class="title">${x.ETAPA}<br><small class="text-muted"><br></small></h5>`;
            let timecontent = `<div class="timeline-content">${h5title}${divcontent}${pusuario}</div>`;
            let span = `<span class="year">${x.FECHA_TRAZA}</span>`;
            let time = `<div class="timeline-icon"><i class="${x.ID_ETAPA == 2 ? 'fas fa-user-edit' : x.ID_ETAPA == 3 ? 'fas fa-clipboard-check' : x.ID_ETAPA == 4 ? 'fas fa-file-alt' : x.ID_ETAPA == 5 ? 'fas fa-user-check' : x.ID_ETAPA == 6 ? 'fas fa-info-circle' : x.ID_ETAPA == 7 ? 'fas fa-comments' : x.ID_ETAPA == 8 ? 'fas fa-check' : x.ID_ETAPA == 9 ? 'fas fa-file-signature' : x.ID_ETAPA == 10 ? 'fas fa-file-upload' : x.ID_ETAPA == 11 ? 'fas fa-check-double' : x.ID_ETAPA == 12 ? 'fas fa-file-contract' : x.ID_ETAPA == 13 ? 'fas fa-bullhorn' : x.ID_ETAPA == 14 ? 'fas fa-calendar-check' : 'fas fa-calendar-check'}"></i></div>`;
            let cierre = `<div class="timeline">${time}${span}${timecontent}</div>`;
            return cierre;
        }).join('');
        $('#seguimiento').html(seguimiento);
    }
    
                                    
                                    
                                    
                                        
                                        
                                        
                                    
                                
}
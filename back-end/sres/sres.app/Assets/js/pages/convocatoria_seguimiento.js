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
            let time = `<div class="timeline-icon"><i class="fas fa-save"></i></div>`;
            let cierre = `<div class="timeline">${time}${span}${timecontent}</div>`;
            return cierre;
        }).join('');
        $('#seguimiento').html(seguimiento);
    }
    
                                    
                                    
                                    
                                        
                                        
                                        
                                    
                                
}
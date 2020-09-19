$(document).ready((e) => {
    $('.barra').removeClass('activo');
    $('.barra a').removeClass('nav-active');
    $('.barra-convocatoria').addClass('activo');
    $('.barra-convocatoria a').addClass('nav-active');
    consultarConvocatoria();
});

var consultarConvocatoria = () => {
    let url = `${baseUrl}api/convocatoria/listarconvocatoriainfo`;
    fetch(url)
        .then(r => r.json())
        .then(j => {
            if (j == null) return;
            if (j.length == 0) return;
            let convocatoria = j.map((x, y) => {
                return `<div class="col-lg-4 col-md-6 col-sm-12 mb-5 wow slideInUp"><div class="set-bg convocatoria-prev" data-setbg="${baseUrl}Assets/images/bg-convocatoria.png"></div><a class="estilo-04 text-sres-verde text-limit-2 text-justify text-uppercase" href="${baseUrl}Convocatoria/${x.ID_CONVOCATORIA}/Informacion" target="_">${x.NOMBRE}</a><small class="d-block pt-2 pb-3 estilo-05 text-sres-gris">${x.TXT_FECHA_INICIO}</small><p class="m-0 estilo-01 text-sres-gris text-limit-5 text-justify">${x.DESCRIPCION}</p></div>`;
            }).join('');
            let inicio = `<div class="col-12 mb-5"><h2 class="p-2 mb-4 estilo-04 text-sres-azul rayado wow slideInUp">ÚLTIMAS CONVOCATORIAS</h2></div>`;
            let final = `<div class="col-lg-12 col-md-12 col-sm-12"><div class="rayado">&nbsp;</div></div>`;
            let contenerdor1 = `<div class="container"><div class="row">${inicio}${convocatoria}${final}</div></div>`;
            $('#info-convocatoria').html(contenerdor1);

            $('.set-bg').each(function () {
                var bg = $(this).data('setbg');
                $(this).css({ 'background-image': 'url(' + bg + ')', 'background-size': 'cover', 'background-position': 'center center' });
            });
        });
}
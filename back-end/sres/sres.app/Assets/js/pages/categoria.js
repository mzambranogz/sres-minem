$(document).ready((e) => {
    $('.barra').removeClass('activo');
    $('.barra a').removeClass('nav-active');
    $('.barra-categoria').addClass('activo');
    $('.barra-categoria a').addClass('nav-active');
    scrollToSection();
});

var scrollToSection = (e) => {
    let hash = location.hash;
    if (hash) {
        e.preventDefault();
        let url = $(e.currentTarget).attr("url");
        $('html, body').animate({ scrollTop: $(hash).offset().top }, 'slow');
    }
}

let opcioncombustible = `<select id="cbo-combustible" class="form-control"></select>`;
let opciontransporte = `<select id="cbo-servicio-transporte" class="form-control"></select>`;

let titulo = `<h4 id="vehiculo-01" class="acordeon">Vehículo 01</h4><div class="dropdown-divider"></div>`;
let rowboton = `<div class="col-lg-2"><div class="form-group mt-2"><label class="estilo-01 text-sres-oscuro">&nbsp;</label><div class="input-group"><button class="btn btn-primary w-100" id="btnComenzar" data-toggle="modal" data-target="#modal-mapa">Agregar ruta</button></div></div></div>`;
let rowkilometros = `<div class="col-sm-10 col-md-10 col-lg-10"><div class="form-group mt-2"><label class="estilo-01" for="txt-kilometros">Kilometros recorridos en una semana:</label><span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-comment-alt"></i></span></div><input class="form-control estilo-01 text-sres-gris" type="text" id="txt-kilometros" placeholder="Ingrese los kilómetros recorridos en una semana"></div></div></div>`;
let rowkilometroboton = `<div class="row">${rowkilometros}${rowboton}</div>`;
let rowmeses = `<div class="row"><div class="col-sm-12 col-md-12 col-lg-12"><div class="form-group mt-2"><label class="estilo-01" for="txt-meses">Meses al año que usa este transporte:</label><span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-comment-alt"></i></span></div><input class="form-control estilo-01 text-sres-gris" type="text" id="txt-meses" placeholder="Ingrese los meses que usa este transporte al año"></div></div></div></div>`;
let rowgastosemana = `<div class="row"><div class="col-sm-12 col-md-12 col-lg-12"><div class="form-group mt-2"><label class="estilo-01" for="txt-gasto-semana">Gasto promedio a la semana:</label><span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-comment-alt"></i></span></div><input class="form-control estilo-01 text-sres-gris" type="text" id="txt-gasto-semana" placeholder="Ingrese el gasto promedio a la semana"></div></div></div></div>`;
let rowtipocombustible = `<div class="row"><div class="col-sm-12 col-md-12 col-lg-12"><div class="form-group mt-2"><label class="estilo-01" for="cbo-combustible">Tipo de combustible que usa:</label><span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-comment-alt"></i></span></div>${opcioncombustible}</div></div></div></div>`;
let rowtipotransporte = `<div class="row"><div class="col-sm-12 col-md-12 col-lg-12"><div class="form-group mt-2"><label class="estilo-01" for="cbo-servicio-transporte">Tipo de servicio de transporte:</label><span class="text-danger font-weight-bold">&nbsp;(*)&nbsp;</span><div class="input-group"><div class="input-group-prepend"><span class="input-group-text"><i class="fas fa-comment-alt"></i></span></div>${opciontransporte}</div></div></div></div>`;
let divgrupo = `<div id="grupo-1" class="mr-5 ml-5 pr-5 pl-5">${rowtipotransporte}${rowtipocombustible}</div>`;
        
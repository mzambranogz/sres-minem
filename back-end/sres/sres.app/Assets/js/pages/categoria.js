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
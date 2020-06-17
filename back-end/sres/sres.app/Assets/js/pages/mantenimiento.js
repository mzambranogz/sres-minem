function fn_irMantenimiento(e) {
    e.preventDefault();
    let valorMantenimiento = $("#cbo-tabla-mantenimiento").val();
    let urlMantenimiento = $("#cbo-tabla-mantenimiento option:checked").attr("data-url");
    let redirectUrl = `${baseUrl}${urlMantenimiento}`;

    if (valorMantenimiento != "0") {
        location.href = redirectUrl;
    }
}
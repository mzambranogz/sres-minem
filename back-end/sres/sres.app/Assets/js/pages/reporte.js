$(document).ready(() => {
    $("#ir-reporte").click((e) => {
        debugger;
        e.preventDefault();
        let url = `${baseUrl}${$("#cbo-tabla-mantenimiento option:selected").attr("data-url")}`;
        fetch(url)
        .then(r => r.text())
        .then((html) => {
            $("#report-view").html(html);
        })

    })
});

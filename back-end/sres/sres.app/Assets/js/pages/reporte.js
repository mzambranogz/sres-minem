$(document).ready(() => {
    $("#ir-reporte").click(irReporte);
});

var irReporte = (e) => {
    e.preventDefault();
    $("#iframe-report-view").css({ "height": "0px" });
    let dataUrl = $("#cbo-tabla-mantenimiento option:selected").attr("data-url");
    if (dataUrl == "") return;
    let url = `${baseUrl}${dataUrl}`;
    $("#iframe-report-view").attr("src", url).on('load', function () {
        //$("#ir-reporte").html("<i class='far fa-hand-point-up px-1'></i> Ir al reporte");
        $("#iframe-report-view").css({ "height": "700px" });
    });
    //fetch(url)
    //.then(r => r.text())
    //.then((html) => {
    //    $("#report-view").html(html);
    //})

};

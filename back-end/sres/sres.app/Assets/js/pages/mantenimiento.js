function fn_irMantenimiento() {
    if ($("#cbo-tabla-mantenimiento").val() == 1) {
        location.href = baseUrl + "Mantenimiento/Criterio";
    }
}
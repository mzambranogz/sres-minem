$('#frmLogin').submit((e) => {
    let tokenValue = $('#token').val();

    if (tokenValue == "") {
        e.preventDefault();

        grecaptcha.ready(() => {
            grecaptcha.execute(key).then((token) => {
                $('#token').val(token);
                $('#frmLogin').submit();
            });
        });
    }
});
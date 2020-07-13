$.fn.alert = function (options) {
    this.defaults = {
        type: '',
        title: '',
        message: '',
        close: {
            time: 3000
        }
    };

    var settings = $.extends({}, $.fn.alert.defaults, options);

    let element = $(`<div class="alert alert-${type} d-flex align-items-stretch" role="alert"></div>`);
    let optionsContent = `<div class="alert-wrap mr-3"><div class="sa"><div class="sa-error"><div class="sa-error-x"><div class="sa-error-left"></div><div class="sa-error-right"></div></div><div class="sa-error-placeholder"></div><div class="sa-error-fix"></div></div></div></div>`;
    let titleContent = `<h6 class="estilo-02">${title}</h6>`;
    let messageContent = `<small class="mb-0 estilo-01">${message}</small>`;
    let content = `${optionsContent}<div class="alert-wrap">${titleContent}<hr class="my-1">${messageContent}</div>`;
    element.html(content);
    $(selector).after(element);
    setTimeout(() => { element.remove(); }, closeTime);
    return this;
};
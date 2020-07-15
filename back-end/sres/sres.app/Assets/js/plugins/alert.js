(function($){
    $.fn.alert = function (options) {
        this._remove = () => {
            let nextElement = this.next();
            if (nextElement.hasClass('alert')) nextElement.remove();
        };

        this._get = () => {
            let nextElement = this.next();
            if (nextElement.hasClass('alert')) return nextElement;
        }

        if (typeof options == 'string') {
            let value = this[`_${options}`]();
            return value;
        }

        this.defaults = {
            type: 'success',
            title: 'Titulo',
            message: 'Mensaje',
            html: '',
            close: null
        };

        var settings = $.extend($.fn.alert.defaults, options);
        let element = $(`<div class="alert alert-${settings.type} d-flex align-items-stretch" role="alert"></div>`);
        let optionsContent = `<div class="alert-wrap mr-3"><div class="sa"><div class="sa-${settings.type.replace('danger', 'error')}"><div class="sa-${settings.type.replace('danger', 'error')}-x"><div class="sa-${settings.type.replace('danger', 'error')}-left"></div><div class="sa-${settings.type.replace('danger', 'error')}-right"></div></div><div class="sa-${settings.type.replace('danger', 'error')}-placeholder"></div><div class="sa-${settings.type.replace('danger', 'error')}-fix"></div></div></div></div>`;
        let titleContent = `<h6 class="estilo-02">${settings.title}</h6>`;
        let messageContent = `<small class="mb-0 estilo-01">${settings.message}</small>`;
        let content = `${optionsContent}<div class="alert-wrap">${titleContent}<hr class="my-1">${messageContent}${settings.html || ''}</div>`;
        element.html(content);
        let nextElement = this.next();
        if (nextElement.hasClass('alert')) nextElement.remove();
        this.after(element);
        if(settings.close != null) setTimeout(() => { element.remove(); }, settings.close.time);
        //return this;
    }
})(jQuery);
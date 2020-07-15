(function ($) {
    $.fn.counter = function (options) {
        this.defaults = {
            start: 0,
            end: 1,
            time: 1000,
            callback: () => { }
        };

        var settings = $.extend($.fn.alert.defaults, options);

        var current = settings.start;
        this.timeout = () => {
            this.html(current);
            if (current == settings.end) {
                clearInterval(sto);
                settings.callback();
            }
            else {
                let increment = settings.start > settings.end ? -1 : 1;
                current += increment;
            }
        };
        var sto = setInterval(this.timeout, settings.time);
    }
})(jQuery);
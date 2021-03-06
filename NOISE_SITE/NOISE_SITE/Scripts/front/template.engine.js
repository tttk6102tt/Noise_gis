;
(function () {
    'use strict';

    var namespace = window,
        pluginName = 'TemplateEngine';

    var TemplateEngine = function TemplateEngine(options) {
        if (!(this instanceof TemplateEngine)) {
            return new TemplateEngine(options);
        }

        this.settings = $.extend({}, TemplateEngine.Defaults, options);
        this._storage = {};

        return this;
    };

    TemplateEngine.Defaults = {
        templateDir: '/content/public/tpl/',
        templateExt: '.tpl',
        cache: false
    };

    TemplateEngine.prototype = {
        constructor: TemplateEngine,

        load: function (name, $deferred) {
            var self = this;
            $deferred = $deferred || $.Deferred();

            if (self.settings.cache) {
                if (self.isCached(name)) {
                    $deferred.resolve(self._storage[name]);
                } else {
                    $.ajax(self.urlFor(name)).done(function (raw) {
                        self.store(name, raw);
                        self.load(name, $deferred);
                    });
                }
            } else {
                $.ajax(self.urlFor(name)).done(function (raw) {
                    $deferred.resolve(raw);
                });
            }

            return $deferred.promise();
        },
        fetch: function (name) {
            var self = this;

            $.ajax(self.urlFor(name)).done(function (raw) {
                self.store(name, raw);
            });
        },
        isCached: function (name) {
            return !!this._storage[name]
        },
        store: function (name, raw) {
            this._storage[name] = raw;
        },
        urlFor: function (name) {
            return (this.settings.templateDir + name + this.settings.templateExt);
        }
    };


    window[pluginName] = TemplateEngine;

})(jQuery);
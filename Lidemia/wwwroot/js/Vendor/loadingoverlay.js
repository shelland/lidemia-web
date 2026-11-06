/***************************************************************************************************
LoadingOverlay - a flexible loading overlay, rewritten in plain JavaScript (no jQuery)
    Based on   : jquery-loading-overlay 2.1.7 by Gaspare Sganga (MIT)
    License    : MIT
    Usage      :
        var overlay = new LoadingOverlay(elementOrSelector, options);
        overlay.show();
        overlay.hide();

        LoadingOverlay.show({ text: "Loading..." });    // whole page
        LoadingOverlay.progress(50);
        LoadingOverlay.hide();

        LoadingOverlay.get(element).destroy();
        LoadingOverlay.setup({ background: "rgba(0, 0, 0, 0.4)" });   // new defaults
***************************************************************************************************/
(function (global) {
    "use strict";

    // Default Settings
    var defaults = {
        // Background
        background              : "rgba(255, 255, 255, 0.8)",
        backgroundClass         : "",
        // Image
        image                   : "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1000 1000'><circle r='80' cx='500' cy='90'/><circle r='80' cx='500' cy='910'/><circle r='80' cx='90' cy='500'/><circle r='80' cx='910' cy='500'/><circle r='80' cx='212' cy='212'/><circle r='80' cx='788' cy='212'/><circle r='80' cx='212' cy='788'/><circle r='80' cx='788' cy='788'/></svg>",
        imageAnimation          : "2000ms rotate_right",
        imageAutoResize         : true,
        imageResizeFactor       : 1,
        imageColor              : "#202020",
        imageClass              : "",
        imageOrder              : 1,
        // Font Awesome
        fontawesome             : "",
        fontawesomeAnimation    : "",
        fontawesomeAutoResize   : true,
        fontawesomeResizeFactor : 1,
        fontawesomeColor        : "#202020",
        fontawesomeOrder        : 2,
        // Custom
        custom                  : "",
        customAnimation         : "",
        customAutoResize        : true,
        customResizeFactor      : 1,
        customOrder             : 3,
        // Text
        text                    : "",
        textAnimation           : "",
        textAutoResize          : true,
        textResizeFactor        : 0.5,
        textColor               : "#202020",
        textClass               : "",
        textOrder               : 4,
        // Progress
        progress                : false,
        progressAutoResize      : true,
        progressResizeFactor    : 0.25,
        progressColor           : "#a0a0a0",
        progressClass           : "",
        progressOrder           : 5,
        progressFixedPosition   : "",
        progressSpeed           : 200,
        progressMin             : 0,
        progressMax             : 100,
        // Sizing
        size                    : 50,
        maxSize                 : 120,
        minSize                 : 20,
        // Misc
        direction               : "column",
        fade                    : true,
        resizeInterval          : 50,
        zIndex                  : 2147483647
    };

    // Required CSS (camelCase CSSStyleDeclaration properties)
    var css = {
        overlay : {
            boxSizing       : "border-box",
            position        : "relative",
            display         : "flex",
            flexWrap        : "nowrap",
            alignItems      : "center",
            justifyContent  : "space-around"
        },
        overlayWholePage : {
            position : "fixed",
            top      : "0",
            left     : "0",
            width    : "100%",
            height   : "100%"
        },
        element : {
            boxSizing       : "border-box",
            overflow        : "visible",
            flex            : "0 0 auto",
            display         : "flex",
            justifyContent  : "center",
            alignItems      : "center"
        },
        elementSvg : {
            width  : "100%",
            height : "100%"
        },
        progressFixed : {
            position : "absolute",
            left     : "0",
            width    : "100%"
        },
        progressWrapper : {
            position : "absolute",
            top      : "0",
            left     : "0",
            width    : "100%",
            height   : "100%"
        },
        progressBar : {
            position : "absolute",
            left     : "0"
        }
    };

    // Whitelists
    var whitelists = {
        animations : [
            "rotate_right",
            "rotate_left",
            "fadein",
            "pulse"
        ],
        progressPosition : [
            "top",
            "bottom"
        ]
    };

    // Default Values
    var defaultValues = {
        animations : {
            name    : "rotate_right",
            time    : "2000ms"
        },
        fade : [400, 200]
    };

    // Supported css size units, longest first so "vmin" is matched before "in"
    var sizeUnits = ["vmin", "vmax", "rem", "px", "em", "cm", "mm", "in", "pt", "pc", "vh", "vw"];

    var keyframes = [
        "@keyframes loadingoverlay_animation__rotate_right {",
            "to {",
                "transform : rotate(360deg);",
            "}",
        "}",
        "@keyframes loadingoverlay_animation__rotate_left {",
            "to {",
                "transform : rotate(-360deg);",
            "}",
        "}",
        "@keyframes loadingoverlay_animation__fadein {",
            "0% {",
                "opacity   : 0;",
                "transform : scale(0.1, 0.1);",
            "}",
            "50% {",
                "opacity   : 1;",
            "}",
            "100% {",
                "opacity   : 0;",
                "transform : scale(1, 1);",
            "}",
        "}",
        "@keyframes loadingoverlay_animation__pulse {",
            "0% {",
                "transform : scale(0, 0);",
            "}",
            "50% {",
                "transform : scale(1, 1);",
            "}",
            "100% {",
                "transform : scale(0, 0);",
            "}",
        "}"
    ].join(" ");

    var instances   = new WeakMap();    // container element -> LoadingOverlay
    var registry    = new Set();        // every instance that currently owns an overlay element
    var elementMeta = new WeakMap();    // overlay element -> { autoResize, resizeFactor }
    var fadeTimers  = new WeakMap();    // overlay element -> pending fade timeout id


    function LoadingOverlay(target, options) {
        var container = resolveElement(target);
        if (!container) {
            throw new Error("LoadingOverlay: container element was not found.");
        }

        // One overlay per container: reuse the existing instance
        var existing = instances.get(container);
        if (existing) {
            return existing;
        }

        this.container          = container;
        this.wholePage          = container === document.body;
        this.options            = deepMerge({}, options || {});
        this.settings           = null;
        this.overlay            = null;
        this.textElement        = null;
        this.progressState      = null;
        this.count              = 0;
        this.resizeIntervalId   = null;

        instances.set(container, this);
    }

    // Public API

    LoadingOverlay.prototype.show = function (options) {
        if (!this.overlay) {
            if (options) {
                deepMerge(this.options, options);
            }
            cleanupDetached();
            this._create();
        }
        this.count++;
        this._syncVisibility();
        return this;
    };

    LoadingOverlay.prototype.hide = function (force) {
        if (!this.overlay) {
            cleanupDetached();
            return this;
        }

        this._syncVisibility();
        this.count--;
        if (!force && this.count > 0) {
            return this;
        }

        var self = this;
        fadeTo(this.overlay, this.settings.fade[1], 0, function () {
            if (self.count > 0) {
                // Shown again while the overlay was fading out
                fadeTo(self.overlay, self.settings.fade[0], 1);
                return;
            }
            self.destroy();
        });
        return this;
    };

    LoadingOverlay.prototype.resize = function () {
        if (!this.overlay) {
            cleanupDetached();
            return this;
        }
        this._syncVisibility();
        this._intervalResize(true);
        return this;
    };

    LoadingOverlay.prototype.text = function (value) {
        if (!this.overlay) {
            cleanupDetached();
            return this;
        }
        this._syncVisibility();
        if (!this.textElement) {
            return this;
        }
        if (value === false) {
            this.textElement.style.display = "none";
        } else {
            this.textElement.style.display = "";
            this.textElement.textContent = value;
        }
        return this;
    };

    LoadingOverlay.prototype.progress = function (value) {
        if (!this.overlay) {
            cleanupDetached();
            return this;
        }
        this._syncVisibility();
        if (!this.progressState) {
            return this;
        }

        var bar = this.progressState.bar;
        if (value === false) {
            bar.style.display = "none";
            return this;
        }

        var state = this.progressState;
        var percent = ((parseFloat(value) || 0) - state.min) * 100 / (state.max - state.min);
        if (!isFinite(percent)) {
            percent = 0;
        }
        percent = Math.max(0, Math.min(100, percent));

        bar.style.display = "";
        if (state.speed > 0) {
            bar.style.transition = "width " + state.speed + "ms ease";
            void bar.offsetWidth;
        }
        bar.style.width = percent + "%";
        return this;
    };

    LoadingOverlay.prototype.destroy = function () {
        if (this.resizeIntervalId) {
            global.clearInterval(this.resizeIntervalId);
            this.resizeIntervalId = null;
        }
        if (this.overlay) {
            stopFade(this.overlay);
            this.overlay.remove();
        }
        registry.delete(this);
        if (instances.get(this.container) === this) {
            instances.delete(this.container);
        }
        this.overlay        = null;
        this.settings       = null;
        this.textElement    = null;
        this.progressState  = null;
        this.count          = 0;
        return this;
    };

    // Build the overlay

    LoadingOverlay.prototype._create = function () {
        var settings = this.settings = prepareSettings(deepMerge(deepMerge({}, defaults), this.options));

        var overlay = document.createElement("div");
        overlay.className = "loadingoverlay";
        applyStyles(overlay, css.overlay);
        overlay.style.flexDirection = String(settings.direction).toLowerCase() === "row" ? "row" : "column";
        if (settings.backgroundClass) {
            addClasses(overlay, settings.backgroundClass);
        } else {
            overlay.style.background = settings.background;
        }
        if (this.wholePage) {
            applyStyles(overlay, css.overlayWholePage);
        }
        if (typeof settings.zIndex !== "undefined") {
            overlay.style.zIndex = settings.zIndex;
        }

        // Image
        if (settings.image) {
            var imageElement = createElement(overlay, settings.imageOrder, settings.imageAutoResize, settings.imageResizeFactor, settings.imageAnimation);
            var image       = String(settings.image);
            var imageLower  = image.toLowerCase();
            var imageColor  = normalizeImageColor(settings.imageColor);

            if (imageLower.indexOf("<svg") === 0 && imageLower.slice(-6) === "</svg>") {
                // Inline SVG
                imageElement.innerHTML = image;
                applySvgStyles(imageElement, imageColor, settings.imageClass);
            } else if (imageLower.slice(-4) === ".svg" || imageLower.indexOf("data:image/svg") === 0) {
                // SVG file or base64-encoded SVG
                fetchText(image, function (svg) {
                    imageElement.innerHTML = svg;
                    applySvgStyles(imageElement, imageColor, settings.imageClass);
                });
            } else {
                // Raster
                applyStyles(imageElement, {
                    backgroundImage    : "url(" + image + ")",
                    backgroundPosition : "center",
                    backgroundRepeat   : "no-repeat",
                    backgroundSize     : "cover"
                });
            }
            if (settings.imageClass) {
                addClasses(imageElement, settings.imageClass);
            }
        }

        // Font Awesome
        if (settings.fontawesome) {
            var faElement = createElement(overlay, settings.fontawesomeOrder, settings.fontawesomeAutoResize, settings.fontawesomeResizeFactor, settings.fontawesomeAnimation);
            faElement.classList.add("loadingoverlay_fa");
            var icon = document.createElement("div");
            icon.className = settings.fontawesome;
            faElement.appendChild(icon);
            if (settings.fontawesomeColor) {
                faElement.style.color = settings.fontawesomeColor;
            }
        }

        // Custom
        if (settings.custom) {
            var customElement = createElement(overlay, settings.customOrder, settings.customAutoResize, settings.customResizeFactor, settings.customAnimation);
            if (typeof settings.custom === "string") {
                customElement.innerHTML = settings.custom;
            } else {
                customElement.appendChild(settings.custom);
            }
        }

        // Text
        if (settings.text) {
            this.textElement = createElement(overlay, settings.textOrder, settings.textAutoResize, settings.textResizeFactor, settings.textAnimation);
            this.textElement.classList.add("loadingoverlay_text");
            this.textElement.textContent = settings.text;
            if (settings.textClass) {
                addClasses(this.textElement, settings.textClass);
            } else if (settings.textColor) {
                this.textElement.style.color = settings.textColor;
            }
        }

        // Progress
        if (settings.progress) {
            var progressElement = createElement(overlay, settings.progressOrder, settings.progressAutoResize, settings.progressResizeFactor, false);
            progressElement.classList.add("loadingoverlay_progress");

            var wrapper = document.createElement("div");
            applyStyles(wrapper, css.progressWrapper);
            progressElement.appendChild(wrapper);

            var bar = document.createElement("div");
            applyStyles(bar, css.progressBar);
            wrapper.appendChild(bar);

            this.progressState = {
                element : progressElement,
                bar     : bar,
                fixed   : false,
                margin  : 0,
                min     : parseFloat(settings.progressMin),
                max     : parseFloat(settings.progressMax),
                speed   : parseInt(settings.progressSpeed, 10) || 0
            };

            var parts = String(settings.progressFixedPosition).replace(/\s\s+/g, " ").toLowerCase().split(" ");
            if (parts.length === 2 && validateProgressPosition(parts[0])) {
                this.progressState.fixed  = parts[0];
                this.progressState.margin = parseSize(parts[1]);
            } else if (parts.length === 2 && validateProgressPosition(parts[1])) {
                this.progressState.fixed  = parts[1];
                this.progressState.margin = parseSize(parts[0]);
            } else if (parts.length === 1 && validateProgressPosition(parts[0])) {
                this.progressState.fixed  = parts[0];
                this.progressState.margin = 0;
            }

            if (this.progressState.fixed === "top") {
                applyStyles(progressElement, css.progressFixed);
                progressElement.style.top = progressMarginValue(this.progressState.margin);
            } else if (this.progressState.fixed === "bottom") {
                applyStyles(progressElement, css.progressFixed);
                progressElement.style.top = "auto";
            }

            if (settings.progressClass) {
                addClasses(bar, settings.progressClass);
            } else if (settings.progressColor) {
                bar.style.background = settings.progressColor;
            }
        }

        // Attach and fade in
        this.overlay = overlay;
        registry.add(this);
        overlay.style.opacity = "0.01";
        (document.body || document.documentElement).appendChild(overlay);

        this._intervalResize(true);
        if (settings.resizeInterval > 0) {
            var self = this;
            this.resizeIntervalId = global.setInterval(function () {
                self._intervalResize(false);
            }, settings.resizeInterval);
        }

        fadeTo(overlay, settings.fade[0], 1);
    };

    // Sizing / positioning

    LoadingOverlay.prototype._intervalResize = function (force) {
        var overlay = this.overlay;
        if (!overlay) {
            return;
        }

        var settings  = this.settings;
        var container = this.container;
        var containerStyles = global.getComputedStyle(container);

        // Overlay
        if (!this.wholePage) {
            var isFixed = containerStyles.position === "fixed";
            var rect    = container.getBoundingClientRect();
            var scrollX = global.pageXOffset || document.documentElement.scrollLeft || 0;
            var scrollY = global.pageYOffset || document.documentElement.scrollTop || 0;
            var top     = isFixed ? rect.top : rect.top + scrollY;
            var left    = isFixed ? rect.left : rect.left + scrollX;

            overlay.style.position = isFixed ? "fixed" : "absolute";
            overlay.style.top      = (top + (parseFloat(containerStyles.borderTopWidth) || 0)) + "px";
            overlay.style.left     = (left + (parseFloat(containerStyles.borderLeftWidth) || 0)) + "px";
            overlay.style.width    = container.clientWidth + "px";
            overlay.style.height   = container.clientHeight + "px";
        }

        // Elements
        if (!settings.size) {
            return;
        }

        var size = settings.size.value;
        if (!settings.size.fixed) {
            var availableWidth  = this.wholePage ? global.innerWidth : container.clientWidth;
            var availableHeight = this.wholePage ? global.innerHeight : container.clientHeight;
            size = Math.min(availableWidth, availableHeight) * size / 100;
            if (settings.maxSize && size > settings.maxSize) size = settings.maxSize;
            if (settings.minSize && size < settings.minSize) size = settings.minSize;
        }
        var units = settings.size.units;

        var self = this;
        Array.prototype.forEach.call(overlay.children, function (child) {
            if (!child.classList.contains("loadingoverlay_element")) {
                return;
            }

            var meta = elementMeta.get(child) || {};
            if (!force && !meta.autoResize) {
                return;
            }

            var factor = (typeof meta.resizeFactor === "number") ? meta.resizeFactor : 1;
            var amount = size * factor;

            if (child.classList.contains("loadingoverlay_fa") || child.classList.contains("loadingoverlay_text")) {
                child.style.fontSize = amount + units;
            } else if (child.classList.contains("loadingoverlay_progress")) {
                var progress = self.progressState;
                if (!progress) {
                    return;
                }
                progress.bar.style.height = amount + units;
                if (!progress.fixed) {
                    var barTop = child.offsetTop - amount * 0.5;
                    progress.bar.style.top = barTop + "px";
                } else if (progress.fixed === "bottom") {
                    var margin = progress.margin ? parseFloat(progress.margin.value) || 0 : 0;
                    child.style.bottom = (margin + amount) + units;
                }
            } else {
                child.style.width  = amount + units;
                child.style.height = amount + units;
            }
        });
    };

    LoadingOverlay.prototype._syncVisibility = function () {
        if (!this.overlay) {
            return;
        }
        // Mirror the container visibility, like the jQuery version did
        this.overlay.style.display = this.container.getClientRects().length > 0 ? "flex" : "none";
    };


    // Static helpers

    LoadingOverlay.setup = function (settings) {
        deepMerge(defaults, settings);
        return LoadingOverlay;
    };

    LoadingOverlay.show = function (options) {
        return pageInstance(options).show();
    };

    LoadingOverlay.hide = function (force) {
        var instance = instances.get(document.body);
        if (instance) {
            instance.hide(force);
        }
        return LoadingOverlay;
    };

    LoadingOverlay.resize = function () {
        var instance = instances.get(document.body);
        if (instance) {
            instance.resize();
        }
        return LoadingOverlay;
    };

    LoadingOverlay.text = function (value) {
        var instance = instances.get(document.body);
        if (instance) {
            instance.text(value);
        }
        return LoadingOverlay;
    };

    LoadingOverlay.progress = function (value) {
        var instance = instances.get(document.body);
        if (instance) {
            instance.progress(value);
        }
        return LoadingOverlay;
    };

    LoadingOverlay.get = function (target) {
        var container = resolveElement(target);
        return container ? instances.get(container) || null : null;
    };


    // Internals

    function pageInstance(options) {
        var instance = instances.get(document.body);
        if (instance) {
            if (options) {
                deepMerge(instance.options, options);
            }
            return instance;
        }
        return new LoadingOverlay(document.body, options);
    }

    function resolveElement(target) {
        if (target === undefined || target === null) {
            return document.body;
        }
        if (typeof target === "string") {
            return document.querySelector(target);
        }
        if (typeof target.nodeType === "number") {
            return target;
        }
        return null;
    }

    function cleanupDetached() {
        // Remove overlays whose container is no longer in the document
        registry.forEach(function (instance) {
            if (!document.documentElement.contains(instance.container)) {
                instance.destroy();
            }
        });
    }

    function prepareSettings(settings) {
        settings.size           = parseSize(settings.size);
        settings.maxSize        = parseInt(settings.maxSize, 10) || 0;
        settings.minSize        = parseInt(settings.minSize, 10) || 0;
        settings.resizeInterval = parseInt(settings.resizeInterval, 10) || 0;
        settings.fade           = normalizeFade(settings.fade);
        return settings;
    }

    function normalizeFade(fade) {
        if (!fade) {
            return [0, 0];
        }
        if (fade === true) {
            return defaultValues.fade.slice();
        }
        if (typeof fade === "string" || typeof fade === "number") {
            return [parseInt(fade, 10) || 0, parseInt(fade, 10) || 0];
        }
        var values = Array.isArray(fade) ? fade : [fade, fade];
        var from   = parseInt(values[0], 10) || 0;
        var to     = (values.length > 1) ? parseInt(values[1], 10) || 0 : from;
        return [from, to];
    }

    function normalizeImageColor(color) {
        if (Array.isArray(color)) {
            if (color.length === 0) {
                return false;
            }
            if (color.length === 1) {
                return { fill: color[0] };
            }
            return { fill: color[0], stroke: color[1] };
        }
        return color ? { fill: color } : false;
    }

    function applySvgStyles(element, color, hasCustomClass) {
        Array.prototype.forEach.call(element.children, function (child) {
            applyStyles(child, css.elementSvg);
        });
        if (!hasCustomClass && color) {
            Array.prototype.forEach.call(element.querySelectorAll("*"), function (node) {
                if (color.fill) {
                    node.style.fill = color.fill;
                }
                if (color.stroke) {
                    node.style.stroke = color.stroke;
                }
            });
        }
    }

    function fetchText(url, onSuccess) {
        if (typeof global.fetch !== "function") {
            return;
        }
        global.fetch(url, { credentials: "same-origin" })
            .then(function (response) {
                return response.ok ? response.text() : null;
            })
            .then(function (text) {
                if (text) {
                    onSuccess(text);
                }
            })
            .catch(function () {
                // Keep the overlay without the image, same as the jQuery version
            });
    }

    function createElement(parent, order, autoResize, resizeFactor, animation) {
        var element = document.createElement("div");
        element.className = "loadingoverlay_element";
        applyStyles(element, css.element);
        element.style.order = order;
        elementMeta.set(element, {
            autoResize   : autoResize,
            resizeFactor : resizeFactor
        });
        parent.appendChild(element);

        if (animation === true) {
            animation = defaultValues.animations.time + " " + defaultValues.animations.name;
        }
        if (typeof animation === "string" && animation) {
            var parsed = parseAnimation(animation);
            if (parsed) {
                element.style.animationName            = "loadingoverlay_animation__" + parsed.name;
                element.style.animationDuration        = parsed.time;
                element.style.animationTimingFunction  = "linear";
                element.style.animationIterationCount  = "infinite";
            }
        }

        return element;
    }

    function parseAnimation(animation) {
        var parts = animation.replace(/\s\s+/g, " ").toLowerCase().split(" ");

        if (parts.length === 2 && validateCssTime(parts[0]) && validateAnimation(parts[1])) {
            return { name: parts[1], time: parts[0] };
        }
        if (parts.length === 2 && validateCssTime(parts[1]) && validateAnimation(parts[0])) {
            return { name: parts[0], time: parts[1] };
        }
        if (parts.length === 1 && validateCssTime(parts[0])) {
            return { name: defaultValues.animations.name, time: parts[0] };
        }
        if (parts.length === 1 && validateAnimation(parts[0])) {
            return { name: parts[0], time: defaultValues.animations.time };
        }
        return null;
    }

    function validateCssTime(value) {
        return !isNaN(parseFloat(value)) && (value.slice(-1) === "s" || value.slice(-2) === "ms");
    }

    function validateAnimation(value) {
        return whitelists.animations.indexOf(value) > -1;
    }

    function validateProgressPosition(value) {
        return whitelists.progressPosition.indexOf(value) > -1;
    }

    function parseSize(value) {
        if (!value || value < 0) {
            return false;
        }
        if (typeof value === "string") {
            var normalized = value.trim().toLowerCase();
            for (var i = 0; i < sizeUnits.length; i++) {
                var unit = sizeUnits[i];
                if (normalized.length > unit.length && normalized.slice(-unit.length) === unit) {
                    var amount = parseFloat(normalized.slice(0, -unit.length));
                    if (!isNaN(amount)) {
                        return { fixed: true, units: unit, value: amount };
                    }
                }
            }
        }
        var numeric = parseFloat(value);
        if (isNaN(numeric)) {
            return false;
        }
        return { fixed: false, units: "px", value: numeric };
    }

    function progressMarginValue(margin) {
        if (!margin) {
            return "0";
        }
        return margin.value + (margin.fixed ? margin.units : "%");
    }

    function applyStyles(element, styles) {
        Object.keys(styles).forEach(function (name) {
            element.style[name] = styles[name];
        });
    }

    function addClasses(element, value) {
        String(value).split(/\s+/).forEach(function (name) {
            if (name) {
                element.classList.add(name);
            }
        });
    }

    function fadeTo(element, duration, opacity, done) {
        stopFade(element);

        var milliseconds = parseInt(duration, 10) || 0;
        if (milliseconds <= 0) {
            element.style.opacity = opacity;
            if (done) {
                done();
            }
            return;
        }

        element.style.transition = "opacity " + milliseconds + "ms linear";
        void element.offsetWidth;   // flush styles so the transition starts from the current opacity
        element.style.opacity = opacity;

        fadeTimers.set(element, global.setTimeout(function () {
            fadeTimers.delete(element);
            element.style.transition = "";
            if (done) {
                done();
            }
        }, milliseconds + 20));
    }

    function stopFade(element) {
        var timerId = fadeTimers.get(element);
        if (timerId) {
            global.clearTimeout(timerId);
            fadeTimers.delete(element);
        }
        element.style.transition = "";
    }

    function isPlainObject(value) {
        return value !== null && typeof value === "object" && !Array.isArray(value) && typeof value.nodeType !== "number";
    }

    function deepMerge(target, source) {
        if (!source) {
            return target;
        }
        Object.keys(source).forEach(function (key) {
            var value = source[key];
            if (value === undefined) {
                return;
            }
            if (isPlainObject(value)) {
                target[key] = deepMerge(isPlainObject(target[key]) ? target[key] : {}, value);
            } else if (Array.isArray(value)) {
                target[key] = value.slice();
            } else {
                target[key] = value;
            }
        });
        return target;
    }

    function injectKeyframes() {
        if (!document.head || document.getElementById("loadingoverlay-animation-styles")) {
            return;
        }
        var style = document.createElement("style");
        style.id = "loadingoverlay-animation-styles";
        style.textContent = keyframes;
        document.head.appendChild(style);
    }

    if (document.head) {
        injectKeyframes();
    } else {
        document.addEventListener("DOMContentLoaded", injectKeyframes);
    }

    global.LoadingOverlay = LoadingOverlay;

})(window);

    if ("undefined" == typeof jQuery) throw new Error("Bootstrap's JavaScript requires jQuery"); + function(a) {
    "use strict";
    var b = a.fn.jquery.split(" ")[0].split(".");
    if (b[0] < 2 && b[1] < 9 || 1 == b[0] && 9 == b[1] && b[2] < 1 || b[0] > 2) throw new Error("Bootstrap's JavaScript requires jQuery version 1.9.1 or higher, but lower than version 3")
}(jQuery), + function(a) {
    "use strict";

    function b() {
        var a = document.createElement("bootstrap"),
            b = {
                WebkitTransition: "webkitTransitionEnd",
                MozTransition: "transitionend",
                OTransition: "oTransitionEnd otransitionend",
                transition: "transitionend"
            };
        for (var c in b)
            if (void 0 !== a.style[c]) return {
                end: b[c]
            };
        return !1
    }
    a.fn.emulateTransitionEnd = function(b) {
        var c = !1,
            d = this;
        a(this).one("bsTransitionEnd", function() {
            c = !0
        });
        var e = function() {
            c || a(d).trigger(a.support.transition.end)
        };
        return setTimeout(e, b), this
    }, a(function() {
        a.support.transition = b(), a.support.transition && (a.event.special.bsTransitionEnd = {
            bindType: a.support.transition.end,
            delegateType: a.support.transition.end,
            handle: function(b) {
                return a(b.target).is(this) ? b.handleObj.handler.apply(this, arguments) : void 0
            }
        })
    })
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var c = a(this),
                e = c.data("bs.alert");
            e || c.data("bs.alert", e = new d(this)), "string" == typeof b && e[b].call(c)
        })
    }
    var c = '[data-dismiss="alert"]',
        d = function(b) {
            a(b).on("click", c, this.close)
        };
    d.VERSION = "3.3.6", d.TRANSITION_DURATION = 150, d.prototype.close = function(b) {
        function c() {
            g.detach().trigger("closed.bs.alert").remove()
        }
        var e = a(this),
            f = e.attr("data-target");
        f || (f = e.attr("href"), f = f && f.replace(/.*(?=#[^\s]*$)/, ""));
        var g = a(f);
        b && b.preventDefault(), g.length || (g = e.closest(".alert")), g.trigger(b = a.Event("close.bs.alert")), b.isDefaultPrevented() || (g.removeClass("in"), a.support.transition && g.hasClass("fade") ? g.one("bsTransitionEnd", c).emulateTransitionEnd(d.TRANSITION_DURATION) : c())
    };
    var e = a.fn.alert;
    a.fn.alert = b, a.fn.alert.Constructor = d, a.fn.alert.noConflict = function() {
        return a.fn.alert = e, this
    }, a(document).on("click.bs.alert.data-api", c, d.prototype.close)
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.button"),
                f = "object" == typeof b && b;
            e || d.data("bs.button", e = new c(this, f)), "toggle" == b ? e.toggle() : b && e.setState(b)
        })
    }
    var c = function(b, d) {
        this.$element = a(b), this.options = a.extend({}, c.DEFAULTS, d), this.isLoading = !1
    };
    c.VERSION = "3.3.6", c.DEFAULTS = {
        loadingText: "loading..."
    }, c.prototype.setState = function(b) {
        var c = "disabled",
            d = this.$element,
            e = d.is("input") ? "val" : "html",
            f = d.data();
        b += "Text", null == f.resetText && d.data("resetText", d[e]()), setTimeout(a.proxy(function() {
            d[e](null == f[b] ? this.options[b] : f[b]), "loadingText" == b ? (this.isLoading = !0, d.addClass(c).attr(c, c)) : this.isLoading && (this.isLoading = !1, d.removeClass(c).removeAttr(c))
        }, this), 0)
    }, c.prototype.toggle = function() {
        var a = !0,
            b = this.$element.closest('[data-toggle="buttons"]');
        if (b.length) {
            var c = this.$element.find("input");
            "radio" == c.prop("type") ? (c.prop("checked") && (a = !1), b.find(".active").removeClass("active"), this.$element.addClass("active")) : "checkbox" == c.prop("type") && (c.prop("checked") !== this.$element.hasClass("active") && (a = !1), this.$element.toggleClass("active")), c.prop("checked", this.$element.hasClass("active")), a && c.trigger("change")
        } else this.$element.attr("aria-pressed", !this.$element.hasClass("active")), this.$element.toggleClass("active")
    };
    var d = a.fn.button;
    a.fn.button = b, a.fn.button.Constructor = c, a.fn.button.noConflict = function() {
        return a.fn.button = d, this
    }, a(document).on("click.bs.button.data-api", '[data-toggle^="button"]', function(c) {
        var d = a(c.target);
        d.hasClass("btn") || (d = d.closest(".btn")), b.call(d, "toggle"), a(c.target).is('input[type="radio"]') || a(c.target).is('input[type="checkbox"]') || c.preventDefault()
    }).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]', function(b) {
        a(b.target).closest(".btn").toggleClass("focus", /^focus(in)?$/.test(b.type))
    })
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.carousel"),
                f = a.extend({}, c.DEFAULTS, d.data(), "object" == typeof b && b),
                g = "string" == typeof b ? b : f.slide;
            e || d.data("bs.carousel", e = new c(this, f)), "number" == typeof b ? e.to(b) : g ? e[g]() : f.interval && e.pause().cycle()
        })
    }
    var c = function(b, c) {
        this.$element = a(b), this.$indicators = this.$element.find(".carousel-indicators"), this.options = c, this.paused = null, this.sliding = null, this.interval = null, this.$active = null, this.$items = null, this.options.keyboard && this.$element.on("keydown.bs.carousel", a.proxy(this.keydown, this)), "hover" == this.options.pause && !("ontouchstart" in document.documentElement) && this.$element.on("mouseenter.bs.carousel", a.proxy(this.pause, this)).on("mouseleave.bs.carousel", a.proxy(this.cycle, this))
    };
    c.VERSION = "3.3.6", c.TRANSITION_DURATION = 600, c.DEFAULTS = {
        interval: 5e3,
        pause: "hover",
        wrap: !0,
        keyboard: !0
    }, c.prototype.keydown = function(a) {
        if (!/input|textarea/i.test(a.target.tagName)) {
            switch (a.which) {
                case 37:
                    this.prev();
                    break;
                case 39:
                    this.next();
                    break;
                default:
                    return
            }
            a.preventDefault()
        }
    }, c.prototype.cycle = function(b) {
        return b || (this.paused = !1), this.interval && clearInterval(this.interval), this.options.interval && !this.paused && (this.interval = setInterval(a.proxy(this.next, this), this.options.interval)), this
    }, c.prototype.getItemIndex = function(a) {
        return this.$items = a.parent().children(".item"), this.$items.index(a || this.$active)
    }, c.prototype.getItemForDirection = function(a, b) {
        var c = this.getItemIndex(b),
            d = "prev" == a && 0 === c || "next" == a && c == this.$items.length - 1;
        if (d && !this.options.wrap) return b;
        var e = "prev" == a ? -1 : 1,
            f = (c + e) % this.$items.length;
        return this.$items.eq(f)
    }, c.prototype.to = function(a) {
        var b = this,
            c = this.getItemIndex(this.$active = this.$element.find(".item.active"));
        return a > this.$items.length - 1 || 0 > a ? void 0 : this.sliding ? this.$element.one("slid.bs.carousel", function() {
            b.to(a)
        }) : c == a ? this.pause().cycle() : this.slide(a > c ? "next" : "prev", this.$items.eq(a))
    }, c.prototype.pause = function(b) {
        return b || (this.paused = !0), this.$element.find(".next, .prev").length && a.support.transition && (this.$element.trigger(a.support.transition.end), this.cycle(!0)), this.interval = clearInterval(this.interval), this
    }, c.prototype.next = function() {
        return this.sliding ? void 0 : this.slide("next")
    }, c.prototype.prev = function() {
        return this.sliding ? void 0 : this.slide("prev")
    }, c.prototype.slide = function(b, d) {
        var e = this.$element.find(".item.active"),
            f = d || this.getItemForDirection(b, e),
            g = this.interval,
            h = "next" == b ? "left" : "right",
            i = this;
        if (f.hasClass("active")) return this.sliding = !1;
        var j = f[0],
            k = a.Event("slide.bs.carousel", {
                relatedTarget: j,
                direction: h
            });
        if (this.$element.trigger(k), !k.isDefaultPrevented()) {
            if (this.sliding = !0, g && this.pause(), this.$indicators.length) {
                this.$indicators.find(".active").removeClass("active");
                var l = a(this.$indicators.children()[this.getItemIndex(f)]);
                l && l.addClass("active")
            }
            var m = a.Event("slid.bs.carousel", {
                relatedTarget: j,
                direction: h
            });
            return a.support.transition && this.$element.hasClass("slide") ? (f.addClass(b), f[0].offsetWidth, e.addClass(h), f.addClass(h), e.one("bsTransitionEnd", function() {
                f.removeClass([b, h].join(" ")).addClass("active"), e.removeClass(["active", h].join(" ")), i.sliding = !1, setTimeout(function() {
                    i.$element.trigger(m)
                }, 0)
            }).emulateTransitionEnd(c.TRANSITION_DURATION)) : (e.removeClass("active"), f.addClass("active"), this.sliding = !1, this.$element.trigger(m)), g && this.cycle(), this
        }
    };
    var d = a.fn.carousel;
    a.fn.carousel = b, a.fn.carousel.Constructor = c, a.fn.carousel.noConflict = function() {
        return a.fn.carousel = d, this
    };
    var e = function(c) {
        var d, e = a(this),
            f = a(e.attr("data-target") || (d = e.attr("href")) && d.replace(/.*(?=#[^\s]+$)/, ""));
        if (f.hasClass("carousel")) {
            var g = a.extend({}, f.data(), e.data()),
                h = e.attr("data-slide-to");
            h && (g.interval = !1), b.call(f, g), h && f.data("bs.carousel").to(h), c.preventDefault()
        }
    };
    a(document).on("click.bs.carousel.data-api", "[data-slide]", e).on("click.bs.carousel.data-api", "[data-slide-to]", e), a(window).on("load", function() {
        a('[data-ride="carousel"]').each(function() {
            var c = a(this);
            b.call(c, c.data())
        })
    })
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        var c, d = b.attr("data-target") || (c = b.attr("href")) && c.replace(/.*(?=#[^\s]+$)/, "");
        return a(d)
    }

    function c(b) {
        return this.each(function() {
            var c = a(this),
                e = c.data("bs.collapse"),
                f = a.extend({}, d.DEFAULTS, c.data(), "object" == typeof b && b);
            !e && f.toggle && /show|hide/.test(b) && (f.toggle = !1), e || c.data("bs.collapse", e = new d(this, f)), "string" == typeof b && e[b]()
        })
    }
    var d = function(b, c) {
        this.$element = a(b), this.options = a.extend({}, d.DEFAULTS, c), this.$trigger = a('[data-toggle="collapse"][href="#' + b.id + '"],[data-toggle="collapse"][data-target="#' + b.id + '"]'), this.transitioning = null, this.options.parent ? this.$parent = this.getParent() : this.addAriaAndCollapsedClass(this.$element, this.$trigger), this.options.toggle && this.toggle()
    };
    d.VERSION = "3.3.6", d.TRANSITION_DURATION = 350, d.DEFAULTS = {
        toggle: !0
    }, d.prototype.dimension = function() {
        var a = this.$element.hasClass("width");
        return a ? "width" : "height"
    }, d.prototype.show = function() {
        if (!this.transitioning && !this.$element.hasClass("in")) {
            var b, e = this.$parent && this.$parent.children(".panel").children(".in, .collapsing");
            if (!(e && e.length && (b = e.data("bs.collapse"), b && b.transitioning))) {
                var f = a.Event("show.bs.collapse");
                if (this.$element.trigger(f), !f.isDefaultPrevented()) {
                    e && e.length && (c.call(e, "hide"), b || e.data("bs.collapse", null));
                    var g = this.dimension();
                    this.$element.removeClass("collapse").addClass("collapsing")[g](0).attr("aria-expanded", !0), this.$trigger.removeClass("collapsed").attr("aria-expanded", !0), this.transitioning = 1;
                    var h = function() {
                        this.$element.removeClass("collapsing").addClass("collapse in")[g](""), this.transitioning = 0, this.$element.trigger("shown.bs.collapse")
                    };
                    if (!a.support.transition) return h.call(this);
                    var i = a.camelCase(["scroll", g].join("-"));
                    this.$element.one("bsTransitionEnd", a.proxy(h, this)).emulateTransitionEnd(d.TRANSITION_DURATION)[g](this.$element[0][i])
                }
            }
        }
    }, d.prototype.hide = function() {
        if (!this.transitioning && this.$element.hasClass("in")) {
            var b = a.Event("hide.bs.collapse");
            if (this.$element.trigger(b), !b.isDefaultPrevented()) {
                var c = this.dimension();
                this.$element[c](this.$element[c]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse in").attr("aria-expanded", !1), this.$trigger.addClass("collapsed").attr("aria-expanded", !1), this.transitioning = 1;
                var e = function() {
                    this.transitioning = 0, this.$element.removeClass("collapsing").addClass("collapse").trigger("hidden.bs.collapse")
                };
                return a.support.transition ? void this.$element[c](0).one("bsTransitionEnd", a.proxy(e, this)).emulateTransitionEnd(d.TRANSITION_DURATION) : e.call(this)
            }
        }
    }, d.prototype.toggle = function() {
        this[this.$element.hasClass("in") ? "hide" : "show"]()
    }, d.prototype.getParent = function() {
        return a(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each(a.proxy(function(c, d) {
            var e = a(d);
            this.addAriaAndCollapsedClass(b(e), e)
        }, this)).end()
    }, d.prototype.addAriaAndCollapsedClass = function(a, b) {
        var c = a.hasClass("in");
        a.attr("aria-expanded", c), b.toggleClass("collapsed", !c).attr("aria-expanded", c)
    };
    var e = a.fn.collapse;
    a.fn.collapse = c, a.fn.collapse.Constructor = d, a.fn.collapse.noConflict = function() {
        return a.fn.collapse = e, this
    }, a(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]', function(d) {
        var e = a(this);
        e.attr("data-target") || d.preventDefault();
        var f = b(e),
            g = f.data("bs.collapse"),
            h = g ? "toggle" : e.data();
        c.call(f, h)
    })
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        var c = b.attr("data-target");
        c || (c = b.attr("href"), c = c && /#[A-Za-z]/.test(c) && c.replace(/.*(?=#[^\s]*$)/, ""));
        var d = c && a(c);
        return d && d.length ? d : b.parent()
    }

    function c(c) {
        c && 3 === c.which || (a(e).remove(), a(f).each(function() {
            var d = a(this),
                e = b(d),
                f = {
                    relatedTarget: this
                };
            e.hasClass("open") && (c && "click" == c.type && /input|textarea/i.test(c.target.tagName) && a.contains(e[0], c.target) || (e.trigger(c = a.Event("hide.bs.dropdown", f)), c.isDefaultPrevented() || (d.attr("aria-expanded", "false"), e.removeClass("open").trigger(a.Event("hidden.bs.dropdown", f)))))
        }))
    }

    function d(b) {
        return this.each(function() {
            var c = a(this),
                d = c.data("bs.dropdown");
            d || c.data("bs.dropdown", d = new g(this)), "string" == typeof b && d[b].call(c)
        })
    }
    var e = ".dropdown-backdrop",
        f = '[data-toggle="dropdown"]',
        g = function(b) {
            a(b).on("click.bs.dropdown", this.toggle)
        };
    g.VERSION = "3.3.6", g.prototype.toggle = function(d) {
        var e = a(this);
        if (!e.is(".disabled, :disabled")) {
            var f = b(e),
                g = f.hasClass("open");
            if (c(), !g) {
                "ontouchstart" in document.documentElement && !f.closest(".navbar-nav").length && a(document.createElement("div")).addClass("dropdown-backdrop").insertAfter(a(this)).on("click", c);
                var h = {
                    relatedTarget: this
                };
                if (f.trigger(d = a.Event("show.bs.dropdown", h)), d.isDefaultPrevented()) return;
                e.trigger("focus").attr("aria-expanded", "true"), f.toggleClass("open").trigger(a.Event("shown.bs.dropdown", h))
            }
            return !1
        }
    }, g.prototype.keydown = function(c) {
        if (/(38|40|27|32)/.test(c.which) && !/input|textarea/i.test(c.target.tagName)) {
            var d = a(this);
            if (c.preventDefault(), c.stopPropagation(), !d.is(".disabled, :disabled")) {
                var e = b(d),
                    g = e.hasClass("open");
                if (!g && 27 != c.which || g && 27 == c.which) return 27 == c.which && e.find(f).trigger("focus"), d.trigger("click");
                var h = " li:not(.disabled):visible a",
                    i = e.find(".dropdown-menu" + h);
                if (i.length) {
                    var j = i.index(c.target);
                    38 == c.which && j > 0 && j--, 40 == c.which && j < i.length - 1 && j++, ~j || (j = 0), i.eq(j).trigger("focus")
                }
            }
        }
    };
    var h = a.fn.dropdown;
    a.fn.dropdown = d, a.fn.dropdown.Constructor = g, a.fn.dropdown.noConflict = function() {
        return a.fn.dropdown = h, this
    }, a(document).on("click.bs.dropdown.data-api", c).on("click.bs.dropdown.data-api", ".dropdown form", function(a) {
        a.stopPropagation()
    }).on("click.bs.dropdown.data-api", f, g.prototype.toggle).on("keydown.bs.dropdown.data-api", f, g.prototype.keydown).on("keydown.bs.dropdown.data-api", ".dropdown-menu", g.prototype.keydown)
}(jQuery), + function(a) {
    "use strict";

    function b(b, d) {
        return this.each(function() {
            var e = a(this),
                f = e.data("bs.modal"),
                g = a.extend({}, c.DEFAULTS, e.data(), "object" == typeof b && b);
            f || e.data("bs.modal", f = new c(this, g)), "string" == typeof b ? f[b](d) : g.show && f.show(d)
        })
    }
    var c = function(b, c) {
        this.options = c, this.$body = a(document.body), this.$element = a(b), this.$dialog = this.$element.find(".modal-dialog"), this.$backdrop = null, this.isShown = null, this.originalBodyPad = null, this.scrollbarWidth = 0, this.ignoreBackdropClick = !1, this.options.remote && this.$element.find(".modal-content").load(this.options.remote, a.proxy(function() {
            this.$element.trigger("loaded.bs.modal")
        }, this))
    };
    c.VERSION = "3.3.6", c.TRANSITION_DURATION = 300, c.BACKDROP_TRANSITION_DURATION = 150, c.DEFAULTS = {
        backdrop: !0,
        keyboard: !0,
        show: !0
    }, c.prototype.toggle = function(a) {
        return this.isShown ? this.hide() : this.show(a)
    }, c.prototype.show = function(b) {
        var d = this,
            e = a.Event("show.bs.modal", {
                relatedTarget: b
            });
        this.$element.trigger(e), this.isShown || e.isDefaultPrevented() || (this.isShown = !0, this.checkScrollbar(), this.setScrollbar(), this.$body.addClass("modal-open"), this.escape(), this.resize(), this.$element.on("click.dismiss.bs.modal", '[data-dismiss="modal"]', a.proxy(this.hide, this)), this.$dialog.on("mousedown.dismiss.bs.modal", function() {
            d.$element.one("mouseup.dismiss.bs.modal", function(b) {
                a(b.target).is(d.$element) && (d.ignoreBackdropClick = !0)
            })
        }), this.backdrop(function() {
            var e = a.support.transition && d.$element.hasClass("fade");
            d.$element.parent().length || d.$element.appendTo(d.$body), d.$element.show().scrollTop(0), d.adjustDialog(), e && d.$element[0].offsetWidth, d.$element.addClass("in"), d.enforceFocus();
            var f = a.Event("shown.bs.modal", {
                relatedTarget: b
            });
            e ? d.$dialog.one("bsTransitionEnd", function() {
                d.$element.trigger("focus").trigger(f)
            }).emulateTransitionEnd(c.TRANSITION_DURATION) : d.$element.trigger("focus").trigger(f)
        }))
    }, c.prototype.hide = function(b) {
        b && b.preventDefault(), b = a.Event("hide.bs.modal"), this.$element.trigger(b), this.isShown && !b.isDefaultPrevented() && (this.isShown = !1, this.escape(), this.resize(), a(document).off("focusin.bs.modal"), this.$element.removeClass("in").off("click.dismiss.bs.modal").off("mouseup.dismiss.bs.modal"), this.$dialog.off("mousedown.dismiss.bs.modal"), a.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", a.proxy(this.hideModal, this)).emulateTransitionEnd(c.TRANSITION_DURATION) : this.hideModal())
    }, c.prototype.enforceFocus = function() {
        a(document).off("focusin.bs.modal").on("focusin.bs.modal", a.proxy(function(a) {
            this.$element[0] === a.target || this.$element.has(a.target).length || this.$element.trigger("focus")
        }, this))
    }, c.prototype.escape = function() {
        this.isShown && this.options.keyboard ? this.$element.on("keydown.dismiss.bs.modal", a.proxy(function(a) {
            27 == a.which && this.hide()
        }, this)) : this.isShown || this.$element.off("keydown.dismiss.bs.modal")
    }, c.prototype.resize = function() {
        this.isShown ? a(window).on("resize.bs.modal", a.proxy(this.handleUpdate, this)) : a(window).off("resize.bs.modal")
    }, c.prototype.hideModal = function() {
        var a = this;
        this.$element.hide(), this.backdrop(function() {
            a.$body.removeClass("modal-open"), a.resetAdjustments(), a.resetScrollbar(), a.$element.trigger("hidden.bs.modal")
        })
    }, c.prototype.removeBackdrop = function() {
        this.$backdrop && this.$backdrop.remove(), this.$backdrop = null
    }, c.prototype.backdrop = function(b) {
        var d = this,
            e = this.$element.hasClass("fade") ? "fade" : "";
        if (this.isShown && this.options.backdrop) {
            var f = a.support.transition && e;
            if (this.$backdrop = a(document.createElement("div")).addClass("modal-backdrop " + e).appendTo(this.$body), this.$element.on("click.dismiss.bs.modal", a.proxy(function(a) {
                    return this.ignoreBackdropClick ? void(this.ignoreBackdropClick = !1) : void(a.target === a.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus() : this.hide()))
                }, this)), f && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !b) return;
            f ? this.$backdrop.one("bsTransitionEnd", b).emulateTransitionEnd(c.BACKDROP_TRANSITION_DURATION) : b()
        } else if (!this.isShown && this.$backdrop) {
            this.$backdrop.removeClass("in");
            var g = function() {
                d.removeBackdrop(), b && b()
            };
            a.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one("bsTransitionEnd", g).emulateTransitionEnd(c.BACKDROP_TRANSITION_DURATION) : g()
        } else b && b()
    }, c.prototype.handleUpdate = function() {
        this.adjustDialog()
    }, c.prototype.adjustDialog = function() {
        var a = this.$element[0].scrollHeight > document.documentElement.clientHeight;
        this.$element.css({
            paddingLeft: !this.bodyIsOverflowing && a ? this.scrollbarWidth : "",
            paddingRight: this.bodyIsOverflowing && !a ? this.scrollbarWidth : ""
        })
    }, c.prototype.resetAdjustments = function() {
        this.$element.css({
            paddingLeft: "",
            paddingRight: ""
        })
    }, c.prototype.checkScrollbar = function() {
        var a = window.innerWidth;
        if (!a) {
            var b = document.documentElement.getBoundingClientRect();
            a = b.right - Math.abs(b.left)
        }
        this.bodyIsOverflowing = document.body.clientWidth < a, this.scrollbarWidth = this.measureScrollbar()
    }, c.prototype.setScrollbar = function() {
        var a = parseInt(this.$body.css("padding-right") || 0, 10);
        this.originalBodyPad = document.body.style.paddingRight || "", this.bodyIsOverflowing && this.$body.css("padding-right", a + this.scrollbarWidth)
    }, c.prototype.resetScrollbar = function() {
        this.$body.css("padding-right", this.originalBodyPad)
    }, c.prototype.measureScrollbar = function() {
        var a = document.createElement("div");
        a.className = "modal-scrollbar-measure", this.$body.append(a);
        var b = a.offsetWidth - a.clientWidth;
        return this.$body[0].removeChild(a), b
    };
    var d = a.fn.modal;
    a.fn.modal = b, a.fn.modal.Constructor = c, a.fn.modal.noConflict = function() {
        return a.fn.modal = d, this
    }, a(document).on("click.bs.modal.data-api", '[data-toggle="modal"]', function(c) {
        var d = a(this),
            e = d.attr("href"),
            f = a(d.attr("data-target") || e && e.replace(/.*(?=#[^\s]+$)/, "")),
            g = f.data("bs.modal") ? "toggle" : a.extend({
                remote: !/#/.test(e) && e
            }, f.data(), d.data());
        d.is("a") && c.preventDefault(), f.one("show.bs.modal", function(a) {
            a.isDefaultPrevented() || f.one("hidden.bs.modal", function() {
                d.is(":visible") && d.trigger("focus")
            })
        }), b.call(f, g, this)
    })
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.tooltip"),
                f = "object" == typeof b && b;
            (e || !/destroy|hide/.test(b)) && (e || d.data("bs.tooltip", e = new c(this, f)), "string" == typeof b && e[b]())
        })
    }
    var c = function(a, b) {
        this.type = null, this.options = null, this.enabled = null, this.timeout = null, this.hoverState = null, this.$element = null, this.inState = null, this.init("tooltip", a, b)
    };
    c.VERSION = "3.3.6", c.TRANSITION_DURATION = 150, c.DEFAULTS = {
        animation: !0,
        placement: "top",
        selector: !1,
        template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
        trigger: "hover focus",
        title: "",
        delay: 0,
        html: !1,
        container: !1,
        viewport: {
            selector: "body",
            padding: 0
        }
    }, c.prototype.init = function(b, c, d) {
        if (this.enabled = !0, this.type = b, this.$element = a(c), this.options = this.getOptions(d), this.$viewport = this.options.viewport && a(a.isFunction(this.options.viewport) ? this.options.viewport.call(this, this.$element) : this.options.viewport.selector || this.options.viewport), this.inState = {
                click: !1,
                hover: !1,
                focus: !1
            }, this.$element[0] instanceof document.constructor && !this.options.selector) throw new Error("`selector` option must be specified when initializing " + this.type + " on the window.document object!");
        for (var e = this.options.trigger.split(" "), f = e.length; f--;) {
            var g = e[f];
            if ("click" == g) this.$element.on("click." + this.type, this.options.selector, a.proxy(this.toggle, this));
            else if ("manual" != g) {
                var h = "hover" == g ? "mouseenter" : "focusin",
                    i = "hover" == g ? "mouseleave" : "focusout";
                this.$element.on(h + "." + this.type, this.options.selector, a.proxy(this.enter, this)), this.$element.on(i + "." + this.type, this.options.selector, a.proxy(this.leave, this))
            }
        }
        this.options.selector ? this._options = a.extend({}, this.options, {
            trigger: "manual",
            selector: ""
        }) : this.fixTitle()
    }, c.prototype.getDefaults = function() {
        return c.DEFAULTS
    }, c.prototype.getOptions = function(b) {
        return b = a.extend({}, this.getDefaults(), this.$element.data(), b), b.delay && "number" == typeof b.delay && (b.delay = {
            show: b.delay,
            hide: b.delay
        }), b
    }, c.prototype.getDelegateOptions = function() {
        var b = {},
            c = this.getDefaults();
        return this._options && a.each(this._options, function(a, d) {
            c[a] != d && (b[a] = d)
        }), b
    }, c.prototype.enter = function(b) {
        var c = b instanceof this.constructor ? b : a(b.currentTarget).data("bs." + this.type);
        return c || (c = new this.constructor(b.currentTarget, this.getDelegateOptions()), a(b.currentTarget).data("bs." + this.type, c)), b instanceof a.Event && (c.inState["focusin" == b.type ? "focus" : "hover"] = !0), c.tip().hasClass("in") || "in" == c.hoverState ? void(c.hoverState = "in") : (clearTimeout(c.timeout), c.hoverState = "in", c.options.delay && c.options.delay.show ? void(c.timeout = setTimeout(function() {
            "in" == c.hoverState && c.show()
        }, c.options.delay.show)) : c.show())
    }, c.prototype.isInStateTrue = function() {
        for (var a in this.inState)
            if (this.inState[a]) return !0;
        return !1
    }, c.prototype.leave = function(b) {
        var c = b instanceof this.constructor ? b : a(b.currentTarget).data("bs." + this.type);
        return c || (c = new this.constructor(b.currentTarget, this.getDelegateOptions()), a(b.currentTarget).data("bs." + this.type, c)), b instanceof a.Event && (c.inState["focusout" == b.type ? "focus" : "hover"] = !1), c.isInStateTrue() ? void 0 : (clearTimeout(c.timeout), c.hoverState = "out", c.options.delay && c.options.delay.hide ? void(c.timeout = setTimeout(function() {
            "out" == c.hoverState && c.hide()
        }, c.options.delay.hide)) : c.hide())
    }, c.prototype.show = function() {
        var b = a.Event("show.bs." + this.type);
        if (this.hasContent() && this.enabled) {
            this.$element.trigger(b);
            var d = a.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]);
            if (b.isDefaultPrevented() || !d) return;
            var e = this,
                f = this.tip(),
                g = this.getUID(this.type);
            this.setContent(), f.attr("id", g), this.$element.attr("aria-describedby", g), this.options.animation && f.addClass("fade");
            var h = "function" == typeof this.options.placement ? this.options.placement.call(this, f[0], this.$element[0]) : this.options.placement,
                i = /\s?auto?\s?/i,
                j = i.test(h);
            j && (h = h.replace(i, "") || "top"), f.detach().css({
                top: 0,
                left: 0,
                display: "block"
            }).addClass(h).data("bs." + this.type, this), this.options.container ? f.appendTo(this.options.container) : f.insertAfter(this.$element), this.$element.trigger("inserted.bs." + this.type);
            var k = this.getPosition(),
                l = f[0].offsetWidth,
                m = f[0].offsetHeight;
            if (j) {
                var n = h,
                    o = this.getPosition(this.$viewport);
                h = "bottom" == h && k.bottom + m > o.bottom ? "top" : "top" == h && k.top - m < o.top ? "bottom" : "right" == h && k.right + l > o.width ? "left" : "left" == h && k.left - l < o.left ? "right" : h, f.removeClass(n).addClass(h)
            }
            var p = this.getCalculatedOffset(h, k, l, m);
            this.applyPlacement(p, h);
            var q = function() {
                var a = e.hoverState;
                e.$element.trigger("shown.bs." + e.type), e.hoverState = null, "out" == a && e.leave(e)
            };
            a.support.transition && this.$tip.hasClass("fade") ? f.one("bsTransitionEnd", q).emulateTransitionEnd(c.TRANSITION_DURATION) : q()
        }
    }, c.prototype.applyPlacement = function(b, c) {
        var d = this.tip(),
            e = d[0].offsetWidth,
            f = d[0].offsetHeight,
            g = parseInt(d.css("margin-top"), 10),
            h = parseInt(d.css("margin-left"), 10);
        isNaN(g) && (g = 0), isNaN(h) && (h = 0), b.top += g, b.left += h, a.offset.setOffset(d[0], a.extend({
            using: function(a) {
                d.css({
                    top: Math.round(a.top),
                    left: Math.round(a.left)
                })
            }
        }, b), 0), d.addClass("in");
        var i = d[0].offsetWidth,
            j = d[0].offsetHeight;
        "top" == c && j != f && (b.top = b.top + f - j);
        var k = this.getViewportAdjustedDelta(c, b, i, j);
        k.left ? b.left += k.left : b.top += k.top;
        var l = /top|bottom/.test(c),
            m = l ? 2 * k.left - e + i : 2 * k.top - f + j,
            n = l ? "offsetWidth" : "offsetHeight";
        d.offset(b), this.replaceArrow(m, d[0][n], l)
    }, c.prototype.replaceArrow = function(a, b, c) {
        this.arrow().css(c ? "left" : "top", 50 * (1 - a / b) + "%").css(c ? "top" : "left", "")
    }, c.prototype.setContent = function() {
        var a = this.tip(),
            b = this.getTitle();
        a.find(".tooltip-inner")[this.options.html ? "html" : "text"](b), a.removeClass("fade in top bottom left right")
    }, c.prototype.hide = function(b) {
        function d() {
            "in" != e.hoverState && f.detach(), e.$element.removeAttr("aria-describedby").trigger("hidden.bs." + e.type), b && b()
        }
        var e = this,
            f = a(this.$tip),
            g = a.Event("hide.bs." + this.type);
        return this.$element.trigger(g), g.isDefaultPrevented() ? void 0 : (f.removeClass("in"), a.support.transition && f.hasClass("fade") ? f.one("bsTransitionEnd", d).emulateTransitionEnd(c.TRANSITION_DURATION) : d(), this.hoverState = null, this)
    }, c.prototype.fixTitle = function() {
        var a = this.$element;
        (a.attr("title") || "string" != typeof a.attr("data-original-title")) && a.attr("data-original-title", a.attr("title") || "").attr("title", "")
    }, c.prototype.hasContent = function() {
        return this.getTitle()
    }, c.prototype.getPosition = function(b) {
        b = b || this.$element;
        var c = b[0],
            d = "BODY" == c.tagName,
            e = c.getBoundingClientRect();
        null == e.width && (e = a.extend({}, e, {
            width: e.right - e.left,
            height: e.bottom - e.top
        }));
        var f = d ? {
                top: 0,
                left: 0
            } : b.offset(),
            g = {
                scroll: d ? document.documentElement.scrollTop || document.body.scrollTop : b.scrollTop()
            },
            h = d ? {
                width: a(window).width(),
                height: a(window).height()
            } : null;
        return a.extend({}, e, g, h, f)
    }, c.prototype.getCalculatedOffset = function(a, b, c, d) {
        return "bottom" == a ? {
            top: b.top + b.height,
            left: b.left + b.width / 2 - c / 2
        } : "top" == a ? {
            top: b.top - d,
            left: b.left + b.width / 2 - c / 2
        } : "left" == a ? {
            top: b.top + b.height / 2 - d / 2,
            left: b.left - c
        } : {
            top: b.top + b.height / 2 - d / 2,
            left: b.left + b.width
        }
    }, c.prototype.getViewportAdjustedDelta = function(a, b, c, d) {
        var e = {
            top: 0,
            left: 0
        };
        if (!this.$viewport) return e;
        var f = this.options.viewport && this.options.viewport.padding || 0,
            g = this.getPosition(this.$viewport);
        if (/right|left/.test(a)) {
            var h = b.top - f - g.scroll,
                i = b.top + f - g.scroll + d;
            h < g.top ? e.top = g.top - h : i > g.top + g.height && (e.top = g.top + g.height - i)
        } else {
            var j = b.left - f,
                k = b.left + f + c;
            j < g.left ? e.left = g.left - j : k > g.right && (e.left = g.left + g.width - k)
        }
        return e
    }, c.prototype.getTitle = function() {
        var a, b = this.$element,
            c = this.options;
        return a = b.attr("data-original-title") || ("function" == typeof c.title ? c.title.call(b[0]) : c.title)
    }, c.prototype.getUID = function(a) {
        do a += ~~(1e6 * Math.random()); while (document.getElementById(a));
        return a
    }, c.prototype.tip = function() {
        if (!this.$tip && (this.$tip = a(this.options.template), 1 != this.$tip.length)) throw new Error(this.type + " `template` option must consist of exactly 1 top-level element!");
        return this.$tip
    }, c.prototype.arrow = function() {
        return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
    }, c.prototype.enable = function() {
        this.enabled = !0
    }, c.prototype.disable = function() {
        this.enabled = !1
    }, c.prototype.toggleEnabled = function() {
        this.enabled = !this.enabled
    }, c.prototype.toggle = function(b) {
        var c = this;
        b && (c = a(b.currentTarget).data("bs." + this.type), c || (c = new this.constructor(b.currentTarget, this.getDelegateOptions()), a(b.currentTarget).data("bs." + this.type, c))), b ? (c.inState.click = !c.inState.click, c.isInStateTrue() ? c.enter(c) : c.leave(c)) : c.tip().hasClass("in") ? c.leave(c) : c.enter(c)
    }, c.prototype.destroy = function() {
        var a = this;
        clearTimeout(this.timeout), this.hide(function() {
            a.$element.off("." + a.type).removeData("bs." + a.type), a.$tip && a.$tip.detach(), a.$tip = null, a.$arrow = null, a.$viewport = null
        })
    };
    var d = a.fn.tooltip;
    a.fn.tooltip = b, a.fn.tooltip.Constructor = c, a.fn.tooltip.noConflict = function() {
        return a.fn.tooltip = d, this
    }
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.popover"),
                f = "object" == typeof b && b;
            (e || !/destroy|hide/.test(b)) && (e || d.data("bs.popover", e = new c(this, f)), "string" == typeof b && e[b]())
        })
    }
    var c = function(a, b) {
        this.init("popover", a, b)
    };
    if (!a.fn.tooltip) throw new Error("Popover requires tooltip.js");
    c.VERSION = "3.3.6", c.DEFAULTS = a.extend({}, a.fn.tooltip.Constructor.DEFAULTS, {
        placement: "right",
        trigger: "click",
        content: "",
        template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
    }), c.prototype = a.extend({}, a.fn.tooltip.Constructor.prototype), c.prototype.constructor = c, c.prototype.getDefaults = function() {
        return c.DEFAULTS
    }, c.prototype.setContent = function() {
        var a = this.tip(),
            b = this.getTitle(),
            c = this.getContent();
        a.find(".popover-title")[this.options.html ? "html" : "text"](b), a.find(".popover-content").children().detach().end()[this.options.html ? "string" == typeof c ? "html" : "append" : "text"](c), a.removeClass("fade top bottom left right in"), a.find(".popover-title").html() || a.find(".popover-title").hide()
    }, c.prototype.hasContent = function() {
        return this.getTitle() || this.getContent()
    }, c.prototype.getContent = function() {
        var a = this.$element,
            b = this.options;
        return a.attr("data-content") || ("function" == typeof b.content ? b.content.call(a[0]) : b.content)
    }, c.prototype.arrow = function() {
        return this.$arrow = this.$arrow || this.tip().find(".arrow")
    };
    var d = a.fn.popover;
    a.fn.popover = b, a.fn.popover.Constructor = c, a.fn.popover.noConflict = function() {
        return a.fn.popover = d, this
    }
}(jQuery), + function(a) {
    "use strict";

    function b(c, d) {
        this.$body = a(document.body), this.$scrollElement = a(a(c).is(document.body) ? window : c), this.options = a.extend({}, b.DEFAULTS, d), this.selector = (this.options.target || "") + " .nav li > a", this.offsets = [], this.targets = [], this.activeTarget = null, this.scrollHeight = 0, this.$scrollElement.on("scroll.bs.scrollspy", a.proxy(this.process, this)), this.refresh(), this.process()
    }

    function c(c) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.scrollspy"),
                f = "object" == typeof c && c;
            e || d.data("bs.scrollspy", e = new b(this, f)), "string" == typeof c && e[c]()
        })
    }
    b.VERSION = "3.3.6", b.DEFAULTS = {
        offset: 10
    }, b.prototype.getScrollHeight = function() {
        return this.$scrollElement[0].scrollHeight || Math.max(this.$body[0].scrollHeight, document.documentElement.scrollHeight)
    }, b.prototype.refresh = function() {
        var b = this,
            c = "offset",
            d = 0;
        this.offsets = [], this.targets = [], this.scrollHeight = this.getScrollHeight(), a.isWindow(this.$scrollElement[0]) || (c = "position", d = this.$scrollElement.scrollTop()), this.$body.find(this.selector).map(function() {
            var b = a(this),
                e = b.data("target") || b.attr("href"),
                f = /^#./.test(e) && a(e);
            return f && f.length && f.is(":visible") && [
                [f[c]().top + d, e]
            ] || null
        }).sort(function(a, b) {
            return a[0] - b[0]
        }).each(function() {
            b.offsets.push(this[0]), b.targets.push(this[1])
        })
    }, b.prototype.process = function() {
        var a, b = this.$scrollElement.scrollTop() + this.options.offset,
            c = this.getScrollHeight(),
            d = this.options.offset + c - this.$scrollElement.height(),
            e = this.offsets,
            f = this.targets,
            g = this.activeTarget;
        if (this.scrollHeight != c && this.refresh(), b >= d) return g != (a = f[f.length - 1]) && this.activate(a);
        if (g && b < e[0]) return this.activeTarget = null, this.clear();
        for (a = e.length; a--;) g != f[a] && b >= e[a] && (void 0 === e[a + 1] || b < e[a + 1]) && this.activate(f[a])
    }, b.prototype.activate = function(b) {
        this.activeTarget = b, this.clear();
        var c = this.selector + '[data-target="' + b + '"],' + this.selector + '[href="' + b + '"]',
            d = a(c).parents("li").addClass("active");
        d.parent(".dropdown-menu").length && (d = d.closest("li.dropdown").addClass("active")), d.trigger("activate.bs.scrollspy")
    }, b.prototype.clear = function() {
        a(this.selector).parentsUntil(this.options.target, ".active").removeClass("active")
    };
    var d = a.fn.scrollspy;
    a.fn.scrollspy = c, a.fn.scrollspy.Constructor = b, a.fn.scrollspy.noConflict = function() {
        return a.fn.scrollspy = d, this
    }, a(window).on("load.bs.scrollspy.data-api", function() {
        a('[data-spy="scroll"]').each(function() {
            var b = a(this);
            c.call(b, b.data())
        })
    })
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.tab");
            e || d.data("bs.tab", e = new c(this)), "string" == typeof b && e[b]()
        })
    }
    var c = function(b) {
        this.element = a(b)
    };
    c.VERSION = "3.3.6", c.TRANSITION_DURATION = 150, c.prototype.show = function() {
        var b = this.element,
            c = b.closest("ul:not(.dropdown-menu)"),
            d = b.data("target");
        if (d || (d = b.attr("href"), d = d && d.replace(/.*(?=#[^\s]*$)/, "")), !b.parent("li").hasClass("active")) {
            var e = c.find(".active:last a"),
                f = a.Event("hide.bs.tab", {
                    relatedTarget: b[0]
                }),
                g = a.Event("show.bs.tab", {
                    relatedTarget: e[0]
                });
            if (e.trigger(f), b.trigger(g), !g.isDefaultPrevented() && !f.isDefaultPrevented()) {
                var h = a(d);
                this.activate(b.closest("li"), c), this.activate(h, h.parent(), function() {
                    e.trigger({
                        type: "hidden.bs.tab",
                        relatedTarget: b[0]
                    }), b.trigger({
                        type: "shown.bs.tab",
                        relatedTarget: e[0]
                    })
                })
            }
        }
    }, c.prototype.activate = function(b, d, e) {
        function f() {
            g.removeClass("active").find("> .dropdown-menu > .active").removeClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !1), b.addClass("active").find('[data-toggle="tab"]').attr("aria-expanded", !0), h ? (b[0].offsetWidth, b.addClass("in")) : b.removeClass("fade"), b.parent(".dropdown-menu").length && b.closest("li.dropdown").addClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !0), e && e()
        }
        var g = d.find("> .active"),
            h = e && a.support.transition && (g.length && g.hasClass("fade") || !!d.find("> .fade").length);
        g.length && h ? g.one("bsTransitionEnd", f).emulateTransitionEnd(c.TRANSITION_DURATION) : f(), g.removeClass("in")
    };
    var d = a.fn.tab;
    a.fn.tab = b, a.fn.tab.Constructor = c, a.fn.tab.noConflict = function() {
        return a.fn.tab = d, this
    };
    var e = function(c) {
        c.preventDefault(), b.call(a(this), "show")
    };
    a(document).on("click.bs.tab.data-api", '[data-toggle="tab"]', e).on("click.bs.tab.data-api", '[data-toggle="pill"]', e)
}(jQuery), + function(a) {
    "use strict";

    function b(b) {
        return this.each(function() {
            var d = a(this),
                e = d.data("bs.affix"),
                f = "object" == typeof b && b;
            e || d.data("bs.affix", e = new c(this, f)), "string" == typeof b && e[b]()
        })
    }
    var c = function(b, d) {
        this.options = a.extend({}, c.DEFAULTS, d), this.$target = a(this.options.target).on("scroll.bs.affix.data-api", a.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", a.proxy(this.checkPositionWithEventLoop, this)), this.$element = a(b), this.affixed = null, this.unpin = null, this.pinnedOffset = null, this.checkPosition()
    };
    c.VERSION = "3.3.6", c.RESET = "affix affix-top affix-bottom", c.DEFAULTS = {
        offset: 0,
        target: window
    }, c.prototype.getState = function(a, b, c, d) {
        var e = this.$target.scrollTop(),
            f = this.$element.offset(),
            g = this.$target.height();
        if (null != c && "top" == this.affixed) return c > e ? "top" : !1;
        if ("bottom" == this.affixed) return null != c ? e + this.unpin <= f.top ? !1 : "bottom" : a - d >= e + g ? !1 : "bottom";
        var h = null == this.affixed,
            i = h ? e : f.top,
            j = h ? g : b;
        return null != c && c >= e ? "top" : null != d && i + j >= a - d ? "bottom" : !1
    }, c.prototype.getPinnedOffset = function() {
        if (this.pinnedOffset) return this.pinnedOffset;
        this.$element.removeClass(c.RESET).addClass("affix");
        var a = this.$target.scrollTop(),
            b = this.$element.offset();
        return this.pinnedOffset = b.top - a
    }, c.prototype.checkPositionWithEventLoop = function() {
        setTimeout(a.proxy(this.checkPosition, this), 1)
    }, c.prototype.checkPosition = function() {
        if (this.$element.is(":visible")) {
            var b = this.$element.height(),
                d = this.options.offset,
                e = d.top,
                f = d.bottom,
                g = Math.max(a(document).height(), a(document.body).height());
            "object" != typeof d && (f = e = d), "function" == typeof e && (e = d.top(this.$element)), "function" == typeof f && (f = d.bottom(this.$element));
            var h = this.getState(g, b, e, f);
            if (this.affixed != h) {
                null != this.unpin && this.$element.css("top", "");
                var i = "affix" + (h ? "-" + h : ""),
                    j = a.Event(i + ".bs.affix");
                if (this.$element.trigger(j), j.isDefaultPrevented()) return;
                this.affixed = h, this.unpin = "bottom" == h ? this.getPinnedOffset() : null, this.$element.removeClass(c.RESET).addClass(i).trigger(i.replace("affix", "affixed") + ".bs.affix")
            }
            "bottom" == h && this.$element.offset({
                top: g - b - f
            })
        }
    };
    var d = a.fn.affix;
    a.fn.affix = b, a.fn.affix.Constructor = c, a.fn.affix.noConflict = function() {
        return a.fn.affix = d, this
    }, a(window).on("load", function() {
        a('[data-spy="affix"]').each(function() {
            var c = a(this),
                d = c.data();
            d.offset = d.offset || {}, null != d.offsetBottom && (d.offset.bottom = d.offsetBottom), null != d.offsetTop && (d.offset.top = d.offsetTop), b.call(c, d)
        })
    })
}(jQuery);
/* rangeslider */
(function(f) {
    "function" === typeof define && define.amd ? define(["jquery"], function(n) {
        return f(n, document, window, navigator)
    }) : "object" === typeof exports ? f(require("jquery"), document, window, navigator) : f(jQuery, document, window, navigator)
})(function(f, n, k, r, p) {
    var t = 0,
        m = function() {
            var a = r.userAgent,
                b = /msie\s\d+/i;
            return 0 < a.search(b) && (a = b.exec(a).toString(), a = a.split(" ")[1], 9 > a) ? (f("html").addClass("lt-ie9"), !0) : !1
        }();
    Function.prototype.bind || (Function.prototype.bind = function(a) {
        var b = this,
            d = [].slice;
        if ("function" != typeof b) throw new TypeError;
        var c = d.call(arguments, 1),
            e = function() {
                if (this instanceof e) {
                    var g = function() {};
                    g.prototype = b.prototype;
                    var g = new g,
                        l = b.apply(g, c.concat(d.call(arguments)));
                    return Object(l) === l ? l : g
                }
                return b.apply(a, c.concat(d.call(arguments)))
            };
        return e
    });
    Array.prototype.indexOf || (Array.prototype.indexOf = function(a, b) {
        if (null == this) throw new TypeError('"this" is null or not defined');
        var d = Object(this),
            c = d.length >>> 0;
        if (0 === c) return -1;
        var e = +b || 0;
        Infinity === Math.abs(e) && (e = 0);
        if (e >= c) return -1;
        for (e = Math.max(0 <= e ? e : c - Math.abs(e), 0); e < c;) {
            if (e in d && d[e] === a) return e;
            e++
        }
        return -1
    });
    var q = function(a, b, d) {
        this.VERSION = "2.2.0";
        this.input = a;
        this.plugin_count = d;
        this.old_to = this.old_from = this.update_tm = this.calc_count = this.current_plugin = 0;
        this.raf_id = this.old_min_interval = null;
        this.no_diapason = this.force_redraw = this.dragging = !1;
        this.has_tab_index = !0;
        this.is_update = this.is_key = !1;
        this.is_start = !0;
        this.is_click = this.is_resize = this.is_active = this.is_finish = !1;
        b = b || {};
        this.$cache = {
            win: f(k),
            body: f(n.body),
            input: f(a),
            cont: null,
            rs: null,
            min: null,
            max: null,
            from: null,
            to: null,
            single: null,
            bar: null,
            line: null,
            s_single: null,
            s_from: null,
            s_to: null,
            shad_single: null,
            shad_from: null,
            shad_to: null,
            edge: null,
            grid: null,
            grid_labels: []
        };
        this.coords = {
            x_gap: 0,
            x_pointer: 0,
            w_rs: 0,
            w_rs_old: 0,
            w_handle: 0,
            p_gap: 0,
            p_gap_left: 0,
            p_gap_right: 0,
            p_step: 0,
            p_pointer: 0,
            p_handle: 0,
            p_single_fake: 0,
            p_single_real: 0,
            p_from_fake: 0,
            p_from_real: 0,
            p_to_fake: 0,
            p_to_real: 0,
            p_bar_x: 0,
            p_bar_w: 0,
            grid_gap: 0,
            big_num: 0,
            big: [],
            big_w: [],
            big_p: [],
            big_x: []
        };
        this.labels = {
            w_min: 0,
            w_max: 0,
            w_from: 0,
            w_to: 0,
            w_single: 0,
            p_min: 0,
            p_max: 0,
            p_from_fake: 0,
            p_from_left: 0,
            p_to_fake: 0,
            p_to_left: 0,
            p_single_fake: 0,
            p_single_left: 0
        };
        var c = this.$cache.input;
        a = c.prop("value");
        var e;
        d = {
            type: "single",
            min: 10,
            max: 100,
            from: null,
            to: null,
            step: 1,
            min_interval: 0,
            max_interval: 0,
            drag_interval: !1,
            values: [],
            p_values: [],
            from_fixed: !1,
            from_min: null,
            from_max: null,
            from_shadow: !1,
            to_fixed: !1,
            to_min: null,
            to_max: null,
            to_shadow: !1,
            prettify_enabled: !0,
            prettify_separator: " ",
            prettify: null,
            force_edges: !1,
            keyboard: !0,
            grid: !1,
            grid_margin: !0,
            grid_num: 4,
            grid_snap: !1,
            hide_min_max: !1,
            hide_from_to: !1,
            prefix: "",
            postfix: "",
            max_postfix: "",
            decorate_both: !0,
            values_separator: " \u2014 ",
            input_values_separator: ";",
            disable: !1,
            block: !1,
            extra_classes: "",
            scope: null,
            onStart: null,
            onChange: null,
            onFinish: null,
            onUpdate: null
        };
        "INPUT" !== c[0].nodeName && console && console.warn && console.warn("Base element should be <input>!", c[0]);
        c = {
            type: c.data("type"),
            min: c.data("min"),
            max: c.data("max"),
            from: c.data("from"),
            to: c.data("to"),
            step: c.data("step"),
            min_interval: c.data("minInterval"),
            max_interval: c.data("maxInterval"),
            drag_interval: c.data("dragInterval"),
            values: c.data("values"),
            from_fixed: c.data("fromFixed"),
            from_min: c.data("fromMin"),
            from_max: c.data("fromMax"),
            from_shadow: c.data("fromShadow"),
            to_fixed: c.data("toFixed"),
            to_min: c.data("toMin"),
            to_max: c.data("toMax"),
            to_shadow: c.data("toShadow"),
            prettify_enabled: c.data("prettifyEnabled"),
            prettify_separator: c.data("prettifySeparator"),
            force_edges: c.data("forceEdges"),
            keyboard: c.data("keyboard"),
            grid: c.data("grid"),
            grid_margin: c.data("gridMargin"),
            grid_num: c.data("gridNum"),
            grid_snap: c.data("gridSnap"),
            hide_min_max: c.data("hideMinMax"),
            hide_from_to: c.data("hideFromTo"),
            prefix: c.data("prefix"),
            postfix: c.data("postfix"),
            max_postfix: c.data("maxPostfix"),
            decorate_both: c.data("decorateBoth"),
            values_separator: c.data("valuesSeparator"),
            input_values_separator: c.data("inputValuesSeparator"),
            disable: c.data("disable"),
            block: c.data("block"),
            extra_classes: c.data("extraClasses")
        };
        c.values = c.values && c.values.split(",");
        for (e in c) c.hasOwnProperty(e) && (c[e] !== p && "" !== c[e] || delete c[e]);
        a !== p && "" !== a && (a = a.split(c.input_values_separator || b.input_values_separator || ";"), a[0] && a[0] == +a[0] && (a[0] = +a[0]), a[1] && a[1] == +a[1] && (a[1] = +a[1]), b && b.values && b.values.length ? (d.from = a[0] && b.values.indexOf(a[0]), d.to = a[1] && b.values.indexOf(a[1])) : (d.from = a[0] && +a[0], d.to = a[1] && +a[1]));
        f.extend(d, b);
        f.extend(d, c);
        this.options = d;
        this.update_check = {};
        this.validate();
        this.result = {
            input: this.$cache.input,
            slider: null,
            min: this.options.min,
            max: this.options.max,
            from: this.options.from,
            from_percent: 0,
            from_value: null,
            to: this.options.to,
            to_percent: 0,
            to_value: null
        };
        this.init()
    };
    q.prototype = {
        init: function(a) {
            this.no_diapason = !1;
            this.coords.p_step = this.convertToPercent(this.options.step, !0);
            this.target = "base";
            this.toggleInput();
            this.append();
            this.setMinMax();
            a ? (this.force_redraw = !0, this.calc(!0), this.callOnUpdate()) : (this.force_redraw = !0, this.calc(!0), this.callOnStart());
            this.updateScene()
        },
        append: function() {
            this.$cache.input.before('<span class="irs js-irs-' + this.plugin_count + " " + this.options.extra_classes + '"></span>');
            this.$cache.input.prop("readonly", !0);
            this.$cache.cont = this.$cache.input.prev();
            this.result.slider = this.$cache.cont;
            this.$cache.cont.html('<span class="irs"><span class="irs-line" tabindex="0"><span class="irs-line-left"></span><span class="irs-line-mid"></span><span class="irs-line-right"></span></span><span class="irs-min">0</span><span class="irs-max">1</span><span class="irs-from">0</span><span class="irs-to">0</span><span class="irs-single">0</span></span><span class="irs-grid"></span><span class="irs-bar"></span>');
            this.$cache.rs = this.$cache.cont.find(".irs");
            this.$cache.min = this.$cache.cont.find(".irs-min");
            this.$cache.max = this.$cache.cont.find(".irs-max");
            this.$cache.from = this.$cache.cont.find(".irs-from");
            this.$cache.to = this.$cache.cont.find(".irs-to");
            this.$cache.single = this.$cache.cont.find(".irs-single");
            this.$cache.bar = this.$cache.cont.find(".irs-bar");
            this.$cache.line = this.$cache.cont.find(".irs-line");
            this.$cache.grid = this.$cache.cont.find(".irs-grid");
            "single" === this.options.type ? (this.$cache.cont.append('<span class="irs-bar-edge"></span><span class="irs-shadow shadow-single"></span><span class="irs-slider single"></span>'), this.$cache.edge = this.$cache.cont.find(".irs-bar-edge"), this.$cache.s_single = this.$cache.cont.find(".single"), this.$cache.from[0].style.visibility = "hidden", this.$cache.to[0].style.visibility = "hidden", this.$cache.shad_single = this.$cache.cont.find(".shadow-single")) : (this.$cache.cont.append('<span class="irs-shadow shadow-from"></span><span class="irs-shadow shadow-to"></span><span class="irs-slider from"></span><span class="irs-slider to"></span>'), this.$cache.s_from = this.$cache.cont.find(".from"), this.$cache.s_to = this.$cache.cont.find(".to"), this.$cache.shad_from = this.$cache.cont.find(".shadow-from"), this.$cache.shad_to = this.$cache.cont.find(".shadow-to"), this.setTopHandler());
            this.options.hide_from_to && (this.$cache.from[0].style.display = "none", this.$cache.to[0].style.display = "none", this.$cache.single[0].style.display = "none");
            this.appendGrid();
            this.options.disable ? (this.appendDisableMask(), this.$cache.input[0].disabled = !0) : (this.$cache.input[0].disabled = !1, this.removeDisableMask(), this.bindEvents());
            this.options.disable || (this.options.block ? this.appendDisableMask() : this.removeDisableMask());
            this.options.drag_interval && (this.$cache.bar[0].style.cursor = "ew-resize")
        },
        setTopHandler: function() {
            var a = this.options.max,
                b = this.options.to;
            this.options.from > this.options.min && b === a ? this.$cache.s_from.addClass("type_last") : b < a && this.$cache.s_to.addClass("type_last")
        },
        changeLevel: function(a) {
            switch (a) {
                case "single":
                    this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_single_fake);
                    this.$cache.s_single.addClass("state_hover");
                    break;
                case "from":
                    this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_from_fake);
                    this.$cache.s_from.addClass("state_hover");
                    this.$cache.s_from.addClass("type_last");
                    this.$cache.s_to.removeClass("type_last");
                    break;
                case "to":
                    this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_to_fake);
                    this.$cache.s_to.addClass("state_hover");
                    this.$cache.s_to.addClass("type_last");
                    this.$cache.s_from.removeClass("type_last");
                    break;
                case "both":
                    this.coords.p_gap_left = this.toFixed(this.coords.p_pointer - this.coords.p_from_fake), this.coords.p_gap_right = this.toFixed(this.coords.p_to_fake - this.coords.p_pointer), this.$cache.s_to.removeClass("type_last"), this.$cache.s_from.removeClass("type_last")
            }
        },
        appendDisableMask: function() {
            this.$cache.cont.append('<span class="irs-disable-mask"></span>');
            this.$cache.cont.addClass("irs-disabled")
        },
        removeDisableMask: function() {
            this.$cache.cont.remove(".irs-disable-mask");
            this.$cache.cont.removeClass("irs-disabled")
        },
        remove: function() {
            this.$cache.cont.remove();
            this.$cache.cont = null;
            this.$cache.line.off("keydown.irs_" + this.plugin_count);
            this.$cache.body.off("touchmove.irs_" + this.plugin_count);
            this.$cache.body.off("mousemove.irs_" + this.plugin_count);
            this.$cache.win.off("touchend.irs_" + this.plugin_count);
            this.$cache.win.off("mouseup.irs_" + this.plugin_count);
            m && (this.$cache.body.off("mouseup.irs_" + this.plugin_count), this.$cache.body.off("mouseleave.irs_" + this.plugin_count));
            this.$cache.grid_labels = [];
            this.coords.big = [];
            this.coords.big_w = [];
            this.coords.big_p = [];
            this.coords.big_x = [];
            cancelAnimationFrame(this.raf_id)
        },
        bindEvents: function() {
            if (!this.no_diapason) {
                this.$cache.body.on("touchmove.irs_" + this.plugin_count, this.pointerMove.bind(this));
                this.$cache.body.on("mousemove.irs_" + this.plugin_count, this.pointerMove.bind(this));
                this.$cache.win.on("touchend.irs_" + this.plugin_count, this.pointerUp.bind(this));
                this.$cache.win.on("mouseup.irs_" + this.plugin_count, this.pointerUp.bind(this));
                this.$cache.line.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click"));
                this.$cache.line.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click"));
                this.$cache.line.on("focus.irs_" + this.plugin_count, this.pointerFocus.bind(this));
                this.options.drag_interval && "double" === this.options.type ? (this.$cache.bar.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "both")), this.$cache.bar.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "both"))) : (this.$cache.bar.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.bar.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")));
                "single" === this.options.type ? (this.$cache.single.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.s_single.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.shad_single.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.single.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.s_single.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.edge.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.shad_single.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click"))) : (this.$cache.single.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, null)), this.$cache.single.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, null)), this.$cache.from.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.s_from.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.to.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.s_to.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.shad_from.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.shad_to.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.from.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.s_from.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.to.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.s_to.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.shad_from.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.shad_to.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")));
                if (this.options.keyboard) this.$cache.line.on("keydown.irs_" + this.plugin_count, this.key.bind(this, "keyboard"));
                m && (this.$cache.body.on("mouseup.irs_" + this.plugin_count, this.pointerUp.bind(this)), this.$cache.body.on("mouseleave.irs_" + this.plugin_count, this.pointerUp.bind(this)))
            }
        },
        pointerFocus: function(a) {
            if (!this.target) {
                var b = "single" === this.options.type ? this.$cache.single : this.$cache.from;
                a = b.offset().left;
                a += b.width() / 2 - 1;
                this.pointerClick("single", {
                    preventDefault: function() {},
                    pageX: a
                })
            }
        },
        pointerMove: function(a) {
            this.dragging && (this.coords.x_pointer = (a.pageX || a.originalEvent.touches && a.originalEvent.touches[0].pageX) - this.coords.x_gap, this.calc())
        },
        pointerUp: function(a) {
            this.current_plugin === this.plugin_count && this.is_active && (this.is_active = !1, this.$cache.cont.find(".state_hover").removeClass("state_hover"), this.force_redraw = !0, m && f("*").prop("unselectable", !1), this.updateScene(), this.restoreOriginalMinInterval(), (f.contains(this.$cache.cont[0], a.target) || this.dragging) && this.callOnFinish(), this.dragging = !1)
        },
        pointerDown: function(a, b) {
            b.preventDefault();
            var d = b.pageX || b.originalEvent.touches && b.originalEvent.touches[0].pageX;
            2 !== b.button && ("both" === a && this.setTempMinInterval(), a || (a = this.target || "from"), this.current_plugin = this.plugin_count, this.target = a, this.dragging = this.is_active = !0, this.coords.x_gap = this.$cache.rs.offset().left, this.coords.x_pointer = d - this.coords.x_gap, this.calcPointerPercent(), this.changeLevel(a), m && f("*").prop("unselectable", !0), this.$cache.line.trigger("focus"), this.updateScene())
        },
        pointerClick: function(a, b) {
            b.preventDefault();
            var d = b.pageX || b.originalEvent.touches && b.originalEvent.touches[0].pageX;
            2 !== b.button && (this.current_plugin = this.plugin_count, this.target = a, this.is_click = !0, this.coords.x_gap = this.$cache.rs.offset().left, this.coords.x_pointer = +(d - this.coords.x_gap).toFixed(), this.force_redraw = !0, this.calc(), this.$cache.line.trigger("focus"))
        },
        key: function(a, b) {
            if (!(this.current_plugin !== this.plugin_count || b.altKey || b.ctrlKey || b.shiftKey || b.metaKey)) {
                switch (b.which) {
                    case 83:
                    case 65:
                    case 40:
                    case 37:
                        b.preventDefault();
                        this.moveByKey(!1);
                        break;
                    case 87:
                    case 68:
                    case 38:
                    case 39:
                        b.preventDefault(), this.moveByKey(!0)
                }
                return !0
            }
        },
        moveByKey: function(a) {
            var b = this.coords.p_pointer,
                d = (this.options.max - this.options.min) / 100,
                d = this.options.step / d;
            this.coords.x_pointer = this.toFixed(this.coords.w_rs / 100 * (a ? b + d : b - d));
            this.is_key = !0;
            this.calc()
        },
        setMinMax: function() {
            if (this.options)
                if (this.options.hide_min_max) this.$cache.min[0].style.display = "none", this.$cache.max[0].style.display = "none";
                else {
                    if (this.options.values.length) this.$cache.min.html(this.decorate(this.options.p_values[this.options.min])), this.$cache.max.html(this.decorate(this.options.p_values[this.options.max]));
                    else {
                        var a = this._prettify(this.options.min),
                            b = this._prettify(this.options.max);
                        this.result.min_pretty = a;
                        this.result.max_pretty = b;
                        this.$cache.min.html(this.decorate(a, this.options.min));
                        this.$cache.max.html(this.decorate(b, this.options.max))
                    }
                    this.labels.w_min = this.$cache.min.outerWidth(!1);
                    this.labels.w_max = this.$cache.max.outerWidth(!1)
                }
        },
        setTempMinInterval: function() {
            var a = this.result.to - this.result.from;
            null === this.old_min_interval && (this.old_min_interval = this.options.min_interval);
            this.options.min_interval = a
        },
        restoreOriginalMinInterval: function() {
            null !== this.old_min_interval && (this.options.min_interval = this.old_min_interval, this.old_min_interval = null)
        },
        calc: function(a) {
            if (this.options) {
                this.calc_count++;
                if (10 === this.calc_count || a) this.calc_count = 0, this.coords.w_rs = this.$cache.rs.outerWidth(!1), this.calcHandlePercent();
                if (this.coords.w_rs) {
                    this.calcPointerPercent();
                    a = this.getHandleX();
                    "both" === this.target && (this.coords.p_gap = 0, a = this.getHandleX());
                    "click" === this.target && (this.coords.p_gap = this.coords.p_handle / 2, a = this.getHandleX(), this.target = this.options.drag_interval ? "both_one" : this.chooseHandle(a));
                    switch (this.target) {
                        case "base":
                            var b = (this.options.max - this.options.min) / 100;
                            a = (this.result.from - this.options.min) / b;
                            b = (this.result.to - this.options.min) / b;
                            this.coords.p_single_real = this.toFixed(a);
                            this.coords.p_from_real = this.toFixed(a);
                            this.coords.p_to_real = this.toFixed(b);
                            this.coords.p_single_real = this.checkDiapason(this.coords.p_single_real, this.options.from_min, this.options.from_max);
                            this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                            this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                            this.coords.p_single_fake = this.convertToFakePercent(this.coords.p_single_real);
                            this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real);
                            this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real);
                            this.target = null;
                            break;
                        case "single":
                            if (this.options.from_fixed) break;
                            this.coords.p_single_real = this.convertToRealPercent(a);
                            this.coords.p_single_real = this.calcWithStep(this.coords.p_single_real);
                            this.coords.p_single_real = this.checkDiapason(this.coords.p_single_real, this.options.from_min, this.options.from_max);
                            this.coords.p_single_fake = this.convertToFakePercent(this.coords.p_single_real);
                            break;
                        case "from":
                            if (this.options.from_fixed) break;
                            this.coords.p_from_real = this.convertToRealPercent(a);
                            this.coords.p_from_real = this.calcWithStep(this.coords.p_from_real);
                            this.coords.p_from_real > this.coords.p_to_real && (this.coords.p_from_real = this.coords.p_to_real);
                            this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                            this.coords.p_from_real = this.checkMinInterval(this.coords.p_from_real, this.coords.p_to_real, "from");
                            this.coords.p_from_real = this.checkMaxInterval(this.coords.p_from_real, this.coords.p_to_real, "from");
                            this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real);
                            break;
                        case "to":
                            if (this.options.to_fixed) break;
                            this.coords.p_to_real = this.convertToRealPercent(a);
                            this.coords.p_to_real = this.calcWithStep(this.coords.p_to_real);
                            this.coords.p_to_real < this.coords.p_from_real && (this.coords.p_to_real = this.coords.p_from_real);
                            this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                            this.coords.p_to_real = this.checkMinInterval(this.coords.p_to_real, this.coords.p_from_real, "to");
                            this.coords.p_to_real = this.checkMaxInterval(this.coords.p_to_real, this.coords.p_from_real, "to");
                            this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real);
                            break;
                        case "both":
                            if (this.options.from_fixed || this.options.to_fixed) break;
                            a = this.toFixed(a + .001 * this.coords.p_handle);
                            this.coords.p_from_real = this.convertToRealPercent(a) - this.coords.p_gap_left;
                            this.coords.p_from_real = this.calcWithStep(this.coords.p_from_real);
                            this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                            this.coords.p_from_real = this.checkMinInterval(this.coords.p_from_real, this.coords.p_to_real, "from");
                            this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real);
                            this.coords.p_to_real = this.convertToRealPercent(a) + this.coords.p_gap_right;
                            this.coords.p_to_real = this.calcWithStep(this.coords.p_to_real);
                            this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                            this.coords.p_to_real = this.checkMinInterval(this.coords.p_to_real, this.coords.p_from_real, "to");
                            this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real);
                            break;
                        case "both_one":
                            if (!this.options.from_fixed && !this.options.to_fixed) {
                                var d = this.convertToRealPercent(a);
                                a = this.result.to_percent - this.result.from_percent;
                                var c = a / 2,
                                    b = d - c,
                                    d = d + c;
                                0 > b && (b = 0, d = b + a);
                                100 < d && (d = 100, b = d - a);
                                this.coords.p_from_real = this.calcWithStep(b);
                                this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                                this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real);
                                this.coords.p_to_real = this.calcWithStep(d);
                                this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                                this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real)
                            }
                    }
                    "single" === this.options.type ? (this.coords.p_bar_x = this.coords.p_handle / 2, this.coords.p_bar_w = this.coords.p_single_fake, this.result.from_percent = this.coords.p_single_real, this.result.from = this.convertToValue(this.coords.p_single_real), this.result.from_pretty = this._prettify(this.result.from), this.options.values.length && (this.result.from_value = this.options.values[this.result.from])) : (this.coords.p_bar_x = this.toFixed(this.coords.p_from_fake + this.coords.p_handle / 2), this.coords.p_bar_w = this.toFixed(this.coords.p_to_fake - this.coords.p_from_fake), this.result.from_percent = this.coords.p_from_real, this.result.from = this.convertToValue(this.coords.p_from_real), this.result.from_pretty = this._prettify(this.result.from), this.result.to_percent = this.coords.p_to_real, this.result.to = this.convertToValue(this.coords.p_to_real), this.result.to_pretty = this._prettify(this.result.to), this.options.values.length && (this.result.from_value = this.options.values[this.result.from], this.result.to_value = this.options.values[this.result.to]));
                    this.calcMinMax();
                    this.calcLabels()
                }
            }
        },
        calcPointerPercent: function() {
            this.coords.w_rs ? (0 > this.coords.x_pointer || isNaN(this.coords.x_pointer) ? this.coords.x_pointer = 0 : this.coords.x_pointer > this.coords.w_rs && (this.coords.x_pointer = this.coords.w_rs), this.coords.p_pointer = this.toFixed(this.coords.x_pointer / this.coords.w_rs * 100)) : this.coords.p_pointer = 0
        },
        convertToRealPercent: function(a) {
            return a / (100 - this.coords.p_handle) * 100
        },
        convertToFakePercent: function(a) {
            return a / 100 * (100 - this.coords.p_handle)
        },
        getHandleX: function() {
            var a = 100 - this.coords.p_handle,
                b = this.toFixed(this.coords.p_pointer - this.coords.p_gap);
            0 > b ? b = 0 : b > a && (b = a);
            return b
        },
        calcHandlePercent: function() {
            this.coords.w_handle = "single" === this.options.type ? this.$cache.s_single.outerWidth(!1) : this.$cache.s_from.outerWidth(!1);
            this.coords.p_handle = this.toFixed(this.coords.w_handle / this.coords.w_rs * 100)
        },
        chooseHandle: function(a) {
            return "single" === this.options.type ? "single" : a >= this.coords.p_from_real + (this.coords.p_to_real - this.coords.p_from_real) / 2 ? this.options.to_fixed ? "from" : "to" : this.options.from_fixed ? "to" : "from"
        },
        calcMinMax: function() {
            this.coords.w_rs && (this.labels.p_min = this.labels.w_min / this.coords.w_rs * 100, this.labels.p_max = this.labels.w_max / this.coords.w_rs * 100)
        },
        calcLabels: function() {
            this.coords.w_rs && !this.options.hide_from_to && ("single" === this.options.type ? (this.labels.w_single = this.$cache.single.outerWidth(!1), this.labels.p_single_fake = this.labels.w_single / this.coords.w_rs * 100, this.labels.p_single_left = this.coords.p_single_fake + this.coords.p_handle / 2 - this.labels.p_single_fake / 2) : (this.labels.w_from = this.$cache.from.outerWidth(!1), this.labels.p_from_fake = this.labels.w_from / this.coords.w_rs * 100, this.labels.p_from_left = this.coords.p_from_fake + this.coords.p_handle / 2 - this.labels.p_from_fake / 2, this.labels.p_from_left = this.toFixed(this.labels.p_from_left), this.labels.p_from_left = this.checkEdges(this.labels.p_from_left, this.labels.p_from_fake), this.labels.w_to = this.$cache.to.outerWidth(!1), this.labels.p_to_fake = this.labels.w_to / this.coords.w_rs * 100, this.labels.p_to_left = this.coords.p_to_fake + this.coords.p_handle / 2 - this.labels.p_to_fake / 2, this.labels.p_to_left = this.toFixed(this.labels.p_to_left), this.labels.p_to_left = this.checkEdges(this.labels.p_to_left, this.labels.p_to_fake), this.labels.w_single = this.$cache.single.outerWidth(!1), this.labels.p_single_fake = this.labels.w_single / this.coords.w_rs * 100, this.labels.p_single_left = (this.labels.p_from_left + this.labels.p_to_left + this.labels.p_to_fake) / 2 - this.labels.p_single_fake / 2, this.labels.p_single_left = this.toFixed(this.labels.p_single_left)), this.labels.p_single_left = this.checkEdges(this.labels.p_single_left, this.labels.p_single_fake))
        },
        updateScene: function() {
            this.raf_id && (cancelAnimationFrame(this.raf_id), this.raf_id = null);
            clearTimeout(this.update_tm);
            this.update_tm = null;
            this.options && (this.drawHandles(), this.is_active ? this.raf_id = requestAnimationFrame(this.updateScene.bind(this)) : this.update_tm = setTimeout(this.updateScene.bind(this), 300))
        },
        drawHandles: function() {
            this.coords.w_rs = this.$cache.rs.outerWidth(!1);
            if (this.coords.w_rs) {
                this.coords.w_rs !== this.coords.w_rs_old && (this.target = "base", this.is_resize = !0);
                if (this.coords.w_rs !== this.coords.w_rs_old || this.force_redraw) this.setMinMax(), this.calc(!0), this.drawLabels(), this.options.grid && (this.calcGridMargin(), this.calcGridLabels()), this.force_redraw = !0, this.coords.w_rs_old = this.coords.w_rs, this.drawShadow();
                if (this.coords.w_rs && (this.dragging || this.force_redraw || this.is_key)) {
                    if (this.old_from !== this.result.from || this.old_to !== this.result.to || this.force_redraw || this.is_key) {
                        this.drawLabels();
                        this.$cache.bar[0].style.left = this.coords.p_bar_x + "%";
                        this.$cache.bar[0].style.width = this.coords.p_bar_w + "%";
                        if ("single" === this.options.type) this.$cache.s_single[0].style.left = this.coords.p_single_fake + "%";
                        else {
                            this.$cache.s_from[0].style.left = this.coords.p_from_fake + "%";
                            this.$cache.s_to[0].style.left = this.coords.p_to_fake + "%";
                            if (this.old_from !== this.result.from || this.force_redraw) this.$cache.from[0].style.left = this.labels.p_from_left + "%";
                            if (this.old_to !== this.result.to || this.force_redraw) this.$cache.to[0].style.left = this.labels.p_to_left + "%"
                        }
                        this.$cache.single[0].style.left = this.labels.p_single_left + "%";
                        this.writeToInput();
                        this.old_from === this.result.from && this.old_to === this.result.to || this.is_start || (this.$cache.input.trigger("change"), this.$cache.input.trigger("input"));
                        this.old_from = this.result.from;
                        this.old_to = this.result.to;
                        this.is_resize || this.is_update || this.is_start || this.is_finish || this.callOnChange();
                        if (this.is_key || this.is_click) this.is_click = this.is_key = !1, this.callOnFinish();
                        this.is_finish = this.is_resize = this.is_update = !1
                    }
                    this.force_redraw = this.is_click = this.is_key = this.is_start = !1
                }
            }
        },
        drawLabels: function() {
            if (this.options) {
                var a = this.options.values.length,
                    b = this.options.p_values;
                if (!this.options.hide_from_to)
                    if ("single" === this.options.type) {
                        if (a) a = this.decorate(b[this.result.from]);
                        else {
                            var d = this._prettify(this.result.from);
                            a = this.decorate(d, this.result.from)
                        }
                        this.$cache.single.html(a);
                        this.calcLabels();
                        this.$cache.min[0].style.visibility = this.labels.p_single_left < this.labels.p_min + 1 ? "hidden" : "visible";
                        this.$cache.max[0].style.visibility = this.labels.p_single_left + this.labels.p_single_fake > 100 - this.labels.p_max - 1 ? "hidden" : "visible"
                    } else {
                        a ? (this.options.decorate_both ? (a = this.decorate(b[this.result.from]), a += this.options.values_separator, a += this.decorate(b[this.result.to])) : a = this.decorate(b[this.result.from] + this.options.values_separator + b[this.result.to]), d = this.decorate(b[this.result.from]), b = this.decorate(b[this.result.to])) : (d = this._prettify(this.result.from), b = this._prettify(this.result.to), this.options.decorate_both ? (a = this.decorate(d, this.result.from), a += this.options.values_separator, a += this.decorate(b, this.result.to)) : a = this.decorate(d + this.options.values_separator + b, this.result.to), d = this.decorate(d, this.result.from), b = this.decorate(b, this.result.to));
                        this.$cache.single.html(a);
                        this.$cache.from.html(d);
                        this.$cache.to.html(b);
                        this.calcLabels();
                        a = Math.min(this.labels.p_single_left, this.labels.p_from_left);
                        d = this.labels.p_single_left + this.labels.p_single_fake;
                        var b = this.labels.p_to_left + this.labels.p_to_fake,
                            c = Math.max(d, b);
                        this.labels.p_from_left + this.labels.p_from_fake >= this.labels.p_to_left ? (this.$cache.from[0].style.visibility = "hidden", this.$cache.to[0].style.visibility = "hidden", this.$cache.single[0].style.visibility = "visible", this.result.from === this.result.to ? ("from" === this.target ? this.$cache.from[0].style.visibility = "visible" : "to" === this.target ? this.$cache.to[0].style.visibility = "visible" : this.target || (this.$cache.from[0].style.visibility = "visible"), this.$cache.single[0].style.visibility = "hidden", c = b) : (this.$cache.from[0].style.visibility = "hidden", this.$cache.to[0].style.visibility = "hidden", this.$cache.single[0].style.visibility = "visible", c = Math.max(d, b))) : (this.$cache.from[0].style.visibility = "visible", this.$cache.to[0].style.visibility = "visible", this.$cache.single[0].style.visibility = "hidden");
                        this.$cache.min[0].style.visibility = a < this.labels.p_min + 1 ? "hidden" : "visible";
                        this.$cache.max[0].style.visibility = c > 100 - this.labels.p_max - 1 ? "hidden" : "visible"
                    }
            }
        },
        drawShadow: function() {
            var a = this.options,
                b = this.$cache,
                d = "number" === typeof a.from_min && !isNaN(a.from_min),
                c = "number" === typeof a.from_max && !isNaN(a.from_max),
                e = "number" === typeof a.to_min && !isNaN(a.to_min),
                g = "number" === typeof a.to_max && !isNaN(a.to_max);
            "single" === a.type ? a.from_shadow && (d || c) ? (d = this.convertToPercent(d ? a.from_min : a.min), c = this.convertToPercent(c ? a.from_max : a.max) - d, d = this.toFixed(d - this.coords.p_handle / 100 * d), c = this.toFixed(c - this.coords.p_handle / 100 * c), d += this.coords.p_handle / 2, b.shad_single[0].style.display = "block", b.shad_single[0].style.left = d + "%", b.shad_single[0].style.width = c + "%") : b.shad_single[0].style.display = "none" : (a.from_shadow && (d || c) ? (d = this.convertToPercent(d ? a.from_min : a.min), c = this.convertToPercent(c ? a.from_max : a.max) - d, d = this.toFixed(d - this.coords.p_handle / 100 * d), c = this.toFixed(c - this.coords.p_handle / 100 * c), d += this.coords.p_handle / 2, b.shad_from[0].style.display = "block", b.shad_from[0].style.left = d + "%", b.shad_from[0].style.width = c + "%") : b.shad_from[0].style.display = "none", a.to_shadow && (e || g) ? (e = this.convertToPercent(e ? a.to_min : a.min), a = this.convertToPercent(g ? a.to_max : a.max) - e, e = this.toFixed(e - this.coords.p_handle / 100 * e), a = this.toFixed(a - this.coords.p_handle / 100 * a), e += this.coords.p_handle / 2, b.shad_to[0].style.display = "block", b.shad_to[0].style.left = e + "%", b.shad_to[0].style.width = a + "%") : b.shad_to[0].style.display = "none")
        },
        writeToInput: function() {
            "single" === this.options.type ? (this.options.values.length ? this.$cache.input.prop("value", this.result.from_value) : this.$cache.input.prop("value", this.result.from), this.$cache.input.data("from", this.result.from)) : (this.options.values.length ? this.$cache.input.prop("value", this.result.from_value + this.options.input_values_separator + this.result.to_value) : this.$cache.input.prop("value", this.result.from + this.options.input_values_separator + this.result.to), this.$cache.input.data("from", this.result.from), this.$cache.input.data("to", this.result.to))
        },
        callOnStart: function() {
            this.writeToInput();
            if (this.options.onStart && "function" === typeof this.options.onStart)
                if (this.options.scope) this.options.onStart.call(this.options.scope, this.result);
                else this.options.onStart(this.result)
        },
        callOnChange: function() {
            this.writeToInput();
            if (this.options.onChange && "function" === typeof this.options.onChange)
                if (this.options.scope) this.options.onChange.call(this.options.scope, this.result);
                else this.options.onChange(this.result)
        },
        callOnFinish: function() {
            this.writeToInput();
            if (this.options.onFinish && "function" === typeof this.options.onFinish)
                if (this.options.scope) this.options.onFinish.call(this.options.scope, this.result);
                else this.options.onFinish(this.result)
        },
        callOnUpdate: function() {
            this.writeToInput();
            if (this.options.onUpdate && "function" === typeof this.options.onUpdate)
                if (this.options.scope) this.options.onUpdate.call(this.options.scope, this.result);
                else this.options.onUpdate(this.result)
        },
        toggleInput: function() {
            this.$cache.input.toggleClass("irs-hidden-input");
            this.has_tab_index ? this.$cache.input.prop("tabindex", -1) : this.$cache.input.removeProp("tabindex");
            this.has_tab_index = !this.has_tab_index
        },
        convertToPercent: function(a, b) {
            var d = this.options.max - this.options.min;
            return d ? this.toFixed((b ? a : a - this.options.min) / (d / 100)) : (this.no_diapason = !0, 0)
        },
        convertToValue: function(a) {
            var b = this.options.min,
                d = this.options.max,
                c = b.toString().split(".")[1],
                e = d.toString().split(".")[1],
                g, l, f = 0,
                h = 0;
            if (0 === a) return this.options.min;
            if (100 === a) return this.options.max;
            c && (f = g = c.length);
            e && (f = l = e.length);
            g && l && (f = g >= l ? g : l);
            0 > b && (h = Math.abs(b), b = +(b + h).toFixed(f), d = +(d + h).toFixed(f));
            a = (d - b) / 100 * a + b;
            (b = this.options.step.toString().split(".")[1]) ? a = +a.toFixed(b.length): (a /= this.options.step, a *= this.options.step, a = +a.toFixed(0));
            h && (a -= h);
            h = b ? +a.toFixed(b.length) : this.toFixed(a);
            h < this.options.min ? h = this.options.min : h > this.options.max && (h = this.options.max);
            return h
        },
        calcWithStep: function(a) {
            var b = Math.round(a / this.coords.p_step) * this.coords.p_step;
            100 < b && (b = 100);
            100 === a && (b = 100);
            return this.toFixed(b)
        },
        checkMinInterval: function(a, b, d) {
            var c = this.options;
            if (!c.min_interval) return a;
            a = this.convertToValue(a);
            b = this.convertToValue(b);
            "from" === d ? b - a < c.min_interval && (a = b - c.min_interval) : a - b < c.min_interval && (a = b + c.min_interval);
            return this.convertToPercent(a)
        },
        checkMaxInterval: function(a, b, d) {
            var c = this.options;
            if (!c.max_interval) return a;
            a = this.convertToValue(a);
            b = this.convertToValue(b);
            "from" === d ? b - a > c.max_interval && (a = b - c.max_interval) : a - b > c.max_interval && (a = b + c.max_interval);
            return this.convertToPercent(a)
        },
        checkDiapason: function(a, b, d) {
            a = this.convertToValue(a);
            var c = this.options;
            "number" !== typeof b && (b = c.min);
            "number" !== typeof d && (d = c.max);
            a < b && (a = b);
            a > d && (a = d);
            return this.convertToPercent(a)
        },
        toFixed: function(a) {
            a = a.toFixed(20);
            return +a
        },
        _prettify: function(a) {
            return this.options.prettify_enabled ? this.options.prettify && "function" === typeof this.options.prettify ? this.options.prettify(a) : this.prettify(a) : a
        },
        prettify: function(a) {
            return a.toString().replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, "$1" + this.options.prettify_separator)
        },
        checkEdges: function(a, b) {
            if (!this.options.force_edges) return this.toFixed(a);
            0 > a ? a = 0 : a > 100 - b && (a = 100 - b);
            return this.toFixed(a)
        },
        validate: function() {
            var a = this.options,
                b = this.result,
                d = a.values,
                c = d.length,
                e;
            "string" === typeof a.min && (a.min = +a.min);
            "string" === typeof a.max && (a.max = +a.max);
            "string" === typeof a.from && (a.from = +a.from);
            "string" === typeof a.to && (a.to = +a.to);
            "string" === typeof a.step && (a.step = +a.step);
            "string" === typeof a.from_min && (a.from_min = +a.from_min);
            "string" === typeof a.from_max && (a.from_max = +a.from_max);
            "string" === typeof a.to_min && (a.to_min = +a.to_min);
            "string" === typeof a.to_max && (a.to_max = +a.to_max);
            "string" === typeof a.grid_num && (a.grid_num = +a.grid_num);
            a.max < a.min && (a.max = a.min);
            if (c)
                for (a.p_values = [], a.min = 0, a.max = c - 1, a.step = 1, a.grid_num = a.max, a.grid_snap = !0, e = 0; e < c; e++) {
                    var g = +d[e];
                    isNaN(g) ? g = d[e] : (d[e] = g, g = this._prettify(g));
                    a.p_values.push(g)
                }
            if ("number" !== typeof a.from || isNaN(a.from)) a.from = a.min;
            if ("number" !== typeof a.to || isNaN(a.to)) a.to = a.max;
            "single" === a.type ? (a.from < a.min && (a.from = a.min), a.from > a.max && (a.from = a.max)) : (a.from < a.min && (a.from = a.min), a.from > a.max && (a.from = a.max), a.to < a.min && (a.to = a.min), a.to > a.max && (a.to = a.max), this.update_check.from && (this.update_check.from !== a.from && a.from > a.to && (a.from = a.to), this.update_check.to !== a.to && a.to < a.from && (a.to = a.from)), a.from > a.to && (a.from = a.to), a.to < a.from && (a.to = a.from));
            if ("number" !== typeof a.step || isNaN(a.step) || !a.step || 0 > a.step) a.step = 1;
            "number" === typeof a.from_min && a.from < a.from_min && (a.from = a.from_min);
            "number" === typeof a.from_max && a.from > a.from_max && (a.from = a.from_max);
            "number" === typeof a.to_min && a.to < a.to_min && (a.to = a.to_min);
            "number" === typeof a.to_max && a.from > a.to_max && (a.to = a.to_max);
            if (b) {
                b.min !== a.min && (b.min = a.min);
                b.max !== a.max && (b.max = a.max);
                if (b.from < b.min || b.from > b.max) b.from = a.from;
                if (b.to < b.min || b.to > b.max) b.to = a.to
            }
            if ("number" !== typeof a.min_interval || isNaN(a.min_interval) || !a.min_interval || 0 > a.min_interval) a.min_interval = 0;
            if ("number" !== typeof a.max_interval || isNaN(a.max_interval) || !a.max_interval || 0 > a.max_interval) a.max_interval = 0;
            a.min_interval && a.min_interval > a.max - a.min && (a.min_interval = a.max - a.min);
            a.max_interval && a.max_interval > a.max - a.min && (a.max_interval = a.max - a.min)
        },
        decorate: function(a, b) {
            var d = "",
                c = this.options;
            c.prefix && (d += c.prefix);
            d += a;
            c.max_postfix && (c.values.length && a === c.p_values[c.max] ? (d += c.max_postfix, c.postfix && (d += " ")) : b === c.max && (d += c.max_postfix, c.postfix && (d += " ")));
            c.postfix && (d += c.postfix);
            return d
        },
        updateFrom: function() {
            this.result.from = this.options.from;
            this.result.from_percent = this.convertToPercent(this.result.from);
            this.result.from_pretty = this._prettify(this.result.from);
            this.options.values && (this.result.from_value = this.options.values[this.result.from])
        },
        updateTo: function() {
            this.result.to = this.options.to;
            this.result.to_percent = this.convertToPercent(this.result.to);
            this.result.to_pretty = this._prettify(this.result.to);
            this.options.values && (this.result.to_value = this.options.values[this.result.to])
        },
        updateResult: function() {
            this.result.min = this.options.min;
            this.result.max = this.options.max;
            this.updateFrom();
            this.updateTo()
        },
        appendGrid: function() {
            if (this.options.grid) {
                var a = this.options,
                    b;
                var d = a.max - a.min;
                var c = a.grid_num,
                    e = 4,
                    g = "";
                this.calcGridMargin();
                if (a.grid_snap)
                    if (50 < d) {
                        c = 50 / a.step;
                        var f = this.toFixed(a.step / .5)
                    } else c = d / a.step, f = this.toFixed(a.step / (d / 100));
                else f = this.toFixed(100 / c);
                4 < c && (e = 3);
                7 < c && (e = 2);
                14 < c && (e = 1);
                28 < c && (e = 0);
                for (d = 0; d < c + 1; d++) {
                    var k = e;
                    var h = this.toFixed(f * d);
                    100 < h && (h = 100);
                    this.coords.big[d] = h;
                    var m = (h - f * (d - 1)) / (k + 1);
                    for (b = 1; b <= k && 0 !== h; b++) {
                        var n = this.toFixed(h - m * b);
                        g += '<span class="irs-grid-pol small" style="left: ' + n + '%"></span>'
                    }
                    g += '<span class="irs-grid-pol" style="left: ' + h + '%"></span>';
                    b = this.convertToValue(h);
                    b = a.values.length ? a.p_values[b] : this._prettify(b);
                    g += '<span class="irs-grid-text js-grid-text-' + d + '" style="left: ' + h + '%">' + b + "</span>"
                }
                this.coords.big_num = Math.ceil(c + 1);
                this.$cache.cont.addClass("irs-with-grid");
                this.$cache.grid.html(g);
                this.cacheGridLabels()
            }
        },
        cacheGridLabels: function() {
            var a, b = this.coords.big_num;
            for (a = 0; a < b; a++) {
                var d = this.$cache.grid.find(".js-grid-text-" + a);
                this.$cache.grid_labels.push(d)
            }
            this.calcGridLabels()
        },
        calcGridLabels: function() {
            var a;
            var b = [];
            var d = [],
                c = this.coords.big_num;
            for (a = 0; a < c; a++) this.coords.big_w[a] = this.$cache.grid_labels[a].outerWidth(!1), this.coords.big_p[a] = this.toFixed(this.coords.big_w[a] / this.coords.w_rs * 100), this.coords.big_x[a] = this.toFixed(this.coords.big_p[a] / 2), b[a] = this.toFixed(this.coords.big[a] - this.coords.big_x[a]), d[a] = this.toFixed(b[a] + this.coords.big_p[a]);
            this.options.force_edges && (b[0] < -this.coords.grid_gap && (b[0] = -this.coords.grid_gap, d[0] = this.toFixed(b[0] + this.coords.big_p[0]), this.coords.big_x[0] = this.coords.grid_gap), d[c - 1] > 100 + this.coords.grid_gap && (d[c - 1] = 100 + this.coords.grid_gap, b[c - 1] = this.toFixed(d[c - 1] - this.coords.big_p[c - 1]), this.coords.big_x[c - 1] = this.toFixed(this.coords.big_p[c - 1] - this.coords.grid_gap)));
            this.calcGridCollision(2, b, d);
            this.calcGridCollision(4, b, d);
            for (a = 0; a < c; a++) b = this.$cache.grid_labels[a][0], this.coords.big_x[a] !== Number.POSITIVE_INFINITY && (b.style.marginLeft = -this.coords.big_x[a] + "%")
        },
        calcGridCollision: function(a, b, d) {
            var c, e = this.coords.big_num;
            for (c = 0; c < e; c += a) {
                var g = c + a / 2;
                if (g >= e) break;
                var f = this.$cache.grid_labels[g][0];
                f.style.visibility = d[c] <= b[g] ? "visible" : "hidden"
            }
        },
        calcGridMargin: function() {
            this.options.grid_margin && (this.coords.w_rs = this.$cache.rs.outerWidth(!1), this.coords.w_rs && (this.coords.w_handle = "single" === this.options.type ? this.$cache.s_single.outerWidth(!1) : this.$cache.s_from.outerWidth(!1), this.coords.p_handle = this.toFixed(this.coords.w_handle / this.coords.w_rs * 100), this.coords.grid_gap = this.toFixed(this.coords.p_handle / 2 - .1), this.$cache.grid[0].style.width = this.toFixed(100 - this.coords.p_handle) + "%", this.$cache.grid[0].style.left = this.coords.grid_gap + "%"))
        },
        update: function(a) {
            this.input && (this.is_update = !0, this.options.from = this.result.from, this.options.to = this.result.to, this.update_check.from = this.result.from, this.update_check.to = this.result.to, this.options = f.extend(this.options, a), this.validate(), this.updateResult(a), this.toggleInput(), this.remove(), this.init(!0))
        },
        reset: function() {
            this.input && (this.updateResult(), this.update())
        },
        destroy: function() {
            this.input && (this.toggleInput(), this.$cache.input.prop("readonly", !1), f.data(this.input, "ionRangeSlider", null), this.remove(), this.options = this.input = null)
        }
    };
    f.fn.ionRangeSlider = function(a) {
        return this.each(function() {
            f.data(this, "ionRangeSlider") || f.data(this, "ionRangeSlider", new q(this, a, t++))
        })
    };
    (function() {
        for (var a = 0, b = ["ms", "moz", "webkit", "o"], d = 0; d < b.length && !k.requestAnimationFrame; ++d) k.requestAnimationFrame = k[b[d] + "RequestAnimationFrame"], k.cancelAnimationFrame = k[b[d] + "CancelAnimationFrame"] || k[b[d] + "CancelRequestAnimationFrame"];
        k.requestAnimationFrame || (k.requestAnimationFrame = function(b, d) {
            var c = (new Date).getTime(),
                e = Math.max(0, 16 - (c - a)),
                f = k.setTimeout(function() {
                    b(c + e)
                }, e);
            a = c + e;
            return f
        });
        k.cancelAnimationFrame || (k.cancelAnimationFrame = function(a) {
            clearTimeout(a)
        })
    })()
}) 
    /* imagesloaded */
    ! function(t, e) {
        "function" == typeof define && define.amd ? define("ev-emitter/ev-emitter", e) : "object" == typeof module && module.exports ? module.exports = e() : t.EvEmitter = e()
    }("undefined" != typeof window ? window : this, function() {
        function t() {}
        var e = t.prototype;
        return e.on = function(t, e) {
            if (t && e) {
                var i = this._events = this._events || {},
                    n = i[t] = i[t] || [];
                return -1 == n.indexOf(e) && n.push(e), this
            }
        }, e.once = function(t, e) {
            if (t && e) {
                this.on(t, e);
                var i = this._onceEvents = this._onceEvents || {},
                    n = i[t] = i[t] || {};
                return n[e] = !0, this
            }
        }, e.off = function(t, e) {
            var i = this._events && this._events[t];
            if (i && i.length) {
                var n = i.indexOf(e);
                return -1 != n && i.splice(n, 1), this
            }
        }, e.emitEvent = function(t, e) {
            var i = this._events && this._events[t];
            if (i && i.length) {
                var n = 0,
                    o = i[n];
                e = e || [];
                for (var r = this._onceEvents && this._onceEvents[t]; o;) {
                    var s = r && r[o];
                    s && (this.off(t, o), delete r[o]), o.apply(this, e), n += s ? 0 : 1, o = i[n]
                }
                return this
            }
        }, t
    }),
    function(t, e) {
        "use strict";
        "function" == typeof define && define.amd ? define(["ev-emitter/ev-emitter"], function(i) {
            return e(t, i)
        }) : "object" == typeof module && module.exports ? module.exports = e(t, require("ev-emitter")) : t.imagesLoaded = e(t, t.EvEmitter)
    }("undefined" != typeof window ? window : this, function(t, e) {
        function i(t, e) {
            for (var i in e) t[i] = e[i];
            return t
        }

        function n(t) {
            var e = [];
            if (Array.isArray(t)) e = t;
            else if ("number" == typeof t.length)
                for (var i = 0; i < t.length; i++) e.push(t[i]);
            else e.push(t);
            return e
        }

        function o(t, e, r) {
            return this instanceof o ? ("string" == typeof t && (t = document.querySelectorAll(t)), this.elements = n(t), this.options = i({}, this.options), "function" == typeof e ? r = e : i(this.options, e), r && this.on("always", r), this.getImages(), h && (this.jqDeferred = new h.Deferred), void setTimeout(function() {
                this.check()
            }.bind(this))) : new o(t, e, r)
        }

        function r(t) {
            this.img = t
        }

        function s(t, e) {
            this.url = t, this.element = e, this.img = new Image
        }
        var h = t.jQuery,
            a = t.console;
        o.prototype = Object.create(e.prototype), o.prototype.options = {}, o.prototype.getImages = function() {
            this.images = [], this.elements.forEach(this.addElementImages, this)
        }, o.prototype.addElementImages = function(t) {
            "IMG" == t.nodeName && this.addImage(t), this.options.background === !0 && this.addElementBackgroundImages(t);
            var e = t.nodeType;
            if (e && d[e]) {
                for (var i = t.querySelectorAll("img"), n = 0; n < i.length; n++) {
                    var o = i[n];
                    this.addImage(o)
                }
                if ("string" == typeof this.options.background) {
                    var r = t.querySelectorAll(this.options.background);
                    for (n = 0; n < r.length; n++) {
                        var s = r[n];
                        this.addElementBackgroundImages(s)
                    }
                }
            }
        };
        var d = {
            1: !0,
            9: !0,
            11: !0
        };
        return o.prototype.addElementBackgroundImages = function(t) {
            var e = getComputedStyle(t);
            if (e)
                for (var i = /url\((['"])?(.*?)\1\)/gi, n = i.exec(e.backgroundImage); null !== n;) {
                    var o = n && n[2];
                    o && this.addBackground(o, t), n = i.exec(e.backgroundImage)
                }
        }, o.prototype.addImage = function(t) {
            var e = new r(t);
            this.images.push(e)
        }, o.prototype.addBackground = function(t, e) {
            var i = new s(t, e);
            this.images.push(i)
        }, o.prototype.check = function() {
            function t(t, i, n) {
                setTimeout(function() {
                    e.progress(t, i, n)
                })
            }
            var e = this;
            return this.progressedCount = 0, this.hasAnyBroken = !1, this.images.length ? void this.images.forEach(function(e) {
                e.once("progress", t), e.check()
            }) : void this.complete()
        }, o.prototype.progress = function(t, e, i) {
            this.progressedCount++, this.hasAnyBroken = this.hasAnyBroken || !t.isLoaded, this.emitEvent("progress", [this, t, e]), this.jqDeferred && this.jqDeferred.notify && this.jqDeferred.notify(this, t), this.progressedCount == this.images.length && this.complete(), this.options.debug && a && a.log("progress: " + i, t, e)
        }, o.prototype.complete = function() {
            var t = this.hasAnyBroken ? "fail" : "done";
            if (this.isComplete = !0, this.emitEvent(t, [this]), this.emitEvent("always", [this]), this.jqDeferred) {
                var e = this.hasAnyBroken ? "reject" : "resolve";
                this.jqDeferred[e](this)
            }
        }, r.prototype = Object.create(e.prototype), r.prototype.check = function() {
            var t = this.getIsImageComplete();
            return t ? void this.confirm(0 !== this.img.naturalWidth, "naturalWidth") : (this.proxyImage = new Image, this.proxyImage.addEventListener("load", this), this.proxyImage.addEventListener("error", this), this.img.addEventListener("load", this), this.img.addEventListener("error", this), void(this.proxyImage.src = this.img.src))
        }, r.prototype.getIsImageComplete = function() {
            return this.img.complete && void 0 !== this.img.naturalWidth
        }, r.prototype.confirm = function(t, e) {
            this.isLoaded = t, this.emitEvent("progress", [this, this.img, e])
        }, r.prototype.handleEvent = function(t) {
            var e = "on" + t.type;
            this[e] && this[e](t)
        }, r.prototype.onload = function() {
            this.confirm(!0, "onload"), this.unbindEvents()
        }, r.prototype.onerror = function() {
            this.confirm(!1, "onerror"), this.unbindEvents()
        }, r.prototype.unbindEvents = function() {
            this.proxyImage.removeEventListener("load", this), this.proxyImage.removeEventListener("error", this), this.img.removeEventListener("load", this), this.img.removeEventListener("error", this)
        }, s.prototype = Object.create(r.prototype), s.prototype.check = function() {
            this.img.addEventListener("load", this), this.img.addEventListener("error", this), this.img.src = this.url;
            var t = this.getIsImageComplete();
            t && (this.confirm(0 !== this.img.naturalWidth, "naturalWidth"), this.unbindEvents())
        }, s.prototype.unbindEvents = function() {
            this.img.removeEventListener("load", this), this.img.removeEventListener("error", this)
        }, s.prototype.confirm = function(t, e) {
            this.isLoaded = t, this.emitEvent("progress", [this, this.element, e])
        }, o.makeJQueryPlugin = function(e) {
            e = e || t.jQuery, e && (h = e, h.fn.imagesLoaded = function(t, e) {
                var i = new o(this, t, e);
                return i.jqDeferred.promise(h(this))
            })
        }, o.makeJQueryPlugin(), o
    });
/* Owl Carousel v2.3.4 */
! function(a, b, c, d) {
    function e(b, c) {
        this.settings = null, this.options = a.extend({}, e.Defaults, c), this.$element = a(b), this._handlers = {}, this._plugins = {}, this._supress = {}, this._current = null, this._speed = null, this._coordinates = [], this._breakpoint = null, this._width = null, this._items = [], this._clones = [], this._mergers = [], this._widths = [], this._invalidated = {}, this._pipe = [], this._drag = {
            time: null,
            target: null,
            pointer: null,
            stage: {
                start: null,
                current: null
            },
            direction: null
        }, this._states = {
            current: {},
            tags: {
                initializing: ["busy"],
                animating: ["busy"],
                dragging: ["interacting"]
            }
        }, a.each(["onResize", "onThrottledResize"], a.proxy(function(b, c) {
            this._handlers[c] = a.proxy(this[c], this)
        }, this)), a.each(e.Plugins, a.proxy(function(a, b) {
            this._plugins[a.charAt(0).toLowerCase() + a.slice(1)] = new b(this)
        }, this)), a.each(e.Workers, a.proxy(function(b, c) {
            this._pipe.push({
                filter: c.filter,
                run: a.proxy(c.run, this)
            })
        }, this)), this.setup(), this.initialize()
    }
    e.Defaults = {
        items: 3,
        loop: !1,
        center: !1,
        rewind: !1,
        checkVisibility: !0,
        mouseDrag: !0,
        touchDrag: !0,
        pullDrag: !0,
        freeDrag: !1,
        margin: 0,
        stagePadding: 0,
        merge: !1,
        mergeFit: !0,
        autoWidth: !1,
        startPosition: 0,
        rtl: !1,
        smartSpeed: 250,
        fluidSpeed: !1,
        dragEndSpeed: !1,
        responsive: {},
        responsiveRefreshRate: 200,
        responsiveBaseElement: b,
        fallbackEasing: "swing",
        slideTransition: "",
        info: !1,
        nestedItemSelector: !1,
        itemElement: "div",
        stageElement: "div",
        refreshClass: "owl-refresh",
        loadedClass: "owl-loaded",
        loadingClass: "owl-loading",
        rtlClass: "owl-rtl",
        responsiveClass: "owl-responsive",
        dragClass: "owl-drag",
        itemClass: "owl-item",
        stageClass: "owl-stage",
        stageOuterClass: "owl-stage-outer",
        grabClass: "owl-grab"
    }, e.Width = {
        Default: "default",
        Inner: "inner",
        Outer: "outer"
    }, e.Type = {
        Event: "event",
        State: "state"
    }, e.Plugins = {}, e.Workers = [{
        filter: ["width", "settings"],
        run: function() {
            this._width = this.$element.width()
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function(a) {
            a.current = this._items && this._items[this.relative(this._current)]
        }
    }, {
        filter: ["items", "settings"],
        run: function() {
            this.$stage.children(".cloned").remove()
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function(a) {
            var b = this.settings.margin || "",
                c = !this.settings.autoWidth,
                d = this.settings.rtl,
                e = {
                    width: "auto",
                    "margin-left": d ? b : "",
                    "margin-right": d ? "" : b
                };
            !c && this.$stage.children().css(e), a.css = e
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function(a) {
            var b = (this.width() / this.settings.items).toFixed(3) - this.settings.margin,
                c = null,
                d = this._items.length,
                e = !this.settings.autoWidth,
                f = [];
            for (a.items = {
                    merge: !1,
                    width: b
                }; d--;) c = this._mergers[d], c = this.settings.mergeFit && Math.min(c, this.settings.items) || c, a.items.merge = c > 1 || a.items.merge, f[d] = e ? b * c : this._items[d].width();
            this._widths = f
        }
    }, {
        filter: ["items", "settings"],
        run: function() {
            var b = [],
                c = this._items,
                d = this.settings,
                e = Math.max(2 * d.items, 4),
                f = 2 * Math.ceil(c.length / 2),
                g = d.loop && c.length ? d.rewind ? e : Math.max(e, f) : 0,
                h = "",
                i = "";
            for (g /= 2; g > 0;) b.push(this.normalize(b.length / 2, !0)), h += c[b[b.length - 1]][0].outerHTML, b.push(this.normalize(c.length - 1 - (b.length - 1) / 2, !0)), i = c[b[b.length - 1]][0].outerHTML + i, g -= 1;
            this._clones = b, a(h).addClass("cloned").appendTo(this.$stage), a(i).addClass("cloned").prependTo(this.$stage)
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function() {
            for (var a = this.settings.rtl ? 1 : -1, b = this._clones.length + this._items.length, c = -1, d = 0, e = 0, f = []; ++c < b;) d = f[c - 1] || 0, e = this._widths[this.relative(c)] + this.settings.margin, f.push(d + e * a);
            this._coordinates = f
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function() {
            var a = this.settings.stagePadding,
                b = this._coordinates,
                c = {
                    width: Math.ceil(Math.abs(b[b.length - 1])) + 2 * a,
                    "padding-left": a || "",
                    "padding-right": a || ""
                };
            this.$stage.css(c)
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function(a) {
            var b = this._coordinates.length,
                c = !this.settings.autoWidth,
                d = this.$stage.children();
            if (c && a.items.merge)
                for (; b--;) a.css.width = this._widths[this.relative(b)], d.eq(b).css(a.css);
            else c && (a.css.width = a.items.width, d.css(a.css))
        }
    }, {
        filter: ["items"],
        run: function() {
            this._coordinates.length < 1 && this.$stage.removeAttr("style")
        }
    }, {
        filter: ["width", "items", "settings"],
        run: function(a) {
            a.current = a.current ? this.$stage.children().index(a.current) : 0, a.current = Math.max(this.minimum(), Math.min(this.maximum(), a.current)), this.reset(a.current)
        }
    }, {
        filter: ["position"],
        run: function() {
            this.animate(this.coordinates(this._current))
        }
    }, {
        filter: ["width", "position", "items", "settings"],
        run: function() {
            var a, b, c, d, e = this.settings.rtl ? 1 : -1,
                f = 2 * this.settings.stagePadding,
                g = this.coordinates(this.current()) + f,
                h = g + this.width() * e,
                i = [];
            for (c = 0, d = this._coordinates.length; c < d; c++) a = this._coordinates[c - 1] || 0, b = Math.abs(this._coordinates[c]) + f * e, (this.op(a, "<=", g) && this.op(a, ">", h) || this.op(b, "<", g) && this.op(b, ">", h)) && i.push(c);
            this.$stage.children(".active").removeClass("active"), this.$stage.children(":eq(" + i.join("), :eq(") + ")").addClass("active"), this.$stage.children(".center").removeClass("center"), this.settings.center && this.$stage.children().eq(this.current()).addClass("center")
        }
    }], e.prototype.initializeStage = function() {
        this.$stage = this.$element.find("." + this.settings.stageClass), this.$stage.length || (this.$element.addClass(this.options.loadingClass), this.$stage = a("<" + this.settings.stageElement + ">", {
            class: this.settings.stageClass
        }).wrap(a("<div/>", {
            class: this.settings.stageOuterClass
        })), this.$element.append(this.$stage.parent()))
    }, e.prototype.initializeItems = function() {
        var b = this.$element.find(".owl-item");
        if (b.length) return this._items = b.get().map(function(b) {
            return a(b)
        }), this._mergers = this._items.map(function() {
            return 1
        }), void this.refresh();
        this.replace(this.$element.children().not(this.$stage.parent())), this.isVisible() ? this.refresh() : this.invalidate("width"), this.$element.removeClass(this.options.loadingClass).addClass(this.options.loadedClass)
    }, e.prototype.initialize = function() {
        if (this.enter("initializing"), this.trigger("initialize"), this.$element.toggleClass(this.settings.rtlClass, this.settings.rtl), this.settings.autoWidth && !this.is("pre-loading")) {
            var a, b, c;
            a = this.$element.find("img"), b = this.settings.nestedItemSelector ? "." + this.settings.nestedItemSelector : d, c = this.$element.children(b).width(), a.length && c <= 0 && this.preloadAutoWidthImages(a)
        }
        this.initializeStage(), this.initializeItems(), this.registerEventHandlers(), this.leave("initializing"), this.trigger("initialized")
    }, e.prototype.isVisible = function() {
        return !this.settings.checkVisibility || this.$element.is(":visible")
    }, e.prototype.setup = function() {
        var b = this.viewport(),
            c = this.options.responsive,
            d = -1,
            e = null;
        c ? (a.each(c, function(a) {
            a <= b && a > d && (d = Number(a))
        }), e = a.extend({}, this.options, c[d]), "function" == typeof e.stagePadding && (e.stagePadding = e.stagePadding()), delete e.responsive, e.responsiveClass && this.$element.attr("class", this.$element.attr("class").replace(new RegExp("(" + this.options.responsiveClass + "-)\\S+\\s", "g"), "$1" + d))) : e = a.extend({}, this.options), this.trigger("change", {
            property: {
                name: "settings",
                value: e
            }
        }), this._breakpoint = d, this.settings = e, this.invalidate("settings"), this.trigger("changed", {
            property: {
                name: "settings",
                value: this.settings
            }
        })
    }, e.prototype.optionsLogic = function() {
        this.settings.autoWidth && (this.settings.stagePadding = !1, this.settings.merge = !1)
    }, e.prototype.prepare = function(b) {
        var c = this.trigger("prepare", {
            content: b
        });
        return c.data || (c.data = a("<" + this.settings.itemElement + "/>").addClass(this.options.itemClass).append(b)), this.trigger("prepared", {
            content: c.data
        }), c.data
    }, e.prototype.update = function() {
        for (var b = 0, c = this._pipe.length, d = a.proxy(function(a) {
                return this[a]
            }, this._invalidated), e = {}; b < c;)(this._invalidated.all || a.grep(this._pipe[b].filter, d).length > 0) && this._pipe[b].run(e), b++;
        this._invalidated = {}, !this.is("valid") && this.enter("valid")
    }, e.prototype.width = function(a) {
        switch (a = a || e.Width.Default) {
            case e.Width.Inner:
            case e.Width.Outer:
                return this._width;
            default:
                return this._width - 2 * this.settings.stagePadding + this.settings.margin
        }
    }, e.prototype.refresh = function() {
        this.enter("refreshing"), this.trigger("refresh"), this.setup(), this.optionsLogic(), this.$element.addClass(this.options.refreshClass), this.update(), this.$element.removeClass(this.options.refreshClass), this.leave("refreshing"), this.trigger("refreshed")
    }, e.prototype.onThrottledResize = function() {
        b.clearTimeout(this.resizeTimer), this.resizeTimer = b.setTimeout(this._handlers.onResize, this.settings.responsiveRefreshRate)
    }, e.prototype.onResize = function() {
        return !!this._items.length && (this._width !== this.$element.width() && (!!this.isVisible() && (this.enter("resizing"), this.trigger("resize").isDefaultPrevented() ? (this.leave("resizing"), !1) : (this.invalidate("width"), this.refresh(), this.leave("resizing"), void this.trigger("resized")))))
    }, e.prototype.registerEventHandlers = function() {
        a.support.transition && this.$stage.on(a.support.transition.end + ".owl.core", a.proxy(this.onTransitionEnd, this)), !1 !== this.settings.responsive && this.on(b, "resize", this._handlers.onThrottledResize), this.settings.mouseDrag && (this.$element.addClass(this.options.dragClass), this.$stage.on("mousedown.owl.core", a.proxy(this.onDragStart, this)), this.$stage.on("dragstart.owl.core selectstart.owl.core", function() {
            return !1
        })), this.settings.touchDrag && (this.$stage.on("touchstart.owl.core", a.proxy(this.onDragStart, this)), this.$stage.on("touchcancel.owl.core", a.proxy(this.onDragEnd, this)))
    }, e.prototype.onDragStart = function(b) {
        var d = null;
        3 !== b.which && (a.support.transform ? (d = this.$stage.css("transform").replace(/.*\(|\)| /g, "").split(","), d = {
            x: d[16 === d.length ? 12 : 4],
            y: d[16 === d.length ? 13 : 5]
        }) : (d = this.$stage.position(), d = {
            x: this.settings.rtl ? d.left + this.$stage.width() - this.width() + this.settings.margin : d.left,
            y: d.top
        }), this.is("animating") && (a.support.transform ? this.animate(d.x) : this.$stage.stop(), this.invalidate("position")), this.$element.toggleClass(this.options.grabClass, "mousedown" === b.type), this.speed(0), this._drag.time = (new Date).getTime(), this._drag.target = a(b.target), this._drag.stage.start = d, this._drag.stage.current = d, this._drag.pointer = this.pointer(b), a(c).on("mouseup.owl.core touchend.owl.core", a.proxy(this.onDragEnd, this)), a(c).one("mousemove.owl.core touchmove.owl.core", a.proxy(function(b) {
            var d = this.difference(this._drag.pointer, this.pointer(b));
            a(c).on("mousemove.owl.core touchmove.owl.core", a.proxy(this.onDragMove, this)), Math.abs(d.x) < Math.abs(d.y) && this.is("valid") || (b.preventDefault(), this.enter("dragging"), this.trigger("drag"))
        }, this)))
    }, e.prototype.onDragMove = function(a) {
        var b = null,
            c = null,
            d = null,
            e = this.difference(this._drag.pointer, this.pointer(a)),
            f = this.difference(this._drag.stage.start, e);
        this.is("dragging") && (a.preventDefault(), this.settings.loop ? (b = this.coordinates(this.minimum()), c = this.coordinates(this.maximum() + 1) - b, f.x = ((f.x - b) % c + c) % c + b) : (b = this.settings.rtl ? this.coordinates(this.maximum()) : this.coordinates(this.minimum()), c = this.settings.rtl ? this.coordinates(this.minimum()) : this.coordinates(this.maximum()), d = this.settings.pullDrag ? -1 * e.x / 5 : 0, f.x = Math.max(Math.min(f.x, b + d), c + d)), this._drag.stage.current = f, this.animate(f.x))
    }, e.prototype.onDragEnd = function(b) {
        var d = this.difference(this._drag.pointer, this.pointer(b)),
            e = this._drag.stage.current,
            f = d.x > 0 ^ this.settings.rtl ? "left" : "right";
        a(c).off(".owl.core"), this.$element.removeClass(this.options.grabClass), (0 !== d.x && this.is("dragging") || !this.is("valid")) && (this.speed(this.settings.dragEndSpeed || this.settings.smartSpeed), this.current(this.closest(e.x, 0 !== d.x ? f : this._drag.direction)), this.invalidate("position"), this.update(), this._drag.direction = f, (Math.abs(d.x) > 3 || (new Date).getTime() - this._drag.time > 300) && this._drag.target.one("click.owl.core", function() {
            return !1
        })), this.is("dragging") && (this.leave("dragging"), this.trigger("dragged"))
    }, e.prototype.closest = function(b, c) {
        var e = -1,
            f = 30,
            g = this.width(),
            h = this.coordinates();
        return this.settings.freeDrag || a.each(h, a.proxy(function(a, i) {
            return "left" === c && b > i - f && b < i + f ? e = a : "right" === c && b > i - g - f && b < i - g + f ? e = a + 1 : this.op(b, "<", i) && this.op(b, ">", h[a + 1] !== d ? h[a + 1] : i - g) && (e = "left" === c ? a + 1 : a), -1 === e
        }, this)), this.settings.loop || (this.op(b, ">", h[this.minimum()]) ? e = b = this.minimum() : this.op(b, "<", h[this.maximum()]) && (e = b = this.maximum())), e
    }, e.prototype.animate = function(b) {
        var c = this.speed() > 0;
        this.is("animating") && this.onTransitionEnd(), c && (this.enter("animating"), this.trigger("translate")), a.support.transform3d && a.support.transition ? this.$stage.css({
            transform: "translate3d(" + b + "px,0px,0px)",
            transition: this.speed() / 1e3 + "s" + (this.settings.slideTransition ? " " + this.settings.slideTransition : "")
        }) : c ? this.$stage.animate({
            left: b + "px"
        }, this.speed(), this.settings.fallbackEasing, a.proxy(this.onTransitionEnd, this)) : this.$stage.css({
            left: b + "px"
        })
    }, e.prototype.is = function(a) {
        return this._states.current[a] && this._states.current[a] > 0
    }, e.prototype.current = function(a) {
        if (a === d) return this._current;
        if (0 === this._items.length) return d;
        if (a = this.normalize(a), this._current !== a) {
            var b = this.trigger("change", {
                property: {
                    name: "position",
                    value: a
                }
            });
            b.data !== d && (a = this.normalize(b.data)), this._current = a, this.invalidate("position"), this.trigger("changed", {
                property: {
                    name: "position",
                    value: this._current
                }
            })
        }
        return this._current
    }, e.prototype.invalidate = function(b) {
        return "string" === a.type(b) && (this._invalidated[b] = !0, this.is("valid") && this.leave("valid")), a.map(this._invalidated, function(a, b) {
            return b
        })
    }, e.prototype.reset = function(a) {
        (a = this.normalize(a)) !== d && (this._speed = 0, this._current = a, this.suppress(["translate", "translated"]), this.animate(this.coordinates(a)), this.release(["translate", "translated"]))
    }, e.prototype.normalize = function(a, b) {
        var c = this._items.length,
            e = b ? 0 : this._clones.length;
        return !this.isNumeric(a) || c < 1 ? a = d : (a < 0 || a >= c + e) && (a = ((a - e / 2) % c + c) % c + e / 2), a
    }, e.prototype.relative = function(a) {
        return a -= this._clones.length / 2, this.normalize(a, !0)
    }, e.prototype.maximum = function(a) {
        var b, c, d, e = this.settings,
            f = this._coordinates.length;
        if (e.loop) f = this._clones.length / 2 + this._items.length - 1;
        else if (e.autoWidth || e.merge) {
            if (b = this._items.length)
                for (c = this._items[--b].width(), d = this.$element.width(); b-- && !((c += this._items[b].width() + this.settings.margin) > d););
            f = b + 1
        } else f = e.center ? this._items.length - 1 : this._items.length - e.items;
        return a && (f -= this._clones.length / 2), Math.max(f, 0)
    }, e.prototype.minimum = function(a) {
        return a ? 0 : this._clones.length / 2
    }, e.prototype.items = function(a) {
        return a === d ? this._items.slice() : (a = this.normalize(a, !0), this._items[a])
    }, e.prototype.mergers = function(a) {
        return a === d ? this._mergers.slice() : (a = this.normalize(a, !0), this._mergers[a])
    }, e.prototype.clones = function(b) {
        var c = this._clones.length / 2,
            e = c + this._items.length,
            f = function(a) {
                return a % 2 == 0 ? e + a / 2 : c - (a + 1) / 2
            };
        return b === d ? a.map(this._clones, function(a, b) {
            return f(b)
        }) : a.map(this._clones, function(a, c) {
            return a === b ? f(c) : null
        })
    }, e.prototype.speed = function(a) {
        return a !== d && (this._speed = a), this._speed
    }, e.prototype.coordinates = function(b) {
        var c, e = 1,
            f = b - 1;
        return b === d ? a.map(this._coordinates, a.proxy(function(a, b) {
            return this.coordinates(b)
        }, this)) : (this.settings.center ? (this.settings.rtl && (e = -1, f = b + 1), c = this._coordinates[b], c += (this.width() - c + (this._coordinates[f] || 0)) / 2 * e) : c = this._coordinates[f] || 0, c = Math.ceil(c))
    }, e.prototype.duration = function(a, b, c) {
        return 0 === c ? 0 : Math.min(Math.max(Math.abs(b - a), 1), 6) * Math.abs(c || this.settings.smartSpeed)
    }, e.prototype.to = function(a, b) {
        var c = this.current(),
            d = null,
            e = a - this.relative(c),
            f = (e > 0) - (e < 0),
            g = this._items.length,
            h = this.minimum(),
            i = this.maximum();
        this.settings.loop ? (!this.settings.rewind && Math.abs(e) > g / 2 && (e += -1 * f * g), a = c + e, (d = ((a - h) % g + g) % g + h) !== a && d - e <= i && d - e > 0 && (c = d - e, a = d, this.reset(c))) : this.settings.rewind ? (i += 1, a = (a % i + i) % i) : a = Math.max(h, Math.min(i, a)), this.speed(this.duration(c, a, b)), this.current(a), this.isVisible() && this.update()
    }, e.prototype.next = function(a) {
        a = a || !1, this.to(this.relative(this.current()) + 1, a)
    }, e.prototype.prev = function(a) {
        a = a || !1, this.to(this.relative(this.current()) - 1, a)
    }, e.prototype.onTransitionEnd = function(a) {
        if (a !== d && (a.stopPropagation(), (a.target || a.srcElement || a.originalTarget) !== this.$stage.get(0))) return !1;
        this.leave("animating"), this.trigger("translated")
    }, e.prototype.viewport = function() {
        var d;
        return this.options.responsiveBaseElement !== b ? d = a(this.options.responsiveBaseElement).width() : b.innerWidth ? d = b.innerWidth : c.documentElement && c.documentElement.clientWidth ? d = c.documentElement.clientWidth : console.warn("Can not detect viewport width."), d
    }, e.prototype.replace = function(b) {
        this.$stage.empty(), this._items = [], b && (b = b instanceof jQuery ? b : a(b)), this.settings.nestedItemSelector && (b = b.find("." + this.settings.nestedItemSelector)), b.filter(function() {
            return 1 === this.nodeType
        }).each(a.proxy(function(a, b) {
            b = this.prepare(b), this.$stage.append(b), this._items.push(b), this._mergers.push(1 * b.find("[data-merge]").addBack("[data-merge]").attr("data-merge") || 1)
        }, this)), this.reset(this.isNumeric(this.settings.startPosition) ? this.settings.startPosition : 0), this.invalidate("items")
    }, e.prototype.add = function(b, c) {
        var e = this.relative(this._current);
        c = c === d ? this._items.length : this.normalize(c, !0), b = b instanceof jQuery ? b : a(b), this.trigger("add", {
            content: b,
            position: c
        }), b = this.prepare(b), 0 === this._items.length || c === this._items.length ? (0 === this._items.length && this.$stage.append(b), 0 !== this._items.length && this._items[c - 1].after(b), this._items.push(b), this._mergers.push(1 * b.find("[data-merge]").addBack("[data-merge]").attr("data-merge") || 1)) : (this._items[c].before(b), this._items.splice(c, 0, b), this._mergers.splice(c, 0, 1 * b.find("[data-merge]").addBack("[data-merge]").attr("data-merge") || 1)), this._items[e] && this.reset(this._items[e].index()), this.invalidate("items"), this.trigger("added", {
            content: b,
            position: c
        })
    }, e.prototype.remove = function(a) {
        (a = this.normalize(a, !0)) !== d && (this.trigger("remove", {
            content: this._items[a],
            position: a
        }), this._items[a].remove(), this._items.splice(a, 1), this._mergers.splice(a, 1), this.invalidate("items"), this.trigger("removed", {
            content: null,
            position: a
        }))
    }, e.prototype.preloadAutoWidthImages = function(b) {
        b.each(a.proxy(function(b, c) {
            this.enter("pre-loading"), c = a(c), a(new Image).one("load", a.proxy(function(a) {
                c.attr("src", a.target.src), c.css("opacity", 1), this.leave("pre-loading"), !this.is("pre-loading") && !this.is("initializing") && this.refresh()
            }, this)).attr("src", c.attr("src") || c.attr("data-src") || c.attr("data-src-retina"))
        }, this))
    }, e.prototype.destroy = function() {
        this.$element.off(".owl.core"), this.$stage.off(".owl.core"), a(c).off(".owl.core"), !1 !== this.settings.responsive && (b.clearTimeout(this.resizeTimer), this.off(b, "resize", this._handlers.onThrottledResize));
        for (var d in this._plugins) this._plugins[d].destroy();
        this.$stage.children(".cloned").remove(), this.$stage.unwrap(), this.$stage.children().contents().unwrap(), this.$stage.children().unwrap(), this.$stage.remove(), this.$element.removeClass(this.options.refreshClass).removeClass(this.options.loadingClass).removeClass(this.options.loadedClass).removeClass(this.options.rtlClass).removeClass(this.options.dragClass).removeClass(this.options.grabClass).attr("class", this.$element.attr("class").replace(new RegExp(this.options.responsiveClass + "-\\S+\\s", "g"), "")).removeData("owl.carousel")
    }, e.prototype.op = function(a, b, c) {
        var d = this.settings.rtl;
        switch (b) {
            case "<":
                return d ? a > c : a < c;
            case ">":
                return d ? a < c : a > c;
            case ">=":
                return d ? a <= c : a >= c;
            case "<=":
                return d ? a >= c : a <= c
        }
    }, e.prototype.on = function(a, b, c, d) {
        a.addEventListener ? a.addEventListener(b, c, d) : a.attachEvent && a.attachEvent("on" + b, c)
    }, e.prototype.off = function(a, b, c, d) {
        a.removeEventListener ? a.removeEventListener(b, c, d) : a.detachEvent && a.detachEvent("on" + b, c)
    }, e.prototype.trigger = function(b, c, d, f, g) {
        var h = {
                item: {
                    count: this._items.length,
                    index: this.current()
                }
            },
            i = a.camelCase(a.grep(["on", b, d], function(a) {
                return a
            }).join("-").toLowerCase()),
            j = a.Event([b, "owl", d || "carousel"].join(".").toLowerCase(), a.extend({
                relatedTarget: this
            }, h, c));
        return this._supress[b] || (a.each(this._plugins, function(a, b) {
            b.onTrigger && b.onTrigger(j)
        }), this.register({
            type: e.Type.Event,
            name: b
        }), this.$element.trigger(j), this.settings && "function" == typeof this.settings[i] && this.settings[i].call(this, j)), j
    }, e.prototype.enter = function(b) {
        a.each([b].concat(this._states.tags[b] || []), a.proxy(function(a, b) {
            this._states.current[b] === d && (this._states.current[b] = 0), this._states.current[b]++
        }, this))
    }, e.prototype.leave = function(b) {
        a.each([b].concat(this._states.tags[b] || []), a.proxy(function(a, b) {
            this._states.current[b]--
        }, this))
    }, e.prototype.register = function(b) {
        if (b.type === e.Type.Event) {
            if (a.event.special[b.name] || (a.event.special[b.name] = {}), !a.event.special[b.name].owl) {
                var c = a.event.special[b.name]._default;
                a.event.special[b.name]._default = function(a) {
                    return !c || !c.apply || a.namespace && -1 !== a.namespace.indexOf("owl") ? a.namespace && a.namespace.indexOf("owl") > -1 : c.apply(this, arguments)
                }, a.event.special[b.name].owl = !0
            }
        } else b.type === e.Type.State && (this._states.tags[b.name] ? this._states.tags[b.name] = this._states.tags[b.name].concat(b.tags) : this._states.tags[b.name] = b.tags, this._states.tags[b.name] = a.grep(this._states.tags[b.name], a.proxy(function(c, d) {
            return a.inArray(c, this._states.tags[b.name]) === d
        }, this)))
    }, e.prototype.suppress = function(b) {
        a.each(b, a.proxy(function(a, b) {
            this._supress[b] = !0
        }, this))
    }, e.prototype.release = function(b) {
        a.each(b, a.proxy(function(a, b) {
            delete this._supress[b]
        }, this))
    }, e.prototype.pointer = function(a) {
        var c = {
            x: null,
            y: null
        };
        return a = a.originalEvent || a || b.event, a = a.touches && a.touches.length ? a.touches[0] : a.changedTouches && a.changedTouches.length ? a.changedTouches[0] : a, a.pageX ? (c.x = a.pageX, c.y = a.pageY) : (c.x = a.clientX, c.y = a.clientY), c
    }, e.prototype.isNumeric = function(a) {
        return !isNaN(parseFloat(a))
    }, e.prototype.difference = function(a, b) {
        return {
            x: a.x - b.x,
            y: a.y - b.y
        }
    }, a.fn.owlCarousel = function(b) {
        var c = Array.prototype.slice.call(arguments, 1);
        return this.each(function() {
            var d = a(this),
                f = d.data("owl.carousel");
            f || (f = new e(this, "object" == typeof b && b), d.data("owl.carousel", f), a.each(["next", "prev", "to", "destroy", "refresh", "replace", "add", "remove"], function(b, c) {
                f.register({
                    type: e.Type.Event,
                    name: c
                }), f.$element.on(c + ".owl.carousel.core", a.proxy(function(a) {
                    a.namespace && a.relatedTarget !== this && (this.suppress([c]), f[c].apply(this, [].slice.call(arguments, 1)), this.release([c]))
                }, f))
            })), "string" == typeof b && "_" !== b.charAt(0) && f[b].apply(f, c)
        })
    }, a.fn.owlCarousel.Constructor = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    var e = function(b) {
        this._core = b, this._interval = null, this._visible = null, this._handlers = {
            "initialized.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.autoRefresh && this.watch()
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers)
    };
    e.Defaults = {
        autoRefresh: !0,
        autoRefreshInterval: 500
    }, e.prototype.watch = function() {
        this._interval || (this._visible = this._core.isVisible(), this._interval = b.setInterval(a.proxy(this.refresh, this), this._core.settings.autoRefreshInterval))
    }, e.prototype.refresh = function() {
        this._core.isVisible() !== this._visible && (this._visible = !this._visible, this._core.$element.toggleClass("owl-hidden", !this._visible), this._visible && this._core.invalidate("width") && this._core.refresh())
    }, e.prototype.destroy = function() {
        var a, c;
        b.clearInterval(this._interval);
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (c in Object.getOwnPropertyNames(this)) "function" != typeof this[c] && (this[c] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.AutoRefresh = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    var e = function(b) {
        this._core = b, this._loaded = [], this._handlers = {
            "initialized.owl.carousel change.owl.carousel resized.owl.carousel": a.proxy(function(b) {
                if (b.namespace && this._core.settings && this._core.settings.lazyLoad && (b.property && "position" == b.property.name || "initialized" == b.type)) {
                    var c = this._core.settings,
                        e = c.center && Math.ceil(c.items / 2) || c.items,
                        f = c.center && -1 * e || 0,
                        g = (b.property && b.property.value !== d ? b.property.value : this._core.current()) + f,
                        h = this._core.clones().length,
                        i = a.proxy(function(a, b) {
                            this.load(b)
                        }, this);
                    for (c.lazyLoadEager > 0 && (e += c.lazyLoadEager, c.loop && (g -= c.lazyLoadEager, e++)); f++ < e;) this.load(h / 2 + this._core.relative(g)), h && a.each(this._core.clones(this._core.relative(g)), i), g++
                }
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers)
    };
    e.Defaults = {
        lazyLoad: !1,
        lazyLoadEager: 0
    }, e.prototype.load = function(c) {
        var d = this._core.$stage.children().eq(c),
            e = d && d.find(".owl-lazy");
        !e || a.inArray(d.get(0), this._loaded) > -1 || (e.each(a.proxy(function(c, d) {
            var e, f = a(d),
                g = b.devicePixelRatio > 1 && f.attr("data-src-retina") || f.attr("data-src") || f.attr("data-srcset");
            this._core.trigger("load", {
                element: f,
                url: g
            }, "lazy"), f.is("img") ? f.one("load.owl.lazy", a.proxy(function() {
                f.css("opacity", 1), this._core.trigger("loaded", {
                    element: f,
                    url: g
                }, "lazy")
            }, this)).attr("src", g) : f.is("source") ? f.one("load.owl.lazy", a.proxy(function() {
                this._core.trigger("loaded", {
                    element: f,
                    url: g
                }, "lazy")
            }, this)).attr("srcset", g) : (e = new Image, e.onload = a.proxy(function() {
                f.css({
                    "background-image": 'url("' + g + '")',
                    opacity: "1"
                }), this._core.trigger("loaded", {
                    element: f,
                    url: g
                }, "lazy")
            }, this), e.src = g)
        }, this)), this._loaded.push(d.get(0)))
    }, e.prototype.destroy = function() {
        var a, b;
        for (a in this.handlers) this._core.$element.off(a, this.handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Lazy = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    var e = function(c) {
        this._core = c, this._previousHeight = null, this._handlers = {
            "initialized.owl.carousel refreshed.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.autoHeight && this.update()
            }, this),
            "changed.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.autoHeight && "position" === a.property.name && this.update()
            }, this),
            "loaded.owl.lazy": a.proxy(function(a) {
                a.namespace && this._core.settings.autoHeight && a.element.closest("." + this._core.settings.itemClass).index() === this._core.current() && this.update()
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers), this._intervalId = null;
        var d = this;
        a(b).on("load", function() {
            d._core.settings.autoHeight && d.update()
        }), a(b).resize(function() {
            d._core.settings.autoHeight && (null != d._intervalId && clearTimeout(d._intervalId), d._intervalId = setTimeout(function() {
                d.update()
            }, 250))
        })
    };
    e.Defaults = {
        autoHeight: !1,
        autoHeightClass: "owl-height"
    }, e.prototype.update = function() {
        var b = this._core._current,
            c = b + this._core.settings.items,
            d = this._core.settings.lazyLoad,
            e = this._core.$stage.children().toArray().slice(b, c),
            f = [],
            g = 0;
        a.each(e, function(b, c) {
            f.push(a(c).height())
        }), g = Math.max.apply(null, f), g <= 1 && d && this._previousHeight && (g = this._previousHeight), this._previousHeight = g, this._core.$stage.parent().height(g).addClass(this._core.settings.autoHeightClass)
    }, e.prototype.destroy = function() {
        var a, b;
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.AutoHeight = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    var e = function(b) {
        this._core = b, this._videos = {}, this._playing = null, this._handlers = {
            "initialized.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.register({
                    type: "state",
                    name: "playing",
                    tags: ["interacting"]
                })
            }, this),
            "resize.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.video && this.isInFullScreen() && a.preventDefault()
            }, this),
            "refreshed.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.is("resizing") && this._core.$stage.find(".cloned .owl-video-frame").remove()
            }, this),
            "changed.owl.carousel": a.proxy(function(a) {
                a.namespace && "position" === a.property.name && this._playing && this.stop()
            }, this),
            "prepared.owl.carousel": a.proxy(function(b) {
                if (b.namespace) {
                    var c = a(b.content).find(".owl-video");
                    c.length && (c.css("display", "none"), this.fetch(c, a(b.content)))
                }
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this._core.$element.on(this._handlers), this._core.$element.on("click.owl.video", ".owl-video-play-icon", a.proxy(function(a) {
            this.play(a)
        }, this))
    };
    e.Defaults = {
        video: !1,
        videoHeight: !1,
        videoWidth: !1
    }, e.prototype.fetch = function(a, b) {
        var c = function() {
                return a.attr("data-vimeo-id") ? "vimeo" : a.attr("data-vzaar-id") ? "vzaar" : "youtube"
            }(),
            d = a.attr("data-vimeo-id") || a.attr("data-youtube-id") || a.attr("data-vzaar-id"),
            e = a.attr("data-width") || this._core.settings.videoWidth,
            f = a.attr("data-height") || this._core.settings.videoHeight,
            g = a.attr("href");
        if (!g) throw new Error("Missing video URL.");
        if (d = g.match(/(http:|https:|)\/\/(player.|www.|app.)?(vimeo\.com|youtu(be\.com|\.be|be\.googleapis\.com|be\-nocookie\.com)|vzaar\.com)\/(video\/|videos\/|embed\/|channels\/.+\/|groups\/.+\/|watch\?v=|v\/)?([A-Za-z0-9._%-]*)(\&\S+)?/), d[3].indexOf("youtu") > -1) c = "youtube";
        else if (d[3].indexOf("vimeo") > -1) c = "vimeo";
        else {
            if (!(d[3].indexOf("vzaar") > -1)) throw new Error("Video URL not supported.");
            c = "vzaar"
        }
        d = d[6], this._videos[g] = {
            type: c,
            id: d,
            width: e,
            height: f
        }, b.attr("data-video", g), this.thumbnail(a, this._videos[g])
    }, e.prototype.thumbnail = function(b, c) {
        var d, e, f, g = c.width && c.height ? "width:" + c.width + "px;height:" + c.height + "px;" : "",
            h = b.find("img"),
            i = "src",
            j = "",
            k = this._core.settings,
            l = function(c) {
                e = '<div class="owl-video-play-icon"></div>', d = k.lazyLoad ? a("<div/>", {
                    class: "owl-video-tn " + j,
                    srcType: c
                }) : a("<div/>", {
                    class: "owl-video-tn",
                    style: "opacity:1;background-image:url(" + c + ")"
                }), b.after(d), b.after(e)
            };
        if (b.wrap(a("<div/>", {
                class: "owl-video-wrapper",
                style: g
            })), this._core.settings.lazyLoad && (i = "data-src", j = "owl-lazy"), h.length) return l(h.attr(i)), h.remove(), !1;
        "youtube" === c.type ? (f = "//img.youtube.com/vi/" + c.id + "/hqdefault.jpg", l(f)) : "vimeo" === c.type ? a.ajax({
            type: "GET",
            url: "//vimeo.com/api/v2/video/" + c.id + ".json",
            jsonp: "callback",
            dataType: "jsonp",
            success: function(a) {
                f = a[0].thumbnail_large, l(f)
            }
        }) : "vzaar" === c.type && a.ajax({
            type: "GET",
            url: "//vzaar.com/api/videos/" + c.id + ".json",
            jsonp: "callback",
            dataType: "jsonp",
            success: function(a) {
                f = a.framegrab_url, l(f)
            }
        })
    }, e.prototype.stop = function() {
        this._core.trigger("stop", null, "video"), this._playing.find(".owl-video-frame").remove(), this._playing.removeClass("owl-video-playing"), this._playing = null, this._core.leave("playing"), this._core.trigger("stopped", null, "video")
    }, e.prototype.play = function(b) {
        var c, d = a(b.target),
            e = d.closest("." + this._core.settings.itemClass),
            f = this._videos[e.attr("data-video")],
            g = f.width || "100%",
            h = f.height || this._core.$stage.height();
        this._playing || (this._core.enter("playing"), this._core.trigger("play", null, "video"), e = this._core.items(this._core.relative(e.index())), this._core.reset(e.index()), c = a('<iframe frameborder="0" allowfullscreen mozallowfullscreen webkitAllowFullScreen ></iframe>'), c.attr("height", h), c.attr("width", g), "youtube" === f.type ? c.attr("src", "//www.youtube.com/embed/" + f.id + "?autoplay=1&rel=0&v=" + f.id) : "vimeo" === f.type ? c.attr("src", "//player.vimeo.com/video/" + f.id + "?autoplay=1") : "vzaar" === f.type && c.attr("src", "//view.vzaar.com/" + f.id + "/player?autoplay=true"), a(c).wrap('<div class="owl-video-frame" />').insertAfter(e.find(".owl-video")), this._playing = e.addClass("owl-video-playing"))
    }, e.prototype.isInFullScreen = function() {
        var b = c.fullscreenElement || c.mozFullScreenElement || c.webkitFullscreenElement;
        return b && a(b).parent().hasClass("owl-video-frame")
    }, e.prototype.destroy = function() {
        var a, b;
        this._core.$element.off("click.owl.video");
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Video = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    var e = function(b) {
        this.core = b, this.core.options = a.extend({}, e.Defaults, this.core.options), this.swapping = !0, this.previous = d, this.next = d, this.handlers = {
            "change.owl.carousel": a.proxy(function(a) {
                a.namespace && "position" == a.property.name && (this.previous = this.core.current(), this.next = a.property.value)
            }, this),
            "drag.owl.carousel dragged.owl.carousel translated.owl.carousel": a.proxy(function(a) {
                a.namespace && (this.swapping = "translated" == a.type)
            }, this),
            "translate.owl.carousel": a.proxy(function(a) {
                a.namespace && this.swapping && (this.core.options.animateOut || this.core.options.animateIn) && this.swap()
            }, this)
        }, this.core.$element.on(this.handlers)
    };
    e.Defaults = {
        animateOut: !1,
        animateIn: !1
    }, e.prototype.swap = function() {
        if (1 === this.core.settings.items && a.support.animation && a.support.transition) {
            this.core.speed(0);
            var b, c = a.proxy(this.clear, this),
                d = this.core.$stage.children().eq(this.previous),
                e = this.core.$stage.children().eq(this.next),
                f = this.core.settings.animateIn,
                g = this.core.settings.animateOut;
            this.core.current() !== this.previous && (g && (b = this.core.coordinates(this.previous) - this.core.coordinates(this.next), d.one(a.support.animation.end, c).css({
                left: b + "px"
            }).addClass("animated owl-animated-out").addClass(g)), f && e.one(a.support.animation.end, c).addClass("animated owl-animated-in").addClass(f))
        }
    }, e.prototype.clear = function(b) {
        a(b.target).css({
            left: ""
        }).removeClass("animated owl-animated-out owl-animated-in").removeClass(this.core.settings.animateIn).removeClass(this.core.settings.animateOut), this.core.onTransitionEnd()
    }, e.prototype.destroy = function() {
        var a, b;
        for (a in this.handlers) this.core.$element.off(a, this.handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Animate = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    var e = function(b) {
        this._core = b, this._call = null, this._time = 0, this._timeout = 0, this._paused = !0, this._handlers = {
            "changed.owl.carousel": a.proxy(function(a) {
                a.namespace && "settings" === a.property.name ? this._core.settings.autoplay ? this.play() : this.stop() : a.namespace && "position" === a.property.name && this._paused && (this._time = 0)
            }, this),
            "initialized.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.autoplay && this.play()
            }, this),
            "play.owl.autoplay": a.proxy(function(a, b, c) {
                a.namespace && this.play(b, c)
            }, this),
            "stop.owl.autoplay": a.proxy(function(a) {
                a.namespace && this.stop()
            }, this),
            "mouseover.owl.autoplay": a.proxy(function() {
                this._core.settings.autoplayHoverPause && this._core.is("rotating") && this.pause()
            }, this),
            "mouseleave.owl.autoplay": a.proxy(function() {
                this._core.settings.autoplayHoverPause && this._core.is("rotating") && this.play()
            }, this),
            "touchstart.owl.core": a.proxy(function() {
                this._core.settings.autoplayHoverPause && this._core.is("rotating") && this.pause()
            }, this),
            "touchend.owl.core": a.proxy(function() {
                this._core.settings.autoplayHoverPause && this.play()
            }, this)
        }, this._core.$element.on(this._handlers), this._core.options = a.extend({}, e.Defaults, this._core.options)
    };
    e.Defaults = {
        autoplay: !1,
        autoplayTimeout: 5e3,
        autoplayHoverPause: !1,
        autoplaySpeed: !1
    }, e.prototype._next = function(d) {
        this._call = b.setTimeout(a.proxy(this._next, this, d), this._timeout * (Math.round(this.read() / this._timeout) + 1) - this.read()), this._core.is("interacting") || c.hidden || this._core.next(d || this._core.settings.autoplaySpeed)
    }, e.prototype.read = function() {
        return (new Date).getTime() - this._time
    }, e.prototype.play = function(c, d) {
        var e;
        this._core.is("rotating") || this._core.enter("rotating"), c = c || this._core.settings.autoplayTimeout, e = Math.min(this._time % (this._timeout || c), c), this._paused ? (this._time = this.read(), this._paused = !1) : b.clearTimeout(this._call), this._time += this.read() % c - e, this._timeout = c, this._call = b.setTimeout(a.proxy(this._next, this, d), c - e)
    }, e.prototype.stop = function() {
        this._core.is("rotating") && (this._time = 0, this._paused = !0, b.clearTimeout(this._call), this._core.leave("rotating"))
    }, e.prototype.pause = function() {
        this._core.is("rotating") && !this._paused && (this._time = this.read(), this._paused = !0, b.clearTimeout(this._call))
    }, e.prototype.destroy = function() {
        var a, b;
        this.stop();
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this)) "function" != typeof this[b] && (this[b] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.autoplay = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    "use strict";
    var e = function(b) {
        this._core = b, this._initialized = !1, this._pages = [], this._controls = {}, this._templates = [], this.$element = this._core.$element, this._overrides = {
            next: this._core.next,
            prev: this._core.prev,
            to: this._core.to
        }, this._handlers = {
            "prepared.owl.carousel": a.proxy(function(b) {
                b.namespace && this._core.settings.dotsData && this._templates.push('<div class="' + this._core.settings.dotClass + '">' + a(b.content).find("[data-dot]").addBack("[data-dot]").attr("data-dot") + "</div>")
            }, this),
            "added.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.dotsData && this._templates.splice(a.position, 0, this._templates.pop())
            }, this),
            "remove.owl.carousel": a.proxy(function(a) {
                a.namespace && this._core.settings.dotsData && this._templates.splice(a.position, 1)
            }, this),
            "changed.owl.carousel": a.proxy(function(a) {
                a.namespace && "position" == a.property.name && this.draw()
            }, this),
            "initialized.owl.carousel": a.proxy(function(a) {
                a.namespace && !this._initialized && (this._core.trigger("initialize", null, "navigation"), this.initialize(), this.update(), this.draw(), this._initialized = !0, this._core.trigger("initialized", null, "navigation"))
            }, this),
            "refreshed.owl.carousel": a.proxy(function(a) {
                a.namespace && this._initialized && (this._core.trigger("refresh", null, "navigation"), this.update(), this.draw(), this._core.trigger("refreshed", null, "navigation"))
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this.$element.on(this._handlers)
    };
    e.Defaults = {
        nav: !1,
        navText: ['<span aria-label="Previous">&#x2039;</span>', '<span aria-label="Next">&#x203a;</span>'],
        navSpeed: !1,
        navElement: 'button type="button" role="presentation"',
        navContainer: !1,
        navContainerClass: "owl-nav",
        navClass: ["owl-prev", "owl-next"],
        slideBy: 1,
        dotClass: "owl-dot",
        dotsClass: "owl-dots",
        dots: !0,
        dotsEach: !1,
        dotsData: !1,
        dotsSpeed: !1,
        dotsContainer: !1
    }, e.prototype.initialize = function() {
        var b, c = this._core.settings;
        this._controls.$relative = (c.navContainer ? a(c.navContainer) : a("<div>").addClass(c.navContainerClass).appendTo(this.$element)).addClass("disabled"), this._controls.$previous = a("<" + c.navElement + ">").addClass(c.navClass[0]).html(c.navText[0]).prependTo(this._controls.$relative).on("click", a.proxy(function(a) {
            this.prev(c.navSpeed)
        }, this)), this._controls.$next = a("<" + c.navElement + ">").addClass(c.navClass[1]).html(c.navText[1]).appendTo(this._controls.$relative).on("click", a.proxy(function(a) {
            this.next(c.navSpeed)
        }, this)), c.dotsData || (this._templates = [a('<button role="button">').addClass(c.dotClass).append(a("<span>")).prop("outerHTML")]), this._controls.$absolute = (c.dotsContainer ? a(c.dotsContainer) : a("<div>").addClass(c.dotsClass).appendTo(this.$element)).addClass("disabled"), this._controls.$absolute.on("click", "button", a.proxy(function(b) {
            var d = a(b.target).parent().is(this._controls.$absolute) ? a(b.target).index() : a(b.target).parent().index();
            b.preventDefault(), this.to(d, c.dotsSpeed)
        }, this));
        for (b in this._overrides) this._core[b] = a.proxy(this[b], this)
    }, e.prototype.destroy = function() {
        var a, b, c, d, e;
        e = this._core.settings;
        for (a in this._handlers) this.$element.off(a, this._handlers[a]);
        for (b in this._controls) "$relative" === b && e.navContainer ? this._controls[b].html("") : this._controls[b].remove();
        for (d in this.overides) this._core[d] = this._overrides[d];
        for (c in Object.getOwnPropertyNames(this)) "function" != typeof this[c] && (this[c] = null)
    }, e.prototype.update = function() {
        var a, b, c, d = this._core.clones().length / 2,
            e = d + this._core.items().length,
            f = this._core.maximum(!0),
            g = this._core.settings,
            h = g.center || g.autoWidth || g.dotsData ? 1 : g.dotsEach || g.items;
        if ("page" !== g.slideBy && (g.slideBy = Math.min(g.slideBy, g.items)), g.dots || "page" == g.slideBy)
            for (this._pages = [], a = d, b = 0, c = 0; a < e; a++) {
                if (b >= h || 0 === b) {
                    if (this._pages.push({
                            start: Math.min(f, a - d),
                            end: a - d + h - 1
                        }), Math.min(f, a - d) === f) break;
                    b = 0, ++c
                }
                b += this._core.mergers(this._core.relative(a))
            }
    }, e.prototype.draw = function() {
        var b, c = this._core.settings,
            d = this._core.items().length <= c.items,
            e = this._core.relative(this._core.current()),
            f = c.loop || c.rewind;
        this._controls.$relative.toggleClass("disabled", !c.nav || d), c.nav && (this._controls.$previous.toggleClass("disabled", !f && e <= this._core.minimum(!0)), this._controls.$next.toggleClass("disabled", !f && e >= this._core.maximum(!0))), this._controls.$absolute.toggleClass("disabled", !c.dots || d), c.dots && (b = this._pages.length - this._controls.$absolute.children().length, c.dotsData && 0 !== b ? this._controls.$absolute.html(this._templates.join("")) : b > 0 ? this._controls.$absolute.append(new Array(b + 1).join(this._templates[0])) : b < 0 && this._controls.$absolute.children().slice(b).remove(), this._controls.$absolute.find(".active").removeClass("active"), this._controls.$absolute.children().eq(a.inArray(this.current(), this._pages)).addClass("active"))
    }, e.prototype.onTrigger = function(b) {
        var c = this._core.settings;
        b.page = {
            index: a.inArray(this.current(), this._pages),
            count: this._pages.length,
            size: c && (c.center || c.autoWidth || c.dotsData ? 1 : c.dotsEach || c.items)
        }
    }, e.prototype.current = function() {
        var b = this._core.relative(this._core.current());
        return a.grep(this._pages, a.proxy(function(a, c) {
            return a.start <= b && a.end >= b
        }, this)).pop()
    }, e.prototype.getPosition = function(b) {
        var c, d, e = this._core.settings;
        return "page" == e.slideBy ? (c = a.inArray(this.current(), this._pages), d = this._pages.length, b ? ++c : --c, c = this._pages[(c % d + d) % d].start) : (c = this._core.relative(this._core.current()), d = this._core.items().length, b ? c += e.slideBy : c -= e.slideBy), c
    }, e.prototype.next = function(b) {
        a.proxy(this._overrides.to, this._core)(this.getPosition(!0), b)
    }, e.prototype.prev = function(b) {
        a.proxy(this._overrides.to, this._core)(this.getPosition(!1), b)
    }, e.prototype.to = function(b, c, d) {
        var e;
        !d && this._pages.length ? (e = this._pages.length, a.proxy(this._overrides.to, this._core)(this._pages[(b % e + e) % e].start, c)) : a.proxy(this._overrides.to, this._core)(b, c)
    }, a.fn.owlCarousel.Constructor.Plugins.Navigation = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    "use strict";
    var e = function(c) {
        this._core = c, this._hashes = {}, this.$element = this._core.$element, this._handlers = {
            "initialized.owl.carousel": a.proxy(function(c) {
                c.namespace && "URLHash" === this._core.settings.startPosition && a(b).trigger("hashchange.owl.navigation")
            }, this),
            "prepared.owl.carousel": a.proxy(function(b) {
                if (b.namespace) {
                    var c = a(b.content).find("[data-hash]").addBack("[data-hash]").attr("data-hash");
                    if (!c) return;
                    this._hashes[c] = b.content
                }
            }, this),
            "changed.owl.carousel": a.proxy(function(c) {
                if (c.namespace && "position" === c.property.name) {
                    var d = this._core.items(this._core.relative(this._core.current())),
                        e = a.map(this._hashes, function(a, b) {
                            return a === d ? b : null
                        }).join();
                    if (!e || b.location.hash.slice(1) === e) return;
                    b.location.hash = e
                }
            }, this)
        }, this._core.options = a.extend({}, e.Defaults, this._core.options), this.$element.on(this._handlers), a(b).on("hashchange.owl.navigation", a.proxy(function(a) {
            var c = b.location.hash.substring(1),
                e = this._core.$stage.children(),
                f = this._hashes[c] && e.index(this._hashes[c]);
            f !== d && f !== this._core.current() && this._core.to(this._core.relative(f), !1, !0)
        }, this))
    };
    e.Defaults = {
        URLhashListener: !1
    }, e.prototype.destroy = function() {
        var c, d;
        a(b).off("hashchange.owl.navigation");
        for (c in this._handlers) this._core.$element.off(c, this._handlers[c]);
        for (d in Object.getOwnPropertyNames(this)) "function" != typeof this[d] && (this[d] = null)
    }, a.fn.owlCarousel.Constructor.Plugins.Hash = e
}(window.Zepto || window.jQuery, window, document),
function(a, b, c, d) {
    function e(b, c) {
        var e = !1,
            f = b.charAt(0).toUpperCase() + b.slice(1);
        return a.each((b + " " + h.join(f + " ") + f).split(" "), function(a, b) {
            if (g[b] !== d) return e = !c || b, !1
        }), e
    }

    function f(a) {
        return e(a, !0)
    }
    var g = a("<support>").get(0).style,
        h = "Webkit Moz O ms".split(" "),
        i = {
            transition: {
                end: {
                    WebkitTransition: "webkitTransitionEnd",
                    MozTransition: "transitionend",
                    OTransition: "oTransitionEnd",
                    transition: "transitionend"
                }
            },
            animation: {
                end: {
                    WebkitAnimation: "webkitAnimationEnd",
                    MozAnimation: "animationend",
                    OAnimation: "oAnimationEnd",
                    animation: "animationend"
                }
            }
        },
        j = {
            csstransforms: function() {
                return !!e("transform")
            },
            csstransforms3d: function() {
                return !!e("perspective")
            },
            csstransitions: function() {
                return !!e("transition")
            },
            cssanimations: function() {
                return !!e("animation")
            }
        };
    j.csstransitions() && (a.support.transition = new String(f("transition")), a.support.transition.end = i.transition.end[a.support.transition]), j.cssanimations() && (a.support.animation = new String(f("animation")), a.support.animation.end = i.animation.end[a.support.animation]), j.csstransforms() && (a.support.transform = new String(f("transform")), a.support.transform3d = j.csstransforms3d())
}(window.Zepto || window.jQuery, window, document); 

    /*! blur-up - v5.2.0 */
    ! function(a, b) {
        if (a) {
            var c = function() {
                b(a.lazySizes), a.removeEventListener("lazyunveilread", c, !0)
            };
            b = b.bind(null, a, a.document), "object" == typeof module && module.exports ? b(require("lazysizes")) : a.lazySizes ? c() : a.addEventListener("lazyunveilread", c, !0)
        }
    }("undefined" != typeof window ? window : 0, function(a, b, c) {
        "use strict";
        var d = [].slice,
            e = /blur-up["']*\s*:\s*["']*(always|auto)/,
            f = /image\/(jpeg|png|gif|svg\+xml)/,
            g = function(b) {
                var d = b.getAttribute("data-media") || b.getAttribute("media"),
                    e = b.getAttribute("type");
                return (!e || f.test(e)) && (!d || a.matchMedia(c.cfg.customMedia[d] || d).matches)
            },
            h = function(a, b) {
                var c;
                return (a ? d.call(a.querySelectorAll("source, img")) : [b]).forEach(function(a) {
                    if (!c) {
                        var b = a.getAttribute("data-lowsrc");
                        b && g(a) && (c = b)
                    }
                }), c
            },
            i = function(a, d, e, f) {
                var g, h = !1,
                    i = !1,
                    j = "always" == f ? 0 : Date.now(),
                    k = 0,
                    l = (a || d).parentNode,
                    m = function() {
                        if (e) {
                            var j = function(a) {
                                h = !0, g || (g = a.target), c.rAF(function() {
                                    c.rC(d, "ls-blur-up-is-loading"), g && c.aC(g, "ls-blur-up-loaded")
                                }), g && (g.removeEventListener("load", j), g.removeEventListener("error", j))
                            };
                            g = b.createElement("img"), g.addEventListener("load", j), g.addEventListener("error", j), g.className = "ls-blur-up-img", g.src = e, g.alt = "", g.setAttribute("aria-hidden", "true"), l.insertBefore(g, (a || d).nextSibling), "always" != f && (g.style.visibility = "hidden", c.rAF(function() {
                                g && setTimeout(function() {
                                    g && c.rAF(function() {
                                        !i && g && (g.style.visibility = "")
                                    })
                                }, c.cfg.blurupCacheDelay || 33)
                            }))
                        }
                    },
                    n = function() {
                        g && c.rAF(function() {
                            c.rC(d, "ls-blur-up-is-loading");
                            try {
                                g.parentNode.removeChild(g)
                            } catch (a) {}
                            g = null
                        })
                    },
                    o = function(a) {
                        k++, i = a || i, a ? n() : k > 1 && setTimeout(n, 5e3)
                    },
                    p = function() {
                        d.removeEventListener("load", p), d.removeEventListener("error", p), g && c.rAF(function() {
                            g && c.aC(g, "ls-original-loaded")
                        }), "always" != f && (!h || Date.now() - j < 66) ? o(!0) : o()
                    };
                m(), d.addEventListener("load", p), d.addEventListener("error", p), c.aC(d, "ls-blur-up-is-loading");
                var q = function(a) {
                    l == a.target && (c.aC(g || d, "ls-inview"), o(), l.removeEventListener("lazybeforeunveil", q))
                };
                l.getAttribute("data-expand") || l.setAttribute("data-expand", -1), l.addEventListener("lazybeforeunveil", q), c.aC(l, c.cfg.lazyClass)
            };
        a.addEventListener("lazybeforeunveil", function(a) {
            var b = a.detail;
            if (b.instance == c && b.blurUp) {
                var d = a.target,
                    e = d.parentNode;
                "PICTURE" != e.nodeName && (e = null), i(e, d, h(e, d) || "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==", b.blurUp)
            }
        }), a.addEventListener("lazyunveilread", function(a) {
            var b = a.detail;
            if (b.instance == c) {
                var d = a.target,
                    f = (getComputedStyle(d, null) || {
                        fontFamily: ""
                    }).fontFamily.match(e);
                (f || d.getAttribute("data-lowsrc")) && (b.blurUp = f && f[1] || c.cfg.blurupMode || "always")
            }
        })
    });
/*! lazysizes - v5.2.0 (https://github.com/aFarkas/lazysizes) */
! function(a, b) {
    var c = b(a, a.document, Date);
    a.lazySizes = c, "object" == typeof module && module.exports && (module.exports = c)
}("undefined" != typeof window ? window : {}, function(a, b, c) {
    "use strict";
    var d, e;
    if (function() {
            var b, c = {
                lazyClass: "lazyload",
                loadedClass: "lazyloaded",
                loadingClass: "lazyloading",
                preloadClass: "lazypreload",
                errorClass: "lazyerror",
                autosizesClass: "lazyautosizes",
                srcAttr: "data-src",
                srcsetAttr: "data-srcset",
                sizesAttr: "data-sizes",
                minSize: 40,
                customMedia: {},
                init: !0,
                expFactor: 1.5,
                hFac: .8,
                loadMode: 2,
                loadHidden: !0,
                ricTimeout: 0,
                throttleDelay: 125
            };
            e = a.lazySizesConfig || a.lazysizesConfig || {};
            for (b in c) b in e || (e[b] = c[b])
        }(), !b || !b.getElementsByClassName) return {
        init: function() {},
        cfg: e,
        noSupport: !0
    };
    var f = b.documentElement,
        g = a.HTMLPictureElement,
        h = "addEventListener",
        i = "getAttribute",
        j = a[h].bind(a),
        k = a.setTimeout,
        l = a.requestAnimationFrame || k,
        m = a.requestIdleCallback,
        n = /^picture$/i,
        o = ["load", "error", "lazyincluded", "_lazyloaded"],
        p = {},
        q = Array.prototype.forEach,
        r = function(a, b) {
            return p[b] || (p[b] = new RegExp("(\\s|^)" + b + "(\\s|$)")), p[b].test(a[i]("class") || "") && p[b]
        },
        s = function(a, b) {
            r(a, b) || a.setAttribute("class", (a[i]("class") || "").trim() + " " + b)
        },
        t = function(a, b) {
            var c;
            (c = r(a, b)) && a.setAttribute("class", (a[i]("class") || "").replace(c, " "))
        },
        u = function(a, b, c) {
            var d = c ? h : "removeEventListener";
            c && u(a, b), o.forEach(function(c) {
                a[d](c, b)
            })
        },
        v = function(a, c, e, f, g) {
            var h = b.createEvent("Event");
            return e || (e = {}), e.instance = d, h.initEvent(c, !f, !g), h.detail = e, a.dispatchEvent(h), h
        },
        w = function(b, c) {
            var d;
            !g && (d = a.picturefill || e.pf) ? (c && c.src && !b[i]("srcset") && b.setAttribute("srcset", c.src), d({
                reevaluate: !0,
                elements: [b]
            })) : c && c.src && (b.src = c.src)
        },
        x = function(a, b) {
            return (getComputedStyle(a, null) || {})[b]
        },
        y = function(a, b, c) {
            for (c = c || a.offsetWidth; c < e.minSize && b && !a._lazysizesWidth;) c = b.offsetWidth, b = b.parentNode;
            return c
        },
        z = function() {
            var a, c, d = [],
                e = [],
                f = d,
                g = function() {
                    var b = f;
                    for (f = d.length ? e : d, a = !0, c = !1; b.length;) b.shift()();
                    a = !1
                },
                h = function(d, e) {
                    a && !e ? d.apply(this, arguments) : (f.push(d), c || (c = !0, (b.hidden ? k : l)(g)))
                };
            return h._lsFlush = g, h
        }(),
        A = function(a, b) {
            return b ? function() {
                z(a)
            } : function() {
                var b = this,
                    c = arguments;
                z(function() {
                    a.apply(b, c)
                })
            }
        },
        B = function(a) {
            var b, d = 0,
                f = e.throttleDelay,
                g = e.ricTimeout,
                h = function() {
                    b = !1, d = c.now(), a()
                },
                i = m && g > 49 ? function() {
                    m(h, {
                        timeout: g
                    }), g !== e.ricTimeout && (g = e.ricTimeout)
                } : A(function() {
                    k(h)
                }, !0);
            return function(a) {
                var e;
                (a = !0 === a) && (g = 33), b || (b = !0, e = f - (c.now() - d), e < 0 && (e = 0), a || e < 9 ? i() : k(i, e))
            }
        },
        C = function(a) {
            var b, d, e = 99,
                f = function() {
                    b = null, a()
                },
                g = function() {
                    var a = c.now() - d;
                    a < e ? k(g, e - a) : (m || f)(f)
                };
            return function() {
                d = c.now(), b || (b = k(g, e))
            }
        },
        D = function() {
            var g, m, o, p, y, D, F, G, H, I, J, K, L = /^img$/i,
                M = /^iframe$/i,
                N = "onscroll" in a && !/(gle|ing)bot/.test(navigator.userAgent),
                O = 0,
                P = 0,
                Q = 0,
                R = -1,
                S = function(a) {
                    Q--, (!a || Q < 0 || !a.target) && (Q = 0)
                },
                T = function(a) {
                    return null == K && (K = "hidden" == x(b.body, "visibility")), K || !("hidden" == x(a.parentNode, "visibility") && "hidden" == x(a, "visibility"))
                },
                U = function(a, c) {
                    var d, e = a,
                        g = T(a);
                    for (G -= c, J += c, H -= c, I += c; g && (e = e.offsetParent) && e != b.body && e != f;)(g = (x(e, "opacity") || 1) > 0) && "visible" != x(e, "overflow") && (d = e.getBoundingClientRect(), g = I > d.left && H < d.right && J > d.top - 1 && G < d.bottom + 1);
                    return g
                },
                V = function() {
                    var a, c, h, j, k, l, n, o, q, r, s, t, u = d.elements;
                    if ((p = e.loadMode) && Q < 8 && (a = u.length)) {
                        for (c = 0, R++; c < a; c++)
                            if (u[c] && !u[c]._lazyRace)
                                if (!N || d.prematureUnveil && d.prematureUnveil(u[c])) ba(u[c]);
                                else if ((o = u[c][i]("data-expand")) && (l = 1 * o) || (l = P), r || (r = !e.expand || e.expand < 1 ? f.clientHeight > 500 && f.clientWidth > 500 ? 500 : 370 : e.expand, d._defEx = r, s = r * e.expFactor, t = e.hFac, K = null, P < s && Q < 1 && R > 2 && p > 2 && !b.hidden ? (P = s, R = 0) : P = p > 1 && R > 1 && Q < 6 ? r : O), q !== l && (D = innerWidth + l * t, F = innerHeight + l, n = -1 * l, q = l), h = u[c].getBoundingClientRect(), (J = h.bottom) >= n && (G = h.top) <= F && (I = h.right) >= n * t && (H = h.left) <= D && (J || I || H || G) && (e.loadHidden || T(u[c])) && (m && Q < 3 && !o && (p < 3 || R < 4) || U(u[c], l))) {
                            if (ba(u[c]), k = !0, Q > 9) break
                        } else !k && m && !j && Q < 4 && R < 4 && p > 2 && (g[0] || e.preloadAfterLoad) && (g[0] || !o && (J || I || H || G || "auto" != u[c][i](e.sizesAttr))) && (j = g[0] || u[c]);
                        j && !k && ba(j)
                    }
                },
                W = B(V),
                X = function(a) {
                    var b = a.target;
                    if (b._lazyCache) return void delete b._lazyCache;
                    S(a), s(b, e.loadedClass), t(b, e.loadingClass), u(b, Z), v(b, "lazyloaded")
                },
                Y = A(X),
                Z = function(a) {
                    Y({
                        target: a.target
                    })
                },
                $ = function(a, b) {
                    try {
                        a.contentWindow.location.replace(b)
                    } catch (c) {
                        a.src = b
                    }
                },
                _ = function(a) {
                    var b, c = a[i](e.srcsetAttr);
                    (b = e.customMedia[a[i]("data-media") || a[i]("media")]) && a.setAttribute("media", b), c && a.setAttribute("srcset", c)
                },
                aa = A(function(a, b, c, d, f) {
                    var g, h, j, l, m, p;
                    (m = v(a, "lazybeforeunveil", b)).defaultPrevented || (d && (c ? s(a, e.autosizesClass) : a.setAttribute("sizes", d)), h = a[i](e.srcsetAttr), g = a[i](e.srcAttr), f && (j = a.parentNode, l = j && n.test(j.nodeName || "")), p = b.firesLoad || "src" in a && (h || g || l), m = {
                        target: a
                    }, s(a, e.loadingClass), p && (clearTimeout(o), o = k(S, 2500), u(a, Z, !0)), l && q.call(j.getElementsByTagName("source"), _), h ? a.setAttribute("srcset", h) : g && !l && (M.test(a.nodeName) ? $(a, g) : a.src = g), f && (h || l) && w(a, {
                        src: g
                    })), a._lazyRace && delete a._lazyRace, t(a, e.lazyClass), z(function() {
                        var b = a.complete && a.naturalWidth > 1;
                        p && !b || (b && s(a, "ls-is-cached"), X(m), a._lazyCache = !0, k(function() {
                            "_lazyCache" in a && delete a._lazyCache
                        }, 9)), "lazy" == a.loading && Q--
                    }, !0)
                }),
                ba = function(a) {
                    if (!a._lazyRace) {
                        var b, c = L.test(a.nodeName),
                            d = c && (a[i](e.sizesAttr) || a[i]("sizes")),
                            f = "auto" == d;
                        (!f && m || !c || !a[i]("src") && !a.srcset || a.complete || r(a, e.errorClass) || !r(a, e.lazyClass)) && (b = v(a, "lazyunveilread").detail, f && E.updateElem(a, !0, a.offsetWidth), a._lazyRace = !0, Q++, aa(a, b, f, d, c))
                    }
                },
                ca = C(function() {
                    e.loadMode = 3, W()
                }),
                da = function() {
                    3 == e.loadMode && (e.loadMode = 2), ca()
                },
                ea = function() {
                    if (!m) {
                        if (c.now() - y < 999) return void k(ea, 999);
                        m = !0, e.loadMode = 3, W(), j("scroll", da, !0)
                    }
                };
            return {
                _: function() {
                    y = c.now(), d.elements = b.getElementsByClassName(e.lazyClass), g = b.getElementsByClassName(e.lazyClass + " " + e.preloadClass), j("scroll", W, !0), j("resize", W, !0), j("pageshow", function(a) {
                        if (a.persisted) {
                            var c = b.querySelectorAll("." + e.loadingClass);
                            c.length && c.forEach && l(function() {
                                c.forEach(function(a) {
                                    a.complete && ba(a)
                                })
                            })
                        }
                    }), a.MutationObserver ? new MutationObserver(W).observe(f, {
                        childList: !0,
                        subtree: !0,
                        attributes: !0
                    }) : (f[h]("DOMNodeInserted", W, !0), f[h]("DOMAttrModified", W, !0), setInterval(W, 999)), j("hashchange", W, !0), ["focus", "mouseover", "click", "load", "transitionend", "animationend"].forEach(function(a) {
                        b[h](a, W, !0)
                    }), /d$|^c/.test(b.readyState) ? ea() : (j("load", ea), b[h]("DOMContentLoaded", W), k(ea, 2e4)), d.elements.length ? (V(), z._lsFlush()) : W()
                },
                checkElems: W,
                unveil: ba,
                _aLSL: da
            }
        }(),
        E = function() {
            var a, c = A(function(a, b, c, d) {
                    var e, f, g;
                    if (a._lazysizesWidth = d, d += "px", a.setAttribute("sizes", d), n.test(b.nodeName || ""))
                        for (e = b.getElementsByTagName("source"), f = 0, g = e.length; f < g; f++) e[f].setAttribute("sizes", d);
                    c.detail.dataAttr || w(a, c.detail)
                }),
                d = function(a, b, d) {
                    var e, f = a.parentNode;
                    f && (d = y(a, f, d), e = v(a, "lazybeforesizes", {
                        width: d,
                        dataAttr: !!b
                    }), e.defaultPrevented || (d = e.detail.width) && d !== a._lazysizesWidth && c(a, f, e, d))
                },
                f = function() {
                    var b, c = a.length;
                    if (c)
                        for (b = 0; b < c; b++) d(a[b])
                },
                g = C(f);
            return {
                _: function() {
                    a = b.getElementsByClassName(e.autosizesClass), j("resize", g)
                },
                checkElems: g,
                updateElem: d
            }
        }(),
        F = function() {
            !F.i && b.getElementsByClassName && (F.i = !0, E._(), D._())
        };
    return k(function() {
        e.init && F()
    }), d = {
        cfg: e,
        autoSizer: E,
        loader: D,
        init: F,
        uP: w,
        aC: s,
        rC: t,
        hC: r,
        fire: v,
        gW: y,
        rAF: z
    }
}); 

    /* Soon v1.9.0 - http://rikschennink.nl/products/soon * Copyright (c) 2016 */
    ! function(a, b, c) {
        function d() {
            H !== window.innerWidth && (H = window.innerWidth, g())
        }

        function e(a, b, c, d) {
            var e = parseInt(getComputedStyle(document.documentElement).fontSize, 10) / 16,
                f = parseInt(getComputedStyle(b).fontSize, 10) / 16 / e,
                g = d / b.scrollWidth,
                h = g * f;
            return 4 > h ? (a.style.fontSize = "", c.redraw(), !1) : (a.style.fontSize = h + "rem", a.setAttribute("data-scale-rounded", Math.round(h).toString()), c.redraw(), !0)
        }

        function f(a, b) {
            if (!B.isSlow()) {
                for (var c, d, f = window.getComputedStyle(a.parentNode), g = parseInt(f.getPropertyValue("padding-left"), 10), h = parseInt(f.getPropertyValue("padding-right"), 10), i = a.parentNode.clientWidth - g - h, j = a.getAttribute("data-scale-max"), k = a.getAttribute("data-scale-hide"), l = j ? I.indexOf(j) : J, m = a.querySelectorAll(".soon-group-sub"), n = 0, o = m.length, p = a.querySelector(".soon-group"); o > n; n++) m[n].style.display = "";
                if ("fit" === j || "fill" === j) {
                    if (e(a, p, b, i)) return;
                    l = 0
                }
                c = l;
                do a.setAttribute("data-scale", I[c]), c++; while (p.scrollWidth > i && I[c]);
                if (c !== l && b.redraw(), !(p.scrollWidth <= i || "none" === k)) {
                    n = 0, d = !1;
                    do {
                        if ("0" !== m[n].getAttribute("data-value")) break;
                        m[n].style.display = "none", d = !0, n++
                    } while (p.scrollWidth > i && o > n);
                    if (d && b.redraw(), "empty" !== k) {
                        n = o - 1, d = !1;
                        do m[n].style.display = "none", d = !0, n--; while (p.scrollWidth > i && n > 0);
                        d && b.redraw()
                    }
                }
            }
        }

        function g() {
            for (var a = K.length - 1; a >= 0; a--) f(K[a].node, K[a].presenter)
        }

        function h(a) {
            for (var b = 0, c = K.length; c > b; b++)
                if (K[b].node === a) return b;
            return null
        }

        function i(a) {
            for (var b = 0, c = L.length; c > b; b++)
                if (L[b].node === a) return b;
            return null
        }

        function j(a) {
            var b = h(a);
            return null === b ? null : K[b]
        }

        function k(a) {
            -1 === a.className.indexOf("soon") && (a.className += " soon"), B.supportsAnimation() || (a.className += " soon-no-animation");
            var b = a.getAttribute("data-layout");
            (!b || -1 === b.indexOf("group") && -1 === b.indexOf("line")) && (b || (b = ""), a.setAttribute("data-layout", b + " group")), B.isSlow() && (a.removeAttribute("data-visual"), a.setAttribute("data-view", "text"), a.className += " soon-slow-browser")
        }

        function l(a, b, c) {
            b[c] && !a.getAttribute("data-" + c) && a.setAttribute("data-" + c, b[c])
        }

        function m(a, b) {
            return a.getAttribute("data-" + b)
        }

        function n(a, b) {
            var c = null !== a.due || null !== a.since,
                d = null;
            if (c)
                if (a.since) {
                    var e = a.now ? a.now.valueOf() : (new Date).valueOf();
                    d = D.chain(function(b) {
                        return a.now ? -b : b
                    }, D.offset(e), D.diff(a.since.valueOf()), function(a) {
                        return Math.abs(a)
                    }, function(a) {
                        return Math.max(0, a)
                    }, function(b) {
                        return a.callback.onTick(b, a.since), b
                    }, D.event(function(a) {
                        return 0 === a
                    }, b), D.duration(new Date(e), a.since, a.format, a.cascade))
                } else d = D.chain(D.offset(a.now.valueOf()), D.diff(a.due.valueOf()), function(a) {
                    return Math.max(0, a)
                }, function(b) {
                    return a.callback.onTick(b, a.due), b
                }, D.event(function(a) {
                    return 0 >= a
                }, b), D.duration(a.now, a.due, a.format, a.cascade));
            else d = function() {
                var a = new Date;
                return [a.getHours(), a.getMinutes(), a.getSeconds()]
            }, a.format = ["h", "m", "s"], a.separator = ":";
            return d
        }

        function o(a, b) {
            for (var c, d, e, f, g, h, i, j = null !== a.due || null !== a.since, k = n(a, b), l = {
                    type: "group",
                    options: {
                        transform: k,
                        presenters: []
                    }
                }, m = [], o = a.format.length, r = 0; o > r; r++) h = a.format[r], i = r, c = {
                type: "group",
                options: {
                    className: "soon-group-sub",
                    presenters: []
                }
            }, a.visual && (c.options.presenters.push(p(a, h)), a.reflect && c.options.presenters.push(p(a, h, "soon-reflection"))), d = {
                type: "text",
                options: {
                    className: "soon-label"
                }
            }, d.options.transform = a.singular ? D.plural(a.label[h], a.label[h + "_s"]) : function(b) {
                return function() {
                    return a.label[b + "_s"]
                }
            }(h), e = q(a, h), f = null, a.reflect && !a.visual && (f = q(a, h, "soon-reflection")), c.options.presenters.push(e), f && c.options.presenters.push(f), j && c.options.presenters.push(d), a.separator && (g = {
                type: "group",
                options: {
                    className: "soon-group-separator",
                    presenters: [c]
                }
            }, 0 !== i && (a.reflect && g.options.presenters.unshift({
                type: "text",
                options: {
                    className: "soon-separator soon-reflection",
                    transform: function() {
                        return a.separator
                    }
                }
            }), g.options.presenters.unshift({
                type: "text",
                options: {
                    className: "soon-separator",
                    transform: function() {
                        return a.separator
                    }
                }
            })), c = g), m.push(c);
            return l.options.presenters = m, l
        }

        function p(a, b, c) {
            var d = a.visual.split(" "),
                e = d[0];
            return d.shift(), {
                type: e,
                options: {
                    className: "soon-visual " + (c || ""),
                    transform: D.chain(D.progress(B.MAX[b]), D.cap()),
                    modifiers: d,
                    animate: "ms" !== b
                }
            }
        }

        function q(a, b, c) {
            var d = [];
            return a.face && (d = a.face.split(" "), d.shift()), a.chars ? {
                type: "repeater",
                options: {
                    delay: "text" === a.view ? 0 : 50,
                    className: "soon-value " + (c || ""),
                    transform: D.chain(D.pad(a.padding[b]), D.chars()),
                    presenter: {
                        type: a.view,
                        options: {
                            modifiers: d
                        }
                    }
                }
            } : {
                type: "group",
                options: {
                    className: "soon-group-sub-sub soon-value " + (c || ""),
                    transform: D.pad(a.padding[b]),
                    presenters: [{
                        type: a.view,
                        options: {
                            modifiers: d
                        }
                    }]
                }
            }
        }

        function r(a, b, c, d) {
            K.push({
                node: a,
                ticker: b,
                presenter: c,
                options: d
            })
        }

        function s(a) {
            return new(t(a.type))(a.options || {})
        }

        function t(a) {
            return C[a.charAt(0).toUpperCase() + a.slice(1)]
        }

        function u(a, b) {
            var c = a.getElementsByClassName ? a.getElementsByClassName("soon-placeholder") : a.querySelectorAll("soon-placeholder");
            c.length && (c[0].innerHTML = "", a = c[0]);
            var d = s(b);
            return a.appendChild(d.getElement()), d
        }

        function v(a, b, c, d) {
            var e = new F(function(a) {
                b.setValue(a)
            }, {
                rate: c
            });
            return r(a, e, b, d), e.start(), f(a, b), e
        }

        function w(a) {
            var b, c, d = ["labels", "padding"],
                e = 2,
                f = {
                    since: m(a, "since"),
                    due: m(a, "due"),
                    now: m(a, "now"),
                    face: m(a, "face"),
                    visual: m(a, "visual"),
                    format: m(a, "format"),
                    singular: "true" === m(a, "singular"),
                    reflect: "true" === m(a, "reflect"),
                    scaleMax: m(a, "scale-max"),
                    scaleHide: m(a, "scale-hide"),
                    separateChars: !("false" === m(a, "separate-chars")),
                    cascade: !("false" === m(a, "cascade")),
                    separator: m(a, "separator"),
                    padding: !("false" === m(a, "padding")),
                    eventComplete: m(a, "event-complete"),
                    eventTick: m(a, "event-tick")
                };
            for (var g in M)
                if (M.hasOwnProperty(g))
                    for (b = M[g], c = 0; e > c; c++) f[d[c] + b.option] = m(a, d[c] + "-" + b.option.toLowerCase());
            return A.create(a, f)
        }

        function x(a) {
            var b;
            if (0 === a.indexOf("in ")) {
                var c = a.match(N),
                    d = parseInt(c[1], 10),
                    e = c[2];
                return b = new Date, -1 !== e.indexOf("hour") ? b.setHours(b.getHours() + d) : -1 !== e.indexOf("minute") ? b.setMinutes(b.getMinutes() + d) : -1 !== e.indexOf("second") && b.setSeconds(b.getSeconds() + d), b
            }
            if (-1 !== a.indexOf("at ")) {
                b = new Date;
                var f = b.getTime(),
                    g = -1 !== a.indexOf("reset");
                a = a.replace("reset ", "");
                var h = a.split("at "),
                    i = h[1].match(O),
                    j = parseInt(i[1], 10),
                    k = i[2] ? parseInt(i[2], 10) : 0,
                    l = i[3] ? parseInt(i[3], 10) : 0,
                    m = h[1].split(" zone ");
                if (m && (m = m[1]), h[0].length) {
                    var n = B.getDayIndex(h[0]),
                        o = (n + 7 - b.getDay()) % 7;
                    b.setDate(b.getDate() + o)
                }
                b.setHours(j), b.setMinutes(k), b.setSeconds(l), b.setMilliseconds(0), g && f >= b.getTime() && b.setHours(j + (h[0].length ? 168 : 24));
                var p = B.pad,
                    q = b.getFullYear() + "-" + p(b.getMonth() + 1) + "-" + p(b.getDate()),
                    r = p(b.getHours()) + ":" + p(b.getMinutes()) + ":" + p(b.getSeconds());
                a = q + "T" + r + (m || "")
            }
            return B.isoToDate(a)
        }

        function y(a, b) {
            if (0 === b.indexOf(a)) return "";
            if ("w" === a && -1 !== b.indexOf("M")) return "";
            if ("d" === a) {
                if (-1 !== b.indexOf("w")) return "";
                if (-1 !== b.indexOf("M")) return "00"
            }
            return null
        }

        function z(a, b, c) {
            if (c && -1 !== G.indexOf(a)) return c;
            var d = function(c) {
                return function() {
                    c(), A.destroy(a), A.create(a, b)
                }
            }(c);
            return G.push(a), d
        }
        if (document.querySelectorAll && !a.Soon) {
            var A = {},
                B = {},
                C = {},
                D = {},
                E = {
                    timer: 0,
                    cbs: [],
                    register: function(a) {
                        E.cbs.push(a)
                    },
                    deregister: function(a) {
                        for (var b = E.cbs.length - 1; b >= 0; b--) E.cbs[b] === a && E.cbs.splice(b, 1)
                    },
                    onresize: function() {
                        clearTimeout(E.timer), E.timer = setTimeout(function() {
                            E.resize()
                        }, 100)
                    },
                    resize: function() {
                        for (var a = 0, b = E.cbs.length; b > a; a++) E.cbs[a]()
                    },
                    init: function() {
                        a.addEventListener && a.addEventListener("resize", E.onresize, !1)
                    }
                };
            E.init(), Function.prototype.bind || (Function.prototype.bind = function(a) {
                    if ("function" != typeof this) throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
                    var b = Array.prototype.slice.call(arguments, 1),
                        c = this,
                        d = function() {},
                        e = function() {
                            return c.apply(this instanceof d && a ? this : a, b.concat(Array.prototype.slice.call(arguments)))
                        };
                    return d.prototype = this.prototype, e.prototype = new d, e
                }), Array.prototype.indexOf || (Array.prototype.indexOf = function(a, b) {
                    var c;
                    if (null == this) throw new TypeError('"this" is null or not defined');
                    var d = Object(this),
                        e = d.length >>> 0;
                    if (0 === e) return -1;
                    var f = +b || 0;
                    if (1 / 0 === Math.abs(f) && (f = 0), f >= e) return -1;
                    for (c = Math.max(f >= 0 ? f : e - Math.abs(f), 0); e > c;) {
                        if (c in d && d[c] === a) return c;
                        c++
                    }
                    return -1
                }),
                function() {
                    function a(a) {
                        this.el = a;
                        for (var b = a.className.replace(/^\s+|\s+$/g, "").split(/\s+/), c = 0; c < b.length; c++) d.call(this, b[c])
                    }

                    function b(a, b, c) {
                        Object.defineProperty ? Object.defineProperty(a, b, {
                            get: c
                        }) : a.__defineGetter__(b, c)
                    }
                    if (!("undefined" == typeof window.Element || "classList" in document.documentElement)) {
                        var c = Array.prototype,
                            d = c.push,
                            e = c.splice,
                            f = c.join;
                        a.prototype = {
                            add: function(a) {
                                this.contains(a) || (d.call(this, a), this.el.className = this.toString())
                            },
                            contains: function(a) {
                                return -1 != this.el.className.indexOf(a)
                            },
                            item: function(a) {
                                return this[a] || null
                            },
                            remove: function(a) {
                                if (this.contains(a)) {
                                    for (var b = 0; b < this.length && this[b] != a; b++);
                                    e.call(this, b, 1), this.el.className = this.toString()
                                }
                            },
                            toString: function() {
                                return f.call(this, " ")
                            },
                            toggle: function(a) {
                                return this.contains(a) ? this.remove(a) : this.add(a), this.contains(a)
                            }
                        }, window.DOMTokenList = a, b(Element.prototype, "classList", function() {
                            return new a(this)
                        })
                    }
                }(),
                function() {
                    for (var a = 0, b = ["webkit", "moz"], c = 0; c < b.length && !window.requestAnimationFrame; ++c) window.requestAnimationFrame = window[b[c] + "RequestAnimationFrame"], window.cancelAnimationFrame = window[b[c] + "CancelAnimationFrame"] || window[b[c] + "CancelRequestAnimationFrame"];
                    window.requestAnimationFrame || (window.requestAnimationFrame = function(b) {
                        var c = (new Date).getTime(),
                            d = Math.max(0, 16 - (c - a)),
                            e = window.setTimeout(function() {
                                b(c + d)
                            }, d);
                        return a = c + d, e
                    }), window.cancelAnimationFrame || (window.cancelAnimationFrame = function(a) {
                        clearTimeout(a)
                    })
                }(), B = function() {
                    function a() {
                        if (!window.getComputedStyle) return !1;
                        var a, b = document.createElement("div"),
                            d = {
                                webkitTransform: "-webkit-transform",
                                OTransform: "-o-transform",
                                msTransform: "-ms-transform",
                                MozTransform: "-moz-transform",
                                transform: "transform"
                            };
                        document.body.insertBefore(b, null);
                        for (var e in d) b.style[e] !== c && (b.style[e] = "translate3d(1px,1px,1px)", a = window.getComputedStyle(b).getPropertyValue(d[e]));
                        return document.body.removeChild(b), a !== c && a.length > 0 && "none" !== a
                    }

                    function b() {
                        var a = !1,
                            b = "animation",
                            d = "",
                            e = "Webkit Moz O ms Khtml".split(" "),
                            f = "",
                            g = 0,
                            h = document.body,
                            i = e.length;
                        if (h.style.animationName !== c && (a = !0), a === !1)
                            for (; i > g; g++)
                                if (h.style[e[g] + "AnimationName"] !== c) {
                                    f = e[g], b = f + "Animation", d = "-" + f.toLowerCase() + "-", a = !0;
                                    break
                                } return a
                    }
                    var d, e, f = "textContent" in document.documentElement,
                        g = function(a) {
                            var b, c, d = /^(\d{4}\-\d\d\-\d\d([tT ][\d:\.]*)?)([zZ]|([+\-])(\d\d):(\d\d))?$/,
                                e = d.exec(a) || [];
                            if (e[1]) {
                                b = e[1].split(/\D/);
                                for (var f = 0, g = b.length; g > f; f++) b[f] = parseInt(b[f], 10) || 0;
                                return b[1] -= 1, b = new Date(Date.UTC.apply(Date, b)), b.getDate() ? (e[5] && (c = 60 * parseInt(e[5], 10), e[6] && (c += parseInt(e[6], 10)), "+" == e[4] && (c *= -1), c && b.setUTCMinutes(b.getUTCMinutes() + c)), b) : Number.NaN
                            }
                            return Number.NaN
                        },
                        h = new Date("2015-01-01T12:00:00.123+01:00"),
                        i = isNaN(h) ? function(a) {
                            return g(a)
                        } : function(a) {
                            return new Date(a)
                        };
                    "undefined" != typeof document.hidden ? (e = "hidden", d = "visibilitychange") : "undefined" != typeof document.mozHidden ? (e = "mozHidden", d = "mozvisibilitychange") : "undefined" != typeof document.msHidden ? (e = "msHidden", d = "msvisibilitychange") : "undefined" != typeof document.webkitHidden && (e = "webkitHidden", d = "webkitvisibilitychange");
                    var j = !1,
                        k = 1,
                        l = 1e3 * k,
                        m = 60 * l,
                        n = 60 * m,
                        o = 24 * n,
                        p = 7 * o,
                        q = {
                            MAX: {
                                y: 100,
                                M: 12,
                                w: 52,
                                d: 365,
                                h: 24,
                                m: 60,
                                s: 60,
                                ms: 1e3
                            },
                            AMOUNT: {
                                w: p,
                                d: o,
                                h: n,
                                m: m,
                                s: l,
                                ms: k
                            },
                            NAMES: {
                                y: "years",
                                M: "months",
                                w: "weeks",
                                d: "days",
                                h: "hours",
                                m: "minutes",
                                s: "seconds",
                                ms: "milliseconds"
                            },
                            FORMATS: ["y", "M", "w", "d", "h", "m", "s", "ms"],
                            CIRC: 2 * Math.PI,
                            QUART: .5 * Math.PI,
                            DAYS: ["su", "mo", "tu", "we", "th", "fr", "sa"],
                            setText: null,
                            documentVisibilityEvent: d,
                            pad: function(a) {
                                return ("00" + a).slice(-2)
                            },
                            getDayIndex: function(a) {
                                return this.DAYS.indexOf(a.substr(0, 2))
                            },
                            isSlow: function() {
                                return !f
                            },
                            supportsAnimation: function() {
                                return j = b() && a(), q.supportsAnimation = function() {
                                    return j
                                }, j
                            },
                            toArray: function(a) {
                                return Array.prototype.slice.call(a)
                            },
                            toBoolean: function(a) {
                                return "string" == typeof a ? "true" === a : a
                            },
                            isoToDate: function(a) {
                                if (a.match(/(Z)|([+\-][0-9]{2}:?[0-9]*$)/g)) return i(a);
                                a += -1 !== a.indexOf("T") ? "Z" : "";
                                var b = i(a);
                                return this.dateToLocal(b)
                            },
                            dateToLocal: function(a) {
                                return new Date(a.getTime() + 6e4 * a.getTimezoneOffset())
                            },
                            prefix: function() {
                                for (var a, b = ["webkit", "Moz", "ms", "O"], c = 0, d = b.length, e = document.createElement("div").style; d > c; c++)
                                    if (a = b[c] + "Transform", a in e) return b[c];
                                return null
                            }(),
                            setTransform: function(a, b) {
                                a.style[this.prefix + "Transform"] = b, a.style.transform = b
                            },
                            setTransitionDelay: function(a, b) {
                                a.style[this.prefix + "TransitionDelay"] = b + "," + b + "," + b, a.style.TransitionDelay = b + "," + b + "," + b
                            },
                            getShadowProperties: function(a) {
                                if (a = a ? a.match(/(-?\d+px)|(rgba\(.+\))|(rgb\(.+\))|(#[abcdef\d]+)/g) : null, !a) return null;
                                for (var b, c = 0, d = a.length, e = []; d > c; c++) - 1 !== a[c].indexOf("px") ? e.push(parseInt(a[c], 10)) : b = a[c];
                                return e.push(b), 5 === e.length && e.splice(3, 1), e
                            },
                            getDevicePixelRatio: function() {
                                return window.devicePixelRatio || 1
                            },
                            isDocumentHidden: function() {
                                return e ? document[e] : !1
                            },
                            triggerAnimation: function(a, b) {
                                a.classList.remove(b), window.requestAnimationFrame(function() {
                                    a.offsetLeft, a.classList.add(b)
                                })
                            },
                            getBackingStoreRatio: function(a) {
                                return a.webkitBackingStorePixelRatio || a.mozBackingStorePixelRatio || a.msBackingStorePixelRatio || a.oBackingStorePixelRatio || a.backingStorePixelRatio || 1
                            },
                            setShadow: function(a, b, c, d, e) {
                                a.shadowOffsetX = b, a.shadowOffsetY = c, a.shadowBlur = d, a.shadowColor = e
                            },
                            getColorBetween: function(a, b, c) {
                                function d(a, b) {
                                    return a + Math.round((b - a) * c)
                                }

                                function e(a) {
                                    a = Math.min(a, 255), a = Math.max(a, 0);
                                    var b = a.toString(16);
                                    return b.length < 2 && (b = "0" + b), b
                                }
                                return "#" + e(d(a.r, b.r)) + e(d(a.g, b.g)) + e(d(a.b, b.b))
                            },
                            getGradientColors: function(a, b, c) {
                                for (var d = [], e = 0, f = c, g = 1 / (f - 1), h = 0; f > e; e++) d[e] = this.getColorBetween(a, b, h), h += g;
                                return d
                            },
                            hexToRgb: function(a) {
                                var b = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(a);
                                return b ? {
                                    r: parseInt(b[1], 16),
                                    g: parseInt(b[2], 16),
                                    b: parseInt(b[3], 16)
                                } : null
                            },
                            drawGradientArc: function(a, b, c, d, e, f, g, h, i, j, k, l, m) {
                                if (!(g > h)) {
                                    l && this.drawArc(a, b, c, d, e, f, g, h, i, "transparent", l, m);
                                    for (var n, o, p, q, r, s, t, u = this.hexToRgb(j), v = this.hexToRgb(k), w = this.hexToRgb(this.getColorBetween(u, v, (g - e) / f)), x = this.hexToRgb(this.getColorBetween(u, v, (h - e) / f)), y = h - g, z = Math.ceil(30 * y), A = this.getGradientColors(w, x, z), B = -this.QUART + this.CIRC * g, C = A.length, D = 0, E = this.CIRC * y / C; C > D; D++) o = A[D], p = A[D + 1] || o, q = b + Math.cos(B) * d, s = b + Math.cos(B + E) * d, r = c + Math.sin(B) * d, t = c + Math.sin(B + E) * d, a.beginPath(), n = a.createLinearGradient(q, r, s, t), n.addColorStop(0, o), n.addColorStop(1, p), a.lineCap = m, a.strokeStyle = n, a.arc(b, c, d, B - .005, B + E + .005), a.lineWidth = i, a.stroke(), a.closePath(), B += E
                                }
                            },
                            drawArc: function(a, b, c, d, e, f, g, h, i, j, k, l) {
                                if (!(g > h)) {
                                    if (null !== j.gradient.colors && "follow" === j.gradient.type) return void this.drawGradientArc(a, b, c, d, e, f, g, h, i, j.gradient.colors[0], j.gradient.colors[1], k, l);
                                    if (k) {
                                        var m = "transparent" === j.fill ? 9999 : 0;
                                        a.save(), a.translate(m, 0), this.setShadow(a, k[0] - m, k[1], k[2], k[3])
                                    }
                                    if (a.beginPath(), a.lineWidth = i, a.arc(b, c, d, -this.QUART + this.CIRC * g, -this.QUART + this.CIRC * h, !1), j.gradient.colors) {
                                        var n = "horizontal" === j.gradient.type ? a.createLinearGradient(0, d, 2 * d, d) : a.createLinearGradient(d, 0, d, 2 * d);
                                        n.addColorStop(0, j.gradient.colors[0]), n.addColorStop(1, j.gradient.colors[1]), a.strokeStyle = n
                                    } else a.strokeStyle = "transparent" === j.fill ? "#000" : j.fill;
                                    a.lineCap = l, a.stroke(), k && a.restore()
                                }
                            },
                            drawRing: function(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) {
                                d + e > 1 && (d -= -1 + d + e, c += .5 * e);
                                var q = c,
                                    r = c + d,
                                    s = b * d,
                                    t = .5 - Math.abs(-.5 + b),
                                    u = c + (s - t * e),
                                    v = c + (s + (1 - t) * e);
                                (g || k) && (p ? (this.drawArc(a, f, f, g, c, d, v, r, h, i, j, o), this.drawArc(a, f, f, k, c, d, q, u, l, m, n, o)) : (this.drawArc(a, f, f, g, c, d, q, u, h, i, j, o), this.drawArc(a, f, f, k, c, d, v, r, l, m, n, o)))
                            },
                            setTextContent: function() {
                                return f ? function(a, b) {
                                    a.textContent = b
                                } : function(a, b) {
                                    a.innerText = b
                                }
                            }()
                        };
                    return q
                }(), D.cap = function(a, b) {
                    return a = a || 0, b = b || 1,
                        function(c) {
                            return Math.min(Math.max(c, a), b)
                        }
                }, D.chain = function(a) {
                    return function() {
                        var b, c = a.toArray(arguments),
                            d = c.length;
                        return function(a) {
                            for (b = 0; d > b; b++) a = c[b](a);
                            return a
                        }
                    }
                }(B), D.chars = function() {
                    return function(a) {
                        return (a + "").split("")
                    }
                }, D.diff = function(a) {
                    return function(b) {
                        return a - b
                    }
                }, D.duplicate = function(a) {
                    var b, c = new Array(a);
                    return function(d) {
                        for (b = a; b--;) c[b] = d;
                        return c
                    }
                }, D.duration = function(a) {
                    function b(a, b) {
                        return a.setMonth(a.getMonth() + b), a
                    }

                    function c(a) {
                        return new Date(a.valueOf())
                    }

                    function d(a, b) {
                        return -1 !== b.indexOf(a)
                    }

                    function e(a, d) {
                        var e, f, g = 12 * (d.getFullYear() - a.getFullYear()) + (d.getMonth() - a.getMonth()),
                            h = b(c(a), g);
                        return 0 > d - h ? (e = b(c(a), g - 1), f = (d - h) / (h - e)) : (e = b(c(a), g + 1), f = (d - h) / (e - h)), -(g + f)
                    }
                    var f = a.FORMATS,
                        g = f.length,
                        h = {
                            M: 1,
                            y: 12
                        };
                    return function(i, j, k, l) {
                        var m = d("M", k),
                            n = d("y", k);
                        return function(j) {
                            var o, p, q, r, s, t, u = 0,
                                v = [];
                            for ((m || n || !l) && (o = new Date(i.valueOf() + j), p = e(o, i), t = m ? Math.floor(p) : 12 * Math.floor(p / 12), j = o.valueOf() - b(c(i), t).valueOf()); g > u; u++) r = f[u], 2 > u ? (q = Math.floor(p / h[r]), d(r, k) ? (p -= q * h[r], v.push(Math.max(0, q))) : l || (p -= q * h[r])) : (s = a.AMOUNT[r], q = Math.floor(j / s), d(r, k) ? (j %= s, v.push(Math.max(0, q))) : l || (j %= s));
                            return v
                        }
                    }
                }(B), D.event = function(a, b) {
                    return function(c) {
                        return a(c) && b(), c
                    }
                }, D.modulate = function(a) {
                    return function(b) {
                        return parseInt(b, 10) % 2 === 0 ? a : ""
                    }
                }, D.now = function() {
                    return function() {
                        return (new Date).getTime()
                    }
                }, D.offset = function(a) {
                    return function(b) {
                        return a + b
                    }
                }, D.pad = function(a) {
                    return a = a || "",
                        function(b) {
                            return (a + b).slice(-a.length)
                        }
                }, D.plural = function(a, b) {
                    return function(c) {
                        return 1 === parseInt(c, 10) ? a : b
                    }
                }, D.progress = function(a, b) {
                    return function(c) {
                        return c = parseInt(c, 10), b > a ? c / b : (a - c) / a
                    }
                }, C.Console = function() {
                    var a = function(a) {
                        this._transform = a.transform || function(a) {
                            return a
                        }
                    };
                    return a.prototype = {
                        redraw: function() {},
                        destroy: function() {
                            return null
                        },
                        getElement: function() {
                            return null
                        },
                        setValue: function(a) {
                            console.log(this._transform(a))
                        }
                    }, a
                }(), C.Fill = function(a) {
                    var b = function(a) {
                        this._wrapper = document.createElement("span"), this._wrapper.className = "soon-fill " + (a.className || ""), this._transform = a.transform || function(a) {
                            return a
                        }, this._direction = "to-top";
                        for (var b = 0, c = a.modifiers.length; c > b; b++)
                            if (0 === a.modifiers[b].indexOf("to-")) {
                                this._direction = a.modifiers[b];
                                break
                            } this._fill = document.createElement("span"), this._fill.className = "soon-fill-inner", this._progress = document.createElement("span"), this._progress.className = "soon-fill-progress", this._fill.appendChild(this._progress), this._wrapper.appendChild(this._fill)
                    };
                    return b.prototype = {
                        redraw: function() {},
                        destroy: function() {
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        setValue: function(b) {
                            var c, d = this._transform(b);
                            switch (this._direction) {
                                case "to-top":
                                    c = "translateY(" + (100 - 100 * d) + "%)";
                                    break;
                                case "to-top-right":
                                    c = "scale(1.45) rotateZ(-45deg) translateX(" + (-100 + 100 * d) + "%)";
                                    break;
                                case "to-top-left":
                                    c = "scale(1.45) rotateZ(45deg) translateX(" + (100 - 100 * d) + "%)";
                                    break;
                                case "to-left":
                                    c = "translateX(" + (100 - 100 * d) + "%)";
                                    break;
                                case "to-right":
                                    c = "translateX(" + (-100 + 100 * d) + "%)";
                                    break;
                                case "to-bottom-right":
                                    c = "scale(1.45) rotateZ(45deg) translateX(" + (-100 + 100 * d) + "%)";
                                    break;
                                case "to-bottom-left":
                                    c = "scale(1.45) rotateZ(-45deg) translateX(" + (100 - 100 * d) + "%)";
                                    break;
                                case "to-bottom":
                                    c = "translateY(" + (-100 + 100 * d) + "%)"
                            }
                            a.setTransform(this._progress, c)
                        }
                    }, b
                }(B), C.Flip = function(a) {
                    var b = function(b) {
                        this._wrapper = document.createElement("span"), this._wrapper.className = "soon-flip " + (b.className || ""), this._transform = b.transform || function(a) {
                            return a
                        }, this._inner = document.createElement("span"), this._inner.className = "soon-flip-inner", this._card = document.createElement("span"), this._card.className = "soon-flip-card", a.supportsAnimation() ? (this._front = document.createElement("span"), this._front.className = "soon-flip-front soon-flip-face", this._back = document.createElement("span"), this._back.className = "soon-flip-back soon-flip-face", this._card.appendChild(this._front), this._card.appendChild(this._back), this._top = document.createElement("span"), this._top.className = "soon-flip-top soon-flip-face", this._card.appendChild(this._top), this._bottom = document.createElement("span"), this._bottom.className = "soon-flip-bottom soon-flip-face", this._card.appendChild(this._bottom)) : (this._fallback = document.createElement("span"), this._fallback.className = "soon-flip-fallback", this._card.appendChild(this._fallback)), this._bounding = document.createElement("span"), this._bounding.className = "soon-flip-bounding", this._card.appendChild(this._bounding), this._inner.appendChild(this._card), this._wrapper.appendChild(this._inner), this._frontValue = null, this._backValue = null, this._boundingLength = 0
                    };
                    return b.prototype = {
                        redraw: function() {},
                        _setBoundingForValue: function(a) {
                            var b = (a + "").length;
                            if (b !== this._boundingLength) {
                                this._boundingLength = b;
                                for (var c = "", d = 0; b > d; d++) c += "8";
                                this._bounding.textContent = c;
                                var e = parseInt(getComputedStyle(this._card).fontSize, 10),
                                    f = this._bounding.offsetWidth / e;
                                this._inner.style.width = f + .1 * (b - 1) + "em"
                            }
                        },
                        destroy: function() {
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        setValue: function(b) {
                            return b = this._transform(b), a.supportsAnimation() ? this._frontValue ? void(this._backValue && this._backValue === b || this._frontValue === b || (this._backValue && (this._bottom.textContent = this._backValue, this._front.textContent = this._backValue, this._frontValue = this._backValue), this._setBoundingForValue(b), this._top.textContent = b, this._back.textContent = b, this._backValue = b, a.triggerAnimation(this._inner, "soon-flip-animate"))) : (this._bottom.textContent = b, this._front.textContent = b, this._frontValue = b, void this._setBoundingForValue(b)) : (this._fallback.textContent = b, void this._setBoundingForValue(b))
                        }
                    }, b
                }(B), C.Group = function(a) {
                    var b = function(a) {
                        this._wrapper = document.createElement("span"), this._wrapper.className = "soon-group " + (a.className || ""), this._inner = document.createElement("span"), this._inner.className = "soon-group-inner", this._wrapper.appendChild(this._inner), this._transform = a.transform || function(a) {
                            return a
                        }, this._presenters = a.presenters, this._presenterStorage = []
                    };
                    return b.prototype = {
                        redraw: function() {
                            for (var a = this._presenterStorage.length - 1; a >= 0; a--) this._presenterStorage[a].redraw()
                        },
                        destroy: function() {
                            for (var a = this._presenterStorage.length - 1; a >= 0; a--) this._presenterStorage[a].destroy();
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        setValue: function(b) {
                            this._wrapper.setAttribute("data-value", b), b = this._transform(b);
                            for (var c, d = 0, e = b instanceof Array, f = e ? b.length : this._presenters.length; f > d; d++) c = this._presenterStorage[d], c || (c = a(this._presenters[d]), this._inner.appendChild(c.getElement()), this._presenterStorage[d] = c), c.setValue(e ? b[d] : b)
                        }
                    }, b
                }(s), C.Matrix = function() {
                    var a = {
                            "3x5": {
                                " ": [
                                    [0, 0, 0],
                                    [0, 0, 0],
                                    [0, 0, 0],
                                    [0, 0, 0],
                                    [0, 0, 0]
                                ],
                                0: [
                                    [1, 1, 1],
                                    [1, 0, 1],
                                    [1, 0, 1],
                                    [1, 0, 1],
                                    [1, 1, 1]
                                ],
                                1: [
                                    [1, 1, 0],
                                    [0, 1, 0],
                                    [0, 1, 0],
                                    [0, 1, 0],
                                    [0, 1, 0]
                                ],
                                2: [
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [1, 1, 1],
                                    [1, 0, 0],
                                    [1, 1, 1]
                                ],
                                3: [
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [1, 1, 1]
                                ],
                                4: [
                                    [1, 0, 1],
                                    [1, 0, 1],
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [0, 0, 1]
                                ],
                                5: [
                                    [1, 1, 1],
                                    [1, 0, 0],
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [1, 1, 1]
                                ],
                                6: [
                                    [1, 1, 1],
                                    [1, 0, 0],
                                    [1, 1, 1],
                                    [1, 0, 1],
                                    [1, 1, 1]
                                ],
                                7: [
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [0, 0, 1],
                                    [0, 0, 1],
                                    [0, 0, 1]
                                ],
                                8: [
                                    [1, 1, 1],
                                    [1, 0, 1],
                                    [1, 1, 1],
                                    [1, 0, 1],
                                    [1, 1, 1]
                                ],
                                9: [
                                    [1, 1, 1],
                                    [1, 0, 1],
                                    [1, 1, 1],
                                    [0, 0, 1],
                                    [1, 1, 1]
                                ]
                            },
                            "5x7": {
                                " ": [
                                    [0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0],
                                    [0, 0, 0, 0, 0]
                                ],
                                0: [
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 0]
                                ],
                                1: [
                                    [0, 0, 1, 1, 0],
                                    [0, 1, 1, 1, 0],
                                    [0, 0, 1, 1, 0],
                                    [0, 0, 1, 1, 0],
                                    [0, 0, 1, 1, 0],
                                    [0, 0, 1, 1, 0],
                                    [0, 1, 1, 1, 1]
                                ],
                                2: [
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 0, 1, 1, 0],
                                    [0, 1, 1, 0, 0],
                                    [1, 1, 0, 0, 0],
                                    [1, 1, 1, 1, 1]
                                ],
                                3: [
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 0, 1, 1, 0],
                                    [0, 0, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 0]
                                ],
                                4: [
                                    [0, 0, 1, 1, 1],
                                    [0, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 1, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 0, 0, 1, 1]
                                ],
                                5: [
                                    [1, 1, 1, 1, 1],
                                    [1, 1, 0, 0, 0],
                                    [1, 1, 0, 0, 0],
                                    [1, 1, 1, 1, 0],
                                    [0, 0, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 0]
                                ],
                                6: [
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 0, 0],
                                    [1, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 0]
                                ],
                                7: [
                                    [1, 1, 1, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 0, 1, 1, 0],
                                    [0, 1, 1, 0, 0],
                                    [1, 1, 0, 0, 0],
                                    [1, 1, 0, 0, 0]
                                ],
                                8: [
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 0]
                                ],
                                9: [
                                    [0, 1, 1, 1, 0],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [1, 1, 0, 1, 1],
                                    [0, 1, 1, 1, 1],
                                    [0, 0, 0, 1, 1],
                                    [0, 1, 1, 1, 0]
                                ]
                            }
                        },
                        b = function() {
                            var b, c, d, e, f, g, h = [];
                            for (d in a)
                                if (a.hasOwnProperty(d)) {
                                    for (b = a[d][0].length, c = a[d][0][0].length, g = "", e = 0; b > e; e++) {
                                        for (g += '<span class="soon-matrix-row">', f = 0; c > f; f++) g += '<span class="soon-matrix-dot"></span>';
                                        g += "</span>"
                                    }
                                    h[d] = g
                                } return h
                        }(),
                        c = function(a) {
                            this._wrapper = document.createElement("span"), this._wrapper.className = "soon-matrix " + (a.className || ""), this._inner = document.createElement("span"), this._inner.className = "soon-matrix-inner", this._wrapper.appendChild(this._inner), this._transform = a.transform || function(a) {
                                return a
                            }, this._value = [], this._type = -1 !== a.modifiers.indexOf("3x5") ? "3x5" : "5x7"
                        };
                    return c.prototype = {
                        redraw: function() {},
                        destroy: function() {
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        _addChar: function() {
                            var a = document.createElement("span");
                            return a.className = "soon-matrix-char", a.innerHTML = b[this._type], {
                                node: a,
                                ref: []
                            }
                        },
                        _updateChar: function(b, c) {
                            var d, e = a[this._type][c],
                                f = e.length,
                                g = e[0].length,
                                h = 0,
                                i = b.ref;
                            if (!i.length)
                                for (var j = b.node.getElementsByClassName("soon-matrix-dot"); f > h; h++)
                                    for (i[h] = [], d = 0; g > d; d++) i[h][d] = j[h * g + d];
                            for (; f > h; h++)
                                for (d = 0; g > d; d++) i[h][d].setAttribute("data-state", 1 === e[h][d] ? "1" : "0")
                        },
                        setValue: function(a) {
                            a = this._transform(a), a += "", a = a.split("");
                            for (var b = 0, c = a.length; c > b; b++) {
                                var d = this._value[b];
                                d || (d = this._addChar(), this._inner.appendChild(d.node), this._value[b] = d), this._updateChar(d, a[b])
                            }
                        }
                    }, c
                }(), C.Repeater = function(a) {
                    var b = function(b) {
                        this._wrapper = document.createElement("span"), this._wrapper.className = "soon-repeater " + (b.className || ""), this._delay = b.delay || 0, this._transform = b.transform || function(a) {
                            return a
                        }, this._destroyed = !1, this._presenter = b.presenter, this._Presenter = a(this._presenter.type), this._prepend = "undefined" == typeof b.prepend ? !0 : b.prepend, this._presenterStorage = []
                    };
                    return b.prototype = {
                        redraw: function() {
                            for (var a = this._presenterStorage.length - 1; a >= 0; a--) this._presenterStorage[a].redraw()
                        },
                        destroy: function() {
                            this._destroyed = !0;
                            for (var a = this._presenterStorage.length - 1; a >= 0; a--) this._presenterStorage[a].destroy();
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        setValue: function(a) {
                            a = this._transform(a), a = a instanceof Array ? a : [a], this._prepend && a.reverse();
                            for (var b, c, d, e = 0, f = a.length, g = 0, h = a.length !== this._wrapper.children.length; f > e; e++) b = this._presenterStorage[e], b || (b = new this._Presenter(this._presenter.options || {}), 0 !== this._wrapper.children.length && this._prepend ? this._wrapper.insertBefore(b.getElement(), this._wrapper.firstChild) : this._wrapper.appendChild(b.getElement()), this._presenterStorage[e] = b, this._delay && (g -= this._delay)), this._delay && !h ? (this._setValueDelayed(b, a[e], g), g += this._delay) : this._setValue(b, a[e], h);
                            for (f = this._wrapper.children.length, d = e; f > e; e++) b = this._presenterStorage[e], c = b.destroy(), c.parentNode.removeChild(c), this._presenterStorage[e] = null;
                            this._presenterStorage.length = d
                        },
                        _setValueDelayed: function(a, b, c, d) {
                            var e = this;
                            setTimeout(function() {
                                e._setValue(a, b, d)
                            }, c)
                        },
                        _setValue: function(a, b, c) {
                            c && a.setValue(" "), a.setValue(b)
                        }
                    }, b
                }(t), C.Ring = function(a, b) {
                    var c = function(b) {
                        this._wrapper = document.createElement("span"), this._wrapper.className = "soon-ring " + (b.className || ""), this._transform = b.transform || function(a) {
                            return a
                        }, this._modifiers = b.modifiers, this._animate = b.animate, this._drawn = !1, this._canvas = document.createElement("canvas"), this._wrapper.appendChild(this._canvas), this._style = document.createElement("span"), this._style.className = "soon-ring-progress", this._style.style.visibility = "hidden", this._style.style.position = "absolute", this._wrapper.appendChild(this._style), this._current = 0, this._target = null, this._destroyed = !1, this._lastTick = 0, this._styles = null;
                        var c = this;
                        a.supportsAnimation() ? window.requestAnimationFrame(function(a) {
                            c._tick(a)
                        }) : this._animate = !1
                    };
                    return c.prototype = {
                        destroy: function() {
                            return this._destroyed = !0, b.deregister(this._resizeBind), this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        _getModifier: function(a) {
                            for (var b = 0, c = this._modifiers.length, d = null; c > b; b++)
                                if (-1 !== this._modifiers[b].indexOf(a)) {
                                    d = this._modifiers[b];
                                    break
                                } if (!d) return null;
                            if (-1 === d.indexOf("-")) return !0;
                            var e = d.split("-");
                            if (-1 !== e[1].indexOf("_")) {
                                var f = e[1].split("_");
                                return f[0] = "#" + f[0], f[1] = "#" + f[1], f
                            }
                            var g = parseFloat(e[1]);
                            return isNaN(g) ? e[1] : g / 100
                        },
                        redraw: function() {
                            var b = window.getComputedStyle(this._style);
                            this._styles = {
                                offset: this._getModifier("offset") || 0,
                                gap: this._getModifier("gap") || 0,
                                length: this._getModifier("length") || 1,
                                flip: this._getModifier("flip") || !1,
                                invert: this._getModifier("invert") || null,
                                align: "center",
                                size: 0,
                                radius: 0,
                                padding: parseInt(b.getPropertyValue("padding-bottom"), 10) || 0,
                                cap: 0 === parseInt(b.getPropertyValue("border-top-right-radius"), 10) ? "butt" : "round",
                                progressColor: {
                                    fill: b.getPropertyValue("color") || "#000",
                                    gradient: {
                                        colors: this._getModifier("progressgradient") || null,
                                        type: this._getModifier("progressgradienttype") || "follow"
                                    }
                                },
                                progressWidth: parseInt(b.getPropertyValue("border-top-width"), 10) || 2,
                                progressShadow: a.getShadowProperties(b.getPropertyValue("text-shadow")),
                                ringColor: {
                                    fill: b.getPropertyValue("background-color") || "#fff",
                                    gradient: {
                                        colors: this._getModifier("ringgradient") || null,
                                        type: this._getModifier("ringgradienttype") || "follow"
                                    }
                                },
                                ringWidth: parseInt(b.getPropertyValue("border-bottom-width"), 10) || 2,
                                ringShadow: a.getShadowProperties(b.getPropertyValue("box-shadow"))
                            };
                            var c = this._canvas.getContext("2d"),
                                d = this._canvas.parentNode.clientWidth,
                                e = a.getDevicePixelRatio(),
                                f = a.getBackingStoreRatio(c),
                                g = e / f,
                                h = 125 > d ? Math.min(1, .005 * d) : 1;
                            if (this._styles.ringWidth = Math.ceil(this._styles.ringWidth * h), this._styles.progressWidth = Math.ceil(this._styles.progressWidth * h), "transparent" === this._styles.ringColor.fill && (this._styles.ringColor.fill = "rgba(0,0,0,0)"), "transparent" === this._styles.progressColor.fill && (this._styles.progressColor.fill = "rgba(0,0,0,0)"), "round" === this._styles.cap && -1 === this._modifiers.join("").indexOf("gap-") && (this._styles.gap = .5 * (this._styles.ringWidth + this._styles.progressWidth) * .005), d) {
                                e !== f ? (this._canvas.width = d * g, this._canvas.height = d * g, this._canvas.style.width = d + "px", this._canvas.style.height = d + "px", c.scale(g, g)) : (this._canvas.width = d, this._canvas.height = d), this._styles.size = .5 * d;
                                var i = this._styles.size - this._styles.padding;
                                this._styles.ringRadius = i - .5 * this._styles.ringWidth, this._styles.progressRadius = i - .5 * this._styles.progressWidth, this._styles.progressWidth === this._styles.ringWidth ? this._styles.progressRadius = this._styles.ringRadius : this._styles.progressWidth < this._styles.ringWidth ? -1 !== this._modifiers.indexOf("align-center") ? this._styles.progressRadius = this._styles.ringRadius : -1 !== this._modifiers.indexOf("align-bottom") ? this._styles.progressRadius = i - (this._styles.ringWidth - .5 * this._styles.progressWidth) : -1 !== this._modifiers.indexOf("align-inside") && (this._styles.progressRadius = i - (this._styles.ringWidth + .5 * this._styles.progressWidth)) : -1 !== this._modifiers.indexOf("align-center") ? this._styles.ringRadius = this._styles.progressRadius : -1 !== this._modifiers.indexOf("align-bottom") ? this._styles.ringRadius = i - (this._styles.progressWidth - .5 * this._styles.ringWidth) : -1 !== this._modifiers.indexOf("align-inside") && (this._styles.ringRadius = i - (this._styles.progressWidth + .5 * this._styles.ringWidth)), -1 !== this._modifiers.indexOf("glow-progress") && this._styles.progressShadow && (this._styles.progressShadow[this._styles.progressShadow.length - 1] = null !== this._styles.progressColor.gradient.colors ? this._styles.progressColor.gradient.colors[0] : this._styles.progressColor.fill), -1 !== this._modifiers.indexOf("glow-background") && this._styles.ringShadow && (this._styles.ringShadow[this._styles.ringShadow.length - 1] = null !== this._styles.ringColor.gradient.colors ? this._styles.ringColor.gradient.colors[0] : this._styles.ringColor.fill), this._drawn = !1
                            }
                        },
                        _tick: function(a) {
                            if (!this._destroyed) {
                                null !== this._target && this._draw(a);
                                var b = this;
                                window.requestAnimationFrame(function(a) {
                                    b._tick(a)
                                })
                            }
                        },
                        _draw: function(b) {
                            if (this._animate) {
                                var c = b - this._lastTick,
                                    d = 250 > c ? 1e3 / c : 30;
                                if (this._lastTick = b, this._current === this._target && this._drawn) return;
                                this._current += (this._target - this._current) / (d / 3), Math.abs(this._current - this._target) <= .001 && (this._current = this._target)
                            } else this._current = this._target;
                            var e = this._canvas.getContext("2d");
                            e.clearRect(0, 0, this._canvas.width, this._canvas.height);
                            var f = this._styles.flip ? 1 - this._current : this._current;
                            a.drawRing(e, f, this._styles.offset, this._styles.length, this._styles.gap, this._styles.size, this._styles.ringRadius, this._styles.ringWidth, this._styles.ringColor, this._styles.ringShadow, this._styles.progressRadius, this._styles.progressWidth, this._styles.progressColor, this._styles.progressShadow, this._styles.cap, this._styles.invert), this._drawn = !0
                        },
                        setValue: function(b) {
                            this._styles || this.redraw(), b = this._transform(b), this._target !== b && (null == this._target && (this._current = b), this._target = b), a.supportsAnimation() || (this._current = this._target, this._draw())
                        }
                    }, c
                }(B, E), C.Slot = function(a) {
                    var b = function(a) {
                        this._forceReplace = "undefined" == typeof a.forceReplace ? !1 : a.forceReplace, this._wrapper = document.createElement("span"), this._wrapper.className = "soon-slot " + (a.className || ""), this._transform = a.transform || function(a) {
                            return a
                        }, this._new = document.createElement("span"), this._new.className = "soon-slot-new", this._old = document.createElement("span"), this._old.className = "soon-slot-old", this._bounding = document.createElement("span"), this._bounding.className = "soon-slot-bounding", this._inner = document.createElement("span"), this._inner.className = "soon-slot-inner soon-slot-animate", this._inner.appendChild(this._old), this._inner.appendChild(this._new), this._inner.appendChild(this._bounding), this._wrapper.appendChild(this._inner), this._newValue = "", this._oldValue = "", this._boundingLength = 0
                    };
                    return b.prototype = {
                        redraw: function() {},
                        destroy: function() {
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        _isEmpty: function() {
                            return !this._newValue
                        },
                        _isSame: function(a) {
                            return this._newValue === a
                        },
                        _setBoundingForValue: function(a) {
                            var b = (a + "").length;
                            if (b !== this._boundingLength) {
                                this._boundingLength = b;
                                for (var c = "", d = 0; b > d; d++) c += "8";
                                this._bounding.textContent = c;
                                var e = parseInt(getComputedStyle(this._wrapper).fontSize, 10),
                                    f = this._bounding.offsetWidth / e;
                                this._inner.style.width = f + .1 * (b - 1) + "em"
                            }
                        },
                        _setNewValue: function(a) {
                            this._newValue = a, " " !== a && (this._new.textContent = a)
                        },
                        _setOldValue: function(a) {
                            this._oldValue = a, this._old.textContent = a
                        },
                        setValue: function(b) {
                            b = this._transform(b), this._isEmpty() ? (this._setNewValue(b), this._setBoundingForValue(b), a.triggerAnimation(this._inner, "soon-slot-animate")) : this._isSame(b) && !this._forceReplace || (this._newValue.length && this._setOldValue(this._newValue), this._setNewValue(b), this._setBoundingForValue(b), a.triggerAnimation(this._inner, "soon-slot-animate"))
                        }
                    }, b
                }(B), C.Text = function(a) {
                    var b = function(a) {
                        this._wrapper = document.createElement("span"), this._wrapper.className = "soon-text " + (a.className || ""), this._transform = a.transform || function(a) {
                            return a
                        }
                    };
                    return b.prototype = {
                        redraw: function() {},
                        destroy: function() {
                            return this._wrapper
                        },
                        getElement: function() {
                            return this._wrapper
                        },
                        setValue: function(b) {
                            a.setTextContent(this._wrapper, this._transform(b))
                        }
                    }, b
                }(B);
            var F = function(a, b) {
                    var c = function(a, c) {
                        c = c || {}, this._rate = c.rate || 1e3, this._offset = null, this._time = 0, this._paused = !1, this._nextTickReference = null, this._tickBind = this._tick.bind(this), this._onTick = a || function() {}, "addEventListener" in document && document.addEventListener(b.documentVisibilityEvent, this)
                    };
                    return c.prototype = {
                        handleEvent: function() {
                            b.isDocumentHidden() ? this._lock() : this._unlock()
                        },
                        isRunning: function() {
                            return null !== this._offset
                        },
                        isPaused: function() {
                            return this.isRunning() && this._paused
                        },
                        start: function() {
                            this.isRunning() || this.reset()
                        },
                        getTime: function() {
                            return this._time
                        },
                        reset: function() {
                            this.pause(), this._offset = (new Date).getTime(), this._time = 0, this.resume()
                        },
                        stop: function() {
                            var a = this;
                            setTimeout(function() {
                                a._clearTimer(), a._offset = null
                            }, 0)
                        },
                        pause: function() {
                            this._paused = !0, this._clearTimer()
                        },
                        resume: function() {
                            if (this.isPaused()) {
                                this._paused = !1;
                                var a = (new Date).getTime();
                                this._time += a - this._offset, this._offset = a, this._tick()
                            }
                        },
                        _clearTimer: function() {
                            clearTimeout(this._nextTickReference), this._nextTickReference = null
                        },
                        _lock: function() {
                            this._clearTimer()
                        },
                        _unlock: function() {
                            this.isPaused() || (this.pause(), this.resume())
                        },
                        _tick: function() {
                            this._onTick(this._time), this._offset += this._rate, this._time += this._rate, this._nextTickReference = setTimeout(this._tickBind, this._offset - (new Date).getTime())
                        }
                    }, c
                }(this, B),
                G = [],
                H = 0,
                I = ["xxl", "xl", "l", "m", "s", "xs", "xxs"],
                J = 3,
                K = (I.length, []),
                L = [],
                M = {
                    y: {
                        labels: "Year,Years",
                        option: "Years",
                        padding: ""
                    },
                    M: {
                        labels: "Month,Months",
                        option: "Months",
                        padding: "00"
                    },
                    w: {
                        labels: "Week,Weeks",
                        option: "Weeks",
                        padding: "00"
                    },
                    d: {
                        labels: "Day,Days",
                        option: "Days",
                        padding: "000"
                    },
                    h: {
                        labels: "Hour,Hours",
                        option: "Hours",
                        padding: "00"
                    },
                    m: {
                        labels: "Minute,Minutes",
                        option: "Minutes",
                        padding: "00"
                    },
                    s: {
                        labels: "Second,Seconds",
                        option: "Seconds",
                        padding: "00"
                    },
                    ms: {
                        labels: "Millisecond,Milliseconds",
                        option: "Milliseconds",
                        padding: "000"
                    }
                };
            E.register(d);
            var N = /([\d]+)[\s]+([a-z]+)/i,
                O = /([\d]+)[:]*([\d]{2})*[:]*([\d]{2})*/;
            A.parse = function(a) {
                w(a)
            }, A.redraw = function(a) {
                if (a) {
                    var b = j(a);
                    f(b.node, b.presenter)
                } else g()
            }, A.reset = function(a) {
                var b = j(a);
                b && b.ticker.reset()
            }, A.freeze = function(a) {
                var b = j(a);
                b && b.ticker.pause()
            }, A.unfreeze = function(a) {
                var b = j(a);
                b && b.ticker.resume()
            }, A.setOption = function(a, b, c) {
                var d = j(a);
                if (d) {
                    var e = d.options;
                    e[b] = c, this.destroy(a), this.create(a, e)
                }
            }, A.setOptions = function(a, b) {
                var c = j(a);
                if (c) {
                    var d, e = c.options;
                    for (d in b) b.hasOwnProperty(d) && (e[d] = b[d]);
                    this.destroy(a), this.create(a, b)
                }
            }, A.destroy = function(a) {
                var b = h(a);
                if (null !== b) {
                    var c = i(a);
                    null !== c && L.splice(c, 1);
                    var d = K[b];
                    d.ticker && d.ticker.stop(), d.presenter.destroy();
                    var e = d.node.querySelector(".soon-placeholder");
                    d.node.removeChild(e ? e : d.node.querySelector(".soon-group")), a.removeAttribute("data-initialized"), K.splice(b, 1)
                }
            }, A.create = function(a, b) {
                if (!b) return w(a);
                if ("true" === a.getAttribute("data-initialized")) return null;
                a.setAttribute("data-initialized", "true");
                var c = null,
                    d = null;
                b.eventComplete && (c = "string" == typeof b.eventComplete ? window[b.eventComplete] : b.eventComplete), b.eventTick && (d = "string" == typeof b.eventTick ? window[b.eventTick] : b.eventTick), b.due && -1 !== b.due.indexOf("reset") && (c = z(a, b, c), b.eventComplete = c), l(a, b, "layout"), l(a, b, "face"), l(a, b, "visual"), l(a, b, "format"), b.scaleMax && a.setAttribute("data-scale-max", b.scaleMax), b.scaleHide && a.setAttribute("data-scale-hide", b.scaleHide);
                var e, f, g, h = (b.format || "d,h,m,s").split(","),
                    i = -1 === h.indexOf("ms") ? 1e3 : 24,
                    j = {};
                for (e in M) M.hasOwnProperty(e) && (f = M[e], g = (b["labels" + f.option] || f.labels).split(","), j[e] = g[0], j[e + "_s"] = g[1] || g[0]);
                var m = "undefined" == typeof b.padding ? !0 : b.padding,
                    n = {};
                for (e in M) M.hasOwnProperty(e) && (f = M[e], m ? (n[e] = y(e, h), null === n[e] && (n[e] = f.padding), b["padding" + f.option] && (n[e] = b["padding" + f.option])) : n[e] = "");
                var p = (b.face || "text ").split(" ")[0],
                    q = b.due ? x(b.due) : null,
                    r = b.since ? B.isoToDate(b.since) : null,
                    s = b.now ? B.isoToDate(b.now) : r ? null : new Date,
                    t = {
                        due: q,
                        since: r,
                        now: s,
                        view: p,
                        face: b.face,
                        visual: b.visual || null,
                        format: h,
                        separator: b.separator || null,
                        cascade: "undefined" == typeof b.cascade ? !0 : B.toBoolean(b.cascade),
                        singular: b.singular,
                        reflect: b.reflect || !1,
                        chars: "undefined" == typeof b.separateChars ? !0 : B.toBoolean(b.separateChars),
                        label: j,
                        padding: n,
                        callback: {
                            onComplete: c || function() {},
                            onTick: d || function() {}
                        }
                    };
                B.isSlow() && (t.view = "text", t.reflect = !1, t.visual = null);
                var A = null,
                    C = o(t, function() {
                        A && A.stop(), t.callback.onComplete()
                    });
                k(a);
                var D = u(a, C);
                return A = v(a, D, i, b)
            };
            var P;
            ! function(a) {
                P = a()
            }(function(a) {
                function b(a) {
                    for (n = 1; a = d.shift();) a()
                }
                var c, d = [],
                    e = !1,
                    f = document,
                    g = f.documentElement,
                    h = g.doScroll,
                    i = "DOMContentLoaded",
                    j = "addEventListener",
                    k = "onreadystatechange",
                    l = "readyState",
                    m = h ? /^loaded|^c/ : /^loaded|c/,
                    n = m.test(f[l]);
                return f[j] && f[j](i, c = function() {
                    f.removeEventListener(i, c, e), b()
                }, e), h && f.attachEvent(k, c = function() {
                    /^c/.test(f[l]) && (f.detachEvent(k, c), b())
                }), a = h ? function(b) {
                    self != top ? n ? b() : d.push(b) : function() {
                        try {
                            g.doScroll("left")
                        } catch (c) {
                            return setTimeout(function() {
                                a(b)
                            }, 50)
                        }
                        b()
                    }()
                } : function(a) {
                    n ? a() : d.push(a)
                }
            }), P(function() {
                    var a = document.querySelector("script[src*=soon]");
                    if (!a || "false" !== a.getAttribute("data-auto"))
                        for (var b = document.getElementsByClassName ? document.getElementsByClassName("soon") : document.querySelectorAll(".soon"), c = 0, d = b.length; d > c; c++) w(b[c])
                }),
                function(a, b) {
                    "use strict";
                    if (b) {
                        var c = ["destroy", "reset", "resize", "freeze", "unfreeze", "redraw"],
                            d = c.length;
                        b.fn.soon = function() {
                            var b = this;
                            b.create = function(b) {
                                return this.each(function() {
                                    a.create(this, b)
                                })
                            }, b.setOption = function(b, c) {
                                return this.each(function() {
                                    a.setOption(this, b, c)
                                })
                            }, b.setOptions = function(b) {
                                return this.each(function() {
                                    a.setOptions(this, b)
                                })
                            };
                            for (var e = 0; d > e; e++) ! function(c) {
                                b[c] = function() {
                                    return this.each(function() {
                                        a[c](this)
                                    })
                                }
                            }(c[e]);
                            return this
                        }
                    }
                }(A, b), "undefined" != typeof module && module.exports ? module.exports = A : "function" == typeof define && define.amd ? define(function() {
                    return A
                }) : a.Soon = A
        }
    }(window, window.jQuery); 
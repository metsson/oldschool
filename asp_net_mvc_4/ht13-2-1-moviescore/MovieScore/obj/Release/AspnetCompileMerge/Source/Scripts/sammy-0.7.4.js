﻿// -- Sammy.js -- /sammy.js
// http://sammyjs.org
// Version: 0.7.4
// Built: 2013-01-27 13:34:16 -0500
// name: sammy
// version: 0.7.4
// Sammy.js / http://sammyjs.org
(function (e, t) {
    (function (n) {
        typeof define == "function" && define.amd ? define(["jquery"], n) : e.sammy = t.Sammy = n(e)
    })(function (e) {
        var n, r = "([^/]+)",
            i = /:([\w\d]+)/g,
            s = /\?([^#]*)?$/,
            o = function (e) {
                return Array.prototype.slice.call(e)
            }, u = function (e) {
                return Object.prototype.toString.call(e) === "[object Function]"
            }, a = function (e) {
                return Object.prototype.toString.call(e) === "[object Array]"
            }, f = function (e) {
                return Object.prototype.toString.call(e) === "[object RegExp]"
            }, l = function (e) {
                return decodeURIComponent((e || "").replace(/\+/g, " "))
            }, c = encodeURIComponent,
            h = function (e) {
                return String(e).replace(/&(?!\w+;)/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;")
            }, p = function (e) {
                return function () {
                    return this.route.apply(this, [e].concat(Array.prototype.slice.call(arguments)))
                }
            }, d = {}, v = !!t.history && !!history.pushState,
            m = [];
        return n = function () {
            var t = o(arguments),
                r, i;
            n.apps = n.apps || {};
            if (t.length === 0 || t[0] && u(t[0])) return n.apply(n, ["body"].concat(t));
            if (typeof (i = t.shift()) == "string") return r = n.apps[i] || new n.Application, r.element_selector = i, t.length > 0 && e.each(t, function (e, t) {
                r.use(t)
            }), r.element_selector != i && delete n.apps[i], n.apps[r.element_selector] = r, r
        }, n.VERSION = "0.7.4", n.addLogger = function (e) {
            m.push(e)
        }, n.log = function () {
            var t = o(arguments);
            t.unshift("[" + Date() + "]"), e.each(m, function (e, r) {
                r.apply(n, t)
            })
        }, typeof t.console != "undefined" ? u(t.console.log.apply) ? n.addLogger(function () {
            t.console.log.apply(t.console, arguments)
        }) : n.addLogger(function () {
            t.console.log(arguments)
        }) : typeof console != "undefined" && n.addLogger(function () {
            console.log.apply(console, arguments)
        }), e.extend(n, {
            makeArray: o,
            isFunction: u,
            isArray: a
        }), n.Object = function (t) {
            return e.extend(this, t || {})
        }, e.extend(n.Object.prototype, {
            escapeHTML: h,
            h: h,
            toHash: function () {
                var t = {};
                return e.each(this, function (e, n) {
                    u(n) || (t[e] = n)
                }), t
            },
            toHTML: function () {
                var t = "";
                return e.each(this, function (e, n) {
                    u(n) || (t += "<strong>" + e + "</strong> " + n + "<br />")
                }), t
            },
            keys: function (e) {
                var t = [];
                for (var n in this) (!u(this[n]) || !e) && t.push(n);
                return t
            },
            has: function (t) {
                return this[t] && e.trim(this[t].toString()) !== ""
            },
            join: function () {
                var e = o(arguments),
                    t = e.shift();
                return e.join(t)
            },
            log: function () {
                n.log.apply(n, arguments)
            },
            toString: function (t) {
                var n = [];
                return e.each(this, function (e, r) {
                    (!u(r) || t) && n.push('"' + e + '": ' + r.toString())
                }), "Sammy.Object: {" + n.join(",") + "}"
            }
        }), n.targetIsThisWindow = function (r) {
            var i = e(r.target).attr("target");
            return !i || i === t.name || i === "_self" ? !0 : i === "_blank" ? !1 : i === "top" && t === t.top ? !0 : !1
        }, n.DefaultLocationProxy = function (e, t) {
            this.app = e, this.is_native = !1, this.has_history = v, this._startPolling(t)
        }, n.DefaultLocationProxy.fullPath = function (e) {
            var t = e.toString().match(/^[^#]*(#.+)$/),
                n = t ? t[1] : "";
            return [e.pathname, e.search, n].join("")
        }, e.extend(n.DefaultLocationProxy.prototype, {
            bind: function () {
                var r = this,
                    i = this.app,
                    s = n.DefaultLocationProxy;
                e(t).bind("hashchange." + this.app.eventNamespace(), function (e, n) {
                    r.is_native === !1 && !n && (r.is_native = !0, t.clearInterval(s._interval), s._interval = null), i.trigger("location-changed")
                }), v && !i.disable_push_state && (e(t).bind("popstate." + this.app.eventNamespace(), function (e) {
                    i.trigger("location-changed")
                }), e(document).delegate("a", "click.history-" + this.app.eventNamespace(), function (e) {
                    if (e.isDefaultPrevented() || e.metaKey || e.ctrlKey) return;
                    var o = s.fullPath(this);
                    if (this.hostname == t.location.hostname && i.lookupRoute("get", o) && n.targetIsThisWindow(e)) return e.preventDefault(), r.setLocation(o), !1
                })), s._bindings || (s._bindings = 0), s._bindings++
            },
            unbind: function () {
                e(t).unbind("hashchange." + this.app.eventNamespace()), e(t).unbind("popstate." + this.app.eventNamespace()), e(document).undelegate("a", "click.history-" + this.app.eventNamespace()), n.DefaultLocationProxy._bindings--, n.DefaultLocationProxy._bindings <= 0 && (t.clearInterval(n.DefaultLocationProxy._interval), n.DefaultLocationProxy._interval = null)
            },
            getLocation: function () {
                return n.DefaultLocationProxy.fullPath(t.location)
            },
            setLocation: function (e) {
                /^([^#\/]|$)/.test(e) && (v && !this.app.disable_push_state ? e = "/" + e : e = "#!/" + e);
                if (e != this.getLocation()) {
                    if (!(v && !this.app.disable_push_state && /^\//.test(e))) return t.location = e;
                    history.pushState({
                        path: e
                    }, t.title, e), this.app.trigger("location-changed")
                }
            },
            _startPolling: function (r) {
                var i = this;
                if (!n.DefaultLocationProxy._interval) {
                    r || (r = 10);
                    var s = function () {
                        var r = i.getLocation();
                        (typeof n.DefaultLocationProxy._last_location == "undefined" || r != n.DefaultLocationProxy._last_location) && t.setTimeout(function () {
                            e(t).trigger("hashchange", [!0])
                        }, 0), n.DefaultLocationProxy._last_location = r
                    };
                    s(), n.DefaultLocationProxy._interval = t.setInterval(s, r)
                }
            }
        }), n.Application = function (e) {
            var t = this;
            this.routes = {}, this.listeners = new n.Object({}), this.arounds = [], this.befores = [], this.namespace = (new Date).getTime() + "-" + parseInt(Math.random() * 1e3, 10), this.context_prototype = function () {
                n.EventContext.apply(this, arguments)
            }, this.context_prototype.prototype = new n.EventContext, u(e) && e.apply(this, [this]), this._location_proxy || this.setLocationProxy(new n.DefaultLocationProxy(this, this.run_interval_every)), this.debug && this.bindToAllEvents(function (e, n) {
                t.log(t.toString(), e.cleaned_type, n || {})
            })
        }, n.Application.prototype = e.extend({}, n.Object.prototype, {
            ROUTE_VERBS: ["get", "post", "put", "delete"],
            APP_EVENTS: ["run", "unload", "lookup-route", "run-route", "route-found", "event-context-before", "event-context-after", "changed", "error", "check-form-submission", "redirect", "location-changed"],
            _last_route: null,
            _location_proxy: null,
            _running: !1,
            element_selector: "body",
            debug: !1,
            raise_errors: !1,
            run_interval_every: 50,
            disable_push_state: !1,
            template_engine: null,
            toString: function () {
                return "Sammy.Application:" + this.element_selector
            },
            $element: function (t) {
                return t ? e(this.element_selector).find(t) : e(this.element_selector)
            },
            use: function () {
                var e = o(arguments),
                    t = e.shift(),
                    r = t || "";
                try {
                    e.unshift(this), typeof t == "string" && (r = "Sammy." + t, t = n[t]), t.apply(this, e)
                } catch (i) {
                    typeof t == "undefined" ? this.error("Plugin Error: called use() but plugin (" + r.toString() + ") is not defined", i) : u(t) ? this.error("Plugin Error", i) : this.error("Plugin Error: called use() but '" + r.toString() + "' is not a function", i)
                }
                return this
            },
            setLocationProxy: function (e) {
                var t = this._location_proxy;
                this._location_proxy = e, this.isRunning() && (t && t.unbind(), this._location_proxy.bind())
            },
            log: function () {
                n.log.apply(n, Array.prototype.concat.apply([this.element_selector], arguments))
            },
            route: function (t, n) {
                var s = this,
                    o = [],
                    a, f, l = Array.prototype.slice.call(arguments, 2);
                l.length === 0 && u(n) && (n = t, l = [n], t = "any"), t = t.toLowerCase();
                if (n.constructor == String) {
                    i.lastIndex = 0;
                    while ((f = i.exec(n)) !== null) o.push(f[1]);
                    n = new RegExp(n.replace(i, r) + "$")
                }
                return e.each(l, function (e, t) {
                    typeof t == "string" && (l[e] = s[t])
                }), a = function (e) {
                    var t = {
                        verb: e,
                        path: n,
                        callback: l,
                        param_names: o
                    };
                    s.routes[e] = s.routes[e] || [], s.routes[e].push(t)
                }, t === "any" ? e.each(this.ROUTE_VERBS, function (e, t) {
                    a(t)
                }) : a(t), this
            },
            get: p("get"),
            post: p("post"),
            put: p("put"),
            del: p("delete"),
            any: p("any"),
            mapRoutes: function (t) {
                var n = this;
                return e.each(t, function (e, t) {
                    n.route.apply(n, t)
                }), this
            },
            eventNamespace: function () {
                return ["sammy-app", this.namespace].join("-")
            },
            bind: function (e, t, n) {
                var r = this;
                typeof n == "undefined" && (n = t);
                var i = function () {
                    var e, t, i;
                    e = arguments[0], i = arguments[1], i && i.context ? (t = i.context, delete i.context) : t = new r.context_prototype(r, "bind", e.type, i, e.target), e.cleaned_type = e.type.replace(r.eventNamespace(), ""), n.apply(t, [e, i])
                };
                return this.listeners[e] || (this.listeners[e] = []), this.listeners[e].push(i), this.isRunning() && this._listen(e, i), this
            },
            trigger: function (e, t) {
                return this.$element().trigger([e, this.eventNamespace()].join("."), [t]), this
            },
            refresh: function () {
                return this.last_location = null, this.trigger("location-changed"), this
            },
            before: function (e, t) {
                return u(e) && (t = e, e = {}), this.befores.push([e, t]), this
            },
            after: function (e) {
                return this.bind("event-context-after", e)
            },
            around: function (e) {
                return this.arounds.push(e), this
            },
            onComplete: function (e) {
                return this._onComplete = e, this
            },
            isRunning: function () {
                return this._running
            },
            helpers: function (t) {
                return e.extend(this.context_prototype.prototype, t), this
            },
            helper: function (e, t) {
                return this.context_prototype.prototype[e] = t, this
            },
            run: function (r) {
                if (this.isRunning()) return !1;
                var i = this;
                return e.each(this.listeners.toHash(), function (t, n) {
                    e.each(n, function (e, n) {
                        i._listen(t, n)
                    })
                }), this.trigger("run", {
                    start_url: r
                }), this._running = !0, this.last_location = null, !/\#(.+)/.test(this.getLocation()) && typeof r != "undefined" && this.setLocation(r), this._checkLocation(), this._location_proxy.bind(), this.bind("location-changed", function () {
                    i._checkLocation()
                }), this.bind("submit", function (t) {
                    if (!n.targetIsThisWindow(t)) return !0;
                    var r = i._checkFormSubmission(e(t.target).closest("form"));
                    return r === !1 ? t.preventDefault() : !1
                }), e(t).bind("unload", function () {
                    i.unload()
                }), this.trigger("changed")
            },
            unload: function () {
                if (!this.isRunning()) return !1;
                var t = this;
                return this.trigger("unload"), this._location_proxy.unbind(), this.$element().unbind("submit").removeClass(t.eventNamespace()), e.each(this.listeners.toHash(), function (n, r) {
                    e.each(r, function (e, r) {
                        t._unlisten(n, r)
                    })
                }), this._running = !1, this
            },
            destroy: function () {
                return this.unload(), delete n.apps[this.element_selector], this
            },
            bindToAllEvents: function (t) {
                var n = this;
                return e.each(this.APP_EVENTS, function (e, r) {
                    n.bind(r, t)
                }), e.each(this.listeners.keys(!0), function (r, i) {
                    e.inArray(i, n.APP_EVENTS) == -1 && n.bind(i, t)
                }), this
            },
            routablePath: function (e) {
                return e.replace(s, "")
            },
            lookupRoute: function (e, t) {
                var n = this,
                    r = !1,
                    i = 0,
                    s, o;
                if (typeof this.routes[e] != "undefined") {
                    s = this.routes[e].length;
                    for (; i < s; i++) {
                        o = this.routes[e][i];
                        if (n.routablePath(t).match(o.path)) {
                            r = o;
                            break
                        }
                    }
                }
                return r
            },
            runRoute: function (t, n, r, i) {
                var s = this,
                    o = this.lookupRoute(t, n),
                    u, a, f, c, h, p, d, v, m;
                this.debug && this.log("runRoute", [t, n].join(" ")), this.trigger("run-route", {
                    verb: t,
                    path: n,
                    params: r
                }), typeof r == "undefined" && (r = {}), e.extend(r, this._parseQueryString(n));
                if (o) {
                    this.trigger("route-found", {
                        route: o
                    }), (v = o.path.exec(this.routablePath(n))) !== null && (v.shift(), e.each(v, function (e, t) {
                        o.param_names[e] ? r[o.param_names[e]] = l(t) : (r.splat || (r.splat = []), r.splat.push(l(t)))
                    })), u = new this.context_prototype(this, t, n, r, i), f = this.arounds.slice(0), h = this.befores.slice(0), d = [u], r.splat && (d = d.concat(r.splat)), a = function () {
                        var e, t, n;
                        while (h.length > 0) {
                            p = h.shift();
                            if (s.contextMatchesOptions(u, p[0])) {
                                e = p[1].apply(u, [u]);
                                if (e === !1) return !1
                            }
                        }
                        return s.last_route = o, u.trigger("event-context-before", {
                            context: u
                        }), typeof o.callback == "function" && (o.callback = [o.callback]), o.callback && o.callback.length && (t = -1, n = function () {
                            t++, o.callback[t] ? e = o.callback[t].apply(u, d) : s._onComplete && typeof (s._onComplete === "function") && s._onComplete(u)
                        }, d.push(n), n()), u.trigger("event-context-after", {
                            context: u
                        }), e
                    }, e.each(f.reverse(), function (e, t) {
                        var n = a;
                        a = function () {
                            return t.apply(u, [n])
                        }
                    });
                    try {
                        m = a()
                    } catch (g) {
                        this.error(["500 Error", t, n].join(" "), g)
                    }
                    return m
                }
                return this.notFound(t, n)
            },
            contextMatchesOptions: function (t, n, r) {
                var i = n;
                if (typeof i == "string" || f(i)) i = {
                    path: i
                };
                typeof r == "undefined" && (r = !0);
                if (e.isEmptyObject(i)) return !0;
                if (a(i.path)) {
                    var s, o, u, l;
                    s = [];
                    for (o = 0, l = i.path.length; o < l; o += 1) u = e.extend({}, i, {
                        path: i.path[o]
                    }), s.push(this.contextMatchesOptions(t, u));
                    var c = e.inArray(!0, s) > -1 ? !0 : !1;
                    return r ? c : !c
                }
                if (i.only) return this.contextMatchesOptions(t, i.only, !0);
                if (i.except) return this.contextMatchesOptions(t, i.except, !1);
                var h = !0,
                    p = !0;
                return i.path && (f(i.path) || (i.path = new RegExp(i.path.toString() + "$")), h = i.path.test(t.path)), i.verb && (typeof i.verb == "string" ? p = i.verb === t.verb : p = i.verb.indexOf(t.verb) > -1), r ? p && h : !p || !h
            },
            getLocation: function () {
                return this._location_proxy.getLocation()
            },
            setLocation: function (e) {
                return this._location_proxy.setLocation(e)
            },
            swap: function (e, t) {
                var n = this.$element().html(e);
                return u(t) && t(e), n
            },
            templateCache: function (e, t) {
                return typeof t != "undefined" ? d[e] = t : d[e]
            },
            clearTemplateCache: function () {
                return d = {}
            },
            notFound: function (e, t) {
                var n = this.error(["404 Not Found", e, t].join(" "));
                return e === "get" ? n : !0
            },
            error: function (e, t) {
                t || (t = new Error), t.message = [e, t.message].join(" "), this.trigger("error", {
                    message: t.message,
                    error: t
                });
                if (this.raise_errors) throw t;
                this.log(t.message, t)
            },
            _checkLocation: function () {
                var e, t;
                e = this.getLocation();
                if (!this.last_location || this.last_location[0] != "get" || this.last_location[1] != e) this.last_location = ["get", e], t = this.runRoute("get", e);
                return t
            },
            _getFormVerb: function (t) {
                var n = e(t),
                    r, i;
                i = n.find('input[name="_method"]'), i.length > 0 && (r = i.val()), r || (r = n[0].getAttribute("method"));
                if (!r || r === "") r = "get";
                return e.trim(r.toString().toLowerCase())
            },
            _checkFormSubmission: function (t) {
                var n, r, i, s, o;
                return this.trigger("check-form-submission", {
                    form: t
                }), n = e(t), r = n.attr("action") || "", i = this._getFormVerb(n), this.debug && this.log("_checkFormSubmission", n, r, i), i === "get" ? (s = this._serializeFormParams(n), s !== "" && (r += "?" + s), this.setLocation(r), o = !1) : (s = e.extend({}, this._parseFormParams(n)), o = this.runRoute(i, r, s, t.get(0))), typeof o == "undefined" ? !1 : o
            },
            _serializeFormParams: function (e) {
                var t = "",
                    n = e.serializeArray(),
                    r;
                if (n.length > 0) {
                    t = this._encodeFormPair(n[0].name, n[0].value);
                    for (r = 1; r < n.length; r++) t = t + "&" + this._encodeFormPair(n[r].name, n[r].value)
                }
                return t
            },
            _encodeFormPair: function (e, t) {
                return c(e) + "=" + c(t)
            },
            _parseFormParams: function (e) {
                var t = {}, n = e.serializeArray(),
                    r;
                for (r = 0; r < n.length; r++) t = this._parseParamPair(t, n[r].name, n[r].value);
                return t
            },
            _parseQueryString: function (e) {
                var t = {}, n, r, i, o;
                n = e.match(s);
                if (n && n[1]) {
                    r = n[1].split("&");
                    for (o = 0; o < r.length; o++) i = r[o].split("="), t = this._parseParamPair(t, l(i[0]), l(i[1] || ""))
                }
                return t
            },
            _parseParamPair: function (e, t, n) {
                return typeof e[t] != "undefined" ? a(e[t]) ? e[t].push(n) : e[t] = [e[t], n] : e[t] = n, e
            },
            _listen: function (e, t) {
                return this.$element().bind([e, this.eventNamespace()].join("."), t)
            },
            _unlisten: function (e, t) {
                return this.$element().unbind([e, this.eventNamespace()].join("."), t)
            }
        }), n.RenderContext = function (e) {
            this.event_context = e, this.callbacks = [], this.previous_content = null, this.content = null, this.next_engine = !1, this.waiting = !1
        }, n.RenderContext.prototype = e.extend({}, n.Object.prototype, {
            then: function (e) {
                if (!u(e)) {
                    if (!(typeof e == "string" && e in this.event_context)) return this;
                    var n = this.event_context[e];
                    e = function (e) {
                        return n.apply(this.event_context, [e])
                    }
                }
                var r = this;
                return this.waiting ? this.callbacks.push(e) : (this.wait(), t.setTimeout(function () {
                    var t = e.apply(r, [r.content, r.previous_content]);
                    t !== !1 && r.next(t)
                }, 0)), this
            },
            wait: function () {
                this.waiting = !0
            },
            next: function (e) {
                this.waiting = !1, typeof e != "undefined" && (this.previous_content = this.content, this.content = e), this.callbacks.length > 0 && this.then(this.callbacks.shift())
            },
            load: function (t, n, r) {
                var i = this;
                return this.then(function () {
                    var s, o, a, f;
                    u(n) ? (r = n, n = {}) : n = e.extend({}, n), r && this.then(r);
                    if (typeof t == "string") return a = t.match(/\.json$/) || n.json, s = a ? n.cache === !0 : n.cache !== !1, i.next_engine = i.event_context.engineFor(t), delete n.cache, delete n.json, n.engine && (i.next_engine = n.engine, delete n.engine), s && (o = this.event_context.app.templateCache(t)) ? o : (this.wait(), e.ajax(e.extend({
                        url: t,
                        data: {},
                        dataType: a ? "json" : "text",
                        type: "get",
                        success: function (e) {
                            s && i.event_context.app.templateCache(t, e), i.next(e)                            
                        },
                        error: function (e) {
                            // Redirect to SPA startpage
                            document.location.href = "#";
                            if ($("#content").length !== 0) {
                                $("#content").empty();
                            }
                            toastr.error("The page you were looking for, is gone.", "a.k.a 404");

                        }
                    }, n)), !1);
                    if (t.nodeType) return t.innerHTML;
                    if (t.selector) return i.next_engine = t.attr("data-engine"), n.clone === !1 ? t.remove()[0].innerHTML.toString() : t[0].innerHTML.toString()
                })
            },
            loadPartials: function (e) {
                var t;
                if (e) {
                    this.partials = this.partials || {};
                    for (t in e) (function (t, n) {
                        t.load(e[n]).then(function (e) {
                            this.partials[n] = e
                        })
                    })(this, t)
                }
                return this
            },
            render: function (e, t, n, r) {
                return u(e) && !t ? this.then(e) : (u(t) ? (r = n, n = t, t = null) : n && !u(n) && (r = n, n = null), this.loadPartials(r).load(e).interpolate(t, e).then(n))
            },
            partial: function (e, t, n, r) {
                return u(n) ? this.render(e, t, r).swap(n) : u(t) ? this.render(e, {}, n).swap(t) : this.render(e, t, n).swap()
            },
            send: function () {
                var e = this,
                    t = o(arguments),
                    n = t.shift();
                return a(t[0]) && (t = t[0]), this.then(function (r) {
                    return t.push(function (t) {
                        e.next(t)
                    }), e.wait(), n.apply(n, t), !1
                })
            },
            collect: function (t, n, r) {
                var i = this,
                    s = function () {
                        u(t) && (n = t, t = this.content);
                        var r = [],
                            s = !1;
                        return e.each(t, function (e, t) {
                            var o = n.apply(i, [e, t]);
                            return o.jquery && o.length == 1 && (o = o[0], s = !0), r.push(o), o
                        }), s ? r : r.join("")
                    };
                return r ? s() : this.then(s)
            },
            renderEach: function (t, n, r, i) {
                return a(n) && (i = r, r = n, n = null), this.load(t).then(function (s) {
                    var o = this;
                    r || (r = a(this.previous_content) ? this.previous_content : []);
                    if (!i) return this.collect(r, function (e, r) {
                        var i = {}, o = this.next_engine || t;
                        return n ? i[n] = r : i = r, this.event_context.interpolate(s, i, o)
                    }, !0);
                    e.each(r, function (e, r) {
                        var u = {}, a = this.next_engine || t;
                        n ? u[n] = r : u = r, i(r, o.event_context.interpolate(s, u, a))
                    })
                })
            },
            interpolate: function (e, t, n) {
                var r = this;
                return this.then(function (i, s) {
                    !e && s && (e = s), this.next_engine && (t = this.next_engine, this.next_engine = !1);
                    var o = r.event_context.interpolate(i, e, t, this.partials);
                    return n ? s + o : o
                })
            },
            swap: function (e) {
                return this.then(function (t) {
                    return this.event_context.swap(t, e), t
                }).trigger("changed", {})
            },
            appendTo: function (t) {
                return this.then(function (n) {
                    e(t).append(n)
                }).trigger("changed", {})
            },
            prependTo: function (t) {
                return this.then(function (n) {
                    e(t).prepend(n)
                }).trigger("changed", {})
            },
            replace: function (t) {
                return this.then(function (n) {
                    e(t).html(n)
                }).trigger("changed", {})
            },
            trigger: function (e, t) {
                return this.then(function (n) {
                    return typeof t == "undefined" && (t = {
                        content: n
                    }), this.event_context.trigger(e, t), n
                })
            }
        }), n.EventContext = function (e, t, r, i, s) {
            this.app = e, this.verb = t, this.path = r, this.params = new n.Object(i), this.target = s
        }, n.EventContext.prototype = e.extend({}, n.Object.prototype, {
            $element: function () {
                return this.app.$element(o(arguments).shift())
            },
            engineFor: function (e) {
                var t = this,
                    n;
                if (u(e)) return e;
                e = (e || t.app.template_engine).toString();
                if (n = e.match(/\.([^\.\?\#]+)$/)) e = n[1];
                return e && u(t[e]) ? t[e] : t.app.template_engine ? this.engineFor(t.app.template_engine) : function (e, t) {
                    return e
                }
            },
            interpolate: function (e, t, n, r) {
                return this.engineFor(n).apply(this, [e, t, r])
            },
            render: function (e, t, r, i) {
                return (new n.RenderContext(this)).render(e, t, r, i)
            },
            renderEach: function (e, t, r, i) {
                return (new n.RenderContext(this)).renderEach(e, t, r, i)
            },
            load: function (e, t, r) {
                return (new n.RenderContext(this)).load(e, t, r)
            },
            loadPartials: function (e) {
                return (new n.RenderContext(this)).loadPartials(e)
            },
            partial: function (e, t, r, i) {                
                return (new n.RenderContext(this)).partial(e, t, r, i)
            },
            send: function () {
                var e = new n.RenderContext(this);
                return e.send.apply(e, arguments)
            },
            redirect: function () {
                var t, n = o(arguments),
                    r = this.app.getLocation(),
                    i = n.length;
                if (i > 1) {
                    var s = 0,
                        u = [],
                        a = [],
                        f = {}, l = !1;
                    for (; s < i; s++) typeof n[s] == "string" ? u.push(n[s]) : (e.extend(f, n[s]), l = !0);
                    t = u.join("/");
                    if (l) {
                        for (var c in f) a.push(this.app._encodeFormPair(c, f[c]));
                        t += "?" + a.join("&")
                    }
                } else t = n[0];
                this.trigger("redirect", {
                    to: t
                }), this.app.last_location = [this.verb, this.path], this.app.setLocation(t), (new RegExp(t)).test(r) && this.app.trigger("location-changed")
            },
            trigger: function (e, t) {
                return typeof t == "undefined" && (t = {}), t.context || (t.context = this), this.app.trigger(e, t)
            },
            eventNamespace: function () {
                return this.app.eventNamespace()
            },
            swap: function (e, t) {
                return this.app.swap(e, t)
            },
            notFound: function () {
                return this.app.notFound(this.verb, this.path)
            },
            json: function (t) {
                return e.parseJSON(t)
            },
            toString: function () {
                return "Sammy.EventContext: " + [this.verb, this.path, this.params].join(" ")
            }
        }), n
    })
})(jQuery, window);
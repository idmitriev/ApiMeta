(function () {
    var root = this;
    var _require = function(moduleName) {
        var module = root[moduleName];
        if (!module && (typeof require !== 'undefined'))
            module = require('underscore');
        return module;
    };

    var _ = _require('_');
    var $ = _require('$');
    var Backbone = _require('Backbone');
    var Showdown = _require('Showdown');
    var NestedView = _require('NestedView');
    NestedView.getView = function (name) {
        return apidocs.views[name]; 
    };

    var apidocs;
    if (typeof exports !== 'undefined') 
        apidocs = exports;
    else 
        apidocs = root.apidocs = {};

    apidocs.views = apidocs.views || {};
    apidocs.models = apidocs.models || {};
    apidocs.data = apidocs.data || {};
    apidocs.views.helpers = apidocs.views.helpers || {};
    apidocs.settings = apidocs.settings || {};

    apidocs.settings.editable = true;
    apidocs.settings.scrollOffset = -60;

    apidocs.localization = apidocs.localization || {};

    apidocs.views.helpers.typeLink = function (type) {
        var elementType = type.replace("[]", "");
        var data = {
            type: type,
            elementType: elementType,
            arrayBrackets: elementType != type ? "[]" : ""
        };

        var hasDocs = _.memoize(function (docId) {
            return apidocs.data.getType(docId) != undefined;
        });

        if (hasDocs(elementType))
            return _.template('<a class="type" href="#<%= elementType %>"><%= elementType %></a><%= arrayBrackets %>', data);
        return _.template('<span class="type"><%= type %></span>', data);
    };
    
    apidocs.views.helpers.d = function (key) {
        return apidocs.localization[key] || key;
    };
    apidocs.views.helpers.doc = function (docId) {
        return NestedView.viewHelper.apply(this, ['Doc', { model: new apidocs.models.Doc(apidocs.data.getArticle(docId)) }]);
    };
    
    apidocs.views.helpers.scrollTo = function (id) {
        var target = $(id);
        if (target.length > 0) {
            target[0].scrollIntoView();
            scrollBy(0, apidocs.settings.scrollOffset);
        }
    };
    
    apidocs.View = NestedView.extend({
        initialize: function (options) {
            _.extend(this, options);
            this.childViews = {};
            this.postInitialize();

            this.name = Math.random().toString(36).substring(10);
        },
        decorateTemplateData: function (data) {
            data = _.clone(data);

            if (this.data)
                _.extend(data, this.data);

            data._view = this;

            if (this.extendTemplateData)
                data = this.extendTemplateData(data);

            return _.extend(data, {
                typeLink: apidocs.views.helpers.typeLink,
                d: apidocs.views.helpers.d,
                doc: apidocs.views.helpers.doc,
            });
        },
    });

    apidocs.views.LinkList = apidocs.View.extend({
        items: [],
        title: 'Title',
        template: _.template($('#link-list-template').html()),
        render: function () {
            this.$el.html(this.template({
                title: this.title,
                items: this.items,
                d: apidocs.views.helpers.d
            }));
            return this;
        },
    });

    apidocs.views.Resource = apidocs.View.extend({
        data: {},
        tagName: "section",
        className: "resource",
        template: _.template($('#resource-view-template').html()),
        events: {
            "click hgroup": "toggleRequestDetails"
        },
        toggleRequestDetails: function (sender) {
            var requestNode = $(sender.target).parent();

            $('.details', requestNode).toggle();
            $('hgroup.alt', requestNode).toggle();
        }
    });

    apidocs.views.AppView = Backbone.View.extend({
        el: $('body'),
        template: _.template($('#app-view-template').html()),
        render: function () {
            this.$el.html(this.template({}));
            this.renderNavigation().renderContents();
        },
        getSections: function () {
            return {
                "resources": _.map(apidocs.data.resources, function (resource) {
                    return resource.docId || resource.name;
                }),
                "types": _.map(apidocs.data.types, function (type) {
                    return type.docId || type.name;
                }),
            };
        },
        renderNavigation: function () {
            var sections = this.getSections();

            _.chain(sections).keys().each(function (section) {
                new apidocs.views.LinkList({
                    el: this.$('section.' + section),
                    title: apidocs.localization[section] || section,
                    items: sections[section]
                }).render();
            });

            return this;
        },
        renderContents: function () {
            var sections = this.getSections();
            var elements = _.chain(sections).keys().map(function (section) {
                if (section == "resources") {
                    return _.map(sections[section], function (resource) {
                        return new apidocs.views.Resource({ data: apidocs.data.getResource(resource) }).render().$el;
                    });
                } else if (section == "types") {
                    return _.map(sections[section], function (type) {
                        return new apidocs.views.Type({ data: apidocs.data.getType(type) }).render().$el;
                    });
                }
                return [];
            }).flatten().value();

            _.each(elements, function (element) {
                this.$('.content').append(element);
            });

            this.$el.scrollspy({ offset: -apidocs.settings.scrollOffset });

            apidocs.views.helpers.scrollTo(window.location.hash);

            return this;
        },

        hotkey: function (event) {
            if ((/textarea|select/i.test(event.target.nodeName) || event.target.type === "text"))
                return;

            if (event && event.keyCode && event.altKey && event.keyCode == 119) //F8
                this.toggleEditMode();
        },
        toggleEditMode: function () {
            apidocs.settings.editable = !apidocs.settings.editable;
            this.render();
        }
    });

    apidocs.views.Doc = apidocs.View.extend({
        tagName: "article",
        template: _.template($('#doc-view-template').html()),
        extendTemplateData: function (data) {
            return _.extend(data, this.model.toJSON(), {
                html: (new Showdown.converter()).makeHtml(this.model.get("text"))
            });
        },
        postRender: function () {
            this.$el.attr('id', this.model.get('id').replace(/\s/g, '_'));

            var model = this.model;
            var view = this;

            if (apidocs.settings.editable)
                this.$el.editable(function (value, settings) { return (value); }, {
                    type: 'autogrow',
                    submit: 'ok',
                    event: 'dblclick',
                    data: function (value, settings) { return model.get('text'); },
                    callback: function (value, settings) { model.set('text', value); model.save(); view.render(); }
                });
            return this;
        }
    });

    apidocs.views.Article = apidocs.View.extend({
        tagName: "section",
        className: "article",
        template: _.template($('#article-view-template').html()),
    });

    apidocs.views.Type = apidocs.View.extend({
        tagName: "section",
        template: _.template($('#type-view-template').html()),
    });

    apidocs.models.Doc = Backbone.Model.extend({
        defaults: function () {
            return {
                id: "",
                text: ""
            };
        },
        urlRoot: function() {
            return apidocs.settings.url + '/documentation';
        }
    });

    apidocs.data = {
        resources: [],
        types: [],
        articles: {},
        load: function () {
            return $.when.apply($,
                _.union([
                    $.ajax({ url: apidocs.settings.url + '/resourcemetadata', dataType: 'json' }).done(function (data) {
                        apidocs.data.resources = data;
                    }),
                    $.ajax({ url: apidocs.settings.url + '/typemetadata', dataType: 'json' }).done(function (data) {
                        apidocs.data.types = data;
                    }),
                    $.ajax({ url: apidocs.settings.url + '/documentation', dataType: 'json' }).done(function (data) {
                        apidocs.data.articles = data;
                    })
                ])
            );
        },
        getArticle: function (id) {
            return _.find(this.articles, function (a) {
                return a.id == id;
            }) || { id: id, text: "" };
        },
        getResource: function (docId) {
            return _.find(this.resources, function (a) {
                return (a.docId && a.docId == docId) || a.name == docId;
            });
        },
        getType: function (docId) {
            return _.find(this.types, function (a) {
                return (a.docId && a.docId == docId) || a.name == docId;
            });
        }
    };

    var DocsRouter = Backbone.Router.extend({
        routes: {
            "*docId": "scrollToDoc"
        }
    });
    apidocs.pageRouter = new DocsRouter;
    apidocs.pageRouter.on('route:scrollToDoc', function (docId) {
        apidocs.views.helpers.scrollTo("#" + docId);
    });

    Backbone.history.start();
    
})(this);